<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SmartService.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="listAccess" runat="server">

 <div id="container">
 <p>Hola Como estas</p>
</div>
<a onclick="add()">Add</a>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js">
    function add() {
        $("container").append("<p>Hola Mundo</p>");
    }
</script>
</asp:Content>

