using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlfalahApp.Server.Context;
using AlfalahApp.Server.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ClosedXML.Report;
using ClosedXML.Excel;

namespace AlfalahApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaySlipsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _env;

        public PaySlipsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/PaySlips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaySlip>>> GetPaySlips()
        {
            return await _context.PaySlips.ToListAsync();
        }

        // GET: api/PaySlips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaySlip>> GetPaySlip(Guid id)
        {
            var paySlip = await _context.PaySlips.FindAsync(id);

            if (paySlip == null)
            {
                return NotFound();
            }

            return paySlip;
        }

        // PUT: api/PaySlips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaySlip(Guid id, PaySlip paySlip)
        {
            if (id != paySlip.Id)
            {
                return BadRequest();
            }

            _context.Entry(paySlip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaySlipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaySlips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaySlip>> PostPaySlip(PaySlip paySlip)
        {
            _context.PaySlips.Add(paySlip);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaySlipExists(paySlip.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPaySlip", new { id = paySlip.Id }, paySlip);
        }

        [HttpGet]
        [Route("report")]
        public async Task<ActionResult> SalaryReport(string session, string term)
        {
            byte[] result = null;
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", "Payslip.xlsx");

            var salaryReport = new XLWorkbook(templatePath);
            salaryReport.Worksheets.Delete(1);

            var paySlips = await _context.PaySlips.Include(e => e.Emp)
                                               .Where(i => i.AcdSession == session && i.Term == term)
                                               .ToListAsync();

            var slips = GetSlips(paySlips);

            foreach (var slip in slips)
            {
                var template = new XLTemplate(templatePath);
                template.AddVariable(slip);
                template.Generate();

                template.Workbook.Worksheet(1).CopyTo(salaryReport, slip.FullName);
            }
            salaryReport.Protect(true, true, "DirectorOnly");
            using var ms = new MemoryStream();
            salaryReport.SaveAs(ms);
            result = ms.ToArray();

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{session} {term} term Payslip.xlsx");
        }

        private List<Slip> GetSlips(List<PaySlip> slips)
        {
            List<Slip> payslip = new List<Slip>();
            foreach (var item in slips)
            {
                payslip.Add(new Slip()
                {
                    PayDate = item.Paydate.ToString("Y"),
                    Term = item.Term,
                    FullName = item.Emp.EName,
                    Dept = item.Emp.Dept,                    
                    Basic = item.Emp.Basic,
                    Transport = item.Emp.Transport,
                    Rent = item.Emp.Rent,
                    Tax = item.Emp.Tax,
                    OtherDeduction = item.OtherDeduction,
                    OtherEarning = item.OtherEarning,
                    Iou = item.Iou,
                    Loans = item.Loans,
                    Note = item.Note
                });
            }

            return payslip;
        }

        // DELETE: api/PaySlips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaySlip(Guid id)
        {
            var paySlip = await _context.PaySlips.FindAsync(id);
            if (paySlip == null)
            {
                return NotFound();
            }

            _context.PaySlips.Remove(paySlip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaySlipExists(Guid id)
        {
            return _context.PaySlips.Any(e => e.Id == id);
        }
    }
}
