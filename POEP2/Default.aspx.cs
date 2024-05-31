using POEP2.Classes;
using System;
using System.Web.UI;

namespace POEP2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null)
            {
                Response.Redirect("~/Pages/Login.aspx");
            }
            else
            {
                Response.Redirect("~/Pages/Products.aspx");
            }
        }
    }
}