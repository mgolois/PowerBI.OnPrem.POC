<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PowerBi.OnPrem.POC.Reports.Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<style>
		body {
			margin: 0; /* Reset default margin */
		}

		iframe {
			display: block; /* iframes are inline by default */
			background: #000;
			border: none; /* Reset default border */
			height: 100vh; /* Viewport-relative units */
			width: 100vw;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		<iframe frameborder="0" allowfullscreen="true" runat="server" id="IFrame"></iframe>
		<%--<rsweb:ReportViewer Style="display: table; margin: 0px; overflow: auto" ProcessingMode="Remote" ID="ReportViewer1" runat="server" Width="800px" Height="600px" ShowCredentialPrompts="false" ShowBackButton="True">
		</rsweb:ReportViewer>--%>
	</form>
</body>
</html>
