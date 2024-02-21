<%@ Page Title="Master User Role" Language="VB" MasterPageFile="~/site.master" AutoEventWireup="false" CodeFile="MS_UserRole.aspx.vb" Inherits="MS_UserRole" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {
            position: relative;
            min-height: 1px;
            padding-left: 22px;
            padding-right: 12px;
        }
        .widget-box {
            padding: 0;
            box-shadow: none;
            margin: 3px 0;
            border: 1px solid #CCC;
        }
        .widget-header{
            box-sizing: content-box;
            position: relative;
            min-height: 38px;
            background: repeat-x #f7f7f7;
            background-image: linear-gradient(to bottom,#FFF 0,#EEE 100%);
            color: #669FC7;
            border-bottom: 1px solid #DDD;
            padding: 10px 0px 2px 12px;
        }
        .widget-body{
            background-color: #FFF;
        }
        .widget-main{
            padding: 12px;
        }

        .table-header {
            background-color: #307ECC;
            color: #FFF;
            font-size: 14px;
            line-height: 38px;
            padding-left: 12px;
            margin-bottom: 1px;
        }
        .dataTables_length select {
            text-align: center;
            height: 25px !important;
            padding: 2px 3px;
        }
        .thead{
            background-color: rgb(242, 242, 242);
            background-image: linear-gradient(to bottom,#F8F8F8 0,#ECECEC 100%);
        }
        .table {
            background-color: #E4E6E9;
        }

        input[type=text]{
            margin: auto
        }
        .dataTables_wrapper input[type=search], .dataTables_wrapper input[type=text], .dataTables_wrapper select {
            margin-bottom: 0!important;
            margin: 0 4px;
        }
        .dataTables_filter input[type=search], .dataTables_filter input[type=text] {
            width: 125px;
            height: 18px;
            line-height: 18px;
            -webkit-box-sizing: content-box;
            -moz-box-sizing: content-box;
            box-sizing: content-box;
            padding: 4px 6px;
        }
        .dataTables_wrapper .row:first-child {
            padding-top: 12px;
            padding-bottom: 12px;
            background-color: #EFF3F8;
        }
        .dataTables_filter, .dataTables_paginate {
            text-align: right;
        }
        .pager>li>a, .pagination>li>a {
            border-width: 1px;
            border-color: #d4dfe3;
            color: #2283C5;
            background-color: #FAFAFA;
            margin: 0 -1px 0 0;
            position: relative;
            z-index: auto;
        }
        .pager li, .pagination>li {
            display: inline;
        }
        .pagination>li>a, .pagination>li>span {
            position: relative;
            float: left;
            padding: 6px 12px;
            line-height: 1.42857143;
            text-decoration: none;
            color: #337ab7;
            background-color: #fff;
            border: 1px solid #ddd;
            margin-left: -1px;
        }
        .pagination>li.active>a, .pagination>li.active>a:focus, .pagination>li.active>a:hover {
            background-color: #6FAED9;
            border-color: #6FAED9;
            color: #FFF;
            text-shadow: 0 -1px 0 rgb(0 0 0 / 25%);
            z-index: 2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form runat="server" id="Form1">

        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

    <div class="page-header">
       
    </div>
    <!-- /.page-header -->




    <div class="row">
        <div class="col-sm-6">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="smaller">Action													
												</h4>
                </div>

                <div class="widget-body">
                    <div class="widget-main">
                        <input type="button" class="btn btn-primary" value="Add Role" onclick="Add();" />
                    </div>
                </div>
            </div>
        </div>
        <!-- /.col -->


    </div>

        <br />
    <div class="row">
        <div class="col-xs-12">
            <div class="table-header">
                User Role Master
            </div>

            <!-- div.table-responsive -->

            <!-- div.dataTables_borderWrap -->
            <div class="table-responsive">
                <table id="table1" class="table table-striped table-bordered table-hover">
                    <thead class="thead">
                        <tr>
                            <th>No</th>
                            <th>Extra No</th>
                            <th>User Role</th>
                            <th>Description</th>
                            <th>Last Update</th>
                            <th>ACTION</th>
                            <th>ASSIGN MENU</th>
                        </tr>
                    </thead>


                </table>
            </div>
        </div>
    </div>
    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuHead" runat="Server">

    <!-- page specific plugin scripts -->
    <script src="./assets/js/jquery.dataTables.min.js"></script>
    <script src="./assets/js/jquery.dataTables.bootstrap.min.js"></script>
    <script src="./assets/js/dataTables.select.min.js"></script>

    <!-- inline scripts related to this page -->
    <script type="text/javascript">
        jQuery(function ($) {
            //initiate dataTables plugin
            var myTable =
            $('#table1')
            //.wrap("<div class='dataTables_borderWrap' />")   //if you are applying horizontal scrolling (sScrollX)
            .DataTable({
                bAutoWidth: false,
                "aoColumns": [null
                    , null
                    , null
                    , null
                               , { "sClass": "center" }
                                , { "bSortable": false, "mData": null, "sClass": "center", "mRender": function (data, type, full) { return '<a href="#" onclick="Edit(\'' + full[2] + '\');">EDIT</a>&nbsp;&nbsp;&nbsp;<a href="#" onclick="Delete(\'' + full[2] + '\');">DELETE</a>'; } }
                                , { "bSortable": false, "mData": null, "sClass": "center", "mRender": function (data, type, full) { return '<a href="#" onclick="AssignMenu(\'' + full[2] + '\');">ASSIGN MENU</a>'; } }

                ],
                "bProcessing": true,
                "bServerSide": true,
                "sAjaxSource": "GridHandlers/MS_UserRoleGridHandler.ashx"
            });
            myTable.column(1).visible(false);
        });


        function Edit(RecordID) {
            window.open('MS_UserRoleAddEdit.aspx?FormState=Edit&LoginRoleID=' + RecordID.toString(), '_self');
            return false;
        }

        function AssignMenu(RecordID) {
            window.open('MS_UserRoleMenuAssignment.aspx?FormState=Edit&LoginRoleID=' + RecordID.toString(), '_self');
            return false;
        }

        function Delete(RecordID) {
            var bOk = confirm("Apakah anda yakin hapus role : " + RecordID + " ?");
            var obj = {};
            obj.LoginRoleID = $.trim(RecordID);
            if (bOk == true)
            {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("~/MS/MS_UserRole.aspx/Delete") %>',
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var mydata = JSON.parse(data.d);
                        //alert(mydata.status);
                        //return;
                        if (mydata.status == "OKE") //if success
                        {
                            alert("User Role (" + RecordID + ") berhasil di hapus");
                            location.reload();
                        }
                        else if (mydata.status == "SDA") {
                            alert("User Role ("  + RecordID +  ") sudah digunakan tidak bisa dihapus!");
                        }
                        else {
                            alert("User Role (" + RecordID + ") gagal di hapus! err: " + mydata.status);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                            alert(errorThrown);
                    }
                });
            }
            else return false;
        }

        function Add() {
            window.open('MS_UserRoleAddEdit.aspx?FormState=Add', '_self');
            return false;
        }

    </script>


</asp:Content>

