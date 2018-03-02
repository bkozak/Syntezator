<%@ Page Title="" Language="C#" MasterPageFile="~/Syntezator.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Syntezator.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     

            <div id="strona">
                <h1>Syntezator</h1>
                <asp:TextBox ID="textbox" runat="server" Height="150px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                <asp:TextBox ID="duplikatybox" runat="server" Height="150px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                <br />
                <asp:Label ID="Bledy" runat="server" Text=""></asp:Label>
                <br />
                <asp:Button ID="usunDuplikaty_btn" runat="server" Text="Usuń duplikaty" OnClick="usunDuplikaty_btn_Click" />
                <asp:Button ID="wyczysc_btn" runat="server" Text="Wyczyść" OnClick="wyczysc_btn_Click" />
                <asp:Button ID="czytaj" runat="server" Text="Czytaj" OnClick="czytaj_Click" />
            </div>
</asp:Content>
