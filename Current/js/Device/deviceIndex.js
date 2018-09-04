var orderField = 'MeterNo'
var desc = true;
var listSort;
var defaultField = 'MeterNo'
var meterTypeNo
var factoryNo
var openState
var fluidNo
var Operator
var lastName
var page

$(document).ready(function () {

    $('body').delegate('.btn-toHis', 'click', function () {
        var cno = $(this).data('customerno')
        if (cno == '') {
            layer.msg('该表暂时没有历史数据');
            return;
        }
        location.href = '/data/hisdata?cno=' + cno;
    })

    //打印报表
    $('.btn-getExcel').click(function () {
        jQuery.postNL('../deviceAjax/GetDeviceExcelCount', "", function (data) {
            if (data.Message == "1") {
                TestExcel();
            } else {
                layer.msg('没有数据可以导出')
            }
        })
    })

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

    })

    //新奥分公司变化
    $('.input-lv98').change(function () {
        changePage(1);
    })


    //changePage(1);
    changePage(1);

    //跳转到新增设备页面
    $('.btn-addDevice').click(function () {
        location.href = '/device/device'
    })

    //$('body').delegate('.table .tds', 'click', function () {
    //    var no = $(this).parent().find('.center').last().data('id')
    //    location.href = '/device/device?no=' + no;
    //})

    //设备类型筛选
    $('.s-meterType').change(function () {
        meterTypeNo = $(this).val();
        changePage(page);
    })

    //厂家类型筛选
    $('.s-factoryNo').change(function () {
        factoryNo = $(this).val();
        changePage(page);
    })

    //开户状态筛选
    $('.s-openState').change(function () {
        openState = $(this).val();
        changePage(page);
    })

    //价格类型筛选
    $('.s-fluidNo').change(function () {
        fluidNo = $(this).val();
        changePage(page);
    })

    //操作者筛选
    $('.s-operator').change(function () {
        Operator = $(this).val();
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
        meterTypeNo: meterTypeNo,
        factoryNo: factoryNo,
        openState: openState,
        fluidNo: fluidNo,
        Operator: Operator,
        lastName: lastName
    }
    jQuery.postNL('../deviceAjax/GetMeterList', data, function (data) {
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
            var meterNo = item.meterNo;
            var deviceNo = item.deviceNo;
            var customerName = item.customerName
            var address = item.address
            var barCode = item.barCode
            var communicateNo = item.communicateNo
            var customerNo = item.customerNo
            var factoryName = item.factoryName
            var meterTypeName = item.meterTypeName
            var collectorNo = item.collectorNo
            var fluidName = item.fluidName
            var openStateName = item.openStateName
            var caliber = item.caliber
            var baseVolume = item.baseVolume
            var defineNo1 = item.defineNo1
            var defineNo2 = item.defineNo2
            var defineNo3 = item.defineNo3
            var remark = item.remark
            var buildTime = item.buildTime
            var editTime = item.editTime
            var Operator = item.Operator
            var pNo = item.ProtocolNo
            var imgStr = '<img class="tableImg" src="../current/images/sd-logo.ico" alt="Alternate Text" />'
            if (pNo == '1002')
                imgStr = '<img class="tableImg" src="../current/images/Logo.ico" alt="Alternate Text" />'

            var openState = item.openState;

            var osStr = `<i class="fa fa-check fa-2x text-info"></i>`
            if (openState != '1')
                osStr = `<i class="fa fa-times fa-2x text-danger"></i>`

            h += `<tr>
<td class="center tds" title="">${imgStr}</td>
<td class="center tds" title="${deviceNo}">${deviceNo}</td>
<td class="center tds" title="${communicateNo}">${communicateNo}</td>
<td class="center tds" title="${customerName}">${customerName}</td>
<td class="center tds" title="${address}">${address}</td>
<td class="center tds" title="${customerNo}">${customerNo}</td>
<td class="center tds" title="${meterTypeName}">${meterTypeName}</td>
<td class="center tds" title="${fluidName}">${fluidName}</td>
<td class="center tds" title="${openStateName}">${osStr}</td>
<td class="center tds" title="${editTime}">${editTime}</td>
<td class="center tds" ><a href="/device/device?no=${meterNo}" class="inner_btn" data-fmstate="0">查看详情</a><a  class="btn-valve inner_btn btn-toHis" data-customerno="${customerNo}">历史数据</a></td>
<td class="center tds-3x" data-id="${meterNo}"><a href="/alarm/deviceConfig?meterNo=${meterNo}" class="btn-valve inner_btn" data-fmstate="0">报警值设置</a><a href="/config/deviceLatlng?meterNo=${meterNo}" class="btn-valve inner_btn" data-fmstate="0">坐标设置</a></td></tr>`

            //h += '<tr><td class="center tds" title="">' + imgStr + '</td><td class="center tds" title="' + deviceNo + '">' + deviceNo + '</td><td class="center tds" title="' + communicateNo + '">' + communicateNo + '</td><td class="center tds" title="' + customerName + '">' + customerName + '</td><td class="center tds" title="' + address + '">' + address + '</td><td class="center tds" title="' + customerNo + '">' + customerNo + '</td><td class="center tds" title="' + meterTypeName + '">' + meterTypeName + '</td><td class="center tds" title="' + fluidName + '">' + fluidName + '</td><td class="center tds" title="' + openStateName + '">' + osStr + '</td><td class="center tds" title="' + editTime + '">' + editTime + '</td><td class="center tds" ><a href="/device/device?no=' + meterNo + '" class="inner_btn" data-fmstate="0">查看详情</a><a  class="btn-valve inner_btn btn-toHis" data-customerno="' + customerNo + '">历史数据</a></td><td class="center tds-3x" data-id="' + meterNo + '"><a href="/alarm/deviceConfig?meterNo=' + meterNo + '" class="btn-valve inner_btn" data-fmstate="0">报警值设置</a><a href="/config/deviceLatlng?meterNo=' + meterNo + '" class="btn-valve inner_btn" data-fmstate="0">坐标设置</a></td></tr>'


        }
        h += ' </table>'
        $('.tr-del').remove();
        $('.rt_content .table tr:first').nextAll().remove();
        $('.rt_content .table').append(h);
        $('.rt_content .paging2').children().remove();
        var h = getPageHtml(pages, index);
        $('.rt_content .paging2').append(h);

        tableFixedTest("fixtable")
    })
}

//打印报表
function TestExcel() {
    location.href = '/deviceAjax/GetDeviceExcel';
}