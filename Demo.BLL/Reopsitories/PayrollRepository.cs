using Demo.BLL.Interfaces;
using Demo.DAL.Data.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Demo.BLL.Reopsitories
{
    public class PayrollRepository : GenaricRepository<PayrollPeriod>, IPayrollRepository
    {
        public PayrollRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public PayrollPeriod GetWithPayslips(int periodId)
            => _dbcontext.PayrollPeriods
                .Include(P => P.Payslips)
                    .ThenInclude(S => S.Employee)
                        .ThenInclude(E => E.Department)
                .AsNoTracking()
                .FirstOrDefault(P => P.Id == periodId);

        public PayrollPeriod GetByMonthYear(int month, int year)
            => _dbcontext.PayrollPeriods
                .AsNoTracking()
                .FirstOrDefault(P => P.Month == month && P.Year == year);

        public IEnumerable<PayrollPeriod> GetAllOrdered()
            => _dbcontext.PayrollPeriods
                .Include(P => P.Payslips)
                .AsNoTracking()
                .OrderByDescending(P => P.Year)
                .ThenByDescending(P => P.Month)
                .ToList();

        public Payslip GetPayslip(int payslipId)
            => _dbcontext.Payslips
                .Include(S => S.Employee)
                    .ThenInclude(E => E.Department)
                .Include(S => S.PayrollPeriod)
                .AsNoTracking()
                .FirstOrDefault(S => S.Id == payslipId);

        public IEnumerable<Payslip> GetPayslipsByPeriod(int periodId)
            => _dbcontext.Payslips
                .Include(S => S.Employee)
                    .ThenInclude(E => E.Department)
                .AsNoTracking()
                .Where(S => S.PayrollPeriodId == periodId)
                .OrderBy(S => S.Employee.Name)
                .ToList();

        public void AddPayslip(Payslip payslip)
            => _dbcontext.Payslips.Add(payslip);

        public void UpdatePayslip(Payslip payslip)
            => _dbcontext.Payslips.Update(payslip);

        public decimal GetTotalNetSalary(int periodId)
            => _dbcontext.Payslips
                .Where(S => S.PayrollPeriodId == periodId)
                .Sum(S => S.BaseSalary + S.Allowance + S.Bonus - S.Deduction - S.Tax);
    }
}
