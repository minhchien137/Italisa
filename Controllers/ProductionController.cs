using ItalisaTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

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
                    // ── Server-side validation ──
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
                // ── Save image file ──
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

                // ── Build record ──
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
                    image_path    = imagePath
                };

                _context.SVN_Italisa_Production.Add(record);
                await _context.SaveChangesAsync();

                // ── Sync stored procedure (SP tự quản lý transaction bên trong) ──
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

        public IActionResult History()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(
            string? vendor, string? typeValue, string? process,
            string? color, int? productId,
            string? dateFrom, string? dateTo)
        {
            try
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

                if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out var dfrom))
                    query = query.Where(x => x.date_finished >= dfrom);

                if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var dto2))
                    query = query.Where(x => x.date_finished <= dto2.AddDays(1));

                var data = await query
                    .OrderByDescending(x => x.date_finished)
                    .Select(x => new
                    {
                        x.id,
                        x.process,
                        x.product_qty,
                        x.product_id,
                        x.vendor,
                        x.type_value,
                        x.color,
                        x.date_finished,
                        x.description,
                        x.image_path
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
                var connectionString = _context.Database.GetConnectionString();

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("sp_GetItemsByOperation", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@OperationKeyword", process.Trim()));

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    codes.Add(new CodeItemDto
                    {
                        Value = reader.GetInt32(reader.GetOrdinal("product_id")),
                        Text = reader.GetString(reader.GetOrdinal("Operation"))
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

        // ── NEW: Get all product_id → Operation mappings across all processes ──
        [HttpGet]
        public async Task<IActionResult> GetAllCodes()
        {
            var allCodes = new List<CodeItemDto>();
            var seen     = new HashSet<int>();

            try
            {
                var connectionString = _context.Database.GetConnectionString();
                var processes = await _context.SVN_Italisa_Process
                    .Select(p => p.process)
                    .ToListAsync();

                foreach (var proc in processes.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    try
                    {
                        using var conn = new SqlConnection(connectionString);
                        await conn.OpenAsync();

                        using var cmd = new SqlCommand("sp_GetItemsByOperation", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@OperationKeyword", proc.Trim()));

                        using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            var id = reader.GetInt32(reader.GetOrdinal("product_id"));
                            if (seen.Add(id))
                            {
                                allCodes.Add(new CodeItemDto
                                {
                                    Value = id,
                                    Text  = reader.GetString(reader.GetOrdinal("Operation"))
                                });
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Console.WriteLine($"GetAllCodes inner error for '{proc}': {innerEx.Message}");
                    }
                }

                return Json(allCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllCodes Error: {ex.Message}");
                return Json(new List<CodeItemDto>());
            }
        }


        public IActionResult Report()
        {
            return View();
        }
        
        /* Overview*/
        public IActionResult Overview()
        {
            return View();
        }

        // GET: /Production/GetOverviewReport?startDate=2026-05-01&endDate=2026-05-04
        [HttpGet]
        public async Task<IActionResult> GetOverviewReport(string? startDate, string? endDate)
        {
            try
            {
                // Parse dates; empty startDate = all time (use year 2000 as floor)
                DateTime start = string.IsNullOrEmpty(startDate)
                    ? new DateTime(2000, 1, 1)
                    : DateTime.Parse(startDate);

                DateTime end = string.IsNullOrEmpty(endDate)
                    ? DateTime.Today
                    : DateTime.Parse(endDate);

                var connectionString = _context.Database.GetConnectionString();

                using var conn = new SqlConnection(connectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand("sp_Get_Italisa_Production_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // SP expects @StartDate and @EndDate as date type
                cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = start });
                cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = end });

                using var reader = await cmd.ExecuteReaderAsync();

                // Read column names dynamically (pivot columns vary)
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


        // ── DTOs ──────────────────────────────────────────────────────────────

        public class CodeItemDto
        {
            public int    Value { get; set; }
            public string Text  { get; set; } = string.Empty;
        }

        public class ProductionCreateDto
        {
            public string?   Vendor       { get; set; }
            public int?      ProductId    { get; set; }
            public string?   TypeValue    { get; set; }
            public string?   Color        { get; set; }
            public int?      Quantity     { get; set; }
            public string?   Process      { get; set; }
            public DateTime? DateFinished { get; set; }
            public string?   Description  { get; set; }
            public IFormFile? ImageFile   { get; set; }
        }
    }
}
