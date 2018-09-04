var userId;
$(document).ready(function () {
    //用户信息保存
    $('.btn-save').click(function () {
        var name = $('.userName').val()
        var account = $('.userAccount').val()
        var areaId = $('.areaId').val();
        var data = {
            name: name,
            account: account,
            pwd: $('.pwd').val(),
            userId: userId,
            areaId: areaId,
            phone: $('.userPhone').val()
        }
        var url;
        //if (userId != 0) {
        //    url = '../userajax/UpdateSonInfo'
        //} else {
        url = '../userajax/UpdateUserInfo'
        //}
        jQuery.postNL(url, data, function (data) {
            layer.msg('更新成功', {
                time: 1000,
                end: function () {
                    location.reload();
                }
            })
        })
    })
})