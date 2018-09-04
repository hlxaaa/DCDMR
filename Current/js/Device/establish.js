var customerNo;
var meterNo;
var arr;

$(document).ready(function () {

    $('.IsIC').change(function () {
        var v = $(this).val();
        if (v == 1) {
            $('.div-moneyOrVolume').css('display', 'block')
        } else {
            $('.div-moneyOrVolume').css('display', 'none')
        }
    })

    //设备类型变化，修改协议等
    $('.meterTypeNo').change(function () {
        var meterTypeNo = $(this).val();
        if (meterTypeNo == 11 || meterTypeNo == 13 /*|| meterTypeNo == 15*/) {
            $('.ProtocolNo').val(1001);
            $('.LinkMode').val(1);
            $('.IsIC').val(1);
            $('.div-IsIC').css('display', 'none');
            $('.div-moneyOrVolume').css('display', 'none');
        }
        else if (meterTypeNo == 14) {
            $('.div-IsIC').css('display', 'block');
            $('.div-moneyOrVolume').css('display', 'block');
            $('.IsIC').val(1)
            $('.ProtocolNo').val(1002);
        }
        else if (meterTypeNo == 15) {
            $('.ProtocolNo').val(1003);
            $('.div-IsIC').css('display', 'block');
            $('.div-moneyOrVolume').css('display', 'block');
            $('.IsIC').val(1)
            $('.LinkMode').val(1);
        }
        else if (meterTypeNo == 16) {
            $('.ProtocolNo').val(1004);
            $('.div-IsIC').css('display', 'block');
            $('.div-moneyOrVolume').css('display', 'block');
            $('.IsIC').val(1)
            $('.LinkMode').val(1);
        }
        else {
            $('.ProtocolNo').val(1002);
            $('.LinkMode').val(0);
            //$('.LinkMode').val(0)
        }
    });

    //$('.ProtocolNo').change(function () {
    //    var no = $(this).val();
    //    if (no == '1001') {
    //        $('.meterTypeNo').val(11);
    //    } else if (no == '1002') {
    //        $('.meterTypeNo').val(14);
    //    } else if (no == '1003') {
    //        $('.meterTypeNo').val(15);
    //    } else if (no == '1004')
    //        $('.meterTypeNo').val(16);

    //});

    //建点
    $('.btn-save').click(function () {
        jQuery.postNL('../deviceAjax/Establish', GetData(), function (data) {
            var r = data.ListData[0]
            layer.msg('建点成功', {
                time: 1000,
                end: function () {
                    location.href = `/user/guidePage?customerNo=${r.customerNo}`;
                }
            });
        });
    });

    //弹出已有的未开户的客户列表
    $('.btn-SelectCustomer').click(function () {
        $(".pop_bg").fadeIn();
        $('.pop_cont h3').html('选择客户');
        //return;
        jQuery.postNL('../deviceAjax/GetNotOpenCustomer', "", function (data) {
            var data = data.ListData;
            arr = data;
            var h = '<table class="table"><tr><th class="" data-sort="customerNo">客户编号</th><th class="" data-sort="customerName">客户名称</th><th class="" data-sort="mobileNo">电话</th><th class="" data-sort="address">地址</th><th>操作</th></tr>';
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var customerNo = item.customerNo;
                var customerName = item.customerName;
                var mobileNo = item.mobileNo;
                var address = item.address;
                h += '<tr><td class="center tds td-customerNo" title="' + customerNo + '">' + customerNo + '</td><td class="center tds td-customerName" title="' + customerName + '">' + customerName + '</td><td class="center tds td-mobileNo" title="' + mobileNo + '">' + mobileNo + '</td><td class="center tds td-address" title="' + address + '">' + address + '</td><td class="center "  data-id="' + customerNo + '"><a class="inner_btn btn-chooseCustomer">选择</a></td></tr>';
            }
            h += ' </table>';

            $('.pop_cont_input .table').remove();
            $('.pop_cont_input').append(h);

        });
    });

    //选择已有的未开户的客户
    $('body').delegate('.btn-chooseCustomer', 'click', function () {
        $(".pop_bg").fadeOut();
        var e = $(this).parent().parent();
        var customerNoTemp = e.find('.td-customerNo').html();
        customerNo = customerNoTemp;
        var customerName = e.find('.td-customerName').html();
        var mobileNo = e.find('.td-mobileNo').html();
        var address = e.find('.td-address').html();
        $('.customerName').val(customerName);
        $('.address').val(address);
        $('.mobileNo').val(mobileNo);
        //var customerNo = $(this).parent().data('id');
        //console.log(customerNo);
        //var r =  arr.find(function (x) {
        //    x.customerNo== customerNo;
        //})
        //console.log(r.customerNo);
    });

    //弹出已有的未开户的设备列表
    $('.btn-SelectDevice').click(function () {
        $(".pop_bg").fadeIn();
        $('.pop_cont h3').html('选择设备');
        jQuery.postNL('../deviceAjax/GetNotOpenDevice', GetData(), function (data) {
            var data = data.ListData;
            var h = '<table class="table"><tr><th class="" data-sort="meterNo">制造号</th><th class="" data-sort="communicateNo">通讯编号</th><th class="" data-sort="ProtocolNo">协议号</th><th class="" data-sort="meterTypeNo">设备类型</th><th>操作</th></tr>';
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var meterNo = item.meterNo;
                var deviceNo = item.deviceNo;
                var communicateNo = item.communicateNo;
                var ProtocolNo = item.ProtocolNo == null ? "" : item.ProtocolNo;
                var meterTypeNo = item.meterTypeNo;
                h += '<tr><td class="center tds td-meterNo" title="' + deviceNo + '">' + deviceNo + '</td><td class="center tds td-communicateNo" title="' + communicateNo + '">' + communicateNo + '</td><td class="center tds td-ProtocolNo" title="' + ProtocolNo + '">' + ProtocolNo + '</td><td class="center tds td-meterTypeNo" title="' + meterTypeNo + '">' + meterTypeNo + '</td><td class="center "  data-id="' + meterNo + '"><a class="inner_btn btn-chooseDevice">选择</a></td></tr>';
            }
            h += ' </table>';

            $('.pop_cont_input .table').remove();
            $('.pop_cont_input').append(h);
        });
    });

    //选择已有的未开户的设备
    $('body').delegate('.btn-chooseDevice', 'click', function () {
        $(".pop_bg").fadeOut();

        var e = $(this).parent();
        var customerNoTemp = e.data('id');
        meterNo = customerNoTemp;

        var data = {
            meterNo: meterNo
        };
        jQuery.postNL('../deviceAjax/GetDeviceByMeterNo', data, function (data) {
            var data = data.ListData[0];
            $('.factoryNo').val(data.factoryNo);
            var baseVolume = data.baseVolume == '' ? '0.000' : data.baseVolume;
            $('.baseVolume').val(baseVolume);

            $('.fluidNo').val(data.fluidNo);
            $('.communicateNo').val(data.communicateNo);
            $('.barCode').val(data.barCode);
            $('.caliber').val(data.caliber);
            $('.defineNo1').val(data.defineNo1);
            $('.remark').val(data.remark);
            $('.collectorNo').val(data.collectorNo);
            $('.AlarmTimes').val(data.AlarmTimes);
            $('.AlarmInvTime').val(data.AlarmInvTime);
            $('.ProtocolNo').val(data.ProtocolNo);
            $('.deviceNo').val(data.deviceNo);
            $('.IsIC').val(data.IsIC)
            $('.moneyOrVolume').val(data.moneyOrVolume)
            $('.userId').val(data.userId);
            $('.isEncrypt').val(data.isEncrypt);

            $('.meterTypeNo').val(data.meterTypeNo);
            var meterTypeNo = data.meterTypeNo;
            if (meterTypeNo == "11" || meterTypeNo == "13") {
                hideIC_MOV();
            } else
                showIC_MOV()
            var isIc = data.IsIC;
            if (isIc == 1)
                $('.div-moneyOrVolume').css('display', 'block');
            else
                $('.div-moneyOrVolume').css('display', 'none');

            //$('.defineNo2').val(data.defineNo2);
            //$('.defineNo3').val(data.defineNo3);
            //$('.StdFlowUpper').val(data.StdFlowUpper);
            //$('.StdFlowLow').val(data.StdFlowLow);
            //$('.WorkFlowUpper').val(data.WorkFlowUpper);
            //$('.WorkFlowLow').val(data.WorkFlowLow);
            //$('.PressUpper').val(data.PressUpper);
            //$('.PressLow').val(data.PressLow);
            //$('.TempUpper').val(data.TempUpper);
            //$('.TempLow').val(data.TempLow);
            //$('.RemainMoneyLow').val(data.RemainMoneyLow);
            //$('.RemainVolumLow').val(data.RemainVolumLow);
            //$('.OverMoneyUpper').val(data.OverMoneyUpper);
            //$('.OverVolumeUpper').val(data.OverVolumeUpper);
            //$('.PowerUpper').val(data.PowerUpper);
            //$('.PowerLow').val(data.PowerLow);
            //$('.BatteryLow').val(data.BatteryLow);
            //$('.DoorAlarm').val(data.DoorAlarm);




        });
    });

    //弹出文本性提示框
    $(".btn-add").click(function () {
        location.href = '/config/areadetail';
    });
    //弹出：确认按钮
    $(".trueBtn").click(function () {


    });
    //弹出：取消或关闭按钮
    $(".falseBtn").click(function () {
        $(".pop_bg").fadeOut();
    });

    //ESC监听，去除弹出框
    $('body').keyup(function (e) {
        //layer.msg(e.keyCode)
        if (e.keyCode == 27)
            $(".pop_bg").fadeOut();
    });

});

