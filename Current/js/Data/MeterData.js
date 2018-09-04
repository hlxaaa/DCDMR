var orderField = 'id'
var desc = true;
var listSort;
var defaultField = 'id'
var page = 1;
var time;
var lastName;
var server
var commNo

var tabIndex = 0;

$(document).ready(function () {

    //打印报表
    $('.btn-getExcel').click(function () {
        jQuery.postNL('../dataAjax/GetMeterDataCount', "", function (data) {
            if (data.Message == "1") {
                TestExcel();
            } else {
                layer.msg('没有数据可以导出')
            }
        })
    })

    //新奥公司改变
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

    //新奥分公司改变
    $('.input-lv98').change(function () {
        changePage(1);
    })

    //定时刷新
    setInterval(refresh, parseInt(time) * 1000)

    changePage(1);

    //开关阀
    $('.btn-valve').click(function () {
        var data = {
            pId: 1
        }
        jQuery.postNL('../userAjax/IsOpenPermission', data, function () {
            layer.prompt({ title: '输入操作密码，并确认', formType: 1 }, function (pass, index) {
                layer.close(index);
                //layer.prompt({ title: '随便写点啥，并确认', formType: 2 }, function (text, index) {
                //    layer.close(index);
                //    layer.msg('演示完毕！您的口令：' + pass + '<br>您最后写下了：' + text);
                //});
                var data = {
                    pwd: pass,
                    oper: $('.btn-valve').val(),
                    commNo: commNo
                }
                jQuery.postNL('../dataAjax/ValveOper', data, function (data) {
                    layer.msg(data.Message);
                    changePage(page)
                })
            });
        })



    })

    //充值（后台应该还没好
    $('.btn-charge').click(function () {
        var data = {
            pId: 2
        }
        jQuery.postNL('../userAjax/IsOpenPermission', data, function () {
            layer.prompt({ title: '输入操作密码，并确认', formType: 1 }, function (pass, index) {
                layer.close(index);
                layer.prompt({ title: '输入金额，并确认', formType: 1 }, function (text, index) {
                    layer.close(index);
                    //layer.msg('演示完毕！您的口令：' + pass + '<br>您最后写下了：' + text);
                    var data = {
                        commNo: commNo,
                        pwd: pass,
                        money: text
                    }
                    jQuery.postNL('../dataAjax/ChargeOper', data, function (data) {
                        layer.msg(data.Message);
                        changePage(page)
                    })

                });



                //var data = {
                //    pwd: pass,
                //    oper: $('.btn-valve').val(),
                //    commNo: commNo
                //}
                //jQuery.postNL('../dataAjax/ValveOper', data, function (data) {
                //    layer.msg(data.Message);
                //    changePage(page)
                //})
            });
        })
    })

    //点击远程操作事件
    $('body').delegate('.btn-oper', 'click', function () {
        var state = $(this).data('state')
        var valve = $(this).data('valve')
        var deviceNo = $(this).data('deviceno');
        commNo = $(this).data('commno')
        if (state == 0)
            layer.msg('离线设备无法操作')
        else {

            $('.oper-deviceNo').html('正在操作表 ' + deviceNo);

            $(".pop_bg").fadeIn();
            if (valve == 1) {
                $('.btn-valve').val('关阀')
                //$('.btn-valve').data('valve', '0')
            } else {
                $('.btn-valve').val('开阀')
                //$('.btn-valve').data('valve', '1')
            }
        }
    })

    //关闭弹出框
    $('.falseBtn').click(function () {
        $('.pop_bg').fadeOut();
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
        lastName: lastName
    }
    jQuery.postNL('../dataAjax/GetMeterDataList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            //$('.rt_content .table tr').remove();
            //$('.rt_content .table th').remove();
            //$('.rt_content .table tbody').append('<th>没有相关信息</th>')

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
            var LoginState = item.LoginState == null ? "" : item.LoginState
            var DeviceNo = item.deviceNo == null ? "" : item.deviceNo
            var communicateNo = item.communicateNo == null ? "" : item.communicateNo
            var StdSum = item.StdSum == null ? "" : item.StdSum
            var SumTotal = item.SumTotal == null ? "" : item.SumTotal
            var WorkSum = item.WorkSum == null ? "" : item.WorkSum
            var StdFlow = item.StdFlow == null ? "" : item.StdFlow
            var WorkFlow = item.WorkFlow == null ? "" : item.WorkFlow
            var Temperature = item.Temperature == null ? "" : item.Temperature
            var Pressure = item.Pressure == null ? "" : item.Pressure
            var RemainMoney = item.RemainMoney == null ? "" : item.RemainMoney
            var RemainVolume = item.RemainVolume == null ? "" : item.RemainVolume
            var IsIC = item.IsIC;
            var meterTypeNo = item.meterTypeNo;
            //if (meterTypeNo == '11') {//一体机计量
            //    RemainMoney = '-'
            //} else if (meterTypeNo == '13') {//一体机计费
            //    RemainVolume = '-'
            //} else if (meterTypeNo == '14') {//鸿鹄RTU

            //} else if (meterTypeNo == '15') {//信东RTU
            //    RemainMoney = '-'
            //    RemainVolume = '-'
            //}

            var PowerVoltage = item.PowerVoltage == null ? "" : item.PowerVoltage
            var FMStateMsg = item.FMStateMsg == null ? "" : item.FMStateMsg
            //FMStateMsg = FMStateMsg == "" ? "正常" : FMStateMsg
            var InstantTime = item.InstantTime == null ? "" : item.InstantTime
            var customerName = item.customerName == null ? "" : item.customerName
            var address = item.address == null ? "" : item.address
            var remark = item.remark == null ? "" : item.remark
            var id = item.Id;
            var valveState = item.ValveState
            var ValveStateMsg = item.ValveStateMsg
            var lsStr = '<i class="fa fa-rss fa-2x text-info"></i>';
            if (LoginState != 1) {
                lsStr = '<i class="fa fa-ban fa-2x text-danger"></i>';
            }
            //var lsStr = '<img title="通讯状态开" src="../current/images/devicestatus/通讯状态开.png" />';
            //if (LoginState != 1) {
            //    lsStr = '<img title="通讯状态关" src="../current/images/devicestatus/通讯状态关.png" />';
            //}

            h += ' <tr><td class="center tds td-LoginState">' + lsStr + '</td><td class="center tds td-DeviceNo" title="' + DeviceNo + '">' + DeviceNo + '</td><td class="center tds td-communicateNo" title="' + communicateNo + '">' + communicateNo + '</td><td class="center tds td-customerName" title="' + customerName + '">' + customerName + '</td><td class="center tds td-address" title="' + address + '">' + address + '</td>'

            //if (server != '') {
            //    h += '<td class="center tds td-StdSum" title="' + SumTotal + '">' + SumTotal + '</td>'
            //} else {
            //    h += '<td class="center tds td-StdSum" title="' + StdSum + '">' + StdSum + '</td>'
            //}
            h += '<td class="center tds td-StdSum" title="' + SumTotal + '">' + SumTotal + '</td>'
            h += '<td class="center tds td-StdSum" title="' + StdSum + '">' + StdSum + '</td>'

            h += '<td class="center tds td-WorkSum" title="' + WorkSum + '">' + WorkSum + '</td><td class="center tds td-StdFlow" title="' + StdFlow + '">' + StdFlow + '</td><td class="center tds td-WorkFlow" title="' + WorkFlow + '">' + WorkFlow + '</td><td class="center tds td-Temperature" title="' + Temperature + '">' + Temperature + '</td><td class="center tds td-Pressure" title="' + Pressure + '">' + Pressure + '</td><td class="center tds td-RemainMoney" title="' + RemainMoney + '">' + RemainMoney + '</td><td class="center tds td-RemainVolume" title="' + RemainVolume + '">' + RemainVolume + '</td><td class="center tds td-PowerVoltage" title="' + PowerVoltage + '">' + PowerVoltage + '</td><td class="center tds td-FMStateMsg'

            var vsImg = `<img class="tableImg" src="../current/images/deviceStatus/阀门关.png" alt="Alternate Text">`
            if (IsIC == 0) {
                vsImg = `-`
            } else if (valveState == '1')
                vsImg = `<img class="tableImg" src="../current/images/deviceStatus/阀门开.png" alt="Alternate Text">`

            if (server == 'gt')
                h += '" title="' + FMStateMsg + '">' + GetStatusImg2(FMStateMsg) + '</td>'
            else
                h += '" title="' + FMStateMsg + '">' + GetStatusImg(FMStateMsg) + '</td>'

            h += '<td class="center tds td-InstantTime" title="' + ValveStateMsg + '">' + vsImg + '</td><td class="center tds td-WorkSum" title="' + remark + '">' + remark + '</td><td class="center tds td-InstantTime" title="' + InstantTime + '">' + InstantTime + '</td><td class="center"  data-id="' + id + '">'

            var ctrl = '<a class="btn-oper inner_btn" data-state="' + LoginState + '"  data-valve="' + valveState + '" data-commno="' + communicateNo + '" data-deviceno="' + DeviceNo + '">远程控制</a>'
            if (IsIC == 0)
                ctrl = '-'
            h += ctrl;
            h += '</td></tr>'
        }
        h += ' </table>'
        $('.tr-del').remove();
        $('.rt_content .table tr:first').nextAll().remove();
        $('.rt_content .table tbody').append(h);
        //$('.rt_content .paging').before(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        //console.log(h);
        $('.rt_content .paging').append(h);
        //$('.rt_content .table').remove();
        //$('.rt_content .paging').before(h);
        //$('.rt_content .paging').children().remove();
        //var h = getPageHtml(pages, index);
        //$('.rt_content .paging').append(h);

        tableFixedTest("fixTable");
    })
}

