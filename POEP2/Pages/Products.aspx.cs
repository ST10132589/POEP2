using POEP2.Classes;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace POEP2.Pages
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                User user = (User)Session["user"];
                System.Diagnostics.Debug.WriteLine("User retrieved from session: " + (user != null ? user.Username : "null")); // Debug line


                // Check if the User session is null
                if (user == null)
                {
                    // Redirect to Login.aspx
                    Response.Redirect("~/Pages/Login.aspx");
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                string username = user.Username;
                bool isAdmin = user.IsAdmin;
                UsernameLabel.Text = "Logged in as: " + username;

                if (!IsPostBack)
                {
                    if (!isAdmin)
                    {
                        // Only select products that belong to the current user
                        SqlDataSource1.SelectCommand = "SELECT * FROM [Products] WHERE Username = @username";
                        SqlDataSource1.SelectParameters.Clear();
                        SqlDataSource1.SelectParameters.Add("username", user.Username);
                        SqlDataSource1.DataBind();

                        AddProductButton.Visible = true;
                        UserDropDownList.Visible = false;
                    }
                    else
                    {
                        LoadUsers();
                        UserLabel.Visible = true;
                        UserDropDownList.Visible = true;
                        AddProductButton.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                }
            }
        }


        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            // Redirect to the Add Product page
            Response.Redirect("AddProduct.aspx");
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            string username = user.Username;
            bool isAdmin = user.IsAdmin;

            string startDate = StartDateFilter.Text;
            string endDate = EndDateFilter.Text;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime start, end;
                if (DateTime.TryParse(startDate, out start) && DateTime.TryParse(endDate, out end))
                {
                    if (start > DateTime.Now || end > DateTime.Now)
                    {
                        DateErrorMessage.Text = "Dates cannot be in the future.";
                        return;
                    }

                    if (end >= start)
                    {
                        SqlDataSource1.SelectParameters.Clear();

                        if (!isAdmin)
                        {
                            SqlDataSource1.SelectCommand = "SELECT * FROM [Products] WHERE prodDate BETWEEN @startDate AND @endDate AND Username = @username";
                            SqlDataSource1.SelectParameters.Add("username", username);
                        }
                        else
                        {
                            SqlDataSource1.SelectCommand = "SELECT * FROM [Products] WHERE prodDate BETWEEN @startDate AND @endDate";
                        }

                        SqlDataSource1.SelectParameters.Add("startDate", start.ToString("yyyy-MM-dd"));
                        SqlDataSource1.SelectParameters.Add("endDate", end.ToString("yyyy-MM-dd"));
                        SqlDataSource1.DataBind();
                        ProductsGridView.DataBind();

                        if (ProductsGridView.Rows.Count == 0)
                        {
                            NoDataLabel.Text = "No items found for the selected dates.";
                        }
                        else
                        {
                            NoDataLabel.Text = "";
                        }
                        DateErrorMessage.Text = "";
                    }
                    else
                    {
                        // Handle end date before start date
                        DateErrorMessage.Text = "End date must be after start date.";
                    }
                }
                else
                {
                    // Handle invalid date format
                    DateErrorMessage.Text = "Invalid date format.";
                }
            }
            else
            {
                // Handle no date selected
                DateErrorMessage.Text = "Please select both start and end dates.";
            }
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            string username = user.Username;
            bool isAdmin = user.IsAdmin;

            // Clear the date filters
            StartDateFilter.Text = "";
            EndDateFilter.Text = "";
            SqlDataSource1.SelectParameters.Clear();
            // Show all items again
            if (!isAdmin)
            {
                SqlDataSource1.SelectCommand = "SELECT * FROM [Products] WHERE Username = @username";
                SqlDataSource1.SelectParameters.Add("username", username);
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT * FROM [Products]";
            }

            SqlDataSource1.DataBind();
            ProductsGridView.DataBind();

            // Clear any error messages
            DateErrorMessage.Text = "";
            NoDataLabel.Text = "";
        }

        private void LoadUsers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AgriConnectDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Username FROM Users WHERE IsAdmin = 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        UserDropDownList.DataSource = reader;
                        UserDropDownList.DataTextField = "Username";
                        UserDropDownList.DataValueField = "Username";
                        UserDropDownList.DataBind();
                    }
                }
            }

            if (!IsPostBack)
            {
                UserDropDownList.Items.Insert(0, new ListItem("Any", "Any"));
            }
        }
        protected void UserDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUser = UserDropDownList.SelectedValue;
            Session["SelectedUser"] = selectedUser;
            FilterProducts(selectedUser);
        }
        private void FilterProducts(string selectedUser)
        {
            SqlDataSource1.SelectParameters.Clear();

            if (selectedUser == "Any")
            {
                SqlDataSource1.SelectCommand = "SELECT * FROM [Products]";
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT * FROM [Products] WHERE Username = @username";
                SqlDataSource1.SelectParameters.Add("username", selectedUser);
            }

            SqlDataSource1.DataBind();
            ProductsGridView.DataBind();
        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Redirect the user to the Login page
            Response.Redirect("~/Pages/Login.aspx");
        }
    }
}