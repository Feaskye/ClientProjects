<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CSVImport.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>CSV导入数据库</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
<link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <script src="js/webuploader/webuploader.min.js"></script>
    <script src="js/webuploader/webuploader.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container body-content">
            <button type="button" class="btn btn-primary" id="filePicker"><i class="fa fa-check-square-o"></i>&nbsp;选择文件</button>
        <input name="hiddenFile" value="" type="hidden" />
        <label id="fileName"></label>
            <span id="spMessage"></span>
        </div>
        
    </form>
    <script type="text/javascript">
        var originalImgTip = $('span#spMessage');
        //文件上传
        if (!WebUploader.Uploader.support()) {
            showFailedDialog("Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器");
            throw new Error('WebUploader does not support the browser you are using.');
        }

        //创建实例
        var uploader = WebUploader.create({
            pick: '#filePicker',
            accept: { title: 'Images', extensions: 'gif,jpg,jpeg,bmp,png', mimeTypes: 'image/*' },
            server: '/UploadFile.ashx',
            auto: true,
            upload_target: "rrr",
            fileNumLimit: 100,
            fileSingleSizeLimit: 5 * 1024 * 1024
        });

        //上传过程中触发，携带上传进度
        uploader.onUploadProgress = function (file, percentage) {
            originalImgTip.html("文件上传中...");
            var percent = Math.ceil(percentage * 100);
            if (percent === 100) {
                //originalImgTip.html("");
            }
        }

        //当文件上传成功时触发
        uploader.onUploadSuccess = function (file, response) {
            alert(response.data);
            if (response.success) {
                $('#hiddenFile').val(response.data);
                $("#fileName").html(response.data);
            }
            else {
                originalImgTip.html("文件上传失败");
            }
        };

        //当验证不通过时触发
        uploader.onError = function (type, data, file) {
            switch (type) {
                case 'F_EXCEED_SIZE':
                    text = ' 文件大小超出5M';
                    break;
                case 'Q_EXCEED_NUM_LIMIT':
                    text = '文件个数超出';
                    break;
                case 'Q_EXCEED_SIZE_LIMIT':
                    text = '文件总大小超出';
                    break;
                case 'Q_TYPE_DENIED':
                    text = '文件类型错误';
                    break;
                case 'F_DUPLICATE':
                    text = '文件选择重复';
                    break;
                default:
                    text = '上传失败，请重试' + type;
                    break;
            }
            originalImgTip.html(text);
            if (type != "") { uploader.destroy(); }
        };

        //文件上传完成时触发
        uploader.onUploadComplete = function (file) {
            originalImgTip.html("文件上传完毕");
            //setTimeout(function () {
            //    originalImgTip.html("");
            //}, 500);
        }
    </script>
</body>
</html>
