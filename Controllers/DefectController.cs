using ItalisaTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace ItalisaTools.Controllers
{
    public class DefectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DefectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Comparation() => View();
        public IActionResult Upload()      => View();

        // ── GET: Existing endpoint, keep for backwards compatibility ─────────
        [HttpGet]
        public async Task<IActionResult> GetPartCodeMap()
        {
            try
            {
                var connString = _context.Database.GetConnectionString();
                using var conn = new SqlConnection(connString);
                await conn.OpenAsync();

                const string sql = @"
                    SELECT pm.product_id, c.Italisa_no, ISNULL(c.Item_code,'') AS item_code
                    FROM   dbo.ProductMapping      pm
                    INNER JOIN dbo.SVN_Italisa_Code c ON pm.Italisa_no = c.Italisa_no";

                using var cmd    = new SqlCommand(sql, conn);
                using var reader = await cmd.ExecuteReaderAsync();

                var list = new List<object>();
                while (await reader.ReadAsync())
                {
                    list.Add(new
                    {
                        productId = reader.GetInt32(0),
                        italisaNo = reader.GetInt32(1),
                        itemCode  = reader.GetString(2),
                        partCode  = $"Y{reader.GetInt32(1):D4}"
                    });
                }
                return Json(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPartCodeMap Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: One-shot lookup for Upload page
        //   processes : all process names
        //   colors    : all color names (incl. "Non-color")
        //   vendors   : all vendor codes
        //   defects   : [{ nameEn, nameVn, nameCn }] — for matching defect cols
        //   codes     : { "Cr Plating CP": [{ italisaNo:478, productId:..., text:"Cr_Plating_CP-478(ITA)", vendor:"ITA" }, ...], ... }
        // ══════════════════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> GetUploadLookup()
        {
            try
            {
                var processes = await _context.SVN_Italisa_Process
                    .Select(p => p.process).Where(p => p != null).ToListAsync();

                var colors = await _context.sVN_Italisa_Color
                    .OrderBy(c => c.color).Select(c => c.color).ToListAsync();

                var vendors = await _context.SVN_Italisa_vendor
                    .Select(v => v.vendor_code).Where(v => v != null).ToListAsync();

                var defects = await _context.SVN_Italisa_DefectInfor
                    .OrderBy(d => d.defect_name_en)
                    .Select(d => new
                    {
                        nameEn = d.defect_name_en ?? "",
                        nameVn = d.defect_name_vn ?? "",
                        nameCn = d.defect_name_cn ?? ""
                    })
                    .ToListAsync();

                // Build code map: process → [{italisaNo, productId, text, vendor}]
                var codes = new Dictionary<string, List<object>>();
                var connString = _context.Database.GetConnectionString();
                var rxCode = new Regex(@"-(\d+)\s*\(\s*([^)]+?)\s*\)\s*$", RegexOptions.Compiled);

                foreach (var proc in processes)
                {
                    var list = new List<object>();
                    try
                    {
                        using var conn = new SqlConnection(connString);
                        await conn.OpenAsync();
                        using var cmd = new SqlCommand("sp_GetItemsByOperation", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.Add(new SqlParameter("@OperationKeyword", proc));
                        using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            var productId = reader.GetInt32(reader.GetOrdinal("product_id"));
                            var text      = reader.GetString(reader.GetOrdinal("Operation"));
                            var m         = rxCode.Match(text);
                            int italisaNo = m.Success ? int.Parse(m.Groups[1].Value) : 0;
                            string vendor = m.Success ? m.Groups[2].Value : "";
                            list.Add(new { italisaNo, productId, text, vendor });
                        }
                    }
                    catch (Exception ex) { Console.WriteLine($"sp_GetItemsByOperation({proc}): {ex.Message}"); }
                    codes[proc] = list;
                }

                return Json(new { processes, colors, vendors, defects, codes });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUploadLookup Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── POST: Save batch of production records ───────────────────────────

        [HttpPost]
        public async Task<IActionResult> SaveBatch([FromBody] List<ProductionBatchDto>? records)
        {
            try
            {
                // 1. Kiểm tra input
                if (records == null)
                    return BadRequest("records is null — JSON body không deserialize được.");
                if (!records.Any())
                    return BadRequest("No records.");

                // 2. Kiểm tra DbSet
                if (_context.SVN_Italisa_Production_ByExcel == null)
                    return StatusCode(500, "DbSet SVN_Italisa_Production_ByExcel is null — kiểm tra ApplicationDbContext.");

                // 3. Map DTO → Entity
                var entities = new List<SVN_Italisa_Production_ByExcel>();
                for (int i = 0; i < records.Count; i++)
                {
                    var r = records[i];
                    if (r == null)
                    {
                        Console.WriteLine($"[SaveBatch] records[{i}] is null — bỏ qua");
                        continue;
                    }

                    entities.Add(new SVN_Italisa_Production_ByExcel
                    {
                        process       = r.process,
                        product_qty   = r.product_qty,
                        product_id    = r.product_id,
                        color         = r.color,
                        vendor        = r.vendor,
                        type_value    = r.type_value,
                        defect_name   = r.defect_name,
                        date_finished = DateTime.TryParse(r.date_finished, out var dt) ? dt : DateTime.Now,
                        description   = r.description,
                        image_path    = r.image_path,
                    });
                }

                if (!entities.Any())
                    return BadRequest("Không có bản ghi hợp lệ sau khi map.");

                // 4. Save
                await _context.SVN_Italisa_Production_ByExcel.AddRangeAsync(entities);
                await _context.SaveChangesAsync();

                try { await _context.Database.ExecuteSqlRawAsync("EXEC SVN_Sync_Production_By_Hour_ITA"); }
                catch (Exception syncEx) { Console.WriteLine($"[SaveBatch] Sync warning: {syncEx.Message}"); }

                return Ok(new { saved = entities.Count });
            }
            catch (Exception ex)
            {
                // Log toàn bộ stack để xác định đúng dòng lỗi
                Console.WriteLine($"[SaveBatch] ERROR: {ex.Message}");
                Console.WriteLine($"[SaveBatch] INNER: {ex.InnerException?.Message}");
                Console.WriteLine($"[SaveBatch] STACK: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    error      = ex.Message,
                    inner      = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // DTO khớp chính xác với JSON payload từ JS
        public class ProductionBatchDto
        {
            public string? process { get; set; }
            public int? product_qty { get; set; }
            public int? product_id { get; set; }
            public string? color { get; set; }
            public string? vendor { get; set; }
            public string? type_value { get; set; }
            public string? defect_name { get; set; }
            public string? date_finished { get; set; }  // nhận string, parse thủ công
            public string? description { get; set; }
            public string? image_path { get; set; }
        }

    }
}