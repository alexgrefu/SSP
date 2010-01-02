<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SkybrarySearchPrototype.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div style="width:600px; margin:0 auto;">
    <table style="margin-top:100px; margin-bottom:100px; border:solid 1px black;">
        <tr>
            <td valign="top">
                <img src="Assets/Images/error.png" width="48" alt="error" style="margin: 5px;" />
            </td>
            <td>
                <h3>Well, this is embarrassing...</h3>
                <p>
                    The developers were notified about this error. 
                </p>
                <p>
                    Please remember that this is an early beta software and bugs are to be expected. Please try to 
                    write down exactly what were you doing when this error occured. This might help the developers to track down the bug 
                    faster.
                </p>
                <p>
                    We appologize for the inconvenience this might have cause you and we'll begin investigating the issue immediatly. 
                </p>
            </td>
        </tr>
    </table>
    
    
</div>
</asp:Content>
