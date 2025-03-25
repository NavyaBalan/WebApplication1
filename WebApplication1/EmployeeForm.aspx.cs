using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class EmployeeForm : System.Web.UI.Page
    {
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string empName = txtEmployeeName.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string department = txtDepartment.Text;

            List<Dependent> dependents = new List<Dependent>();

            // Retrieve dependent details from Request.Form
            string[] dependentNames = Request.Form.GetValues("DependentName");
            string[] relationships = Request.Form.GetValues("Relationship");
            string[] ages = Request.Form.GetValues("Age");

            if (dependentNames != null)
            {
                for (int i = 0; i < dependentNames.Length; i++)
                {
                    dependents.Add(new Dependent
                    {
                        DependentName = dependentNames[i],
                        Relationship = relationships[i],
                        Age = int.Parse(ages[i])
                    });
                }
            }

            SaveEmployeeWithDependents(empName, email, phone, department, dependents);
        }

        private void SaveEmployeeWithDependents(string name, string email, string phone, string department, List<Dependent> dependents)
        {
            string connStr = ConfigurationManager.ConnectionStrings["EmployeeDbContext"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert employee
                    string empQuery = "INSERT INTO Employees (EmployeeName, Email, Phone, Department) OUTPUT INSERTED.EmployeeID VALUES (@Name, @Email, @Phone, @Department)";
                    SqlCommand empCmd = new SqlCommand(empQuery, conn, transaction);
                    empCmd.Parameters.AddWithValue("@Name", name);
                    empCmd.Parameters.AddWithValue("@Email", email);
                    empCmd.Parameters.AddWithValue("@Phone", phone);
                    empCmd.Parameters.AddWithValue("@Department", department);

                    int employeeID = (int)empCmd.ExecuteScalar(); // Get generated EmployeeID

                    // Insert dependents
                    string depQuery = "INSERT INTO Dependents (EmployeeID, DependentName, Relationship, Age) VALUES (@EmployeeID, @DependentName, @Relationship, @Age)";
                    foreach (var dep in dependents)
                    {
                        SqlCommand depCmd = new SqlCommand(depQuery, conn, transaction);
                        depCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        depCmd.Parameters.AddWithValue("@DependentName", dep.DependentName);
                        depCmd.Parameters.AddWithValue("@Relationship", dep.Relationship);
                        depCmd.Parameters.AddWithValue("@Age", dep.Age);
                        depCmd.ExecuteNonQuery();
                    }

                    transaction.Commit(); // Commit transaction
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback in case of failure
                    throw new Exception("Error saving data: " + ex.Message);
                }
            }
        }
    }

    public class Dependent
    {
        public string DependentName { get; set; }
        public string Relationship { get; set; }
        public int Age { get; set; }
    }
}
