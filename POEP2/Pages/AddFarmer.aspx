<%@ Page Title="Add Farmer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddFarmer.aspx.cs" Inherits="POEP2.Pages.AddFarmer" Async="true" %>

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
        }

        .navbar {
             display: none;
        }

        .login-form {
            background-color: #2a2a2a;
            width: 300px;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
            text-align: center;
            margin: 0 auto; /* Added to center the login form */
        }

        .login-form h1 {
            margin-bottom: 20px;
            align-content: center;
            text-align:center;
        }

        .login-form input[type="text"],
        .login-form input[type="password"] {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border: 1px solid #555;
            border-radius: 5px;
            background-color: #333;
            color: white;
        }

        .login-form input[type="submit"] {
            background-color: #007bff;
            border: none;
            color: white;
            padding: 10px 20px;
            margin-top: 10px;
            border-radius: 5px;
            cursor: pointer;
            width: 100%;
        }

        .login-form .error-message {
            color: red;
            margin-top: 10px;
            width: 100%;
        }

        .footer {
            margin-top: 20px;
            font-size: 12px;
        }
    </style>
    <h1 style="text-align: center;">Add User</h1>
    <div class="login-form">
        <asp:TextBox ID="txtUsername" runat="server" placeholder="Username" CssClass="form-control" />
        <br />
        <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" TextMode="Password" CssClass="form-control" />
        <br />
        <asp:RadioButton ID="rbAddEmployee" runat="server" Text="Add Employee" GroupName="UserType" />
        <br />
        <asp:RadioButton ID="rbAddFarmer" runat="server" Text="Add Farmer" GroupName="UserType" Checked="true" />
        <br />
        <asp:Button ID="btnAddFarmer" runat="server" Text="Add User" CssClass="btn btn-primary" OnClick="btnAddFarmer_Click" />
        <br />
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" />
        <br />
        <asp:Button ID="btnBackToProducts" runat="server" Text="Back to Products" CssClass="btn btn-secondary" OnClick="btnBackToProducts_Click" OnClientClick="return confirm('Are you sure you want to go back to the Products page?');" />
        <br />
    </div>
</asp:Content>