function refresh() {
    changePage(page);
}

//珠海设备状态图片
function GetStatusImg2(FMStateMsg) {
    var imgLack = '<img title="电池不足" src="../current/images/devicestatus/电池不足.png" />'
    var imgNormal = '<img title="电池正常" src="../current/images/devicestatus/电池正常.png" />'
    var imgValveClose = '<img title="阀门关" src="../current/images/devicestatus/阀门关.png" />'
    var imgValveOpen = '<img title="阀门开" src="../current/images/devicestatus/阀门开.png" />'
    var imgVolumeLess = '<img title="购气不足" src="../current/images/devicestatus/购气不足.png" />'
    var imgVolumeNormal = '<img title="购气正常" src="../current/images/devicestatus/购气正常.png" />'
    var imgDoorClose = '<img title="柜门关" src="../current/images/devicestatus/柜门关.png" />'
    var imgDoorOpen = '<img title="柜门开" src="../current/images/devicestatus/柜门开.png" />'
    var imgElectricColse = '<img title="市电状态关" src="../current/images/devicestatus/市电状态关.png" />'
    var imgElectricOpen = '<img title="市电状态开" src="../current/images/devicestatus/市电状态开.png" />'
    var imgOffLine = '<img title="通讯状态关" src="../current/images/devicestatus/通讯状态关.png" />'
    var imgOnLine = '<img title="通讯状态开" src="../current/images/devicestatus/通讯状态开.png" />'
    var imgOverdraft = '<img title="透支" src="../current/images/devicestatus/透支1.png" />'
    var imgNotOver = '<img title="未透支" src="../current/images/devicestatus/未透支.png" />'
    var h = '';
    var arr = FMStateMsg.split(";");
    //console.log(arr)
    if (FMStateMsg == '')
        return '正常'
    if (FMStateMsg.indexOf('电池异常') > -1) {
        h += imgLack;
    }
    if (FMStateMsg.indexOf('压力异常') > -1) {
        h += '压力异常';
    }
    if (FMStateMsg.indexOf('温度异常') > -1) {
        h += '温度异常';
    }
    if (FMStateMsg.indexOf('流量异常') > -1) {
        h += '流量异常';
    }
    return h;
}

