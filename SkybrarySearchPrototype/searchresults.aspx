<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" 
    AutoEventWireup="true" CodeBehind="searchresults.aspx.cs" 
    Inherits="SkybrarySearchPrototype.searchresults" 
    EnableViewState="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<script type="text/javascript">

    $(document).ready(function() {
        // set up the jquery ui question dialog
        $('div[id$=TextViewerDialog]').dialog({
            autoOpen: false,
            modal: true,
            width: 600,
            resizable: false,
            title: "Skybrary Quick Text Viewer"
        });


        $('#spanTooltip').easyTooltip({
            useElement: "ctl00_cphMain_tooltipItem"
        });
    });

    function ShowDialog(id, type) {

        $('div[id$=dialogContainer]').html("<h3>Loading...</h3>");
        
        //alert(id + " " + type);
        SkybrarySearchPrototype.Services.SearchService.GetTextElementContent(id, type, OnGetContentSucces, OnServiceFailure);
        
        $('div[id$=TextViewerDialog]').dialog('open');

    }

    function ShowNext(id, type) {
        SkybrarySearchPrototype.Services.SearchService.GetTextElementContent(id, type, OnGetContentSucces, OnServiceFailure);
    }

    function OnGetContentSucces(result) {
        $('div[id$=dialogContainer]').html(result.Content);
        $('div[id$=TextViewerDialog]').dialog('option', 'position', 'center');

    }

    function OnServiceFailure(result, context, methodName) {
        alert(methodName + ": Call failed! " + result.get_message());
    }


</script>

<div>
    <h3 style="color:#008829" runat="server" id="infoHeader"></h3>
    <p runat="server" id="filterText"></p>
</div>
    

<act:Accordion ID="accSearchResults" runat="server" TransitionDuration="1" >
<HeaderTemplate >
    <div class="search-result-header">
        <img src="Assets/Images/doc_down.png" />
        <b><%# Eval("Title") %> (Hits: <%# Eval("Count") %>)</b>  
    </div>
</HeaderTemplate>

<ContentTemplate>
  <div class="search-result-content">
        <asp:ListView ID="lvSearchForContents" runat="server" DataSource='<%# Eval("Hits") %>' ItemPlaceholderID="lvHitsPlaceHolder">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="lvHitsPlaceHolder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="border-bottom:dashed 1px green" >
                    <p id="P1" runat="server" visible='<%# Eval("Type").ToString() == "paragraph" ? true : false %>'>
                        <b><%# Eval("Chapter") %></b>
                    </p>
                    
                    <p>
                        <a href='javascript:ShowDialog(<%# Eval("Id") %>, "<%# Eval("Type") %>")'><b><%# Eval("Title") %></b></a>
                    </p>
                    <p>
                        <%# Eval("Content") %>
                    </p>
                    <p><a href='javascript:ShowDialog(<%# Eval("Id") %>, "<%# Eval("Type") %>")'>[quick view]</a> <a href='documentview.aspx?id=<%# Eval("Id") %>&type=<%# Eval("Type") %>'>[in document view]</a></p>
                </div>
            </ItemTemplate>
        </asp:ListView>      
    </div>   
    
</ContentTemplate>
</act:Accordion>

<div runat="server" id="tooltipItem">  
    
</div>


<div id="TextViewerDialog">
    <div id="dialogContainer">
    </div>
</div>


</asp:Content>
