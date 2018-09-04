$(document).ready(function () {

    //定时查询有没有新的报警，有就右下角弹出来提示
    GetNewAlarm()

    clearInterval(t);
    var t = setInterval(GetNewAlarm, 10 * 1000)

})

//查询报警信息里有没有新的
function GetNewAlarm() {

    //var data = {
    //    oldCount: alarmCount
    //}
    var data = {
        oldLastId: lastAlarmId
    }

    jQuery.postNL('../alarmAjax/CheckNewAlarm3', data, function (data) {
        var list = data.ListData;
        if (list.length > 0) {
            //alarmCount += list.length;
            lastAlarmId = list[0].biggestId;

            for (var i = 0; i < list.length; i++) {
                rbTips008(list[i].devid)
            }
        }
    })
}


function testLists() {
    var list1 = new Array();
    var list2 = new Array();

    var model1 = { name: 'txy', sex: '1' }
    var model2 = { name: 'wz', sex: '0' }

    list1.push(model1);
    list1.push(model2);

    var model3 = { str: 'str1', str2: 'str2' }
    var model4 = { str: 'str3', str2: 'str4' }

    list2.push(model3);
    list2.push(model4);

    var data = {
        list1: list1,
        list2: list2,
        title: 'this is title'
    }

    jQuery.postNL('../testAjax/testManyList', data, function (data) {

    })
}