//信东设备状态图片
function GetStatusImg(FMStateMsg) {
    var imgs = new Array();

    var imgLack = '<div class="stateImg"><img title="电池不足" src="../current/images/devicestatus/电池不足.png" /></div>'
    //var imgNormal = '<img title="电池正常" src="../current/images/devicestatus/电池正常.png" />'
    //var imgValveClose = '<div class="stateImg"><img title="阀门关" src="../current/images/devicestatus/阀门关.png" /></div>'
    //var imgValveOpen = '<div class="stateImg"><img title="阀门开" src="../current/images/devicestatus/阀门开.png" /></div>'
    var imgVolumeLess = '<div class="stateImg"><img title="购气不足" src="../current/images/devicestatus/购气不足.png" /></div>'
    //var imgVolumeNormal = '<img title="购气正常" src="../current/images/devicestatus/购气正常.png" />'
    var imgDoorClose = '<div class="stateImg"><img title="柜门关" src="../current/images/devicestatus/柜门关.png" /></div>'
    //var imgDoorOpen = '<img title="柜门开" src="../current/images/devicestatus/柜门开.png" />'
    var imgElectricColse = '<div class="stateImg"><img title="市电状态关" src="../current/images/devicestatus/市电状态关.png" /></div>'
    //var imgElectricOpen = '<img title="市电状态开" src="../current/images/devicestatus/市电状态开.png" />'
    var imgOffLine = '<div class="stateImg"><img title="通讯状态关" src="../current/images/devicestatus/通讯状态关.png" /></div>'
    //var imgOnLine = '<img title="通讯状态开" src="../current/images/devicestatus/通讯状态开.png" />'
    var imgOverdraft = '<div class="stateImg"><img title="透支" src="../current/images/devicestatus/透支1.png" /></div>'
    //var imgNotOver = '<img title="未透支" src="../current/images/devicestatus/未透支.png" />'

    var imgLack2 = '<img title="电池不足" src="../current/images/devicestatus/电池不足.png" />'
    var imgVolumeLess2 = '<img title="购气不足" src="../current/images/devicestatus/购气不足.png" />'
    var imgDoorClose2 = '<img title="柜门关" src="../current/images/devicestatus/柜门关.png" />'
    var imgElectricColse2 = '<img title="市电状态关" src="../current/images/devicestatus/市电状态关.png" />'
    var imgOffLine2 = '<img title="通讯状态关" src="../current/images/devicestatus/通讯状态关.png" />'
    var imgOverdraft2 = '<img title="透支" src="../current/images/devicestatus/透支1.png" />'

    var h = '';
    var h2 = '';
    var arr = FMStateMsg.split(";");
    //console.log(arr)
    if (FMStateMsg == '')
        return '正常'
    if (FMStateMsg.indexOf('电池不足') > -1) {
        h += imgLack;
        imgs.push(imgLack2);
    }

    if (FMStateMsg.indexOf('购气不足') > -1) {
        h += imgVolumeLess;
        imgs.push(imgVolumeLess2);
    }
    for (var i = 0; i < arr.length; i++) {
        switch (arr[i]) {
            //case '阀门关':
            //    h += imgValveClose;
            //    break;
            case '柜门关':
                h += imgDoorClose;
                imgs.push(imgDoorClose2);
                break;
            case '市电断电':
                h += imgElectricColse;
                imgs.push(imgElectricColse2);
                break;
            case '通讯异常':
                h += imgOffLine;
                imgs.push(imgOffLine2);
                break;
            case '透支':
                h += imgOverdraft;
                imgs.push(imgOverdraft2);
                break;
        }
    }
    var l = imgs.length;
    //layer.msg(l % 2)
    if (imgs.length % 2 == 0) {
        for (var i = 0; i < imgs.length; i++) {
            h2 += `<div class="stateImg">${imgs[i]}</div>`
        }
    } else {
        for (var i = 0; i < imgs.length - 1; i++) {
            h2 += `<div class="stateImg">${imgs[i]}</div>`
        }
        h2 += imgs[imgs.length - 1]
    }

    h2 += '<div class="fc"></div>';
    h += '<div class="fc"></div>';
    return h2;
}

//打印报表
function TestExcel() {
    location.href = '/dataAjax/GetMeterDataExcel';
}