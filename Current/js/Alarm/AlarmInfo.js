$(document).ready(function () {
    $('body').delegate('.dealAlarm', 'click', function () {
        var id = $(this).data('id');
        var data = {
            id: id
        }
        jQuery.postNL('../alarmAjax/DealAlarm', data, function (data) {
            layer.msg(data.Message, {
                time: 500,
                end: function () {
                    location.reload();
                }
            })
        })
    })
})