<%@ Page Title="Master Parameter" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_SysParameter.aspx.vb" Inherits="SysParameter" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css" title="currentStyle">
        .modalBackground {
            background-image: none;
            background-color: #F6F9AB;
        }
    </style>

    <script type="text/javascript" charset="utf-8">
        function CommandHandler(commandname, IDno) {
            if (commandname == "Edit") {
                Edit(IDno);
            }
        }
        function Edit(IDno) {
            App.direct.EditData(IDno);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuHead" runat="Server">
    <a href="#">IT</a> - Parameter
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
            <%--<StartupMask Message="Loading DCT Web ..." />--%>
        </ext:ResourceManager>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
        <div class="box">
            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout" PaddingSpec="0 0 5 0">
                <Items>
                    <ext:Button ID="btnAdd" runat="server" Height="30" Icon="DatabaseAdd" Text="Tambah" StandOut="true" Hidden="true">
                        <DirectEvents>
                            <Click OnEvent="FormAdd">
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:FieldContainer>

            <asp:Panel ID="GridPanel" runat="server">
                <ext:GridPanel ID="GridPanel1" runat="server" ColumnLines="true" Cls="x-grid-dir" Flex="1" Icon="Table" Frame="true" Title="Parameter" Height="370">
                    <Store>
                        <ext:Store ID="Store1" runat="server" RemoteSort="true" PageSize="10">
                            <Proxy>
                                <ext:AjaxProxy Url="GridHandlers/MS_SysParameterGridhandler.ashx">
                                    <ActionMethods Read="GET" />
                                    <Reader>
                                        <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                    </Reader>
                                </ext:AjaxProxy>
                            </Proxy>
                            <Model>
                                <ext:Model ID="Model2" runat="server" IDProperty="ID">
                                    <Fields>
                                        <ext:ModelField Name="Name" Type="String" />
                                        <ext:ModelField Name="Remark" Type="String" />
                                        <ext:ModelField Name="Value" Type="String" />
                                        <ext:ModelField Name="Name_Status" Type="String" />
                                        <ext:ModelField Name="NameInput" Type="String" />
                                        <ext:ModelField Name="TimeInput" Type="Date" />
                                        <ext:ModelField Name="NameEdit" Type="String" />
                                        <ext:ModelField Name="TimeEdit" Type="Date" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters>
                                <ext:DataSorter Property="Name" Direction="Desc" />
                            </Sorters>
                        </ext:Store>
                    </Store>

                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn ID="RowNumbererColumn1" Text="No" runat="server" Filterable="false" Width="40" />
                            <ext:CommandColumn ID="CommandColumn1" runat="server" Width="50" Text="Action" DataIndex="ID" Sortable="false" Align="Center">
                                <Commands>
                                    <ext:GridCommand Icon="NoteEdit" CommandName="Edit" Text="Edit">
                                        <ToolTip Text="Edit" />
                                    </ext:GridCommand>
                                </Commands>
                                <Listeners>
                                    <Command Handler="CommandHandler(command, record.data.Name);" />
                                </Listeners>
                            </ext:CommandColumn>
                            <ext:Column ID="Column16" runat="server" Text="Name" DataIndex="Name" Width="220px" />
                            <ext:Column ID="Column2" runat="server" Text="Remark" DataIndex="Remark" Width="280px" />
                            <ext:Column ID="Column5" runat="server" Text="Value" DataIndex="Value" Width="350px" />
                            <ext:Column ID="Column3" runat="server" Text="Status" DataIndex="Name_Status" Width="50px" />
                            <ext:Column ID="Column1" runat="server" Text="User Input" DataIndex="NameInput" Width="120px" />
                            <ext:DateColumn ID="Column4" runat="server" Text="Time Input" Format="dd MMM yyyy HH:mm" DataIndex="TimeInput" Width="110px" />
                            <ext:Column ID="Column6" runat="server" Text="User Edit" DataIndex="NameEdit" Width="120px" />
                            <ext:DateColumn ID="Column7" runat="server" Text="Time Edit" Format="dd MMM yyyy HH:mm" DataIndex="TimeEdit" Width="110px" />
                        </Columns>
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader ID="FilterHeader1" runat="server" Remote="true" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolbar3" runat="server" DisplayInfo="true" DisplayMsg="Displaying Order(s) {0} - {1} of {2}"
                            EmptyMsg="No Order(s) to display" />
                    </BottomBar>
                </ext:GridPanel>
            </asp:Panel>
        </div>

        <ext:Window ID="winAdd" runat="server" Title="Parameter" Icon="DatabaseAdd" Width="490" Height="450" Hidden="true" Layout="Fit">
            <Items>
                <ext:FormPanel ID="FormPanel1" runat="server" Region="East" Split="true" Margins="0 5 5 5" BodyPadding="2"
                    Frame="true" Width="320" DefaultAnchor="100%" AutoScroll="True" Height="445">
                    <Items>
                        <ext:TextField ID="txtAction" runat="server" Hidden="true" />
                        <ext:TextField ID="txtName" runat="server" FieldLabel="Name" ReadOnly="true" FieldCls="ReadOnly" />
                        <ext:TextField ID="TxtRemark" runat="server" FieldLabel="Remark" />
                        <ext:TextArea ID="txtValue" runat="server" FieldLabel="Value" Height="250px" StyleHtmlContent="true" />
                        <ext:SelectBox ID="cmbStatus" runat="server" AnchorHorizontal="100%" FieldLabel="Status">
                            <Items>
                                <ext:ListItem Text="Yes" Value="1"></ext:ListItem>
                                <ext:ListItem Text="No" Value="0"></ext:ListItem>
                            </Items>
                        </ext:SelectBox>
                    </Items>

                    <Buttons>
                        <ext:Button ID="btnUpdate" runat="server" Text="Update" Icon="Disk">
                            <LoadingState Text="Processing..." />
                            <DirectEvents>
                                <Click OnEvent="btnUpdate_Click" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button3" runat="server" StandOut="true" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="this.up('window').hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</asp:Content>
