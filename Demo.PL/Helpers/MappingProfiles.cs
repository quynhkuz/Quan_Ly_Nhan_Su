using AutoMapper;
using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeViewModel>();

            CreateMap<EmployeeViewModel, Employee>()
                .ForMember(D => D.Department, O => O.Ignore());

            CreateMap<PayrollPeriod, PayrollPeriodViewModel>()
                .ForMember(D => D.Name, O => O.MapFrom(S => $"{S.Month}/{S.Year}"))
                .ForMember(D => D.EmployeeCount, O => O.MapFrom(S => S.Payslips.Count));

            CreateMap<Payslip, PayslipViewModel>()
                .ForMember(D => D.EmployeeName, O => O.MapFrom(S => S.Employee != null ? S.Employee.Name : ""))
                .ForMember(D => D.DepartmentName, O => O.MapFrom(S => S.Employee != null && S.Employee.Department != null ? DisplayNames.Department(S.Employee.Department.Name) : ""))
                .ForMember(D => D.PeriodName, O => O.MapFrom(S => S.PayrollPeriod != null ? $"{S.PayrollPeriod.Month}/{S.PayrollPeriod.Year}" : ""));

            CreateMap<Payslip, EditPayslipViewModel>()
                .ForMember(D => D.EmployeeName, O => O.MapFrom(S => S.Employee != null ? S.Employee.Name : ""));

            CreateMap<EditPayslipViewModel, Payslip>();
        }
    }
}
