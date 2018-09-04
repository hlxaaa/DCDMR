var orderField = 'id'
var desc = true;
var listSort;
var defaultField = 'id'
var isAdd = true;
var areaId;
var page = 0
var selectMeterNo = 0;
var lastName;
var ths = ''

$(document).ready(function () {

    $('body').delegate('.dealAlarm', 'click', function () {
        //layer.msg(1);
        var id = $(this).data('alarmid');
        var data = {
            id: id
        }
        jQuery.postNL('../alarmAjax/DealAlarm', data, function (data) {
            layer.msg(data.Message, {
                time: 500,
                end: function () {
                    //alarmCount -= 1;
                    changePage(page);
                }
            })
        })

    })

    ChangeDeviceList()
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

        ChangeDeviceList()
    })

    $('.input-lv98').change(function () {
        changePage(1);
        ChangeDeviceList()
    })

    changePage(1);
    //rbTips008(1)

    $('.name').keypress(function (e) {

    })
    $('body').delegate('.table tr .tds', 'click', function () {
        var devid = $(this).parent().find('.tds').last().data('devid')
        location.href = '/alarm/alarminfo?meterNo=' + devid
    })

    $('.meterNo').change(function () {
        selectMeterNo = $(this).val()
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
        customerNo: selectMeterNo,
        lastName: lastName
    }
    jQuery.postNL('../alarmAjax/GetList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {

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
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var { item, siteNo, communicateNo, Devid, DevTypeName, AlarmContent, AlarmTime, DealFlag, DealTime, DealOperator, SmsTimes, SmsSendTimes, SmsInvTime, Linkman, MobileNo, deviceNo, Id } = item;

            var dealStr = "已处理"
            var dealHtml = ''
            if (DealFlag == 0) {
                dealStr = "未处理"
                dealHtml = '<a class="inner_btn dealAlarm" data-alarmid="' + Id + '">设为已处理</a>';
            }


            h += '<tr><td class="center tds td-siteNo" title="' + siteNo + '">' + siteNo + '</td><td class="center tds td-communicateNo" title="' + communicateNo + '">' + communicateNo + '</td><td class="center tds td-deviceNo" title="' + deviceNo + '">' + deviceNo + '</td><td class="center tds td-DevTypeName" title="' + DevTypeName + '">' + DevTypeName + '</td><td class="center tds td-AlarmContent" title="' + AlarmContent + '">' + AlarmContent + '</td><td class="center tds td-AlarmTime" title="' + AlarmTime + '">' + AlarmTime + '</td><td class="center tds td-DealFlag" title="' + dealStr + '">' + dealStr + '</td><td class="center tds td-DealTime" title="' + DealTime + '">' + DealTime + '</td><td class="center tds td-DealOperator" title="' + DealOperator + '">' + DealOperator + '</td><td class="center tds td-SmsTimes" title="' + SmsTimes + '">' + SmsTimes + '</td><td class="center tds td-SmsSendTimes" title="' + SmsSendTimes + '">' + SmsSendTimes + '</td><td class="center tds td-SmsInvTime" title="' + SmsInvTime + '">' + SmsInvTime + '</td><td class="center tds td-Linkman" title="' + Linkman + '">' + Linkman + '</td><td data-devid="' + Devid + '" class="center tds td-MobileNo" title="' + MobileNo + '">' + MobileNo + '</td><td class="center" >' + dealHtml + '</td></tr>'
            //<td class="center "  data-id="' + id +'"><a  title="查看订单" class="link_icon btn-detail" target="_blank">&#118;</a><a  title="删除" class="link_icon btn-del">&#100;</a></td>
        }
        h += ' </table>'
        //$('.rt_content .table').remove();
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


function ChangeDeviceList() {
    GetLastName();
    var data1 = {
        name: lastName
    }
    jQuery.postNL('../deviceAjax/GetDeviceListByCompanyName', data1, function (data) {
        var list = data.ListData;
        $('.meterNo').children().remove();
        //var h = '<option value="0">全部</option>'
        //for (var i = 0; i < list.length; i++) {
        //    h += `<option value="${list[i].customerNo}">${list[i].customerName}</option>`
        //}
        var h = ''
        h += '<option value="0">全部客户</option>'
        for (var i = 0; i < list.length; i++) {
            h += `<option value="${list[i].customerNo}">${list[i].customerName}</option>`
        }

        $('.meterNo').append(h);
    })
}