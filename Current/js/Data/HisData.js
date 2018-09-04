var orderField = 'id'
var desc = true;
var listSort;
var defaultField = 'id'
var page = 1;
var time;
var dateStr;
var selectMeterNo = 0;
var lastName;
var ths = ''


var lastSuccessData = "";

$(document).ready(function () {
    start();
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
            ChangeDeviceList()
            changePage(1)
        })


    })

    $('.input-lv98').change(function () {
        ChangeDeviceList2(changePage)
        //changePage(1);

    })

    //changePage(1);

    $('.btn-getExcel').click(function () {
        jQuery.postNL('../dataAjax/GetExcelCount', "", function (data) {
            if (data.Message == "1") {
                TestExcel();
            } else {
                layer.msg('没有数据可以导出')
            }
        })
    })

    $('.meterNo').change(function () {
        selectMeterNo = $(this).val()
        //用注释的那个的，公司，分公司筛选的不行
        //location.href = '/data/hisdata?cno=' + selectMeterNo;
        changePage(1);
    })

    $('#test7').change(function () {
        //if ($(this).val() != '' && $('.s-month').val() != null) {
        //    var year = $(this).val();
        //    var month = $('.s-month').val();
        //    GetDaysHtml(year, month)
        //}
    })

    $('.s-month').change(function () {
        //if ($(this).val() != null && $('#test7').val() != '') {
        //    var month = $(this).val();
        //    var year = $('#test7').val();
        //    GetDaysHtml(year, month)
        //}
    })

    $('.s-day').mousemove(function () {
        if ($('.s-month').val() != null && $('.s-month').val() != '' && $('#test7').val() != '') {
            var month = $('.s-month').val()
            var year = $('#test7').val();
            GetDaysHtml(year, month)
        }
    })

    $('body').delegate('.layui-form-radioed', 'click', function () {
        //console.log($(this).hasClass('layui-form-radioed'))
        var temp = $(this).prev().val();
        if (temp == dateStr) {
            $(this).removeClass('layui-form-radioed')
            dateStr = '';
        } else {
            dateStr = temp;
        }
        //console.log(yStart);
        switch (dateStr) {
            case "year":
                console.log(1);
                $('#yStart').val(yStart)
                $('#yEnd').val(yEnd)
                ClearSelects()
                $('#yStart').css('display', 'block')
                $('#yEnd').css('display', 'block')
                break;
            case "month":
                $('#mStart').val(mStart)
                $('#mEnd').val(mEnd)
                ClearSelects()
                $('#mStart').css('display', 'block')
                $('#mEnd').css('display', 'block')
                break;
            case "day":
                $('#test5').val(dStart)
                $('#test6').val(dEnd)
                ClearSelects()
                $('#test5').css('display', 'block')
                $('#test6').css('display', 'block')
                break;
        }
        //$('#test5').val('')
        //$('#test6').val('')
        changePage(1);
    })

    //$('.radion-date').parent().find('i').click(function () {
    //    $('#test5').val('')
    //    $('#test6').val('')
    //    dateStr = $(this).val()
    //    changePage(1);
    //})

})

function ClearSelects() {
    $('#yStart').css('display', 'none')
    $('#yEnd').css('display', 'none')
    $('#mStart').css('display', 'none')
    $('#mEnd').css('display', 'none')
    $('#test5').css('display', 'none')
    $('#test6').css('display', 'none')
}

function ChangeDeviceList() {
    GetLastName();
    var data1 = {
        name: lastName
    }
    //console.log(data1);
    jQuery.postNL('../deviceAjax/GetDeviceListByCompanyName', data1, function (data) {
        var list = data.ListData;

        $('.meterNo').children().remove();
        var h = `<option value="999999999" selected>选择用户</option>`
        for (var i = 0; i < list.length; i++) {
            if (cno == list[i].customerNo) {
                h += `<option value="${list[i].customerNo}" selected>${list[i].customerName}</option>`
            } else {
                h += `<option value="${list[i].customerNo}">${list[i].customerName}</option>`
            }
        }
        //h += '<option value="0">全部客户</option>'
        $('.meterNo').append(h);
    })
}

function ChangeDeviceList2(callback) {
    GetLastName();
    var data1 = {
        name: lastName
    }
    //console.log(data1);
    jQuery.postNL('../deviceAjax/GetDeviceListByCompanyName', data1, function (data) {
        var list = data.ListData;
        $('.meterNo').children().remove();
        var h = `<option value="999999999" selected>选择用户</option>`
        for (var i = 0; i < list.length; i++) {
            if (cno == list[i].customerNo) {
                h += `<option value="${list[i].customerNo}" selected>${list[i].customerName}</option>`
            } else {
                h += `<option value="${list[i].customerNo}">${list[i].customerName}</option>`
            }

        }
        //h += '<option value="0">全部客户</option>'
        $('.meterNo').append(h);
        callback(1)
    })
}

