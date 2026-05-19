using ItalisaTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;

namespace ItalisaTools.Controllers
{
    public class ProductionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductionController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env     = env;
        }

        // GET: /Production/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Vendors   = await _context.SVN_Italisa_vendor.ToListAsync();
            ViewBag.Processes = await _context.SVN_Italisa_Process.ToListAsync();
            ViewBag.Colors    = await _context.sVN_Italisa_Color.ToListAsync();
            return View();
        }

        // POST: /Production/Create  (multipart/form-data)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductionCreateDto dto)
        {
            if (dto == null)
                return Json(new { success = false, message = "Invalid data." });
            if (string.IsNullOrWhiteSpace(dto.Vendor))
                return Json(new { success = false, message = "Vendor is required." });
            if (string.IsNullOrWhiteSpace(dto.Process))
                return Json(new { success = false, message = "Process is required." });
            if (dto.ProductId == null)
                return Json(new { success = false, message = "Code (Product) is required." });
            if (string.IsNullOrWhiteSpace(dto.Color))
                return Json(new { success = false, message = "Color is required." });
            if (string.IsNullOrWhiteSpace(dto.TypeValue))
                return Json(new { success = false, message = "Type is required." });
            if (dto.Quantity == null || dto.Quantity <= 0)
                return Json(new { success = false, message = "Quantity must be a positive number." });

            try
            {
                string? imagePath = null;
                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "production");
                    Directory.CreateDirectory(uploadsDir);

                    var ext      = Path.GetExtension(dto.ImageFile.FileName).ToLowerInvariant();
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await dto.ImageFile.CopyToAsync(stream);

                    imagePath = $"{Request.PathBase}/uploads/production/{fileName}";
                }

                var record = new SVN_Italisa_Production
                {
                    vendor        = dto.Vendor,
                    product_id    = dto.ProductId,
                    type_value    = dto.TypeValue,
                    product_qty   = dto.Quantity,
                    process       = dto.Process,
                    color         = dto.Color,
                    date_finished = dto.DateFinished ?? DateTime.Now,
                    description   = dto.Description,
                    image_path    = imagePath,
                    defect_name   = dto.TypeValue == "Defect" ? dto.DefectName : null
                };

                _context.SVN_Italisa_Production.Add(record);
                await _context.SaveChangesAsync();

                await _context.Database.ExecuteSqlRawAsync("EXEC SVN_Sync_Production_By_Hour_ITA");

                return Json(new { success = true, message = "Save successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner error: {ex.InnerException.Message}");

                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // ── History page ────────────────────────────────────────────────────

        public IActionResult History() => View();

        [HttpGet]
        public async Task<IActionResult> GetHistory(
            string? vendor, string? typeValue, string? process,
            string? color, int? productId, string? defectName,
            string? dateFrom, string? dateTo)
        {
            try
            {
                var data = await BuildHistoryQuery(vendor, typeValue, process, color, productId, defectName, dateFrom, dateTo)
                    .Select(x => new
                    {
                        x.id, x.process, x.product_qty, x.product_id,
                        x.vendor, x.type_value, x.color,
                        x.date_finished, x.description, x.image_path,
                        x.defect_name
                    })
                    .ToListAsync();

                return Json(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetHistory Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── Export Excel with embedded images ───────────────────────────────

        [HttpGet]
        public async Task<IActionResult> ExportHistoryExcel(
            string? vendor, string? typeValue, string? process,
            string? color, int? productId, string? defectName,
            string? dateFrom, string? dateTo)
        {
            try
            {
                // 1. Fetch data (same filters as GetHistory)
                var data = await BuildHistoryQuery(vendor, typeValue, process, color, productId, defectName, dateFrom, dateTo)
                    .ToListAsync();

                // 2. Build product_id → Operation label map
                var codeMap = await GetCodeMapAsync();

                // 3. Generate Excel with EPPlus
                ExcelPackage.License.SetNonCommercialOrganization("ItalisaTools");
                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Production History");

                // ── Column definitions ──────────────────────────────────────
                double[] colWidths = { 5, 14, 18, 32, 16, 20, 24, 12, 22, 40, 16 };
                string[] headers   = { "#", "Vendor", "Process", "Operation", "Color", "Type", "Defect", "Quantity", "Date", "Description", "Image" };

                // ── Header row ──────────────────────────────────────────────
                for (int c = 0; c < headers.Length; c++)
                {
                    var cell = ws.Cells[1, c + 1];
                    cell.Value = headers[c];
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Color.SetColor(Color.White);
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x1B, 0x4F, 0x8A)); // --denim
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment   = ExcelVerticalAlignment.Center;
                    cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    cell.Style.Border.Bottom.Color.SetColor(Color.FromArgb(0x0F, 0x2F, 0x52));
                }
                ws.Row(1).Height = 22;

                // ── Data rows ───────────────────────────────────────────────
                const int    ImgCol        = 11;
                const double ImgRowHeight  = 65;
                const int    ImgSizePx     = 70;

                for (int i = 0; i < data.Count; i++)
                {
                    var item   = data[i];
                    int exlRow = i + 2;

                    ws.Cells[exlRow, 1].Value = i + 1;
                    ws.Cells[exlRow, 2].Value = item.vendor      ?? "";
                    ws.Cells[exlRow, 3].Value = item.process     ?? "";
                    ws.Cells[exlRow, 4].Value = codeMap.TryGetValue(item.product_id ?? -1, out var op) ? op
                                                : item.product_id.HasValue ? $"ID:{item.product_id}" : "";
                    ws.Cells[exlRow, 5].Value = item.color       ?? "";
                    ws.Cells[exlRow, 6].Value = item.type_value  ?? "";
                    ws.Cells[exlRow, 7].Value = item.defect_name ?? "";
                    ws.Cells[exlRow, 8].Value = item.product_qty ?? 0;
                    ws.Cells[exlRow, 9].Value = item.date_finished.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[exlRow, 10].Value = item.description ?? "";

                    if (i % 2 == 1)
                    {
                        using var range = ws.Cells[exlRow, 1, exlRow, ImgCol];
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0xEF, 0xF6, 0xFF));
                    }

                    var physPath = ResolvePhysicalImagePath(item.image_path);
                    if (physPath != null)
                    {
                        try
                        {
                            ws.Row(exlRow).Height = ImgRowHeight;

                            using var fileStream = new FileStream(physPath, FileMode.Open, FileAccess.Read);
                            var pic = ws.Drawings.AddPicture($"img_{exlRow}", fileStream);
                            pic.SetPosition(exlRow - 1, 3, ImgCol - 1, 3);
                            pic.SetSize(ImgSizePx, ImgSizePx);
                        }
                        catch (Exception imgEx)
                        {
                            Console.WriteLine($"ExportExcel image error row {exlRow}: {imgEx.Message}");
                        }
                    }

                    ws.Cells[exlRow, 1, exlRow, ImgCol - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                ws.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[2, 8, data.Count + 1, 8].Style.Numberformat.Format = "#,##0";

                if (data.Count > 0)
                {
                    var dataRange = ws.Cells[1, 1, data.Count + 1, ImgCol];
                    dataRange.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(0xE2, 0xE8, 0xF0));
                    for (int r = 2; r <= data.Count + 1; r++)
                        ws.Cells[r, 1, r, ImgCol].Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                }

                for (int c = 0; c < colWidths.Length; c++)
                    ws.Column(c + 1).Width = colWidths[c];

                ws.View.FreezePanes(2, 1);

                var bytes    = package.GetAsByteArray();
                var fileName = $"Production_History_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ExportHistoryExcel Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── Shared query builder ─────────────────────────────────────────────

        private IQueryable<SVN_Italisa_Production> BuildHistoryQuery(
            string? vendor, string? typeValue, string? process,
            string? color, int? productId, string? defectName,
            string? dateFrom, string? dateTo)
        {
            var query = _context.SVN_Italisa_Production.AsQueryable();

            if (!string.IsNullOrEmpty(vendor))
                query = query.Where(x => x.vendor == vendor);
            if (!string.IsNullOrEmpty(typeValue))
                query = query.Where(x => x.type_value == typeValue);
            if (!string.IsNullOrEmpty(process))
                query = query.Where(x => x.process == process);
            if (!string.IsNullOrEmpty(color))
                query = query.Where(x => x.color == color);
            if (productId.HasValue)
                query = query.Where(x => x.product_id == productId.Value);
            if (!string.IsNullOrEmpty(defectName))
                query = query.Where(x => x.defect_name == defectName);
            if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out var dfrom))
                query = query.Where(x => x.date_finished >= dfrom);
            if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var dto2))
                query = query.Where(x => x.date_finished <= dto2.AddDays(1));

            return query.OrderByDescending(x => x.date_finished);
        }

        // ── Helper: resolve stored image path → physical file path ──────────

        private string? ResolvePhysicalImagePath(string? storedPath)
        {
            if (string.IsNullOrEmpty(storedPath)) return null;

            var path      = storedPath;
            var pathBase  = HttpContext.Request.PathBase.Value ?? "";

            if (!string.IsNullOrEmpty(pathBase) &&
                path.StartsWith(pathBase, StringComparison.OrdinalIgnoreCase))
            {
                path = path[pathBase.Length..];
            }

            path = path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physical = Path.Combine(_env.WebRootPath, path);

            return System.IO.File.Exists(physical) ? physical : null;
        }

        // ── Helper: build product_id → Operation text map ────────────────────

        private async Task<Dictionary<int, string>> GetCodeMapAsync()
        {
            var map        = new Dictionary<int, string>();
            var seen       = new HashSet<int>();
            var connString = _context.Database.GetConnectionString();

            var processes = await _context.SVN_Italisa_Process
                .Select(p => p.process)
                .ToListAsync();

            foreach (var proc in processes.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                try
                {
                    using var conn = new SqlConnection(connString);
                    await conn.OpenAsync();

                    using var cmd = new SqlCommand("sp_GetItemsByOperation", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@OperationKeyword", proc!.Trim()));

                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt32(reader.GetOrdinal("product_id"));
                        if (seen.Add(id))
                            map[id] = reader.GetString(reader.GetOrdinal("Operation"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GetCodeMapAsync error for '{proc}': {ex.Message}");
                }
            }

            return map;
        }

        // ── Lookup endpoints ─────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetVendors()
        {
            var vendors = await _context.SVN_Italisa_vendor
                .Select(v => v.vendor_code)
                .ToListAsync();
            return Json(vendors);
        }

        [HttpGet]
        public async Task<IActionResult> GetProcesses()
        {
            var processes = await _context.SVN_Italisa_Process
                .Select(p => p.process)
                .ToListAsync();
            return Json(processes);
        }

        [HttpGet]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _context.sVN_Italisa_Color
                .OrderBy(c => c.color)
                .Select(c => c.color)
                .ToListAsync();
            return Json(colors);
        }

        [HttpGet]
        public async Task<IActionResult> GetCodesByProcess(string process)
        {
            if (string.IsNullOrWhiteSpace(process))
                return Json(new List<CodeItemDto>());

            var codes = new List<CodeItemDto>();
            try
            {
                var connString = _context.Database.GetConnectionString();
                using var conn = new SqlConnection(connString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand("sp_GetItemsByOperation", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@OperationKeyword", process.Trim()));

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    codes.Add(new CodeItemDto
                    {
                        Value = reader.GetInt32(reader.GetOrdinal("product_id")),
                        Text  = reader.GetString(reader.GetOrdinal("Operation"))
                    });
                }
                return Json(codes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCodesByProcess Error: {ex.Message}");
                return Json(new List<CodeItemDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDefectNames()
        {
            var names = await _context.SVN_Italisa_DefectInfor
                .OrderBy(d => d.defect_name_en)
                .Select(d => d.defect_name_en ?? "")
                .Where(d => d != "")
                .ToListAsync();
            return Json(names);
        }

        [HttpGet]
        public async Task<IActionResult> GetDefects()
        {
            var defects = await _context.SVN_Italisa_DefectInfor
                .OrderBy(d => d.defect_name_en)
                .Select(d => new DefectItemDto
                {
                    NameEn = d.defect_name_en ?? "",
                    NameVn = d.defect_name_vn ?? "",
                    NameCn = d.defect_name_cn ?? ""
                })
                .ToListAsync();
            return Json(defects);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCodes()
        {
            var map = await GetCodeMapAsync();
            var result = map.Select(kv => new CodeItemDto { Value = kv.Key, Text = kv.Value }).ToList();
            return Json(result);
        }

        // ════════════════════════════════════════════════════════════════════════
        // YIELD REPORT — Filter-based table switch (Cách B):
        //   • dateTo >= today (bao trùm hôm nay/tương lai) → SVN_Italisa_Production         (live, GIỮ NGUYÊN LOGIC CŨ)
        //   • dateTo <  today (range hoàn toàn quá khứ)    → SVN_Italisa_Production_ByExcel (QC đã upload)
        // ════════════════════════════════════════════════════════════════════════

        public IActionResult DefectReport() => View();

        [HttpGet]
        public async Task<IActionResult> GetDefectReportData(
            string? dateFrom, string? dateTo,
            string? process, string? color, int? productId)
        {
            try
            {
                // ── Quyết định bảng nguồn dựa trên dateTo ────────────────
                var today = DateTime.Today;
                bool useExcel = false;
                if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var dtoCheck))
                {
                    useExcel = dtoCheck.Date < today;
                }

                // ── Fetch records: project về anonymous type chung ───────
                // (cả 2 bảng có schema giống hệt nhau, C# unify anonymous type)
                var records = useExcel
                    ? await BuildExcelQuery(dateFrom, dateTo, process, color, productId)
                        .Select(x => new
                        {
                            x.date_finished,
                            x.product_id,
                            x.color,
                            x.process,
                            x.type_value,
                            x.defect_name,
                            x.product_qty
                        })
                        .ToListAsync()
                    : await BuildProductionQuery(dateFrom, dateTo, process, color, productId)
                        .Select(x => new
                        {
                            x.date_finished,
                            x.product_id,
                            x.color,
                            x.process,
                            x.type_value,
                            x.defect_name,
                            x.product_qty
                        })
                        .ToListAsync();

                Console.WriteLine(
                    $"GetDefectReportData: source={(useExcel ? "ByExcel" : "Production")} " +
                    $"(dateTo={dateTo}, today={today:yyyy-MM-dd}), {records.Count} records");

                // ── Rest of logic unchanged ───────────────────────────────
                var (italisaNameMap, svnMap, productItalisaMap) = await GetItemNameFromOverviewAsync();

                var defectInfoAll  = await _context.SVN_Italisa_DefectInfor.ToListAsync();
                var defectInfoByEn = defectInfoAll
                    .Where(d => !string.IsNullOrEmpty(d.defect_name_en))
                    .ToDictionary(d => d.defect_name_en!, d => d);

                var grouped = records
                    .GroupBy(x => new
                    {
                        Date    = x.date_finished.Date,
                        ProdId  = x.product_id,
                        Color   = x.color   ?? "-",
                        Process = x.process ?? "-"
                    })
                    .Select(g =>
                    {
                        // Chỉ tính Production Qty làm input
                        var inputQty = g.Where(x => x.type_value == "Production Qty").Sum(x => x.product_qty ?? 0);
                        var ngQty    = g.Where(x => x.type_value == "Defect").Sum(x => x.product_qty ?? 0);
                        var okQty    = Math.Max(0, inputQty - ngQty);

                        var defects = g
                            .Where(x => x.type_value == "Defect" && !string.IsNullOrEmpty(x.defect_name))
                            .GroupBy(x => x.defect_name!)
                            .ToDictionary(dg => dg.Key, dg => dg.Sum(x => x.product_qty ?? 0));

                        // product_id → Italisa_no → Item_name
                        productItalisaMap.TryGetValue(g.Key.ProdId ?? -1, out var italisaNo);

                        var partCode = italisaNo > 0
                            ? $"Y{italisaNo:D4}"
                            : (g.Key.ProdId.HasValue ? g.Key.ProdId.Value.ToString() : "-");

                        italisaNameMap.TryGetValue(italisaNo, out var itemName);
                        var product = !string.IsNullOrEmpty(itemName) ? itemName : partCode;

                        // SVN Code + SVN Name: key = "ItalisaNo_Color"
                        var svnKey = $"{italisaNo}_{g.Key.Color}";
                        svnMap.TryGetValue(svnKey, out var svnInfo);

                        return new
                        {
                            date      = g.Key.Date.ToString("dd-MMM-yyyy"),
                            productId = g.Key.ProdId,
                            italisaNo = italisaNo > 0 ? (int?)italisaNo : null,
                            partCode,
                            product,
                            svnCode   = svnInfo.SvnCode ?? "",
                            svnName   = svnInfo.SvnName ?? "",
                            coating   = g.Key.Color,
                            process   = g.Key.Process,
                            inputQty,
                            okQty,
                            ngQty,
                            outputQty  = okQty,
                            defectRate = inputQty > 0 ? Math.Round((double)ngQty / inputQty * 100, 1) : 0.0,
                            yieldRate  = inputQty > 0 ? Math.Round((double)okQty  / inputQty * 100, 1) : 100.0,
                            defects
                        };
                    })
                    .Where(x => x.inputQty > 0 || x.ngQty > 0)
                    .OrderByDescending(x => x.date)
                    .ThenBy(x => x.partCode)
                    .ToList();

                var usedDefectEnNames = grouped
                    .SelectMany(x => x.defects.Keys)
                    .Distinct().OrderBy(x => x).ToList();

                var defectTypes = usedDefectEnNames.Select(en =>
                {
                    defectInfoByEn.TryGetValue(en, out var info);
                    return new { en, vn = info?.defect_name_vn ?? en, cn = info?.defect_name_cn ?? en };
                }).ToList();

                return Json(new { rows = grouped, defectTypes });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetDefectReportData Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── Helpers: build query với cùng bộ filter cho 2 bảng ────────────────
        private IQueryable<SVN_Italisa_Production> BuildProductionQuery(
            string? dateFrom, string? dateTo,
            string? process, string? color, int? productId)
        {
            var q = _context.SVN_Italisa_Production.AsQueryable();
            if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out var dfrom))
                q = q.Where(x => x.date_finished >= dfrom);
            if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var dto2))
                q = q.Where(x => x.date_finished <= dto2.AddDays(1).AddSeconds(-1));
            if (!string.IsNullOrEmpty(process))
                q = q.Where(x => x.process == process);
            if (!string.IsNullOrEmpty(color))
                q = q.Where(x => x.color == color);
            if (productId.HasValue)
                q = q.Where(x => x.product_id == productId.Value);
            return q;
        }

        private IQueryable<SVN_Italisa_Production_ByExcel> BuildExcelQuery(
            string? dateFrom, string? dateTo,
            string? process, string? color, int? productId)
        {
            var q = _context.SVN_Italisa_Production_ByExcel.AsQueryable();
            if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out var dfrom))
                q = q.Where(x => x.date_finished >= dfrom);
            if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var dto2))
                q = q.Where(x => x.date_finished <= dto2.AddDays(1).AddSeconds(-1));
            if (!string.IsNullOrEmpty(process))
                q = q.Where(x => x.process == process);
            if (!string.IsNullOrEmpty(color))
                q = q.Where(x => x.color == color);
            if (productId.HasValue)
                q = q.Where(x => x.product_id == productId.Value);
            return q;
        }

        // ── Helper: lấy item name + SVN info từ stored proc Overview ─────────────
        private async Task<(
            Dictionary<int, string> italisaNameMap,
            Dictionary<string, (string SvnCode, string SvnName)> svnMap,
            Dictionary<int, int> productItalisaMap)>
            GetItemNameFromOverviewAsync()
        {
            var italisaNameMap = new Dictionary<int, string>();
            var svnMap = new Dictionary<string, (string, string)>();
            var productItalisaMap = new Dictionary<int, int>();

            var connString = _context.Database.GetConnectionString();
            try
            {
                using var conn = new SqlConnection(connString);
                await conn.OpenAsync();

                // Step 1: Italisa_no → Item_name + SVN Code + SVN Name
                using (var cmd = new SqlCommand("sp_Get_Italisa_Production_Report_Color", conn)
                { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = new DateTime(2000, 1, 1) });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = DateTime.Today });

                    using var reader = await cmd.ExecuteReaderAsync();

                    int pnIdx = -1, nameIdx = -1, colorIdx = -1, svnCodeIdx = -1, svnNameIdx = -1;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var col = reader.GetName(i);
                        if (col == "Italisa PN#") pnIdx = i;
                        if (col == "Item Name") nameIdx = i;
                        if (col == "Color") colorIdx = i;
                        if (col == "SVN Code") svnCodeIdx = i;
                        if (col == "SVN Name") svnNameIdx = i;
                    }

                    if (pnIdx >= 0 && nameIdx >= 0)
                    {
                        while (await reader.ReadAsync())
                        {
                            if (reader.IsDBNull(pnIdx)) continue;
                            var pn = Convert.ToInt32(reader.GetValue(pnIdx));
                            var name = nameIdx >= 0 && !reader.IsDBNull(nameIdx) ? reader.GetString(nameIdx) : "";
                            var col2 = colorIdx >= 0 && !reader.IsDBNull(colorIdx) ? reader.GetString(colorIdx) : "";
                            var svnCode = svnCodeIdx >= 0 && !reader.IsDBNull(svnCodeIdx) ? reader.GetString(svnCodeIdx) : "";
                            var svnName = svnNameIdx >= 0 && !reader.IsDBNull(svnNameIdx) ? reader.GetString(svnNameIdx) : "";

                            italisaNameMap.TryAdd(pn, name);

                            var key = $"{pn}_{col2}";
                            svnMap.TryAdd(key, (svnCode, svnName));
                        }
                    }
                }

                // Step 2: product_id → Italisa_no từ ProductMapping
                using (var cmd2 = new SqlCommand("SELECT product_id, Italisa_no FROM dbo.ProductMapping", conn))
                using (var reader2 = await cmd2.ExecuteReaderAsync())
                {
                    while (await reader2.ReadAsync())
                        productItalisaMap.TryAdd(reader2.GetInt32(0), reader2.GetInt32(1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetItemNameFromOverviewAsync Error: {ex.Message}");
            }

            return (italisaNameMap, svnMap, productItalisaMap);
        }


        public IActionResult Report() => View();

        public IActionResult Overview() => View();

        [HttpGet]
        public async Task<IActionResult> GetOverviewReport(string? startDate, string? endDate)
        {
            try
            {
                DateTime start = string.IsNullOrEmpty(startDate) ? new DateTime(2000, 1, 1) : DateTime.Parse(startDate);
                DateTime end = string.IsNullOrEmpty(endDate) ? DateTime.Today : DateTime.Parse(endDate);

                var connString = _context.Database.GetConnectionString();
                using var conn = new SqlConnection(connString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand("sp_Get_Italisa_Production_Report_Color", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = start });
                cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = end });

                using var reader = await cmd.ExecuteReaderAsync();

                var columns = Enumerable.Range(0, reader.FieldCount)
                                        .Select(i => reader.GetName(i))
                                        .ToList();

                var rows = new List<Dictionary<string, object?>>();
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    foreach (var col in columns)
                    {
                        var ordinal = reader.GetOrdinal(col);
                        row[col] = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
                    }
                    rows.Add(row);
                }

                return Json(new { columns, rows });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOverviewReport Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }


        // ── DTOs ─────────────────────────────────────────────────────────────

        public class DefectItemDto
        {
            public string NameEn { get; set; } = string.Empty;
            public string NameVn { get; set; } = string.Empty;
            public string NameCn { get; set; } = string.Empty;
        }

        public class CodeItemDto
        {
            public int    Value { get; set; }
            public string Text  { get; set; } = string.Empty;
        }

        public class ProductionCreateDto
        {
            public string?    Vendor       { get; set; }
            public int?       ProductId    { get; set; }
            public string?    TypeValue    { get; set; }
            public string?    Color        { get; set; }
            public int?       Quantity     { get; set; }
            public string?    Process      { get; set; }
            public DateTime?  DateFinished { get; set; }
            public string?    Description  { get; set; }
            public IFormFile? ImageFile    { get; set; }
            public string?    DefectName   { get; set; }
        }
    }
}
