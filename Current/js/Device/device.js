var r;
var no;
$(document).ready(function () {

    $('.IsIC').change(function () {
        var v = $(this).val();
        if (v == 1) {
            $('.div-moneyOrVolume').css('display', 'block')
        } else {
            $('.div-moneyOrVolume').css('display', 'none')
        }
    })

    //设备类型变化，同步协议和连接模式
    $('.meterTypeNo').change(function () {
        var meterTypeNo = $(this).val();
        if (meterTypeNo == 11 || meterTypeNo == 13) {
            $('.ProtocolNo').val(1001);
            $('.LinkMode').val(1);
            $('.IsIC').val(1);
            $('.div-IsIC').css('display', 'none');
            $('.div-moneyOrVolume').css('display', 'none');
        } else if (meterTypeNo == 14) {
            $('.div-IsIC').css('display', 'block');
            $('.div-moneyOrVolume').css('display', 'block');
            $('.IsIC').val(1)
            $('.ProtocolNo').val(1002);
        } else if (meterTypeNo == 15) {
            $('.ProtocolNo').val(1003);
            $('.LinkMode').val(1);
            $('.div-IsIC').css('display', 'block');
            $('.div-moneyOrVolume').css('display', 'block');
            $('.IsIC').val(1)
        } else if (meterTypeNo == 16) {
            $('.ProtocolNo').val(1004);
            $('.div-IsIC').css('display', 'block');
            $('.div-moneyOrVolume').css('display', 'block');
            $('.IsIC').val(1)
            $('.LinkMode').val(1);
        } else {
            $('.ProtocolNo').val(1002);
            $('.LinkMode').val(0);
        }
    });

    //协议变化，同步连接模式
    $('.ProtocolNo').change(function () {
        var pNo = $(this).val();

        //if (pNo == '1001') {
        //    $('.meterTypeNo').val(11);
        //} else if (pNo == '1002') {
        //    $('.meterTypeNo').val(14);
        //} else if (pNo == '1003') {
        //    $('.meterTypeNo').val(15);
        //} else if (pNo == '1004')
        //    $('.meterTypeNo').val(16);

        if (pNo == '1001')
            $('.LinkMode').val(1);
        else
            $('.LinkMode').val(0);
    });

    //删除设备
    $('.btn-delDevice').click(function () {
        var data = {
            meterNo: $(this).data('meterno')
        };

        layer.confirm('确认删除吗？', {
            btn: ['确定', '取消']
        }, function () {
            jQuery.postNL('../deviceAjax/DeleteDevice', data, function (data) {
                layer.msg(data.Message, {
                    time: 1000,
                    end: function () {
                        location.href = '/device/index';
                    }
                });
            });
            layer.closeAll('dialog');
        }, function () {
            layer.closeAll('dialog');
        })



    });

    //新增、更新设备
    $('.btn-save').click(function () {
        //新增
        if (no == -1) {
            //return;

            jQuery.postNL('../deviceAjax/AddDevice', GetData(), function (data) {
                layer.msg('添加成功,可继续添加', {
                    time: 1000,
                    end: function () {
                        location.reload();
                    }
                });
            });
        }
        //更新
        else {
            jQuery.postNL('../deviceAjax/UpdateDevice', GetData(), function (data) {
                layer.msg('更新成功', {
                    time: 1000,
                    end: function () {
                        location.href = '/device/index';
                    }
                });
            });
        }
    });

    $('.LinkMode').change(function () {
        var v = $(this).val();
        if (v == 1) {
            $('.CommMode').val(0)
        } else if (v == 0)
            $('.CommMode').val(1)
    })

    ClearInput()
});

function ClearInput() {
    console.log(1);
    if ($('.meterTypeNo').val() == '11' || $('.meterTypeNo').val() == '13') {
        $('.div-IsIC').css('display', 'none');
        $('.div-moneyOrVolume').css('display', 'none');
    }
}

//获取表单里的data
function GetData() {
    var data = {
        userId: $('.userId').val(),
        isEncrypt: $('.isEncrypt').val(),
        moneyOrVolume: $('.moneyOrVolume').val(),
        IsIC: $('.IsIC').val(),
        ScadaInvTime: $('.ScadaInvTime').val(),
        AlarmTimes: $('.AlarmTimes').val(),
        AlarmInvTime: $('.AlarmInvTime').val(),
        ProtocolNo: $('.ProtocolNo').val(),
        deviceNo: $('.deviceNo').val(),
        lat: map.BC.lat,
        lng: map.BC.lng,
        meterNo: $('.meterNo').val(),
        factoryNo: $('.factoryNo').val(),
        openState: $('.openState').val(),
        baseVolume: $('.baseVolume').val(),
        meterTypeNo: $('.meterTypeNo').val(),
        fluidNo: $('.fluidNo').val(),
        communicateNo: $('.communicateNo').val(),
        barCode: $('.barCode').val(),
        caliber: $('.caliber').val(),
        defineNo1: $('.defineNo1').val(),
        CommMode: $('.CommMode').val(),
        LinkMode: $('.LinkMode').val(),
        remark: $('.remark').val(),
        collectorNo: $('.collectorNo').val(),
        //lat: $('.lat').val(),
        //lng: $('.lng').val(),
        //defineNo2: $('.defineNo2').val(),
        //defineNo3: $('.defineNo3').val(),
        //StdFlowUpper: $('.StdFlowUpper').val(),
        //StdFlowLow: $('.StdFlowLow').val(),
        //WorkFlowUpper: $('.WorkFlowUpper').val(),
        //WorkFlowLow: $('.WorkFlowLow').val(),
        //PressUpper: $('.PressUpper').val(),
        //PressLow: $('.PressLow').val(),
        //TempUpper: $('.TempUpper').val(),
        //TempLow: $('.TempLow').val(),
        //RemainMoneyLow: $('.RemainMoneyLow').val(),
        //RemainVolumLow: $('.RemainVolumLow').val(),
        //OverMoneyUpper: $('.OverMoneyUpper').val(),
        //OverVolumeUpper: $('.OverVolumeUpper').val(),
        //PowerUpper: $('.PowerUpper').val(),
        //PowerLow: $('.PowerLow').val(),
        //BatteryLow: $('.BatteryLow').val(),
        //DoorAlarm: $('.DoorAlarm').val(),
    };
    return data;
}