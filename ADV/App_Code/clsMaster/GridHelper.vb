Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Web.Script.Serialization

Imports Newtonsoft.Json


Public Class GridHelper


#Region " Variable Declaration"

    Enum enSearchDataType As Integer
        DataType_Date
        DataType_StringAndNumber
    End Enum

    Enum enSearchExactOrPattern As Integer
        Exact
        Pattern
        PrefixPattern
    End Enum

    Private _oMSQLHelper As clsSQLHelper
    Private _TableName As String
    Private _WhereClause As String
    Private _FilterClause As String
    Private _ColumnsToDisplay As String
    Private _DefaultSortColumn As String
    Private _FullSQLString As String
    Private _SortColIndex As Integer
    Private _SortColumn As String
    Private _SortDirection As String

#End Region

#Region " Variables"
    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal value As String)
            _TableName = value
        End Set
    End Property
    Public Property WhereClause() As String
        Get
            Return _WhereClause
        End Get
        Set(ByVal value As String)
            _WhereClause = value
        End Set
    End Property


    Public Property ColumnsToDisplay() As String
        Get
            Return _ColumnsToDisplay
        End Get
        Set(ByVal value As String)
            _ColumnsToDisplay = value
        End Set
    End Property
    Public Property DefaultSortColumn() As String
        Get
            Return _DefaultSortColumn
        End Get
        Set(ByVal value As String)
            _DefaultSortColumn = value
        End Set
    End Property


    Public Property FullSQLString() As String
        Get
            Return _FullSQLString
        End Get
        Set(ByVal value As String)
            _FullSQLString = value
        End Set
    End Property

    Public Property SortColIndex() As Integer
        Get
            Return _SortColIndex
        End Get
        Set(ByVal value As Integer)
            _SortColIndex = value
        End Set
    End Property

    Public Property SortColumn() As String
        Get
            Return _SortColumn
        End Get
        Set(ByVal value As String)
            _SortColumn = value
        End Set
    End Property

    Public Property SortDirection() As String
        Get
            Return _SortDirection
        End Get
        Set(ByVal value As String)
            _SortDirection = value
        End Set
    End Property
