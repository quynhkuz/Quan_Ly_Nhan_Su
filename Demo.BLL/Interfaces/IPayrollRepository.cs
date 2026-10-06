using Demo.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace Demo.BLL.Interfaces
{
    public interface IPayrollRepository : IGenaricRepository<PayrollPeriod>
    {
        PayrollPeriod GetWithPayslips(int periodId);
        PayrollPeriod GetByMonthYear(int month, int year);
        IEnumerable<PayrollPeriod> GetAllOrdered();
        Payslip GetPayslip(int payslipId);
        IEnumerable<Payslip> GetPayslipsByPeriod(int periodId);
        void AddPayslip(Payslip payslip);
        void UpdatePayslip(Payslip payslip);
        decimal GetTotalNetSalary(int periodId);
    }
}
