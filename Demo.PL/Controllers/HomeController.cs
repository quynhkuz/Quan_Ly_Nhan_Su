using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var employees = _unitOfWork.EmployeeRepository.GetAll().ToList();
            var departments = _unitOfWork.DepartmentRepository.GetAll().ToList();
            var periods = _unitOfWork.PayrollRepository.GetAllOrdered().ToList();

            var latestPeriod = periods.FirstOrDefault();

            var model = new DashboardViewModel
            {
                TotalEmployees = employees.Count,
                ActiveEmployees = employees.Count(E => E.IsActive && !E.IsDeleted),
                TotalDepartments = departments.Count,
                ClosedPeriods = periods.Count(P => P.Status == PayrollStatus.Closed),
                AverageSalary = employees.Any() ? Math.Round(employees.Average(E => E.Salary), 0) : 0,
                LatestPeriodName = latestPeriod is null ? null : $"{latestPeriod.Month}/{latestPeriod.Year}",
                LatestPeriodTotal = latestPeriod?.TotalNetSalary ?? 0,
                LatestPeriods = _mapper.Map<IEnumerable<PayrollPeriod>, IEnumerable<PayrollPeriodViewModel>>(periods.Take(5)),
                SalaryByDepartment = departments.Select(D => new DepartmentSalaryStat
                {
                    DepartmentName = DisplayNames.Department(D.Name),
                    EmployeeCount = employees.Count(E => E.DepartmentId == D.Id && !E.IsDeleted),
                    TotalSalary = employees
                        .Where(E => E.DepartmentId == D.Id && !E.IsDeleted)
                        .Sum(E => E.Salary)
                })
                .OrderByDescending(S => S.TotalSalary)
                .ToList()
            };

            return View(model);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