function showIC_MOV() {
    $('.div-IsIC').css('display', 'block');
    $('.div-moneyOrVolume').css('display', 'block');
}
function hideIC_MOV() {
    $('.div-IsIC').css('display', 'none');
    $('.div-moneyOrVolume').css('display', 'none');
}

//获取表单data
function GetData() {
    var lat;
    var lng;
    if (meterNo == null) {
        lat = map.BC.lat;
        lng = map.BC.lng;
    }

    var data = {
        moneyOrVolume: $('.moneyOrVolume').val(),
        isEncrypt: $('.isEncrypt').val(),
        userId: $('.userId').val(),
        IsIC: $('.IsIC').val(),
        meterNo: meterNo,
        factoryNo: $('.factoryNo').val(),
        baseVolume: $('.baseVolume').val(),
        meterTypeNo: $('.meterTypeNo').val(),
        fluidNo: $('.fluidNo').val(),
        communicateNo: $('.communicateNo').val(),
        barCode: $('.barCode').val(),
        caliber: $('.caliber').val(),
        defineNo1: $('.defineNo1').val(),
        defineNo2: $('.defineNo2').val(),
        defineNo3: $('.defineNo3').val(),
        remark: $('.remark').val(),
        collectorNo: $('.collectorNo').val(),
        StdFlowUpper: $('.StdFlowUpper').val(),
        StdFlowLow: $('.StdFlowLow').val(),
        WorkFlowUpper: $('.WorkFlowUpper').val(),
        WorkFlowLow: $('.WorkFlowLow').val(),
        PressUpper: $('.PressUpper').val(),
        PressLow: $('.PressLow').val(),
        TempUpper: $('.TempUpper').val(),
        TempLow: $('.TempLow').val(),
        RemainMoneyLow: $('.RemainMoneyLow').val(),
        RemainVolumLow: $('.RemainVolumLow').val(),
        OverMoneyUpper: $('.OverMoneyUpper').val(),
        OverVolumeUpper: $('.OverVolumeUpper').val(),
        PowerUpper: $('.PowerUpper').val(),
        PowerLow: $('.PowerLow').val(),
        BatteryLow: $('.BatteryLow').val(),
        DoorAlarm: $('.DoorAlarm').val(),
        AlarmTimes: $('.AlarmTimes').val(),
        AlarmInvTime: $('.AlarmInvTime').val(),
        ProtocolNo: $('.ProtocolNo').val(),
        deviceNo: $('.deviceNo').val(),
        customerNo: customerNo,
        customerName: $('.customerName').val(),
        address: $('.address').val(),
        mobileNo: $('.mobileNo').val(),
        CommMode: $('.CommMode').val(),
        LinkMode: $('.LinkMode').val(),
        lat: lat,
        lng: lng

    };
    return data;
}