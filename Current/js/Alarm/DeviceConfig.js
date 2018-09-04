
$(document).ready(function () {
    $('.btn-save').click(function () {
        jQuery.postNL('../AlarmAjax/UpdateDeviceAlarmConfig', GetData(), function (data) {
            layer.msg(data.Message)
        })
    })
})

function GetData() {
    var data = {
        StdFlowUpper: $('.widget .StdFlowUpper').val(),
        StdFlowLow: $('.widget .StdFlowLow').val(),
        WorkFlowUpper: $('.widget .WorkFlowUpper').val(),
        WorkFlowLow: $('.widget .WorkFlowLow').val(),
        PressUpper: $('.widget .PressUpper').val(),
        PressLow: $('.widget .PressLow').val(),
        TempUpper: $('.widget .TempUpper').val(),
        TempLow: $('.widget .TempLow').val(),
        RemainMoneyLow: $('.widget .RemainMoneyLow').val(),
        RemainVolumLow: $('.widget .RemainVolumLow').val(),
        OverMoneyUpper: $('.widget .OverMoneyUpper').val(),
        OverVolumeUpper: $('.widget .OverVolumeUpper').val(),
        PowerUpper: $('.widget .PowerUpper').val(),
        PowerLow: $('.widget .PowerLow').val(),
        BatteryLow: $('.widget .BatteryLow').val(),
        DoorAlarm: $('.widget .DoorAlarm').val(),
        meterNo: $('.btn-save').data('meterno')
    }
    return data;
}