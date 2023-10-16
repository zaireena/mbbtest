Imports Microsoft.VisualBasic
Imports clsConnection
Imports System.Data
Imports System.Data.SqlClient
Public Class clsFunction
    Public Shared Function checkPackage(ByVal aPackageCode As String) As String
        Dim rs As SqlDataReader
        Dim cn As SqlConnection
        Dim package As String = "False"
        cn = New SqlConnection(GetConnectionSetting)
        cn.Open()

        Dim cmd As SqlCommand = cn.CreateCommand

        Try
            cmd.Connection = cn

            cmd.CommandText = "SELECT * FROM tblProduct WHERE ProductCode=@ProductCode "
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ProductCode", aPackageCode)
            rs = cmd.ExecuteReader
            If rs.HasRows Then
                rs.Read()
                package = rs("Package")
            End If
            rs.Close()


        Catch ex As Exception
            clsMain.ActivityLog(clsMain.ConnectionLogLocation, "", "", "", "F", "check balance", ex.ToString)
        Finally
            cn.Close()
        End Try

        Return package
    End Function

    Public Shared Function getSubProduct(ByVal aPackageCode As String) As DataTable
        Dim rs As SqlDataReader
        Dim cn As SqlConnection
        Dim dtSubProduct As New DataTable
        dtSubProduct.Columns.Add("ProductCode")
        dtSubProduct.Columns.Add("Quantity")
        Dim drnew As DataRow
        cn = New SqlConnection(GetConnectionSetting)
        cn.Open()

        Dim cmd As SqlCommand = cn.CreateCommand

        Try
            cmd.Connection = cn

            cmd.CommandText = "SELECT * FROM tblPackage WHERE ProductCode=@ProductCode "
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ProductCode", aPackageCode)
            rs = cmd.ExecuteReader
            If rs.HasRows Then
                While rs.Read
                    drnew = dtSubProduct.NewRow
                    drnew("ProductCode") = rs("SubProductCode")
                    drnew("Quantity") = rs("Quantity")
                    dtSubProduct.Rows.Add(drnew)
                End While
            End If
            rs.Close()


        Catch ex As Exception
            clsMain.ActivityLog(clsMain.ConnectionLogLocation, "", "", "", "F", "check balance", ex.ToString)
        Finally
            cn.Close()
        End Try

        Return dtSubProduct
    End Function

    Public Shared Function CheckSalesBalance(ByVal aStockistCode As String, ByVal aType As String, ByVal dtProduct As DataTable _
    , Optional ByVal aEditBillNo As String = "") As String
        Dim valid As String = ""
        Dim strSQL As String = ""
        Dim cn As SqlClient.SqlConnection
        Dim rs As SqlClient.SqlDataReader
        cn = New SqlClient.SqlConnection(GetConnectionSetting)
        cn.Open()
        Dim cmd As SqlClient.SqlCommand = cn.CreateCommand
        cmd.Connection = cn

        Dim drNew As DataRow

        Try



            If dtProduct.Rows.Count > 0 Then
                For Each drNew In dtProduct.Rows

                    If strSQL = "" Then
                        strSQL = "SELECT " & drNew("Quantity") & " AS Quantity ,'" & drNew("ProductCode") & "' AS ProductCode "
                    Else
                        strSQL = strSQL & "UNION ALL " & _
                                           "SELECT " & drNew("Quantity") & " AS Quantity,'" & drNew("ProductCode") & "' AS ProductCode "
                    End If

                Next
            End If

            If aEditBillNo <> "" Then
                cmd.CommandText = "SELECT ProductCode,MasterCode,SubCode,Quantity " & _
                                  "FROM tblDispurDetails WHERE DispurNo=@DispurNo "
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@DispurNo", aEditBillNo)
                rs = cmd.ExecuteReader
                If rs.HasRows Then
                    While rs.Read()

                        If rs("ProductCode") <> rs("MasterCode") And rs("ProductCode") <> rs("SubCode") Then
                            If strSQL = "" Then
                                strSQL = "SELECT -" & rs("Quantity") & " AS Quantity ,'" & rs("ProductCode") & "' AS ProductCode "
                            Else
                                strSQL = strSQL & "UNION ALL " & _
                                                   "SELECT -" & rs("Quantity") & " AS Quantity,'" & rs("ProductCode") & "' AS ProductCode "
                            End If
                        End If
                    End While
                End If
                rs.Close()
            End If

            If strSQL <> "" Then


                strSQL = "SELECT SUM(Quantity) AS TotQty, ProductCode " & _
                         ",dbo.FN_STOCK_BALANCE (ProductCode,@StockistCode,@nType) as Balance " & _
                         "FROM ( " & strSQL & " ) AS A GROUP BY ProductCode ORDER BY ProductCode "

                cmd.CommandText = strSQL
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@StockistCode", aStockistCode)
                cmd.Parameters.AddWithValue("@nType", aType)
                rs = cmd.ExecuteReader
                If rs.HasRows Then
                    While rs.Read
                        If CInt(rs("Balance")) < CInt(rs("TotQty")) Then
                            valid = rs("ProductCode")
                            Exit While
                        End If
                    End While
                End If
                cn.Close()

            End If
        Catch ex As Exception
            clsMain.ActivityLog(clsMain.ConnectionLogLocation, "", "", "", "F", "check balance", ex.ToString & strSQL)
        Finally
            cn.Close()
        End Try

        Return valid
    End Function


End Class
