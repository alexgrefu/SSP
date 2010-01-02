﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" 
    AutoEventWireup="true" CodeBehind="skysearch.aspx.cs" Inherits="SkybrarySearchPrototype.skysearch" 
    EnableViewState="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<script type="text/javascript">

    Array.prototype.getIndex=function(v)
    {
        for (i=0; i<this.length; i++)
        {
            if (this[i]==v) return i;
        }
        return -1;
    }

    Array.prototype.toHiddenString = function() {
        var t = "";
        for (i = 0; i < this.length; i++) {
            if (this[i].length > 0) {
                if (i == this.length - 1)
                    t += this[i];
                else
                    t += this[i] + "|";
            }
        }

        return t;
    }

    Array.prototype.toTooltip = function() {
        var t = "<h3>Filter by document(s):</h3>";
        if (this.length > 0) {
            t += "<ul>";
            for (i = 0; i < this.length; i++) {
                if (this[i].length > 0) {
                    t += "<li>" + this[i] + "</li>";
                }
            }
            t += "</ul>";
        }
        else {
            t += "<p>none</p>";
        }
        return t;
    }
    
    var actualFilterList = new Array();
    var userFilterList = new Array();
    $(document).ready(function() {
        $('#lnkDocuments').easyTooltip({
        useElement: "tooltipItem"
        });
    });

    function ClientNodeChecked(sender, eventArgs) {
        
        var node = eventArgs.get_node();

        var level = node.get_level();
        var checked = node.get_checked();
        var childCount = node.get_nodes().get_count();

        //alert(childCount);

        if (level == 0) {
            if (checked) {
                if (node.get_text() != "Convention on International Civil Aviation") {

                    var nodes = node.get_nodes();
                    for (x = 0; x < childCount; x++) {

                        var idx = userFilterList.getIndex(nodes.getNode(x).get_text());
                        if (idx == -1) {
                        
                            userFilterList.push(nodes.getNode(x).get_text());
                            var items = GetDocuments(nodes.getNode(x));
                            while (items.length > 0) {
                                actualFilterList.push(items.pop());
                            }
                        }
                    }
                }
                else {
                    actualFilterList.push(node.get_text());
                    userFilterList.push(node.get_text());
                }
            }
            else {
                if (node.get_text() != "Convention on International Civil Aviation") {
                    var nodes = node.get_nodes();
                    for (x = 0; x < childCount; x++) {

                        var idx = userFilterList.getIndex(nodes.getNode(x).get_text());
                        userFilterList.splice(idx, 1);
                        var items = GetDocuments(nodes.getNode(x));
                        while (items.length > 0) {
                            var index = actualFilterList.getIndex(items.pop());

                            actualFilterList.splice(index, 1);


                        }
                    }
                }
                else {
                    var index = actualFilterList.getIndex(node.get_text());
                    var idx = userFilterList.getIndex(node.get_text());
                    actualFilterList.splice(index, 1);
                    userFilterList.splice(idx, 1);
                }
            }
        }
        else if (level == 1) {
            if (checked) {
                userFilterList.push(node.get_text());
                var items = GetDocuments(node);
                while (items.length > 0) {
                    actualFilterList.push(items.pop());
                }
            }
            else {
                var idx = userFilterList.getIndex(node.get_text());
                userFilterList.splice(idx, 1);
                var items = GetDocuments(node);
                while (items.length > 0) {
                    var index = actualFilterList.getIndex(items.pop());
                    actualFilterList.splice(index, 1);
                }
            }
        }
        WriteArrayToHiddenField(actualFilterList, userFilterList);
    }

    function GetDocuments(node) 
    {
        var attributes = node.get_attributes();

        var docs = attributes.getAttribute("filter");
        var arr = docs.split(";");
        return arr.reverse();
    }

    function WriteArrayToHiddenField(actual, user) {
        //alert(actual.toTooltip());
        $('#tooltipItem').html(user.toTooltip());
        $('input[id$=txtActualFilter]').val(actual.toHiddenString());

        //alert($('input[id$=txtActualFilter]').val());
    }

    function ClearAll() {
        var tree = $find("<%= rtvDocumentIndex.ClientID %>");
        
        for (var i = 0; i < tree.get_nodes().get_count(); i++) {
            var node = tree.get_nodes().getNode(i);
            node.uncheck();
        }

        actualFilterList.splice(0, actualFilterList.length);
        userFilterList.splice(0, userFilterList.length);

        WriteArrayToHiddenField(actualFilterList, userFilterList);
    }
    

    
</script>

<h2><a id="lnkDocuments" href="#">ICAO Documents Index</a></h2>
<p>
    Use the following list to filter you query based on one or more documents. 
    If no document is selected, than the search will be performed in <b>all documents</b>.
    Hover over the link above to see any filter that are in effect.
</p>
<p>
    <a href="javascript:ClearAll()">[clear selection]</a>
</p>
<telerik:RadTreeView 
        ID="rtvDocumentIndex" 
        runat="server" CheckBoxes="True" 
        CheckChildNodes="True" Skin="Vista" 
        OnClientNodeChecked="ClientNodeChecked">        
</telerik:RadTreeView>


<asp:HiddenField ID="txtActualFilter" runat="server" />

<div id="tooltipItem">  
    <h3>Filtered documents:</h3>
    <p>
        none
    </p>  
</div>

</asp:Content>
