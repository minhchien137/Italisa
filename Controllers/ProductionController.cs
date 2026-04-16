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
            return View();
        }

        // POST: /Production/Create  (multipart/form-data)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductionCreateDto dto)
        {
            if (dto == null)
                return Json(new { success = false, message = "Invalid data." });

            using var transaction = await _context.Database.BeginTransactionAsync();

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
                    date_finished = dto.DateFinished ?? DateTime.Now,
                    description   = dto.Description,
                    image_path    = imagePath
                };

                _context.SVN_Italisa_Production.Add(record);
                await _context.SaveChangesAsync();

                // ── Sync stored procedure ──
                await _context.Database.ExecuteSqlRawAsync("EXEC SVN_Sync_Production_By_Hour_ITA");

                await transaction.CommitAsync();

                return Json(new { success = true, message = "Save successfully!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
            string? dateFrom, string? dateTo)
        {
            try
            {
                // ── Lấy thẳng từ SVN_Italisa_Production, không Join ──
                var query = _context.SVN_Italisa_Production.AsQueryable();

                if (!string.IsNullOrEmpty(vendor))
                    query = query.Where(x => x.vendor == vendor);

                if (!string.IsNullOrEmpty(typeValue))
                    query = query.Where(x => x.type_value == typeValue);

                if (!string.IsNullOrEmpty(process))
                    query = query.Where(x => x.process == process);

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
            public int?      Quantity     { get; set; }
            public string?   Process      { get; set; }
            public DateTime? DateFinished { get; set; }
            public string?   Description  { get; set; }
            public IFormFile? ImageFile   { get; set; }
        }
    }
}
