<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="GestionFacturas.Website.Views.Error._500" %>
<% Response.StatusCode = 500; %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>500 Error</title>
</head>
<body>
    <form id="form1" runat="server">
   <div class="white_content">
    <div class="content-wrapper">
        
        <h1>Error</h1>
        <h2 class="text-danger">  
        Se ha producido un error mientras se procesaba la petición, disculpa les molestias.
        </h2>
         
             <p><a href="/">Volver a la página principal</a></p>
       
       
    </div>

</div>
    </form>
</body>
</html>
