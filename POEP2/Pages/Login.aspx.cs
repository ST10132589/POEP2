using Microsoft.Ajax.Utilities;
using POEP2.Classes;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POEP2.Pages
{
    public partial class Login : System.Web.UI.Page
    {

        /// <summary>
        /// Event handler for the Page_Load event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.ForeColor = System.Drawing.Color.ForestGreen;
            lblErrorMessage.Text = "Check README For Login Details :)";
        }

        /// <summary>
        /// Event handler for the LoginButton_Click event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected async void LoginButton_Click(object sender, EventArgs e)
        {
            // Check if the username or password fields are empty
            if (txtUsername.Text.IsNullOrWhiteSpace() || txtPassword.Text.IsNullOrWhiteSpace())
            {
                // If either field is empty, display an error message and clear both fields
                lblErrorMessage.ForeColor = System.Drawing.Color.Red;
                lblErrorMessage.Text = "Incorrect Username or Password.";
                txtUsername.Text = "";
                txtPassword.Text = "";
            }
            else
            {
                // If both fields are filled, attempt to log the user in
                await LoginUserAsync();
            }
        }

        /// <summary>
        /// Asynchronously logs in the user.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoginUserAsync()
        {
            // Retrieve the values entered by the user
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["AgriConnectDB"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the SqlConnection.
                    await connection.OpenAsync();

                    // Prepare the SQL query.
                    string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the parameters.
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        // Execute the query and get the result.
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    User user = new User
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Username = reader.GetString(reader.GetOrdinal("Username")),
                                        IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin"))
                                    };
                                    Session["user"] = user;
                                    System.Diagnostics.Debug.WriteLine("User stored in session: " + user.Username); 
                                }
                                lblErrorMessage.Text = "";
                                Response.Redirect("~/Pages/Products.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                // The username and password are incorrect.
                                lblErrorMessage.ForeColor = System.Drawing.Color.Red;
                                lblErrorMessage.Text = "Invalid username or password.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception.
                // You can log the exception or display a generic error message to the user.
                lblErrorMessage.ForeColor = System.Drawing.Color.Red;
                lblErrorMessage.Text = "An error occurred. Please try again later.";
            }
        }
    }
}
