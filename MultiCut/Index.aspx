<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MultiCut.WebForm1" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="js/Index.js"></script>
    <link href="css/Index.css" rel="stylesheet" />
    <title></title>    
</head>
<body>
    <asp:Label ID="HalNavn" runat="server" Text="Label" Width="100%"></asp:Label>
    <asp:Table ID="Table1" runat="server" Width="100%" GridLines="Horizontal"></asp:Table>
</body>
</html>
