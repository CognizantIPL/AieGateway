<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gateway.aspx.cs" Inherits="AIEGateway.Web.Gateway" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/MobileServices.Web-1.0.0.min.js"></script>
    <script src="Scripts/Gateway.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            Gateway.Api.initialize();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="errorlog"></div>
        <ul id="storageLog"></ul>
        <asp:HiddenField ID="hdnTemperature" runat="server" />
        <asp:HiddenField ID="hdnWetDry" runat="server" />
        <footer>
            <input id="btnClearStorage" type="button" value="Clear Local Storage" />
        </footer>
    </form>
</body>
</html>
