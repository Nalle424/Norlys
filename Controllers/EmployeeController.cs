using Microsoft.AspNetCore.Mvc;
using NorlysTestProject.DatabaseAccess;
using NorlysTestProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NorlysTestProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController : ControllerBase {

        public readonly IEmployeeAccess IEA;

        public EmployeeController(IEmployeeAccess IEA) {
            this.IEA = IEA;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public List<Employee> GetAllEmployees() {
            return IEA.GetAllEmployees();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{employeeId}")]
        public Employee GetEmployeeId(int employeeId) {
            return IEA.GetEmployeeById(employeeId);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public Boolean CreateEmployee(Employee createEmployee) {
            return IEA.CreateEmployee(createEmployee);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("updateEmployee")]
        public Boolean UpdateEmployee(Employee updateEmployee) {
            return IEA.UpdateEmployee(updateEmployee);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{employeeId}")]
        public Boolean DeleteEmployee(int employeeId) {
            return IEA.DeleteEmployee(employeeId);
        }
    }
}
