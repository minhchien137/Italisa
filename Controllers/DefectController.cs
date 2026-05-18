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
        public async Task<IActionResult> SaveBatch([FromBody] List<SVN_Italisa_Production> records)
        {
            if (records == null || !records.Any())
                return BadRequest("No records.");

            await _context.SVN_Italisa_Production.AddRangeAsync(records);
            await _context.SaveChangesAsync();

            try { await _context.Database.ExecuteSqlRawAsync("EXEC SVN_Sync_Production_By_Hour_ITA"); }
            catch { /* non-critical */ }

            return Ok(new { saved = records.Count });
        }
    }
}