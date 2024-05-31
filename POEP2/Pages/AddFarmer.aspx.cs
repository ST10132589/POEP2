using POEP2.Classes;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace POEP2.Pages
{
    public partial class AddFarmer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cast the Session["user"] to User object
            User currentUser = (User)Session["user"];
            if  (currentUser == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else if (currentUser.IsAdmin != true)
            {
                Response.Redirect("~/Pages/Products.aspx");
            }
        }

        protected void btnAddFarmer_Click(object sender, EventArgs e)
        {
            // Create a new User object
            User newUser = new User
            {
                Username = txtUsername.Text,
                Password = txtPassword.Text,
                IsAdmin = rbAddEmployee.Checked // Set IsAdmin to true if the "Add Employee" radio button is checked
            };

            // Validate the newUser
            if (!ValidateUser(newUser))
            {
                // If the newUser is not valid, return and do not add it to the database
                return;
            }

            // Add the newUser to the database
            AddUserToDatabase(newUser);

            // Clear the input fields
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            rbAddEmployee.Checked = false; // Clear the "Add Employee" radio button
            rbAddFarmer.Checked = true; // Set the "Add Farmer" radio button to checked
        }

        private void AddUserToDatabase(User user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AgriConnectDB"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the SqlConnection.
                    connection.Open();

                    // Prepare the SQL query.
                    string query = "INSERT INTO Users (Username, Password, IsAdmin) VALUES (@Username, @Password, @IsAdmin)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the parameters.
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

                        // Execute the query.
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "An error occurred while adding the Farmer. Please try again.";
            }
        }

        private bool ValidateUser(User user)
        {
            // Check if the Username is null or empty
            if (string.IsNullOrEmpty(user.Username))
            {
                // lblErrorMessage.Text = "Username is required.";
                return false;
            }

            // Check if the Password is null or empty
            if (string.IsNullOrEmpty(user.Password))
            {
                // lblErrorMessage.Text = "Password is required.";
                return false;
            }

            // If all checks pass, return true
            return true;
        }
        protected void btnBackToProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("Products.aspx");
        }
    }
}