<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="SkybrarySearchPrototype.search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtSearch" runat="server" Width="400px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Search" Width="120px" 
            onclick="Button1_Click" />
        <hr />
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <div id="htmlContainer" runat="server">
        </div>
    </div>
    </form>
</body>
</html>
