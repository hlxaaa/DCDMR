var orderField = 'customerNo'
var desc = true;
var listSort;
var defaultField = 'customerNo'
var useState;
var customerType
var estateNo
var factoryNo
var meterType
var operator
var lastName;
var page;

$(document).ready(function () {

    //打印报表
    $('.btn-getExcel').click(function () {
        jQuery.postNL('../deviceAjax/GetCustomerExcelCount', "", function (data) {
            if (data.Message == "1") {
                TestExcel();
            } else {
                layer.msg('没有数据可以导出')
            }
        })
    })

    changePage(1);

    //新奥公司变化
    $('.input-lv99').change(function () {
        var name = $(this).val();
        var data = {
            name: name
        }
        jQuery.postNL('../userAjax/GetSonSelects', data, function (data) {
            var list = data.ListData;
            var h = ''
            for (var i = 0; i < list.length; i++) {
                h += `<option value="${list[i].name}"></option>`
            }
            $('#history2').children().remove()
            $('#history2').append(h);
            $('.input-lv98').val('')
            changePage(1)
        })
        //changePage(1)
    })

    //新奥分公司变化
    $('.input-lv98').change(function () {
        changePage(1);
    })

    //新增客户按钮
    $('.btn-addCustomer').click(function () {
        location.href = '/device/customer'
    })

    //进入客户详情页面
    $('body').delegate('.table .tds', 'click', function () {
        var no = $(this).parent().find('.center').last().data('id')
        location.href = '/device/customer?no=' + no;
    })

    //设备开户状态筛选
    $('.s-useState').change(function () {
        useState = $(this).val();
        changePage(page);
    })

    //客户类型筛选
    $('.s-customerType').change(function () {
        customerType = $(this).val();
        changePage(page);
    })

    //好像不用了
    $('.s-estateNo').change(function () {
        estateNo = $(this).val();
        changePage(page);
    })

    //设备厂家筛选
    $('.s-factoryNo').change(function () {
        factoryNo = $(this).val();
        changePage(page);
    })

    //设备类型筛选
    $('.s-meterType').change(function () {
        meterType = $(this).val();
        changePage(page);
    })

    //操作者筛选
    $('.s-operator').change(function () {
        operator = $(this).val();
        changePage(page);
    })

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
        useState: useState,
        customerType: customerType,
        estateNo: estateNo,
        factoryNo: factoryNo,
        meterType: meterType,
        operatorName: operator,
        lastName: lastName
    }
    jQuery.postNL('../deviceAjax/GetCustomerList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            //$('.rt_content .table tr').remove();
            //$('.rt_content .table tbody').append('<tr><th>没有相关信息</th></tr></tr>')

            if ($('.rt_content .table th').length > 1) {
                ths = $('.rt_content .table tr:first').html();
            }
            $('.rt_content .table tr').remove();
            $('.rt_content .table tbody').append('<tr class="tr-del"><th>没有相关信息</th></tr>')
            $('.rt_content .paging').children().remove();
            return;
        }

        var h = ''
        if ($('.rt_content .table th').length < 2) {
            h += `<tr>${ths}</tr>`;
        }
        //<th class="tds-x">操作</th>
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var customerNo = item.customerNo;
            var customerName = item.customerName;
            var customerType = item.customerType;
            var useState = '未启用'
            if (item.useState == 1)
                useState = '已使用'
            else if (item.useState == 2)
                useState = '已停用'
            var factoryName = item.factoryName;
            var meterTypeName = item.meterTypeName;
            var telNo = item.telNo;
            var mobileNo = item.mobileNo;
            var estateNo = item.estateNo;
            var estateName = item.estateName;
            var address = item.address;
            var defineNo1 = item.defineNo1;
            var defineNo2 = item.defineNo2;
            var defineNo3 = item.defineNo3;
            var remarks = item.remark;
            var buildTime = item.buildTime;
            var editTime = item.editTime;
            var Operator = item.Operator;
            h += '<tr><td class="center tds " title="' + customerNo + '">' + customerNo + '</td><td class="center tds" title="' + customerName + '">' + customerName + '</td><td class="center tds " title="' + customerType + '">' + customerType + '</td><td class="center tds " title="' + useState + '">' + useState + '</td><td class="center tds " title="' + factoryName + '">' + factoryName + '</td><td class="center tds " title="' + meterTypeName + '">' + meterTypeName + '</td><td class="center tds" title="' + telNo + '">' + telNo + '</td><td class="center tds " title="' + mobileNo + '">' + mobileNo + '</td><td class="center tds " title="' + estateName + '">' + estateName + '</td><td class="center tds " title="' + address + '">' + address + '</td><td class="center tds " title="' + defineNo1 + '">' + defineNo1 + '</td><td class="center tds " title="' + defineNo2 + '">' + defineNo2 + '</td><td class="center tds" title="' + defineNo3 + '">' + defineNo3 + '</td><td class="center tds " title="' + remarks + '">' + remarks + '</td><td class="center tds " title="' + buildTime + '">' + buildTime + '</td><td class="center tds " title="' + editTime + '">' + editTime + '</td><td class="center tds " data-id="' + customerNo + '" title="' + Operator + '">' + Operator + '</td></tr>'
            //<td class="center tds-x" data-id="' + customerNo + '"><a  title="查看订单" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td>
        }
        h += ' </table>'
        //$('.rt_content .table').remove();
        //$('.rt_content .paging').before(h);
        //$('.rt_content .paging').children().remove();
        //var h = getPageHtml(pages, index);
        //$('.rt_content .paging').append(h);
        $('.tr-del').remove();
        $('.rt_content .table tr:first').nextAll().remove();
        $('.rt_content .table').append(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging').append(h);

        tableFixedTest("fixtable")
    })
}

//打印报表
function TestExcel() {
    location.href = '/deviceAjax/GetCustomerExcel';
}