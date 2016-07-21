<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CSVImport.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>CSV导入数据库</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
<link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" runat="server" href="/">CSV导入数据库</a>
                </div>
               <%-- <div class="navbar-collapse collapse">
                    <ul class="nav navbar-nav">
                        <li><a runat="server" href="~/">主页</a></li>
                        <li><a runat="server" href="~/About">关于</a></li>
                        <li><a runat="server" href="~/Contact">联系方式</a></li>
                    </ul>
                </div>--%>
            </div>
        </div>
        <div class="container body-content">

        </div>
    </form>
</body>
</html>
