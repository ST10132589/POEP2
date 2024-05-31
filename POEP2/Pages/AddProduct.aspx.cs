using POEP2.Classes;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;

namespace POEP2.Pages
{
    public partial class AddProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User from the session
            User user = (User)Session["user"];
            lblErrorMessage.ForeColor = Color.Red;

            // Check if the user is null
            if (user == null)
            {
                // If the user is null, redirect to the Login page
                Response.Redirect("~/Pages/Login.aspx");
            }
            else if (user.IsAdmin)
            {
                // If the user is an admin, redirect to the Products page
                Response.Redirect("~/Pages/Products.aspx");
            }
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            // Get the User from the session
            User user = (User)Session["user"];

            // Create a new Product
            Product product = new Product
            {
                Username = user.Username,
                Name = txtName.Text,
                Price = double.Parse(txtPrice.Text),
                Description = txtDescription.Text,
                prodDate = DateTime.Parse(txtProdDate.Text)
            };

            // Validate the product
            if (!ValidateProduct(product))
            {
                // If the product is not valid, return and do not add it to the database
                return;
            }

            // Add the product to the database
            AddProductToDatabase(product);

            // Clear the input fields
            txtName.Text = "";
            txtPrice.Text = "";
            txtDescription.Text = "";
            txtProdDate.Text = "";

            // Optionally, show a success message
            lblErrorMessage.ForeColor = Color.ForestGreen;
            lblErrorMessage.Text = "Product added successfully!";
            Response.Redirect("~/Pages/Products");
        }

        private void AddProductToDatabase(Product product)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AgriConnectDB"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the SqlConnection.
                    connection.Open();

                    // Prepare the SQL query.
                    string query = "INSERT INTO Products (Username, Name, Price, Description, prodDate) VALUES (@Username, @Name, @Price, @Description, @prodDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the parameters.
                        command.Parameters.AddWithValue("@Username", product.Username);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@prodDate", product.prodDate);

                        // Execute the query.
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception.
                // You can log the exception or display a generic error message to the user.
                lblErrorMessage.Text = "An error occurred while adding the product. Please try again later.";
            }
        }

        private bool ValidateProduct(Product product)
        {
            // Check if the Name is null or empty
            if (string.IsNullOrEmpty(product.Name))
            {
                lblErrorMessage.Text = "Product Name is required.";
                return false;
            }

            // Check if the Price is less than or equal to zero
            if (product.Price <= 0)
            {
                lblErrorMessage.Text = "Product Price must be greater than zero.";
                return false;
            }

            // Check if the Description is null or empty
            if (string.IsNullOrEmpty(product.Description))
            {
                lblErrorMessage.Text = "Product Description is required.";
                return false;
            }

            // Check if the prodDate is a future date
            if (product.prodDate > DateTime.Now)
            {
                lblErrorMessage.Text = "Product Date cannot be a future date.";
                return false;
            }

            // If all checks pass, return true
            return true;
        }

    }
}