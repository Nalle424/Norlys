using NorlysTestProject.Model;
using System;
using System.Collections.Generic;

namespace NorlysTestProject.DatabaseAccess {
    public interface IEmployeeAccess {

        Employee GetEmployeeById(int employeeId);
        Boolean UpdateEmployee(Employee employee);
        Boolean CreateEmployee(Employee createEmployee);
        Boolean DeleteEmployee(int employeeId);
        List<Employee> GetAllEmployees();
    }
}