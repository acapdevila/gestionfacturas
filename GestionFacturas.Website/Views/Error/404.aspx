<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="GestionFacturas.Website._404" %>

<% Response.StatusCode = 404; %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <title>404 Página no encontrada
    </title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="white_content">
            <div class="content-wrapper">
                <h1>404 La página solicitada no existe (aspx).</h1>
                <p><a href="/">Volver a la página principal</a></p>
            </div>
        </div>
    </form>
</body>
</html>

