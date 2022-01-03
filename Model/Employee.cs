using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorlysTestProject.Model {
    public class Employee {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNo { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int WorkplaceID { get; set; }

        public Employee(int employeeId, string firstname, string lastname, int phoneno, string email, DateTime dateofbirth, int workplaceid) {
            EmployeeId = employeeId;
            FirstName = firstname;
            LastName = lastname;
            PhoneNo = phoneno;
            Email = email;
            DateOfBirth = dateofbirth;
            WorkplaceID = workplaceid;
        }
    }
}
