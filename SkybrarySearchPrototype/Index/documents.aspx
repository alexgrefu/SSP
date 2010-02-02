<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Documents.aspx.cs" Inherits="SkybrarySearchPrototype.Documents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="htmlContainer" runat="server">
        
    </div>
    
    <h1>Documents List</h1>
    
    <div>
        <asp:ListView ID="lvDocuments" runat="server" DataKeyNames="Id">
        <LayoutTemplate>
            <table>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th align="center">Action</th>
                    </tr>              
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td style="width:70%"><%# Eval("Title") %></td>
                <td align="center" style="width:30%"><a href='viewdocument.aspx?id=<%# Eval("Id") %>'>view</a> | <a href='indexdocument.aspx?id=<%# Eval("Id") %>'>index</a> </td>
            </tr>
        </ItemTemplate>
     </asp:ListView>
    </div>
    </form>
</body>
</html>
