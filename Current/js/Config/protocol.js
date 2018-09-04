var orderField = 'protocolNo'
var desc = true;
var listSort;
var defaultField = 'protocolNo'
var isAdd = true;
var protocol;
var page = 0

$(document).ready(function () {
    changePage(1);

    $('body').delegate('.tds', 'click', function () {
        protocol = $(this).parent().find('td').last().data('id')
        var name = $(this).parent().find('.td-ProtocolName').html();
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('更新协议');
        isAdd = false;
        $('.name').val(name)
        $('.no').val(protocol)
    })
    $('body').delegate('.btn-detail', 'click', function () {
        protocol = $(this).parent().parent().find('td').last().data('id')
        var name = $(this).parent().parent().find('.td-ProtocolName').html();
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('更新协议');
        isAdd = false;
        $('.name').val(name)
        $('.no').val(protocol)
    })

    //弹出文本性提示框
    $(".btn-add").click(function () {
        $('.pop_bg h3').html('添加新协议');
        $(".pop_bg").fadeIn();
        isAdd = true;
        $('.name').val('')
        $('.no').val('')
    });
    //弹出：确认按钮
    $(".trueBtn").click(function () {

        var name = $('.name').val();
        var no = $('.no').val();
        if (isAdd) {
            var data = {
                name: name,
                no:no
            }
            jQuery.postNL('../configAjax/AddProtocol', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
                $(".pop_bg").fadeOut();
            })


        } else {
            //var data = {
            //    name: name,
            //    id: areaId
            //}



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
    jQuery.postNL('../configAjax/GetProtocolList', data, function (data) {
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

        var h = '<table class="table"><tr><th class="' + listSort[0] + '" data-sort="ProtocolNo">协议编号</th><th class="' + listSort[1] +'" data-sort="ProtocolName">协议名称</th><th>操作</th></tr>';
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var ProtocolNo = item.ProtocolNo
            var ProtocolName = item.ProtocolName
            h += '<tr><td class="center tds td-ProtocolNo" title="' + ProtocolNo + '">' + ProtocolNo + '</td><td class="center tds td-ProtocolName" title="' + ProtocolName + '">' + ProtocolName + '</td><td class="center "  data-id="' + ProtocolNo +'"><a  title="编辑" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td></tr>'
        }
        h += ' </table>'
        $('.rt_content .table').remove();
        $('.rt_content .paging').before(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging').append(h);
    })
}