function changePage(index) {
    if ($('.input-lv98').length > 0) {
        if ($('.input-lv98').val().length > 0)
            lastName = $('.input-lv98').val()
        else {
            if ($('.input-lv99').val().length > 0) {
                lastName = $('.input-lv99').val()
            } else
                lastName = '';
        }
    } else if ($('.input-lv99').length > 0) {
        if ($('.input-lv99').val().length > 0) {
            lastName = $('.input-lv99').val()
        } else
            lastName = '';
    } else
        lastName = '';

    page = index;
    listSort = new Array();

    var sTime = $('#test5').val()
    var eTime = $('#test6').val()
    if (sTime != '' || eTime != '')
        $('.layui-form-radio').removeClass('layui-form-radioed')


    $('.table th').each(function () {
        listSort.push($(this).attr('class'))
    })

    var data1;
    switch (dateStr) {
        case "year":
            data1 = {
                search: $('.input-search').val(),
                pageIndex: index,
                orderField: orderField,
                isDesc: desc,
                listSort: listSort,

                yStart: $('#yStart').val(),
                yEnd: $('#yEnd').val(),

                customerNo: $('.meterNo').val(),
                //year: $('#test7').val(),
                //month: $('.s-month').val(),
                //day: $('.s-day').val(),
                dateStr: dateStr,
                lastName: lastName
            }
            break;
        case "month":
            data1 = {
                search: $('.input-search').val(),
                pageIndex: index,
                orderField: orderField,
                isDesc: desc,
                listSort: listSort,

                mStart: $('#mStart').val(),
                mEnd: $('#mEnd').val(),
                customerNo: $('.meterNo').val(),
                //year: $('#test7').val(),
                //month: $('.s-month').val(),
                //day: $('.s-day').val(),
                dateStr: dateStr,
                lastName: lastName
            }
            break;
        default:
            data1 = {
                search: $('.input-search').val(),
                pageIndex: index,
                orderField: orderField,
                isDesc: desc,
                listSort: listSort,
                startTime: sTime,
                endTime: eTime,
                customerNo: $('.meterNo').val(),
                //year: $('#test7').val(),
                //month: $('.s-month').val(),
                //day: $('.s-day').val(),
                dateStr: dateStr,
                lastName: lastName
            }
            break;
    }
    //data1 = {
    //    search: $('.input-search').val(),
    //    pageIndex: index,
    //    orderField: orderField,
    //    isDesc: desc,
    //    listSort: listSort,
    //    startTime: sTime,
    //    endTime: eTime,
    //    yStart: $('#yStart').val(),
    //    yEnd: $('#yEnd').val(),
    //    mStart: $('#mStart').val(),
    //    mEnd: $('#mEnd').val(),
    //    customerNo: $('.meterNo').val(),
    //    //year: $('#test7').val(),
    //    //month: $('.s-month').val(),
    //    //day: $('.s-day').val(),
    //    dateStr: dateStr,
    //    lastName: lastName
    //}

    jQuery.postNL('../dataAjax/GetHisData', data1, function (data) {
        lastSuccessData = data1;
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            //$('.rt_content .table tr').remove();

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
        //var h = '<table class="table" id="fixtable"><tr><th class="' + listSort[0] + ' " data-sort="deviceNo">制造号</th><th class="' + listSort[1] + '" data-sort="communicateNo">通讯编号</th><th class="' + listSort[2] + ' " data-sort="StdSum">总累计量</th><th class="' + listSort[3] + ' " data-sort="WorkSum">工况总量</th><th class="' + listSort[4] + ' " data-sort="StdFlow">瞬时流量</th><th class="' + listSort[5] + ' " data-sort="WorkFlow">工况流量</th><th class="' + listSort[6] + ' " data-sort="Temperature">温度</th><th class="' + listSort[7] + ' " data-sort="Pressure">压力</th><th class="' + listSort[8] + '" data-sort="RemainMoney">剩余金额</th><th class="' + listSort[9] + '" data-sort="RemainVolume">剩余气量</th><th class="' + listSort[10] + ' " data-sort="PowerVoltage">供电电压</th><th class="' + listSort[11] + ' " data-sort="FMStateMsg">表具状态</th><th class="' + listSort[12] + ' " data-sort="customerName">客户名称</th><th class="' + listSort[13] + ' " data-sort="address">客户地址</th><th class="' + listSort[14] + ' " data-sort="InstantTime">采集时间</th></tr>';
        //<th>操作</th>
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var deviceNo = item.deviceNo == null ? "" : item.deviceNo
            var communicateNo = item.communicateNo == null ? "" : item.communicateNo
            var StdSum = item.StdSum == null ? "" : item.StdSum
            var WorkSum = item.WorkSum == null ? "" : item.WorkSum
            var StdFlow = item.StdFlow == null ? "" : item.StdFlow
            var WorkFlow = item.WorkFlow == null ? "" : item.WorkFlow
            var Temperature = item.Temperature == null ? "" : item.Temperature
            var Pressure = item.Pressure == null ? "" : item.Pressure
            var RemainMoney = item.RemainMoney == null ? "" : item.RemainMoney
            var RemainVolume = item.RemainVolume == null ? "" : item.RemainVolume
            var PowerVoltage = item.PowerVoltage == null ? "" : item.PowerVoltage
            var FMStateMsg = item.FMStateMsg == null ? "" : item.FMStateMsg
            FMStateMsg = FMStateMsg == '' ? '正常' : FMStateMsg
            var customerName = item.customerName == null ? "" : item.customerName
            var address = item.address == null ? "" : item.address
            var InstantTime = item.InstantTime == null ? "" : item.InstantTime
            InstantTime = ChangeTimeFormat(InstantTime)
            var id = item.rId;
            h += '<tr><td class="center tds td-deviceNo" title="' + deviceNo + '">' + deviceNo + '</td><td class="center tds td-communicateNo" title="' + communicateNo + '">' + communicateNo + '</td><td class="center tds td-StdSum" title="' + StdSum + '">' + StdSum + '</td><td class="center tds td-WorkSum" title="' + WorkSum + '">' + WorkSum + '</td><td class="center tds td-StdFlow" title="' + StdFlow + '">' + StdFlow + '</td><td class="center tds td-WorkFlow" title="' + WorkFlow + '">' + WorkFlow + '</td><td class="center tds td-Temperature" title="' + Temperature + '">' + Temperature + '</td><td class="center tds td-Pressure" title="' + Pressure + '">' + Pressure + '</td><td class="center tds td-RemainMoney" title="' + RemainMoney + '">' + RemainMoney + '</td><td class="center tds td-RemainVolume" title="' + RemainVolume + '">' + RemainVolume + '</td><td class="center tds td-PowerVoltage" title="' + PowerVoltage + '">' + PowerVoltage + '</td><td class="center tds td-FMStateMsg" title="' + FMStateMsg + '">' + FMStateMsg + '</td><td class="center tds td-customerName" title="' + customerName + '">' + customerName + '</td><td class="center tds td-address" title="' + address + '">' + address + '</td><td class="center tds td-InstantTime" title="' + InstantTime + '">' + InstantTime + '</td></tr>'
            //<td class="center tds-3x"  data-id="' + id + '"><a  title="查看订单" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td>
        }
        h += ' </table>'
        $('.tr-del').remove();
        $('.rt_content .table tr:first').nextAll().remove();
        $('.rt_content .table').append(h);
        //$('.rt_content .paging').before(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        //console.log(h);
        $('.rt_content .paging').append(h);

        tableFixedTest("fixtable")
    })
}

