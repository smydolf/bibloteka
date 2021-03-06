﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{

    public class Employees
    {
        public List<Employee> EmployeeList { get; set; }

        public Employee GetEmployee(int employeeId)
        {
            Employee e = new Employee();
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"GetEmployeeDetails2";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter p1 = new SqlParameter("businessEntityId", System.Data.SqlDbType.Int);
                    p1.Value = employeeId;
                    cmd.Parameters.Add(p1);
                    SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        e.Load(reader);
                    }
                }
            }
            return e;
        }


        public Employee GetEmployeeDONOTCall(int employeeId)
        {
            Employee e = new Employee();

            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select * from HumanResources.Employee E
			JOIN Person.Person P ON E.BusinessEntityId = P.BusinessEntityID AND P.PersonType = 'EM'
			JOIN HumanResources.EmployeeDepartmentHistory EH ON E.BusinessEntityId = EH.BusinessEntityId
			JOIN HumanResources.Department D ON D.DepartmentId = Eh.DepartmentId

			where
			    E.BusinessEntityId = {0}";
                    cmd.CommandText = string.Format(cmd.CommandText, employeeId.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        e.Load(reader);
                    }
                }
            }
            return e;
        }

    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string JobTitle { get; set; }

        public void Load(SqlDataReader reader)
        {
            EmployeeId = Int32.Parse(reader["BusinessEntityId"].ToString());
            FirstName = reader["FirstName"].ToString();
            LastName = reader["LastName"].ToString();
            DepartmentId = Int32.Parse(reader["DepartmentId"].ToString());
            JobTitle = reader["JobTitle"].ToString();
        }


    }
}