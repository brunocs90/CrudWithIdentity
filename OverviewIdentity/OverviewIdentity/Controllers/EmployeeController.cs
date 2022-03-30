using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OverviewIdentity.Data;
using OverviewIdentity.Models;

namespace OverviewIdentity.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }


        // GET: Employee/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Employee());
            else
                return View(_context.Employees.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("EmployeeId,FullName,NameCompany,Position,OfficeLocation")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var type = string.Empty;
                if (employee.EmployeeId == 0)
                {
                    type = "Criou";
                    _context.Add(employee);
                }                    
                else
                {
                    type = "Atualizou";
                    _context.Update(employee);
                }                   

                await _context.SaveChangesAsync();
                LogAuditoria(type, employee);

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }


        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var employee = await _context.Employees.FindAsync(id);

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
            LogAuditoria("Delete", employee);

            return RedirectToAction(nameof(Index));
        }

        private void LogAuditoria(string message, Employee employee)
        {
            _context.LogAuditoria.Add(
              new LogAuditoria
              {
                  EmailUsuario = User.Identity.Name,
                  DetalhesAuditoria = $"{message} Funcionário: {employee.FullName} Data: {DateTime.Now.ToLongDateString()}"              
              });
            _context.SaveChanges();
        }
    }
}