function GetDaysHtml(year, month) {
    var data = {
        year: year,
        month: month
    }
    jQuery.postNL('../dataAjax/GetMonthDays', data, function (data) {
        var h = data.ListData[0];
        $('.s-day option:eq(0)').nextAll().remove();
        $('.s-day option:eq(0)').after(h);
    })
}

function TestExcel() {
    location.href = '/dataAjax/GetHisDataExcel';
}

function start() {
    GetLastName();
    var data1 = {
        name: lastName
    }

    jQuery.postNL('../deviceAjax/GetDeviceListByCompanyName', data1, function (data) {
        var list = data.ListData;
        if (cno != 0) {
            var flag = false;
            for (var i = 0; i < list.length; i++) {
                if (list[i].customerNo == cno)
                    flag = true;
            }
            if (!flag) {
                layer.msg('该表暂时没有历史数据', {
                    time: 2000,
                    end: function () {
                        location.href = '/device/index'
                    }
                })
            }
        }

        $('.meterNo').children().remove();
        var h = `<option value="999999999" selected>选择用户</option>`
        for (var i = 0; i < list.length; i++) {
            if (cno == list[i].customerNo) {
                h += `<option value="${list[i].customerNo}" selected>${list[i].customerName}</option>`
            } else {
                h += `<option value="${list[i].customerNo}">${list[i].customerName}</option>`
            }
        }
        //h += '<option value="0">全部客户</option>'
        $('.meterNo').append(h);
        changePage(1)
    })
    //selectMeterNo = $('.meterNo').val();

}

//function func1(func2) {
//    console.log(1);
//    func2();
//}
//function func22() {
//    console.log(2);
//}