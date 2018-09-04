var orderField = 'factoryno'
var desc = true;
var listSort;
var defaultField = 'factoryno'
var meterTypeNo
var factoryNo
var openState
var fluidNo
var Operator
var lastName
var page
var isAdd = true;

$(document).ready(function () {
    $('body').delegate('.btn-del', 'click', function () {
        var no = $(this).parent().data('id');
        layer.confirm('确认删除吗？', {
            btn: ['确定', '取消']
        }, function () {
            var data = {
                factoryNo: no
            }

            jQuery.postNL('../configAjax/DeleteFactory', data, function (data) {
                layer.msg(data.Message, {
                    time: 500,
                    end: function () {
                        changePage(page);
                    }
                })
            })

            layer.closeAll('dialog');
        }, function () {
            layer.closeAll('dialog');
        })
    })

    $(".trueBtn").click(function () {
        var data = {
            factoryName: $('.factoryName').val(),
            factoryNo: $('.factoryNo').val(),
            MarkCode: $('.MarkCode').val()
        }
        if (isAdd) {
            jQuery.postNL('../configAjax/AddFactory', data, function (data) {
                layer.msg(data.Message)
                changePage(page)
                $(".pop_bg").fadeOut();
            })
        } else {
            jQuery.postNL('../configAjax/UpdateFactory', data, function (data) {
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

    $('body').delegate('.tds', 'click', function () {
        var no = $(this).parent().find('td').eq(0).html();
        var name = $(this).parent().find('td').eq(1).html();
        var code = $(this).parent().find('td').eq(2).html();
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('更新厂家');
        $('.factoryNo').attr('readonly', 'readonly')
        isAdd = false;
        $('.factoryName').val(name)
        $('.factoryNo').val(no)
        $('.MarkCode').val(code)
        //areaId = $(this).parent().find('td').last().data('id')
    })
    $('body').delegate('.btn-detail', 'click', function () {
        var no = $(this).parent().parent().find('td').eq(0).html();
        var name = $(this).parent().parent().find('td').eq(1).html();
        var code = $(this).parent().parent().find('td').eq(2).html();
        $('.factoryNo').attr('readonly', 'readonly')
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('更新厂家');
        isAdd = false;
        $('.factoryName').val(name)
        $('.factoryNo').val(no)
        $('.MarkCode').val(code)
        //areaId = $(this).parent().parent().find('td').last().data('id')
    })

    $('.btn-addDevice').click(function () {
        isAdd = true;
        $('.factoryNo').removeAttr('readonly');
        $(".pop_bg").fadeIn();
        $('.pop_bg h3').html('新增厂家');
        $('.factoryName').val('')
        $('.factoryNo').val('')
        $('.MarkCode').val('')
    })


    //changePage(1);
    changePage(1);



})

function changePage(index) {
    GetLastName();
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
        listSort: listSort,
        meterTypeNo: meterTypeNo,
        factoryNo: factoryNo,
        openState: openState,
        fluidNo: fluidNo,
        Operator: Operator,
        lastName: lastName
    }
    jQuery.postNL('../configAjax/GetFactoryList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            //$('.rt_content .table tr').remove();
            //$('.rt_content .table tbody').append('<tr><th>没有相关信息</th></tr>')

            if ($('.rt_content .table th').length > 1) {
                ths = $('.rt_content .table tr:first').html();
            }
            $('.rt_content .table tr').remove();
            $('.rt_content .table tbody').append('<tr class="tr-del"><th>没有相关信息</th></tr>')

            $('.rt_content .paging2').children().remove();
            return;
        }

        var h = ''
        if ($('.rt_content .table th').length < 2) {
            h += `<tr>${ths}</tr>`;
        }
        //< tr > <th class="tds-3x">操作</th></tr > ';
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var factoryNo = item.factoryNo == null ? "" : item.factoryNo
            var factoryName = item.factoryName == null ? "" : item.factoryName
            var MarkCode = item.MarkCode == null ? "" : item.MarkCode

            h += '<tr><td class="center tds td-factoryNo" title="' + factoryNo + '">' + factoryNo + '</td><td class="center tds td-factoryName" title="' + factoryName + '">' + factoryName + '</td><td class="center tds td-MarkCode" title="' + MarkCode + '">' + MarkCode + '</td><td class="center tds-3x"  data-id="' + factoryNo + '"><a  title="查看订单" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td></tr>'

        }
        h += ' </table>'
        $('.tr-del').remove();
        $('.rt_content .table tr:first').nextAll().remove();
        $('.rt_content .table').append(h);
        $('.rt_content .paging2').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging2').append(h);

        //tableFixedTest("fixtable")
    })
}

