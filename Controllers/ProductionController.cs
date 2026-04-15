using ItalisaTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItalisaTools.Controllers
{
    public class ProductionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Production/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Vendors = await _context.SVN_Italisa_vendor.ToListAsync();
            ViewBag.Codes = await _context.SVN_Italisa_Code.ToListAsync();
            ViewBag.Processes = await _context.SVN_Italisa_Process.ToListAsync();
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductionCreateDto dto)
        {
            if (dto == null)
                return Json(new { success = false, message = "Invalid data." });

            try
            {
                var record = new SVN_Italisa_Production
                {
                    vendor = dto.Vendor,
                    product_id = dto.ProductId,
                    type_value = dto.TypeValue,
                    product_qty = dto.Quantity,
                    process = dto.Process, 
                    date_finished = DateTime.Now
                };

                _context.SVN_Italisa_Production.Add(record);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult History()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(string? vendor, string? typeValue, string? process, string? dateFrom, string? dateTo)
        {
            var query = _context.SVN_Italisa_Production
                .Join(_context.SVN_Italisa_Code,
                    p => p.product_id,
                    c => c.Italisa_no,
                    (p, c) => new
                    {
                        p.id,
                        p.vendor,
                        p.process,
                        ItemCode = c.Italisa_no + " - " + c.Item_code,
                        p.type_value,
                        p.product_qty,
                        p.date_finished
                    })
                .AsQueryable();

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

            var data = await query.OrderByDescending(x => x.date_finished).ToListAsync();
            return Json(data);
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
    }

    public class ProductionCreateDto
    {
        public string? Vendor { get; set; }
        public int? ProductId { get; set; }
        public string? TypeValue { get; set; }
        public int? Quantity { get; set; }
        public string? Process { get; set; } 
    }
}