$(document).ready(function () {
    $('.login-wrapper').keypress(function (e) {
        if (e.keyCode == 13)
            $('.btn-login').click();
    })
})

function login() {
    var data = {
        account: $('#userName').val(),
        pwd: $('#Password1').val()
    }
    jQuery.postNL('../login/login', data, function (data) {
        layer.msg(data.Message);
        location.href='/'
    })
}