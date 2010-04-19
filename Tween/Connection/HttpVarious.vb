﻿Imports System.Net
Imports System.Collections.Generic

Public Class HttpVarious
    Inherits HttpConnection

    Private Const PostMethod As String = "POST"
    Private Const GetMethod As String = "GET"

    Public Function GetRedirectTo(ByVal url As String) As String
        Dim req As HttpWebRequest = CreateRequest(GetMethod, New Uri(url), Nothing, False)
        req.Timeout = 5000
        req.AllowAutoRedirect = False
        Try
            Dim data As String = ""
            Dim head As New Dictionary(Of String, String)
            Dim ret As HttpStatusCode = GetResponse(req, data, head, False)
            If head.ContainsKey("Location") Then
                Return head("Location")
            Else
                Return url
            End If
        Catch ex As Exception
            Return url
        End Try
    End Function

    Public Function GetImage(ByVal url As String) As Image
        Dim req As HttpWebRequest = CreateRequest(GetMethod, New Uri(url), Nothing, False)
        req.Timeout = 5000
        Try
            Dim img As Bitmap = Nothing
            Dim ret As HttpStatusCode = GetResponse(req, img, Nothing, False)
            If ret = HttpStatusCode.OK Then Return img
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function PostData(ByVal Url As String, ByVal param As Dictionary(Of String, String)) As Boolean
        Dim req As HttpWebRequest = CreateRequest(PostMethod, New Uri(Url), param, False)
        Try
            Dim res As HttpStatusCode = Me.GetResponse(req, Nothing, False)
            If res = HttpStatusCode.OK Then Return True
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function PostData(ByVal Url As String, ByVal param As Dictionary(Of String, String), ByRef content As String) As Boolean
        Dim req As HttpWebRequest = CreateRequest(PostMethod, New Uri(Url), param, False)
        Try
            Dim res As HttpStatusCode = Me.GetResponse(req, content, Nothing, False)
            If res = HttpStatusCode.OK Then Return True
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetData(ByVal Url As String, ByVal param As Dictionary(Of String, String), ByRef content As String) As Boolean
        Dim req As HttpWebRequest = CreateRequest(GetMethod, New Uri(Url), param, False)
        Try
            Dim res As HttpStatusCode = Me.GetResponse(req, content, Nothing, False)
            If res = HttpStatusCode.OK Then Return True
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetContent(ByVal method As String, ByVal Url As Uri, ByVal param As Dictionary(Of String, String), ByRef content As String, ByVal headerInfo As Dictionary(Of String, String), ByVal userAgent As String) As HttpStatusCode
        Dim req As HttpWebRequest = CreateRequest(method, Url, param, False)
        req.UserAgent = userAgent
        Return Me.GetResponse(req, content, headerInfo, False)
    End Function

    Public Function GetDataToFile(ByVal Url As String, ByVal savePath As String) As Boolean
        Dim req As HttpWebRequest = CreateRequest(GetMethod, New Uri(Url), Nothing, False)
        req.AutomaticDecompression = DecompressionMethods.Deflate Or DecompressionMethods.GZip
        Using strm As New System.IO.FileStream(savePath, IO.FileMode.Create, IO.FileAccess.Write)
            Try
                Dim res As HttpStatusCode = Me.GetResponse(req, strm, Nothing, False)
                strm.Close()
                If res = HttpStatusCode.OK Then Return True
                Return False
            Catch ex As Exception
                strm.Close()
                Return False
            End Try
        End Using
    End Function
End Class