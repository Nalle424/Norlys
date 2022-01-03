using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NorlysTestProject.Model;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace NorlysTestProject.DatabaseAccess {
    public class EmployeeAccess : IEmployeeAccess {

		private readonly string connectionString;

        public EmployeeAccess(IOptions<DBConnection> connectionString) {
            this.connectionString = connectionString.Value.Norlys.ToString();
        }

        public Employee GetEmployeeById(int employeeId) {
			Employee foundEmployee = null;

			String queryString = "SELECT employeeId, firstName, lastName, phoneNo, email, DateOfBirth, WorkplaceID FROM Employee WHERE employeeId = @employeeId";
			using (SqlConnection con = new SqlConnection(connectionString))
			using (SqlCommand readCommand = new SqlCommand(queryString, con)) {

				SqlParameter idParam = new SqlParameter("@employeeId", employeeId);
				readCommand.Parameters.Add(idParam);

				con.Open();
				SqlDataReader reader = readCommand.ExecuteReader();

				if (reader.HasRows) {
					int tempEmployeeId, tempPhoneNo, tempWorkplaceID;
					string tempFirstName, tempLastName, tempEmail;
					DateTime tempDob;

					while (reader.Read()) {
						tempEmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
						tempFirstName = reader.GetString(reader.GetOrdinal("firstName"));
						tempLastName = reader.GetString(reader.GetOrdinal("lastName"));
						tempPhoneNo = reader.GetInt32(reader.GetOrdinal("phoneNo"));
						tempEmail = reader.GetString(reader.GetOrdinal("email"));
						tempDob = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"));
						tempWorkplaceID = reader.GetInt32(reader.GetOrdinal("WorkplaceID"));

						foundEmployee = new Employee(tempEmployeeId, tempFirstName, tempLastName, tempPhoneNo, tempEmail, tempDob, tempWorkplaceID);
					}
				}
			}
			return foundEmployee;
		}

		public List<Employee> GetAllEmployees() {
			List<Employee> allEmployees = new List<Employee>();

			String querystring = "SELECT * FROM Employee";
			using (SqlConnection con = new SqlConnection(connectionString))
			using (SqlCommand readCommand = new SqlCommand(querystring, con)) {

				con.Open();
				SqlDataReader reader = readCommand.ExecuteReader();

				if (reader.HasRows) {
					int tempEmployeeId, tempPhoneNo, tempWorkplaceID;
					string tempFirstName, tempLastName, tempEmail;
					DateTime tempDob;

					while (reader.Read()) {
						tempEmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
						tempFirstName = reader.GetString(reader.GetOrdinal("firstName"));
						tempLastName = reader.GetString(reader.GetOrdinal("lastName"));
						tempPhoneNo = reader.GetInt32(reader.GetOrdinal("phoneNo"));
						tempEmail = reader.GetString(reader.GetOrdinal("email"));
						tempDob = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"));
						tempWorkplaceID = reader.GetInt32(reader.GetOrdinal("WorkplaceID"));

						allEmployees.Add(new Employee(tempEmployeeId, tempFirstName, tempLastName, tempPhoneNo, tempEmail, tempDob, tempWorkplaceID));
					}
				}
			}
			return allEmployees;
		}

		public Boolean UpdateEmployee(Employee updateEmployee) {
			Boolean UpdateEmp = false;

			using (TransactionScope scope = new TransactionScope()) {
				try {
					string queryString = "UPDATE EMPLOYEE SET firstname = @firstname, lastname = @lastname, email = @email, phoneNo = @phoneNo, dateofbirth = @dateofbirth, WorkplaceID = @WorkplaceID WHERE employeeID = @employeeID";
					using (SqlConnection con = new SqlConnection(connectionString))
					using (SqlCommand readCommand = new SqlCommand(queryString, con)) {
						con.Open();

						SqlParameter employeeIDParam = new SqlParameter("@employeeId", updateEmployee.EmployeeId);
						readCommand.Parameters.Add(employeeIDParam);
						SqlParameter firstNameParam = new SqlParameter("@firstname", updateEmployee.FirstName);
						readCommand.Parameters.Add(firstNameParam);
						SqlParameter lastNameParam = new SqlParameter("@lastname", updateEmployee.LastName);
						readCommand.Parameters.Add(lastNameParam);
						SqlParameter emailParam = new SqlParameter("@email", updateEmployee.Email);
						readCommand.Parameters.Add(emailParam);
						SqlParameter phoneNoParam = new SqlParameter("@phoneno", updateEmployee.PhoneNo);
						readCommand.Parameters.Add(phoneNoParam);
						SqlParameter dateOfBirthParam = new SqlParameter("@dateofbirth", updateEmployee.DateOfBirth);
						readCommand.Parameters.Add(dateOfBirthParam);
						SqlParameter WorkplaceIDParam = new SqlParameter("@WorkplaceID", updateEmployee.WorkplaceID);
						readCommand.Parameters.Add(WorkplaceIDParam);

						readCommand.ExecuteNonQuery();
					}
					scope.Complete();
					UpdateEmp = true;

				} catch (TransactionAbortedException ex) {
					Console.WriteLine("Transaction aborted exeption for update employee: {0}", ex.Message);
				}
			}
			return UpdateEmp;
		}

		public Boolean CreateEmployee(Employee createEmployee) {
			Boolean createEmp = false;

			using (TransactionScope scope = new TransactionScope()) {
				try {

					if (EmployeeCount(createEmployee.WorkplaceID)) {
						String queryString = "INSERT INTO EMPLOYEE(firstname, lastname, email, phoneNo, dateofbirth, WorkplaceID) VALUES ( @firstname, @lastname, @email, @phoneNo, @dateofbirth, @WorkplaceID)";
						using (SqlConnection con = new SqlConnection(connectionString))
						using (SqlCommand readCommand = new SqlCommand(queryString, con)) {
							con.Open();

							SqlParameter firstNameParam = new SqlParameter("@firstname", createEmployee.FirstName);
							readCommand.Parameters.Add(firstNameParam);
							SqlParameter lastNameParam = new SqlParameter("@lastname", createEmployee.LastName);
							readCommand.Parameters.Add(lastNameParam);
							SqlParameter emailParam = new SqlParameter("@email", createEmployee.Email);
							readCommand.Parameters.Add(emailParam);
							SqlParameter phoneNoParam = new SqlParameter("@phoneno", createEmployee.PhoneNo);
							readCommand.Parameters.Add(phoneNoParam);
							SqlParameter dateOfBirthParam = new SqlParameter("@dateofbirth", createEmployee.DateOfBirth);
							readCommand.Parameters.Add(dateOfBirthParam);
							SqlParameter WorkplaceIDParam = new SqlParameter("@WorkplaceID", createEmployee.WorkplaceID);
							readCommand.Parameters.Add(WorkplaceIDParam);

							readCommand.ExecuteNonQuery();
						}
						scope.Complete();
						createEmp = true;
					}

				} catch (TransactionAbortedException ex) {
					Console.WriteLine("Transaction aborted exeption for create employee Msg: {0}", ex.Message);
				}
			}
			return createEmp;
		}

		public Boolean DeleteEmployee(int employeeId) {
			Boolean DeleteEmp = false;
			using (TransactionScope scope = new TransactionScope()) {
				try {
					string queryString = "DELETE FROM EMPLOYEE WHERE employeeId = @employeeId";
					using (SqlConnection con = new SqlConnection(connectionString))
					using (SqlCommand readCommand = new SqlCommand(queryString, con)) {
						SqlParameter empId = new SqlParameter("@employeeId", employeeId);
						readCommand.Parameters.Add(empId);
						con.Open();
						readCommand.ExecuteNonQuery();

					}
					DeleteEmp = true;
					scope.Complete();
				} catch (TransactionAbortedException ex) {
					Console.WriteLine("Transaction aborted exeption for delete employee: {0}", ex.Message);
				}
			}

			return DeleteEmp;
		}

		public Boolean EmployeeCount(int workplaceId) {

			int count = 0;
			Boolean roomForEmployee = false;
			try {
				string queryString = "SELECT COUNT(*) FROM WORKPLACE WHERE id = @workplaceId AND MaxOccupancy > (SELECT COUNT(*) FROM EMPLOYEE WHERE WorkplaceID = @workplaceId)";
				using (SqlConnection con = new SqlConnection(connectionString))
				using (SqlCommand readCommand = new SqlCommand(queryString, con)) {
					SqlParameter empId = new SqlParameter("@workplaceId", workplaceId);
					readCommand.Parameters.Add(empId);
					con.Open();
					readCommand.ExecuteNonQuery();

					count = (Int32) readCommand.ExecuteScalar();
					roomForEmployee = Convert.ToBoolean(count);
				}
			} 
			catch (HttpRequestException ex) {
				Console.WriteLine(ex);
			}
			return roomForEmployee;
		}
	}
}