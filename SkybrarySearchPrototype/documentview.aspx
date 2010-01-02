<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" 
         AutoEventWireup="true" CodeBehind="documentview.aspx.cs" 
         Inherits="SkybrarySearchPrototype.documentview" EnableViewState="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript">

function pageLoad()
{
    var tree = $find("<%= rtvDocument.ClientID %>");
   var selectedNode = tree.get_selectedNode()
   if (selectedNode != null)
   {
       window.setTimeout(function() { selectedNode.scrollIntoView(); }, 200);
   }
   var attributes = selectedNode.get_attributes();
   //alert(attributes.getAttribute("t"));
   LoadTextElement(selectedNode.get_value(), attributes.getAttribute("t"));
}

function LoadTextElement(id, type) {
    $('#textContent').html("<b>Loading...</b>");
    SkybrarySearchPrototype.Services.SearchService.GetTextElementForDocument(id, type, OnGetContentSucces, OnServiceFailure);
}

function OnGetContentSucces(result) {
    $('#textContent').html(result.Content);
}

function OnServiceFailure(result, context, methodName) {
    alert(methodName + ": Call failed! " + result.get_message());
}

function ClientNodeClicked(sender, eventArgs) {
    var node = eventArgs.get_node();

    if (node.get_level() == 0) {
        LoadTextElement(node.get_value(), "chapter");
    }
    else {
        LoadTextElement(node.get_value(), "paragraph");
    }
    //alert("You clicked " + node.get_text());
}


</script>

<h3 runat="server" id="headerTitle"></h3>
<p><a href="#" onclick="history.back()">Back to search results</a></p>
<div style="width:878px; padding:10px; border:solid 1px black;">
    <div style="width:250px; height:550px; overflow:auto; float:left; border-right:solid 2px black;">
        <telerik:RadTreeView ID="rtvDocument" runat="server" OnClientNodeClicked="ClientNodeClicked"
            Skin="Web20" 
            Font-Size="10px" 
            Font-Names="Arial,Verdana", Width="248px" Height="550px" style="white-space:normal">
        </telerik:RadTreeView>
    </div> 
    <div id="textContent" style="width:586; padding:10px; height:550px; overflow:auto">
        <b>Loading...</b>
    </div> 
</div>
</asp:Content>
