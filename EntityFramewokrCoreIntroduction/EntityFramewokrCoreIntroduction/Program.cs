using EntityFramewokrCoreIntroduction.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EntityFramewokrCoreIntroduction
{
    class Program
    {
        static void Main(string[] args)
        {
           var db = new SoftUniContext();
            //var result = GetEmployeesFullInformation(db);
            //Console.WriteLine(result);

            //var result2 = GetEmployeesWithSalaryOver50000(db);
            //Console.WriteLine(result2);

            //var result3 = GetEmployeesFromResearchAndDevelopment(db);
            //Console.WriteLine(result3);

            //var result4 = AddNewAddressToEmployee(db);
            //Console.WriteLine(result4);

            //var result5 = GetEmployeesInPeriod(db);
            //Console.WriteLine(result5);

            //var result6 = GetAddressesByTown(db);
            //Console.WriteLine(result6);

            //var result7 = GetEmployee147(db);
            //Console.WriteLine(result7);

            //var result8 = GetDepartmentsWithMoreThan5Employees(db);
            //Console.WriteLine(result8);

            //var result8 = GetLatestProjects(db);
            //Console.WriteLine(result8);

            //var result9 = IncreaseSalaries(db);
            //Console.WriteLine(result9);

            //var result10 = GetEmployeesByFirstNameStartingWithSa(db);
            // Console.WriteLine(result10);

            //var result11 = DeleteProjectById(db);
            //Console.WriteLine(result11);

            var result12 = RemoveTown(db);
            Console.WriteLine(result12);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new { x.EmployeeId, x.FirstName, x.LastName, x.MiddleName, x.JobTitle, x.Salary})
                .OrderBy(x => x.EmployeeId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:F2}");
            }
            return sb.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Salary > 50_000)
                .Select(x => new { x.FirstName, x.Salary })
                .OrderBy(x => x.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} - {emp.Salary:F2}");
            }
            return sb.ToString();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new { x.FirstName, x.LastName, x.Department.Name, x.Salary })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.Name} - ${emp.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(x => x.LastName == "Nakov")
                .FirstOrDefault();
            employee.Address = new Address { TownId = 4, AddressText = "Vitoshka 15" };
            context.SaveChanges();

            var employees = context.Employees
                .Select(x => new { x.Address.AddressText, x.AddressId })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.AddressText}");
            }
            return sb.ToString();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var emplyees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFirstName = x.FirstName,
                    EmployeeLastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        ProjectStart = p.Project.StartDate,
                        ProjectEnd = p.Project.EndDate
                    })
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in emplyees)
            {
                sb.AppendLine($"{emp.EmployeeFirstName} {emp.EmployeeLastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");

                foreach (var pro in emp.Projects)
                {
                    var endDate = pro.ProjectEnd.HasValue ? pro.ProjectEnd.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished" ;
                    sb.AppendLine($"-- {pro.ProjectName} - {pro.ProjectStart.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");
                }
            }
            return sb.ToString();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    EmployeeCount = x.Employees.Count()
                })              
                .OrderByDescending(x => x.EmployeeCount)
                .OrderBy(x => x.TownName)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount}");
            }

            return sb.ToString();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name
                    })
                })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var proj in employee.Projects.OrderBy(x => x.ProjectName))
            {
                sb.AppendLine($"{proj.ProjectName}");
            }

            return sb.ToString();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Count = x.Employees.Count,
                    Employees = x.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).Select(x => new
                    {
                        EmployeeFirstName = x.FirstName,
                        EmployeeLastName = x.LastName,
                        EmployeeJobTitle = x.JobTitle
                    })
                })
                .OrderBy(x => x.Count)
                .ThenBy(x => x.DepartmentName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var dept in departments)
            {
                sb.AppendLine($"{dept.DepartmentName} {dept.ManagerFirstName} {dept.ManagerLastName}");
                foreach (var emp in dept.Employees)
                {
                    sb.AppendLine($"{emp.EmployeeFirstName} {emp.EmployeeLastName} {emp.EmployeeJobTitle}");
                }
            }
            return sb.ToString();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.StartDate
                })
                .Take(10)
                .OrderBy(x => x.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var proj in projects)
            {
                sb.AppendLine($"{proj.Name}");
                sb.AppendLine($"{proj.Description}");
                sb.AppendLine($"{proj.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
            }
            return sb.ToString();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employeesToUpate = context.Employees
                .Where(x => new string[]{ "Engineering", "Tool Design",
                "Marketing", "Information Services" }.Contains(x.Department.Name))
                .ToList();

            foreach (var emp in employeesToUpate)
            {
                emp.Salary *= 1.12M;
            }
            context.SaveChanges();

            employeesToUpate = employeesToUpate.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var emp in employeesToUpate)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.Salary:F2}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.ToLower().StartsWith("sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.FirstName} - {emp.LastName} - {emp.Salary:F2}");
            }

            return sb.ToString();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects.Find(2);

            var projects = context.EmployeesProjects
                .Where(x => x.ProjectId == 2)
                .ToList();

            foreach (var proj in projects)
            {
                context.EmployeesProjects.Remove(proj);
            }

            context.Projects.Remove(project);

            context.SaveChanges();

            var takeProjects = context.Projects.Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var proj in takeProjects)
            {
                Console.WriteLine($"{proj.Name}");
            }

            return sb.ToString();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Address.Town.Name == "Seattle")
                .ToList();

            foreach (var emp in employees)
            {
                emp.Address = null;
            }

            var addresses = context.Addresses
                .Where(x => x.Town.Name == "Seattle")
                .ToList();

            int count = addresses.Count;

            foreach (var address in addresses)
            {
                context.Addresses.Remove(address);
            }

            var town = context.Towns.FirstOrDefault(x => x.Name == "Seattle");

            context.Towns.Remove(town);
            context.SaveChanges();
            return $"{count} addresses in Seattle were removed";
        }
    }
}
