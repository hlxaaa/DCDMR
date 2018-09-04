var orderField = 'id'
var desc = true;
var listSort;
var defaultField = 'id'
var isAdd = true;
var areaId;
var page = 0

$(document).ready(function () {
    changePage(1);

    $('body').delegate('.tds', 'click', function () {
        var name = $(this).parent().find('td').eq(0).html();
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('更新区域');
        isAdd = false;
        $('.name').val(name)
        areaId = $(this).parent().find('td').last().data('id')
    })
    $('body').delegate('.btn-detail', 'click', function () {
        var name = $(this).parent().parent().find('td').eq(0).html();
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('更新区域');
        isAdd = false;
        $('.name').val(name)
        areaId = $(this).parent().parent().find('td').last().data('id')
    })
    //弹出文本性提示框
    $(".btn-add").click(function () {
        //location.href = '/config/areadetail';

        $('.pop_bg h3').html('添加新区域');
        $(".pop_bg").fadeIn();
        isAdd = true;
        $('.name').val('')
    });
    //弹出：确认按钮
    $(".trueBtn").click(function () {

        var name = $('.name').val();
        if (isAdd) {
            var data = {
                name: name
            }
            jQuery.postNL('../configAjax/addarea', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
                $(".pop_bg").fadeOut();
            })


        } else {
            var data = {
                name: name,
                areaId: areaId
            }

            jQuery.postNL('../configAjax/updateArea', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
                $(".pop_bg").fadeOut();
            })

        }
    });
    //弹出：取消或关闭按钮
    $(".falseBtn").click(function () {
        $(".pop_bg").fadeOut();
    });

    $('body').delegate('.btn-del', 'click', function () {
        var id = $(this).parent().data('id');
        var data = {
            id: id
        }

        layer.confirm('确认删除吗？', {
            btn: ['确定', '取消']
        }, function () {
            jQuery.postNL('../configAjax/deleteArea', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
            })
            layer.closeAll('dialog');
        }, function () {
            layer.closeAll('dialog');
        })


    })

    $('.name').keypress(function (e) {
        if (e.keyCode == 13)
            $('.trueBtn').click();
    })
})

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
    jQuery.postNL('../configAjax/GetAreaList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            $('.rt_content .table tr').remove();
            $('.rt_content .table tbody').append('<tr><th>没有相关信息</th></tr>')
            $('.rt_content .paging').children().remove();
            return;
        }

        var h = '<table class="table"><tr><th class="' + listSort[0] + '" data-sort="name">名称</th><th>操作</th></tr>';
        for (var i = 0; i < data.length; i++) {
            //var item = data[i]
            //name = item.name
     
            var item = data[i]
            var name = item.name
            var mapAddress = item.mapAddress
            var id = item.id;
            h += '<tr><td class="center tds td-name" title="' + name + '">' + name + '</td><td class="center "  data-id="' + id +'"><a  title="编辑" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td></tr>'
        }
        h += ' </table>'
        $('.rt_content .table').remove();
        $('.rt_content .paging').before(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging').append(h);
    })
}


