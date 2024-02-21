Imports Ext.Net
Imports System.Data
Imports System.IO
Imports OfficeOpenXml
Imports Microsoft.Reporting.WebForms

Public Class Helper

    Public Shared Function RepRDLCtoReport(extensi As String, queryStr As String, namaDataSource As String, namaFileRDLC As String, namaFileExport As String, currPath As String) As MemoryStream

        Dim warnings As Warning()
        Dim streamIds As String()
        Dim mimeType As String = String.Empty
        Dim encoding As String = String.Empty
        Dim extension As String = String.Empty
        Dim dt As DataTable

        Using SqlHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.Advantage)
            SqlHelper.CommandType = CommandType.Text
            SqlHelper.CommandText = queryStr
            dt = SqlHelper.ExecuteDataTable()
        End Using

        Dim rds As New ReportDataSource(namaDataSource, dt)
        Dim viewer As ReportViewer = New ReportViewer()

        viewer.LocalReport.DataSources.Add(rds)
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Path.Combine(currPath, "Reports", namaFileRDLC)

        Dim bytes As Byte() = viewer.LocalReport.Render(extensi, Nothing, mimeType, encoding, extension, streamIds, warnings)
        Return New MemoryStream(bytes)
    End Function
    Public Shared Function RepRDLCtoReportwParam(extensi As String, queryStr As String, namaDataSource As String, namaFileRDLC As String, currPath As String, repParams As ReportParameter()) As MemoryStream

        Dim warnings As Warning()
        Dim streamIds As String()
        Dim mimeType As String = String.Empty
        Dim encoding As String = String.Empty
        Dim extension As String = String.Empty
        Dim dt As DataTable

        Using SqlHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.Advantage)
            SqlHelper.CommandType = CommandType.Text
            SqlHelper.CommandText = queryStr
            dt = SqlHelper.ExecuteDataTable()
        End Using

        Dim rds As New ReportDataSource(namaDataSource, dt)
        Dim viewer As ReportViewer = New ReportViewer()

        viewer.LocalReport.DataSources.Add(rds)
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Path.Combine(currPath, "Reports", namaFileRDLC)
        viewer.LocalReport.SetParameters(repParams)

        Dim bytes As Byte() = viewer.LocalReport.Render(extensi, Nothing, mimeType, encoding, extension, streamIds, warnings)
        Return New MemoryStream(bytes)
    End Function

    Public Shared Function GenerateXLSXFile(ByVal tbl As DataTable, filename As String) As MemoryStream
        Dim excelPackage = New ExcelPackage
        Dim excelWorksheet = excelPackage.Workbook.Worksheets.Add(filename)

        'set judul
        excelWorksheet.Cells("A1").Value = filename
        excelWorksheet.Cells("A1").Style.Font.Bold = True
        excelWorksheet.Cells("A1").Style.Font.Size = 24
        excelWorksheet.Cells(1, 1, 1, tbl.Columns.Count).Merge = True

        Dim rangeTable = excelWorksheet.Cells("A3").LoadFromDataTable(tbl, True)
        Dim iRowIndex As Integer = 3
        If tbl.Rows.Count > 0 Then
            For iColumn As Integer = 0 To tbl.Columns.Count - 1
                If tbl.Columns(iColumn).DataType.ToString = "System.DateTime" Then
                    excelWorksheet.Cells(4, iColumn + 1, tbl.Rows.Count + 4, iColumn + 1).Style.Numberformat.Format = "dd-mmm-yyyy HH:mm"
                    excelWorksheet.Cells(4, iColumn + 1, tbl.Rows.Count + 4, iColumn + 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                End If
                If tbl.Columns(iColumn).DataType.ToString = "System.Decimal" Then
                    excelWorksheet.Cells(4, iColumn + 1, tbl.Rows.Count + 4, iColumn + 1).Style.Numberformat.Format = "_(* #,##0_);_(* \(#,##0\);_(* ""-""??_);_(@_)"
                    excelWorksheet.Cells(4, iColumn + 1, tbl.Rows.Count + 4, iColumn + 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right
                End If
            Next
            'Border
            For i As Integer = 0 To tbl.Rows.Count
                For iColumn As Integer = 0 To tbl.Columns.Count - 1
                    excelWorksheet.Cells(iRowIndex, iColumn + 1).Style.Border.BorderAround(Style.ExcelBorderStyle.Thin)
                Next
                iRowIndex += 1
            Next
        End If
        excelWorksheet.Row(3).Style.Font.Bold = True
        excelWorksheet.Row(3).Style.Font.Size = 12
        'set warna judul
        excelWorksheet.Cells(3, 1, 3, tbl.Columns.Count).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        excelWorksheet.Cells(3, 1, 3, tbl.Columns.Count).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue)

        excelWorksheet.Cells(excelWorksheet.Dimension.Address).AutoFitColumns()
        excelWorksheet.Cells(excelWorksheet.Dimension.Address).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center

        Return New MemoryStream(excelPackage.GetAsByteArray())
    End Function

    Public Shared Sub ShowErr(ex As Exception)
        ShowMsg(ex.Message, ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
    End Sub
    Public Shared Sub ShowTes(Isi As String)
        ShowMsg("tes", Isi)
    End Sub
    Public Shared Sub ShowMsg(Title As String, Isi As String)
        X.Msg.Alert(Title, Isi).Show()
    End Sub
    Public Shared Sub handleEx(ex As Exception)
        If ex.Message.Contains("!!!") Then
            ShowMsg("Info", ex.Message)
        Else
            ShowErr(ex)
        End If
    End Sub
    Public Shared Function GetSelectedMultiCombo(cmb As Object) As String
        Dim s As New List(Of String)
        For i As Integer = 0 To cmb.SelectedItems.Count - 1
            s.Add(" '" & cmb.SelectedItems(i).Value & "'")
        Next
        Return String.Join(",", s.ToArray)
    End Function
    Public Shared Function getBranchSelection(cmb As Object) As String
        Dim str = ""
        Dim branchList As New List(Of String)

        Using oHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.UM)
            oHelper.CommandType = CommandType.Text

            Dim selectedBranchList As New List(Of String)
            For Each item As ListItem In cmb.SelectedItems
                selectedBranchList.Add(item.Value)
            Next

            If selectedBranchList.Contains("Select All") Then
                oHelper.CommandText = "Select GroupID FROM UM_DB.dbo.UM_Group WHERE IsAktif = 'Y'"
                Dim dtCabang = oHelper.ExecuteDataTable

                For Each dr As DataRow In dtCabang.Rows
                    branchList.Add(dr("GroupID").ToString)
                Next
            Else
                branchList.AddRange(selectedBranchList)
            End If
            str = IIf(branchList.Count > 0, String.Join(",", branchList), "''")

        End Using
        Return str
    End Function
    Public Shared Function getBranchNameSelection(cmb As Object) As String
        Dim str = ""
        Dim branchNameList As New List(Of String)

        Using oHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.UM)
            oHelper.CommandType = CommandType.Text

            Dim selectedBranchList As New List(Of String)
            For Each item As ListItem In cmb.SelectedItems
                selectedBranchList.Add(item.Value)
            Next

            If selectedBranchList.Contains("Select All") Then
                oHelper.CommandText = "Select Name FROM UM_DB.dbo.UM_Group WHERE IsAktif = 'Y'"
                Dim dtCabang = oHelper.ExecuteDataTable

                For Each dr As DataRow In dtCabang.Rows
                    branchNameList.Add(dr("Name").ToString)
                Next
            Else
                branchNameList.AddRange(selectedBranchList)
            End If
            str = IIf(branchNameList.Count > 0, String.Join(",", branchNameList), "''")

        End Using
        Return str
    End Function
    Public Shared Function getBankSelection(cmb As Object) As String
        Dim str As String = ""
        Dim bankList As New List(Of String)

        Using oHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.Advantage)
            oHelper.CommandType = CommandType.Text

            Dim selectedBankList As New List(Of String)
            For Each item As ListItem In cmb.SelectedItems
                selectedBankList.Add("'" + item.Value + "'")
            Next

            If selectedBankList.Contains("'Select All'") Then
                oHelper.CommandText = "Select Code FROM MsBank"
                Dim dtCabang = oHelper.ExecuteDataTable

                For Each dr As DataRow In dtCabang.Rows
                    bankList.Add("'" + dr("Code").ToString + "'")
                Next
            Else
                bankList.AddRange(selectedBankList)
            End If
            str = IIf(bankList.Count > 0, String.Join(",", bankList), "''")

        End Using
        Return str
    End Function
    Public Shared Function getKatProbSelection(cmb As Object) As String
        Dim str As String = ""
        Dim KatProbList As New List(Of String)

        Using oHelper As New MonyetSQLHelper(MonyetSQLHelper.ConnectionTo.Advantage)
            oHelper.CommandType = CommandType.Text

            Dim selectedKatProbList As New List(Of String)
            For Each item As ListItem In cmb.SelectedItems
                selectedKatProbList.Add("'" + item.Value + "'")
            Next

            If selectedKatProbList.Contains("'Select All'") Then
                oHelper.CommandText = "Select Code FROM MsCategory"
                Dim dtCabang = oHelper.ExecuteDataTable

                For Each dr As DataRow In dtCabang.Rows
                    KatProbList.Add("'" + dr("Code").ToString + "'")
                Next
            Else
                KatProbList.AddRange(selectedKatProbList)
            End If
            str = IIf(KatProbList.Count > 0, String.Join(",", KatProbList), "''")

        End Using
        Return str
    End Function
    Public Shared Sub AddSkrip(Skrip As String)
        X.AddScript(Skrip)
    End Sub
End Class
