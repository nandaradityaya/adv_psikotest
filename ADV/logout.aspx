<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        If Session("AuthID") Is Nothing Then
            Session("AuthID") = ""
        End If
        If Session("UserID") Is Nothing Then
            Session("UserID") = ""
        End If
        FormsAuthentication.SignOut()
        Session.Abandon()
        Response.Redirect("https://karir.advantagescm.com/")
        'Response.Redirect("http://10.10.0.223/karir")
    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
