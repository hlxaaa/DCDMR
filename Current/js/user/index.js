var orderField = 'id'
var desc = true;
var listSort;
var defaultField = 'id'
var isAdd = true;
var page = 1;
var dataId;

$(document).ready(function () {
    changePage(1);

    //弹出用户信息编辑框
    $('body').delegate('.btn-detail', 'click', function () {
        //var id = $(this).parent().data('id')
        //location.href = '/user/userDetail?userid='+id;
        var e = $(this).parent().parent();
        Edit(e);
        isAdd = false;
    })

    //删除用户信息
    $('body').delegate('.btn-del', 'click', function () {
        var userId = $(this).parent().data('id')
        layer.confirm('确认删除吗？', {
            btn: ['确定', '取消']
        }, function () {
            var data = {
                userId: userId
            }
            jQuery.postNL('../userajax/DeleteSonInfo', data, function (data) {
                layer.msg('删除成功', {
                    time: 1000,
                    end: function () {
                        changePage(1);
                    }
                })
            })
            layer.closeAll('dialog');
        }, function () {
            layer.closeAll('dialog');
        })
    })

    //弹出用户信息编辑框
    $('body').delegate('.table .tds', 'click', function () {
        if ($('.areaId option').length == 0) {
            layer.msg('请先前往基础配置中添加区域');
            return;
        }
        var e = $(this).parent();
        Edit(e);
        isAdd = false;
    })

    //弹出文本性提示框
    $(".btn-add").click(function () {
        //console.log($('.areaId').val())
        if ($('.areaId option').length == 0) {
            layer.msg('请先前往基础配置中添加区域');
            return;
        }

        $(".pop_bg").fadeIn();
        $('#name').val('')
        $('#account').val('')
        $('#pwd').val('123456')
        isAdd = true;
        $('.pop_cont h3').html('添加子账号')

    });
    //弹出：确认按钮
    $(".trueBtn").click(function () {
        //$(".pop_bg").fadeOut();
        if (isAdd) {
            var data = {
                name: $('#name').val(),
                account: $('#account').val(),
                pwd: $('#pwd').val(),
                areaId: $('.areaId').val()
            }
            jQuery.postNL('../userAjax/addson', data, function (data) {
                layer.msg(data.Message, {
                    time: 1000,
                    end: function () {
                        location.reload();
                    }
                })
                //changePage(page)

                $(".pop_bg").fadeOut();
            })
        } else {
            var data = {
                userId: dataId,
                name: $('#name').val(),
                account: $('#account').val(),
                pwd: $('#pwd').val(),
                areaId: $('.areaId').val()
            }
            jQuery.postNL('../userAjax/UpdateSonInfo', data, function (data) {
                layer.msg(data.Message, {
                    time: 1000,
                    end: function () {
                        location.reload();
                    }
                })
                //changePage(page)

                $(".pop_bg").fadeOut();
            })
        }
    });
    //弹出：取消或关闭按钮
    $(".falseBtn").click(function () {
        $(".pop_bg").fadeOut();
    });

    //输入框Enter监听事件
    $('.pop_cont_input').keypress(function (e) {
        if (e.keyCode == 13)
            $('.trueBtn').click();
    })
})

function testPage() {
    var h = getPageHtml(100, 66);
    $('.paging').children().remove();
    $('.paging').append(h)
}

function changePage(index) {
    page = index;
    listSort = new Array();
    $('.table th').each(function () {
        listSort.push($(this).attr('class'))
    })
    var data = {
        search: $('.input-search').val(),
        pageIndex: index,
        orderField: orderField,
        isDesc: desc,
        listSort: listSort
    }
    jQuery.postNL('../userajax/getList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            //debugger;
            $('.rt_content .table tr').remove();
            $('.rt_content .table tbody').append('<tr><th>没有相关信息</th></tr>')
            $('.rt_content .paging').children().remove();
            return;
        }

        var h = '<table class="table"><tr><th class="' + listSort[0] + '" data-sort="name">名称</th><th class="' + listSort[1] + '" data-sort="account">账号</th><th class="' + listSort[2] + '" data-sort="areaName">区域</th><th>操作</th></tr>';
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var name = item.name;
            var account = item.account;
            var pername = item.pername;
            var areaName = item.areaName;
            var areaId = item.areaId;
            var id = item.id;
            h += '<tr><td class="center tds">' + name + '</td><td class="center tds">' + account + '</td><td class="center tds">' + areaName + '</td><td class="center" data-id="' + id + '" data-areaid="' + areaId + '"><a href="/user/permission?staffid=' + id + '" class="inner_btn" data-fmstate="0">修改权限</a><a  title="编辑" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td></tr>'
        }
        h += ' </table>'
        $('.rt_content .table').remove();
        $('.rt_content .mtb').after(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging').append(h);
        //var res = data.data;

        //layer.msg(data.Message);
        //location.href = '/'
    })
}

function Edit(ele) {
    $('.pop_cont h3').html('编辑员工')
    $(".pop_bg").fadeIn();
    var name = ele.find('td').eq(0).html();
    var account = ele.find('td').eq(1).html();
    var areaId = ele.find('td').last().data('areaid')

    $('#name').val(name)
    $('#account').val(account)
    $('#pwd').val('')
    $('.areaId').val(areaId);
    dataId = ele.find('td').last().data('id');
}