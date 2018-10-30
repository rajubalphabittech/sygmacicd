<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_AjaxData" Codebehind="AjaxData.ascx.cs" %>
<script language="javascript">

    function AjaxFunction(){
        if (xmlHttp = GetXmlHttpObject()){
            xmlHttp.onreadystatechange = function() {
                if (xmlHttp.readyState==4){
                    document.getElementById('ajaxStuff').innerHTML = xmlHttp.responseText;
                }
            }
            xmlHttp.open('GET','<% Response.Write(DataPage); %>',true);
            xmlHttp.send(null);
        }
    }

</script>

<div id="ajaxStuff">
    <% Response.Write(InitialValue); %>
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/loading.gif" />
</div>