$(document).ready(function () {
    $('.btn-save').click(function () {
        var data = {
            FLMeterDataRefreshRate: $('.FLMeterDataRefreshRate').val()
        }
        jQuery.postNL('../ConfigAjax/UpdateAllConfig', data, function (data) {
            layer.msg(data.Message, {
                time: 1000,
                end: function () {
                    location.reload();
                }
            })

        })
    })
    
})

function test() {
    layer.msg(1);
}