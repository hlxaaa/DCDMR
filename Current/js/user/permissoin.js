$(document).ready(function () {
    //修改权限是否开启
    $('body').delegate('.buttons .btn', 'click', function () {
        var e = $(this)
        var perId = e.data('perid')
        var data;
        if (e.hasClass('btn-info')) {
            e.removeClass('btn-info')
            e.addClass('btn-noselect')
            data = {
                perId: perId,
                isOpen: false
            }
        } else {
            e.removeClass('btn-noselect')
            e.addClass('btn-info')
            data = {
                perId: perId,
                isOpen: true
            }
        }
        updatePermission(data);
    })
})

function updatePermission(data) {
    jQuery.postNL('../userAjax/UpdatePermission', data, function (data) {
        layer.msg(data.Message)
    })
}