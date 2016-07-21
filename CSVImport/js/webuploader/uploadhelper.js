//示例 html上传控件及Id这样写
//<a id="filePicker" inputid="PayPhoto" imgid="imgPayPhoto" msgid="txtPayPhotoTip" 
//serverurl="/Upload/Orders" style="position: relative; z-index: 1;">上传付款凭证</a>
var $filePicker = $("#filePicker");
if($filePicker)
{
    var $inputHiddenPicker = $("input#" + $filePicker.attr("inputid"));
    var $imgPicker = $("img#" + $filePicker.attr("imgid"));
    var $msgPicker = $("img#" + $filePicker.attr("msgid"));
    var pickerServerUrl = $filePicker.attr("serverurl");
    //文件上传
    if (!WebUploader.Uploader.support()) {
        showFailedDialog("Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器");
        throw new Error('WebUploader does not support the browser you are using.');
    }

    //创建实例
    var uploader = WebUploader.create({
        pick: '#filePicker',
        accept: { title: 'Images', extensions: 'gif,jpg,jpeg,bmp,png', mimeTypes: 'image/*' },
        server: pickerServerUrl,
        auto: true,
        upload_target: "rrr",
        fileNumLimit: 100,
        fileSingleSizeLimit: 5 * 1024 * 1024
    });

    //上传过程中触发，携带上传进度
    uploader.onUploadProgress = function (file, percentage) {
        $msgPicker.html("图片上传中...");
        var percent = Math.ceil(percentage * 100);
        if (percent === 100) {
            //$msgPicker.html("");
        }
    }

    //当文件上传成功时触发
    uploader.onUploadSuccess = function (file, response) {
        if (response.success) {
            $inputHiddenPicker.val(response.message);
            $imgPicker.attr("src", response.message + "?p=" + Math.random());
        }
        else {
            $msgPicker.html("图片上传失败");
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
        $msgPicker.html(text);
        if (type != "") { uploader.destroy(); }
    };

    //文件上传完成时触发
    uploader.onUploadComplete = function (file) {
        $msgPicker.html("图片上传完毕");
        setTimeout(function () {
            $msgPicker.html("");
        }, 500);
    }
}