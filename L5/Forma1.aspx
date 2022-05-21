<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma1.aspx.cs" Inherits="L5.Forma1" %>

<!DOCTYPE html>
<link rel="stylesheet" href="Stilius.css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label class="labelStyle" ID="Label1" runat="server" Text="Pasirinkite failus su su pasto istorija ir serveriu specifikacijomis"></asp:Label>
            <br />
            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="True" />
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Nuskaityti duomenis" />
            <br />
            <br />
            <asp:Button ID="Button2" runat="server" Text="Button" Visible="False" />
        </div>
        <asp:Table class = "ErrorTable" ID="Table1" runat="server" BorderStyle="None">
        </asp:Table>
        <asp:Label ID="Label2" runat="server" Text="Label" ForeColor="Red" Visible="False"></asp:Label>
    </form>
</body>
</html>
