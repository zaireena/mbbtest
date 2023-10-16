<%@ Page Title="" Language="VB" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="false" CodeFile="Register.aspx.vb" Inherits="Register"  StylesheetTheme="DataGridTheme" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="Javascript">
       
       
        function CheckInt() {
            if (event.keyCode >= 48 && event.keyCode <= 57) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">
                Freelance Registration</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <div class="panel panel-default">
        <div class="panel-heading" align="center">
            Freelance Registration
            
        </div>
        <% If labelMessage.Text <> "" Then%>
        <div class="alert alert-warning">
            <asp:Label ID="labelMessage" runat="server"></asp:Label>
        </div>
        <% End If%>
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-3">
                    <asp:Label ID="label8" runat="server" Text="<%$ Resources:Member, ApplicationInformation %>"
                        Font-Bold="True"></asp:Label>
                </div>
            </div>
          
            <div class="row">
                <div class="col-lg-3">
                    <asp:Label ID="labelUsername" runat="server" Text="<%$ Resources:General, Username %>"></asp:Label>
                </div>
                <div class="col-lg-4">
                    <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                        ErrorMessage="<%$ Resources:Message, RequiredField %>" class="text-danger" ForeColor=""></asp:RequiredFieldValidator>
                </div>
            </div>
          
            <div class="row">
                <div class="col-lg-3">
                    <asp:Label ID="labelMobile" runat="server" Text="<%$ Resources:Member, Mobile %>"></asp:Label>
                </div>
                <div class="col-lg-4">
                    <asp:TextBox CssClass="form-control" ID="txtTelM" runat="server" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTelM"
                        ErrorMessage="<%$ Resources:Message, RequiredField %>" class="text-danger" ForeColor=""></asp:RequiredFieldValidator>
                </div>
                
            </div>
            <div class="row">
                <div class="col-lg-3">
                    
                    <asp:Label ID="labelEmail" runat="server" Text="<%$ Resources:Member, Email %>"></asp:Label>
                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" class="form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ControlToValidate="txtEmail" ErrorMessage="<%$ Resources:Message, InvalidEmail %>"
                        ID="revEmail" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        class="text-danger" ForeColor=""></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="<%$ Resources:Message, RequiredField %>" class="text-danger" ForeColor=""></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-3">
                    
                    <asp:Label ID="label1Skillset" runat="server" Text="Skillset"></asp:Label>
                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtSkillset" runat="server" TextMode="MultiLine" MaxLength="200"  class="form-control"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSkillset"
                        ErrorMessage="<%$ Resources:Message, RequiredField %>" class="text-danger" ForeColor=""></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-3">
                    <asp:Label ID="labelHobby" runat="server" Text="Hobby"></asp:Label>
                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtHobby" runat="server" TextMode="MultiLine" MaxLength="200"  class="form-control"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHobby"
                        ErrorMessage="<%$ Resources:Message, RequiredField %>" class="text-danger" ForeColor=""></asp:RequiredFieldValidator>
                    <br />
                </div>
            </div>
         
        
        
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-3">
                    <asp:Button runat="server" ID="btnSubmit" class="btn btn-primary" Text="<%$Resources:General,Submit %>" />
                    
                </div>
            </div>
        </div>
        
      <asp:Label ID="labelError" runat="server" Visible="false"></asp:Label> 
        
    </div>
</asp:Content>