#End Region

    Public Sub New()
        _oMSQLHelper = New clsSQLHelper
    End Sub


    Public Sub AddSpecificFilterClause(ByVal ColumnName As String, ByVal SearchWord As String, ByVal SearchDataType As enSearchDataType, Optional ByVal SearchExactOrPattern As enSearchExactOrPattern = enSearchExactOrPattern.Pattern)

        If SearchWord <> "" Then



            If SearchDataType = enSearchDataType.DataType_StringAndNumber Then

                If _FilterClause <> "" Then
                    _FilterClause &= " AND "
                End If

                If SearchExactOrPattern = enSearchExactOrPattern.Pattern Then
                    _FilterClause &= ColumnName & " Like '%" & SearchWord & "%'"
                ElseIf SearchExactOrPattern = enSearchExactOrPattern.PrefixPattern Then
                    _FilterClause &= ColumnName & " Like '" & SearchWord & "%'"
                Else
                    _FilterClause &= ColumnName & " = '" & SearchWord & "'"
                End If
            Else

                If IsDate(SearchWord) = True Then

                    If CDate(SearchWord) >= DateSerial(2014, 1, 1) And CDate(SearchWord) < DateSerial(2099, 12, 31) Then

                        If _FilterClause <> "" Then
                            _FilterClause &= " AND "
                        End If

                        _FilterClause &= ColumnName & " >= '" & CDate(SearchWord).ToString("yyyy-MM-dd") & "' AND " & ColumnName & " < '" & CDate(SearchWord).AddDays(1).ToString("yyyy-MM-dd") & "'"
                    End If
                End If
            End If
        End If
    End Sub

    'ini untuk 1 kotak di grid yang untuk multiple search
    Public Sub SetFilterClause(ByVal SearchWord As String)

        Dim MultiFilterClause As String

        If SearchWord <> "" Then
            Dim ColumnNames() As String = Strings.Split(_ColumnsToDisplay, ",")

            MultiFilterClause = "( "

            For i As Integer = 0 To ColumnNames.Length - 1
                MultiFilterClause &= ColumnNames(i) & " Like '%" & SearchWord & "%'"
                If i < ColumnNames.Length - 1 Then
                    MultiFilterClause &= " OR "
                End If
            Next

            MultiFilterClause &= " )"


            If _FilterClause <> "" Then
                _FilterClause &= " AND " & MultiFilterClause
            Else
                _FilterClause = MultiFilterClause
            End If

        End If



    End Sub

    Private Function GetRecordCount() As Integer
        _oMSQLHelper.ClearParameters()
        _oMSQLHelper.CommandType = CommandType.Text
        _oMSQLHelper.CommandText = "Select Count(1) " & "   FROM " & _TableName & IIf(_WhereClause = "", " Where 1=1 ", " Where " & _WhereClause & " ") & IIf(_FilterClause = "", "", " AND " & _FilterClause)
        Return _oMSQLHelper.ExecuteScalar()
    End Function

    Public Function GetDataForDisplayInGrid(ByVal StartIndex As Integer, ByVal PageSize As Integer) As DataTable

        Dim PageNumber As Integer
        Dim SortColumn As String
        Dim ColumnNames() As String = Strings.Split(_ColumnsToDisplay, ",")

        If _SortColumn = "" Then
            If _SortColIndex > 0 Then
                SortColumn = ColumnNames(_SortColIndex - 1)
            Else
                SortColumn = _DefaultSortColumn
            End If
        Else
            SortColumn = _SortColumn
        End If

        If StartIndex < 1 Then StartIndex = 1 Else StartIndex += 1
        PageNumber = Math.Floor(StartIndex / PageSize)

        'SQL 2012
        '        Dim strSelect As String =
        '"	SELECT	" & _ColumnsToDisplay &
        '"   FROM " & _TableName & IIf(_WhereClause = "", " Where 1=1 ", " Where " & _WhereClause & " ") & IIf(_FilterClause = "", "", " AND (" & _FilterClause & ")") &
        '"   Order By " & SortColumn & " " & IIf(_SortDirection = "", "asc", _SortDirection) &
        '"   OFFSET " & PageSize & " * " & PageNumber & " ROWS " &
        '"   FETCH NEXT " & PageSize & " ROWS ONLY"

        'SQL 2008
        Dim strSelect As String = "  Select * FROM ( Select  ROW_NUMBER() OVER(Order By " & SortColumn & " " & IIf(_SortDirection = "", "asc", _SortDirection) & ") PageNumberss , * FROM  " &
        " ( Select " & _ColumnsToDisplay &
        "   FROM " & _TableName & IIf(_WhereClause = "", " Where 1=1 ", " Where " & _WhereClause & " ") & IIf(_FilterClause = "", "", " And (" & _FilterClause & ")") &
        "  ) X123 )   X1233  Where PageNumberss  BETWEEN ((" & PageNumber + 1 & " - 1) * " & PageSize & " + 1)  And (" & PageNumber + 1 & " * " & PageSize & ") "

        'uncomment untuk lihat hasilnya
        'MsgBox(strSelect)

        _FullSQLString = strSelect


        _oMSQLHelper.ClearParameters()
        _oMSQLHelper.CommandType = CommandType.Text
        _oMSQLHelper.CommandText = strSelect

        Return _oMSQLHelper.ExecuteDataTable()

    End Function

    Private Function DataTableToString(ByVal iDisplayStart As Integer, ByVal table As DataTable, ByVal DateFormat As String, ByVal NumberFormat As String) As String

        Dim sb As New StringBuilder
        Dim serializer As New JavaScriptSerializer
        Dim RowCount As Integer = table.Rows.Count
        Dim ColumnCount As Integer = table.Columns.Count

        For i As Integer = 0 To RowCount - 1
            sb.Append("[")

            'RowNumber
            sb.Append(serializer.Serialize(iDisplayStart + i + 1) & ",")

            For j As Integer = 0 To ColumnCount - 1
                Dim value As String = ""

                If table.Columns(j).DataType.ToString = "System.DateTime" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDate(table.Rows(i)(j)).ToString(DateFormat)
                    End If
                ElseIf table.Columns(j).DataType.ToString = "System.Decimal" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDec(table.Rows(i)(j)).ToString(NumberFormat)
                    End If
                ElseIf table.Columns(j).DataType.ToString = "System.Double" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDec(table.Rows(i)(j)).ToString(NumberFormat)
                    End If
                Else
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = table.Rows(i)(j).ToString.Replace(ControlChars.Cr, "").Replace(ControlChars.Lf, " ").Replace(ControlChars.Tab, " ")
                    End If
                End If


                sb.Append(serializer.Serialize(value))

                If j < ColumnCount - 1 Then
                    sb.Append(",")
                End If

            Next
            sb.Append("]")
            If i < RowCount - 1 Then
                sb.Append(",")
            End If
        Next
        Return sb.ToString
    End Function


    Private Function DataTableToString(ByVal iDisplayStart As Integer, ByVal table As DataTable, ByVal FormatStrings() As String) As String

        Dim sb As New StringBuilder

        Dim serializer As New JavaScriptSerializer


        Dim RowCount As Integer = table.Rows.Count
        Dim ColumnCount As Integer = table.Columns.Count

        For i As Integer = 0 To RowCount - 1
            sb.Append("[")

            'RowNumber
            sb.Append(serializer.Serialize(iDisplayStart + i + 1) & ",")

            For j As Integer = 0 To ColumnCount - 1
                Dim value As String = ""

                If table.Columns(j).DataType.ToString = "System.DateTime" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDate(table.Rows(i)(j)).ToString(FormatStrings(j))
                    End If
                ElseIf table.Columns(j).DataType.ToString = "System.Decimal" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDec(table.Rows(i)(j)).ToString(FormatStrings(j))
                    End If
                ElseIf table.Columns(j).DataType.ToString = "System.Int32" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDec(table.Rows(i)(j)).ToString(FormatStrings(j))
                    End If
                ElseIf table.Columns(j).DataType.ToString = "System.Double" Then
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = CDec(table.Rows(i)(j)).ToString(FormatStrings(j))
                    End If
                Else
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        value = table.Rows(i)(j).ToString.Replace(ControlChars.Cr, "").Replace(ControlChars.Lf, " ").Replace(ControlChars.Tab, " ")
                    End If
                End If

                sb.Append(serializer.Serialize(value))



                If j < ColumnCount - 1 Then
                    sb.Append(",")
                End If

            Next
            sb.Append("]")
            If i < RowCount - 1 Then
                sb.Append(",")
            End If
        Next
        Return sb.ToString
    End Function

    Private Function CreateJSON(ByVal sEcho As String, ByVal iTotalRecords As String, ByVal iTotalDisplayRecords As String, ByVal aaData As String) As String
        Dim sb As New StringBuilder
        sb.Append("{ ""sEcho"" : """ & sEcho & """,""iTotalRecords"" : """ & iTotalRecords & """,""iTotalDisplayRecords"" : """ & iTotalDisplayRecords & """,""aaData"" : [")
        sb.Append(aaData)
        sb.Append(" ] }")
        Return sb.ToString
    End Function

    Public Function GetJSONData(ByVal sEcho As String, ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer) As String
        Dim TotalRecordCount As Integer = GetRecordCount()
        Dim dt As DataTable = GetDataForDisplayInGrid(iDisplayStart, iDisplayLength)
        Dim str As String = CreateJSON(sEcho, TotalRecordCount, TotalRecordCount, DataTableToString(iDisplayStart, dt, "dd-MMM-yyyy", "#,##0"))
        Return str
    End Function

    Public Function GetJSONData(ByVal sEcho As String, ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer, ByVal DateFormat As String) As String
        Dim TotalRecordCount As Integer = GetRecordCount()
        Dim dt As DataTable = GetDataForDisplayInGrid(iDisplayStart, iDisplayLength)
        Dim str As String = CreateJSON(sEcho, TotalRecordCount, TotalRecordCount, DataTableToString(iDisplayStart, dt, DateFormat, "#,##0"))
        Return str
    End Function


    Public Function GetJSONData(ByVal sEcho As String, ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer, ByVal FormatStrings() As String) As String
        Dim TotalRecordCount As Integer = GetRecordCount()
        Dim dt As DataTable = GetDataForDisplayInGrid(iDisplayStart, iDisplayLength)
        Dim str As String = CreateJSON(sEcho, TotalRecordCount, TotalRecordCount, DataTableToString(iDisplayStart, dt, FormatStrings))
        Return str
    End Function


#Region " For Ext.JS"


    Private Function DataTableToStringForExtJS(ByVal iDisplayStart As Integer, ByVal table As DataTable) As String

        Dim sb As New StringBuilder

        Dim RowCount As Integer = table.Rows.Count
        Dim ColumnCount As Integer = table.Columns.Count

        For i As Integer = 0 To RowCount - 1
            sb.Append("{")

            sb.Append("""RowNumber"":" & iDisplayStart + i + 1 & ",")

            For j As Integer = 0 To ColumnCount - 1
                sb.Append("""" & table.Columns(j).ColumnName & """:")


                If table.Columns(j).DataType.ToString = "System.DateTime" Then
                    sb.Append("""")
                    If Not IsDBNull(table.Rows(i)(j)) Then
                        sb.Append(CDate(table.Rows(i)(j)).ToString("yyyy-MM-ddTHH:mm:ss")) '"2006-03-27T00:00:00
                    End If
                    sb.Append("""")
                ElseIf table.Columns(j).DataType.ToString = "System.String" Then

					'sb.Append("""")
					If Not IsDBNull(table.Rows(i)(j)) Then
						'Dim FormattedString As String = table.Rows(i)(j).ToString.Replace(ControlChars.Cr, "").Replace(ControlChars.Lf, " ")
						Dim FormattedString As String = JsonConvert.SerializeObject(table.Rows(i)(j).ToString)
						sb.Append(FormattedString)
					Else
						'Dim FormattedString As String = JsonConvert.SerializeObject(table.Rows(i)(j).ToString)
						'sb.Append(FormattedString)
						sb.Append("""""")
					End If
					'sb.Append("""")

                Else
                    sb.Append("""")
                    If Not IsDBNull(table.Rows(i)(j)) Then
						sb.Append(Val(table.Rows(i)(j)).ToString)
                    End If
                    sb.Append("""")
                End If




                If j < ColumnCount - 1 Then
                    sb.Append(",")
                End If

            Next
            sb.Append("}")
            If i < RowCount - 1 Then
                sb.Append(",")
            End If
        Next
        Return sb.ToString
    End Function

    Private Function CreateJSONForExtJS(ByVal iTotalRecords As String, ByVal iTotalDisplayRecords As String, ByVal aaData As String) As String
        Dim sb As New StringBuilder
        sb.Append("{""data"": [")
        sb.Append(aaData)
        sb.Append(" ],""total"":" & iTotalRecords & " }")
        Return sb.ToString
    End Function

    Public Function GetJSONDataForExtJS(ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer) As String
        Dim TotalRecordCount As Integer = GetRecordCount()
        Dim dt As DataTable = GetDataForDisplayInGrid(iDisplayStart, iDisplayLength)
        Dim str As String = CreateJSONForExtJS(TotalRecordCount, TotalRecordCount, DataTableToStringForExtJS(iDisplayStart, dt))
        Return str
    End Function

#End Region

 

End Class
