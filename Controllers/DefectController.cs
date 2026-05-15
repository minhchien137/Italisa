using ItalisaTools.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Upload() => View();


        [HttpPost]
        public async Task<IActionResult> SaveBatch([FromBody] List<SVN_Italisa_Production> records)
        {
            if (records == null || !records.Any()) return BadRequest("No records.");
            await _context.SVN_Italisa_Production.AddRangeAsync(records);
            await _context.SaveChangesAsync();
            return Ok(new { saved = records.Count });
        }

    }
}