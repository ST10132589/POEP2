
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="POEP2.Pages.Products" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
             background-color: #1a1a1a;
             color: white;
             font-family: Arial, sans-serif;
             display: flex;
             justify-content: center;
             align-items: center;
             height: 100vh;
             margin: 0;
             flex-direction: column;
             width:100%;
         }

        .navbar {
             position: fixed;
             top: 0;
             left: 0;
             width: 100%;
             display: flex;
             justify-content: space-between;
             align-items: center;
             padding: 10px 20px;
             background-color: #2a2a2a;
             box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
             z-index: 1;
         }

        .products-form {
             background-color: #2a2a2a;
             width: 100%; /* Adjusted width from 80% to 100% */
             max-width: none; /* Removed the max-width constraint */
             padding: 20px;
             border-radius: 10px;
             box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
             text-align: center;
             margin: 0 auto; /* Added to center the products form */
         }

        .products-form h1 {
            margin-bottom: 20px;
            align-content: center;
            text-align:center;
        }

        .products-form input[type="text"],
        .products-form input[type="date"] {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border: 1px solid #555;
            border-radius: 5px;
            background-color: #333;
            color: white;
        }

        .products-form input[type="submit"] {
            background-color: #007bff;
            border: none;
            color: white;
            padding: 10px 20px;
            margin-top: 10px;
            border-radius: 5px;
            cursor: pointer;
            width: 100%;
        }

        .products-form .error-message {
            color: red;
            margin-top: 10px;
            width: 100%;
        }

        .footer {
            margin-top: 20px;
            font-size: 12px;
        }

        .products-form .grid-view {
            width: 100%;
            margin: 0 auto;
            border-collapse: collapse;
        }

        .products-form .grid-view th, .products-form .grid-view td {
            border: 1px solid #555;
            padding: 10px;
            text-align: left;
        }

        .products-form .grid-view th {
            background-color: #333;
            color: white;
        }

        .products-form .grid-view tr:nth-child(even) {
            background-color: #2a2a2a;
        }

        .products-form .grid-view tr:nth-child(odd) {
            background-color: #1a1a1a;
        }

        .grid-view-container {
             width: 100%;
             max-height: 400px; 
             overflow-y: auto;
             margin-bottom: 20px;
         }
        .filter-container {
            display: flex;
            justify-content: space-between;
            align-items: flex-end; /* Aligns items to the bottom of the container */
            margin-top: 20px;
            width: 100%;
        }

        .my-dropdown {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border: 1px solid #555;
            border-radius: 5px;
            background-color: #333;
            color: white;
        }

        .dropdown-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
        }

         .date-filter-container {
            display: flex;
            justify-content: space-between;
            align-items: center; /* Aligns items to the center of the container */
            margin-top: 20px;
            width: 100%;
        }

        .button-container {
            display: flex;
            justify-content: space-between;
            align-items: center; /* Aligns items to the center of the container */
            width: 100%;
        }

        .logout-section {
            position: fixed;
            bottom: 0;
            left: 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 10px 20px;
            background-color: #2a2a2a;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
            width: 100%;
        }

        .logout-section .username-label {
            color: white;
            margin-right: 10px;
        }

        .logout-section .logout-button {
            background-color: #007bff;
            border: none;
            color: white;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
        }
    </style>


<div class="dropdown-container">
    <asp:Label ID="UserLabel" runat="server" Text="Select User:" Visible="false" />
    <asp:DropDownList ID="UserDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="UserDropDownList_SelectedIndexChanged" CssClass="my-dropdown"></asp:DropDownList>
</div>
<div class="products-form">
    <div class="grid-view-container">
    <asp:GridView ID="ProductsGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID" DataSourceID="SqlDataSource1" CssClass="grid-view">
        <Columns>
            <asp:BoundField DataField="ProductID" HeaderText="Product ID" ReadOnly="True" SortExpression="ProductID" />
            <asp:BoundField DataField="Username" HeaderText="Username" ReadOnly="True" SortExpression="Username" />
            <asp:BoundField DataField="Name" HeaderText="Product Name" SortExpression="Name" />
            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="prodDate" HeaderText="Product Date" SortExpression="prodDate" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
        </Columns>
        </asp:GridView>
    </div>
        <asp:Label ID="NoDataLabel" runat="server" Text="" style="margin-top: 2px;"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AgriConnectDB %>" SelectCommand="SELECT * FROM [Products]"></asp:SqlDataSource>
    <div style="display: flex; justify-content: center; align-items: center; flex-direction: column;">
        <asp:Button ID="AddProductButton" runat="server" Text="Add New Product" OnClick="AddProductButton_Click" style="margin-top: 10px; width: 100%;" Visible ="true" />
        <div class="date-filter-container">
            <div style="display: flex; flex-direction: column; width: 49%;">
                <asp:Label ID="StartDateLabel" runat="server" Text="Start Date:" />
                <asp:TextBox ID="StartDateFilter" runat="server" TextMode="Date" />
            </div>
            <div style="display: flex; flex-direction: column; width: 49%;">
                <asp:Label ID="EndDateLabel" runat="server" Text="End Date:" />
                <asp:TextBox ID="EndDateFilter" runat="server" TextMode="Date" />
            </div>
        </div>
        <asp:Label ID="DateErrorMessage" runat="server" Text="" style="color: red; margin-top: 2px;"></asp:Label>
        <div class="button-container">
            <asp:Button ID="FilterButton" runat="server" Text="Filter" OnClick="FilterButton_Click" style="width: 49%;" />
            <asp:Button ID="ResetButton" runat="server" Text="Reset" OnClick="ResetButton_Click" style="width: 49%;" />
        </div>
    </div>
</div>
<div class="logout-section">
    <asp:Label ID="UsernameLabel" runat="server" CssClass="username-label"></asp:Label>
    <asp:Button ID="LogoutButton" runat="server" Text="Logout" OnClick="LogoutButton_Click" CssClass="logout-button" />
</div>
</asp:Content>