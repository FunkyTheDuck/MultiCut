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


    <form runat="server">
        <div class="row">
            <div class="col-5"></div>
            <asp:DropDownList class="col-1" ID="HalNavnBox" runat="server" OnSelectedIndexChanged="HalNavnBox_SelectedIndexChanged"  AutoPostBack="true"/>
            <asp:DropDownList class="col-1" ID="AfdelingNavnBox" runat="server" OnSelectedIndexChanged="AfdelingNavnBox_SelectedIndexChanged"  AutoPostBack="true"/>
            <div class="col-5"></div>
        </div>
        <asp:Table ID="Table1" runat="server"  GridLines="Horizontal"></asp:Table>
    </form>
</body>
</html>
