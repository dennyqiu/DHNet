$("#imgUpload")
        .fileinput({
            language: 'zh',
            uploadUrl: "/Product/imgDeal",
            autoReplace: true,
            maxFileCount: 1,
            allowedFileExtensions: ["jpg", "png", "gif"],
            browseClass: "btn btn-primary", //按钮样式 
            previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",      
        })
    .on("fileuploaded", function (e, dataa) {
        var res = data.response;
        if (res.state > 0) {
            alert('上传成功');
            alert(res.path);
        }
        else {
            alert('上传失败')
        }
    })