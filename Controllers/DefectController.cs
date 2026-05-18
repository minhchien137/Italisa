    using ItalisaTools.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System.Data;

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
                            productId  = reader.GetInt32(0),
                            italisaNo  = reader.GetInt32(1),
                            itemCode   = reader.GetString(2),
                            // partCode format: "Y" + 4-digit padded
                            partCode   = $"Y{reader.GetInt32(1):D4}"
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

            // ── POST: Save batch of production records ───────────────────────────
            [HttpPost]
            public async Task<IActionResult> SaveBatch([FromBody] List<SVN_Italisa_Production> records)
            {
                if (records == null || !records.Any())
                    return BadRequest("No records.");

                await _context.SVN_Italisa_Production.AddRangeAsync(records);
                await _context.SaveChangesAsync();

                // Sync production data
                try { await _context.Database.ExecuteSqlRawAsync("EXEC SVN_Sync_Production_By_Hour_ITA"); }
                catch { /* non-critical */ }

                return Ok(new { saved = records.Count });
            }
        }
    }