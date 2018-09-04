var orderField = 'meterTypeNo'
var desc = true;
var listSort;
var defaultField = 'meterTypeNo'
var isAdd = true;
var areaId;
var page = 0

$(document).ready(function () {
    changePage(1);

    $('body').delegate('.td-name', 'click', function () {
        //var name = $(this).parent().find('td').eq(0).html();
        //areaId = $(this).parent().find('td').eq(1).data('id')
        //$(".pop_bg").fadeIn();
        //$('.pop_bg h3').html('更新区域');
        //isAdd = false;
        //$('.name').val(name)
    })

    //弹出文本性提示框
    $(".btn-add").click(function () {
        $('.pop_bg h3').html('添加新设备');
        $(".pop_bg").fadeIn();
        isAdd = true;
        $('.name').val('')
        $('.no').val('')
        $('.code').val('')
        $('.no').removeAttr('readonly')
    });

    $('body').delegate('tr .tds', 'click', function () {
        $('.pop_bg h3').html('更新设备信息');
        $(".pop_bg").fadeIn();
        isAdd = false;
        var no = $(this).parent().find(".td-meterTypeNo").html();
        var name = $(this).parent().find(".td-meterTypeName").html();
        var code = $(this).parent().find(".td-MarkCode").html();
        $('.name').val(name)
        $('.no').val(no)
        $('.code').val(code)
        $('.no').attr("readonly", "readonly")
    })

    //弹出：确认按钮
    $(".trueBtn").click(function () {

        var name = $('.name').val();
        var no = $('.no').val();
        var code = $('.code').val();
        if (isAdd) {
            var data = {
                name: name,
                no: no,
                code: code
            }
            jQuery.postNL('../configAjax/addMeterType', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
                $(".pop_bg").fadeOut();
            })
        } else {
            var data = {
                name: name,
                no: no,
                code: code
            }
            jQuery.postNL('../configAjax/UpdateMeterType', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
                $(".pop_bg").fadeOut();
            })


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
    jQuery.postNL('../configAjax/GetMeterTypeList', data, function (data) {
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

        var h = '<table class="table"><tr><th class="' + listSort[0] + '" data-sort="meterTypeNo">设备类型编号</th><th class="' + listSort[1] + '" data-sort="meterTypeName">设备类型名称</th><th class="' + listSort[2] + '" data-sort="MarkCode">代码</th><th>操作</th></tr>';
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var meterTypeNo = item.meterTypeNo
            var meterTypeName = item.meterTypeName
            var MarkCode = item.MarkCode
            h += '<tr><td class="center tds td-meterTypeNo" title="' + meterTypeNo + '">' + meterTypeNo + '</td><td class="center tds td-meterTypeName" title="' + meterTypeName + '">' + meterTypeName + '</td><td class="center tds td-MarkCode" title="' + MarkCode + '">' + MarkCode + '</td><td class="center "  data-id="' + meterTypeNo + '"><a  title="编辑" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td></tr>'
        }
        h += ' </table>'
        $('.rt_content .table').remove();
        $('.rt_content .paging').before(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging').append(h);
    })
}


