var $border_color = "#efefef";
var $grid_color = "#ddd";
var $default_black = "#666";
var $green = "#8ecf67";
var $yellow = "#fac567";
var $orange = "#F08C56";
var $blue = "#1e91cf";
var $red = "#f74e4d";
var $teal = "#28D8CA";
var $grey = "#999999";
var $dark_blue = "#0D4F8B";


var v31 = [[1325376000000, 0], [1328054400000, 0], [1330732800000, 0], [1333411200000, 0], [1336089600000, 0], [1338768000000, 0], [1341446400000, 0], [1344124800000, 0], [1346803200000, 0], [1349481600000, 0], [1352160000000, 0], [1354838400000, 0], [1357516800000, 0], [1360195200000, 0], [1362873600000, 0], [1365552000000, 0], [1368230400000, 0], [1370908800000, 0], [1373587200000, 0], [1376265600000, 0], [1378944000000, 0], [1381622400000, 0], [1384300800000, 0], [1386979200000, 0], [1389657600000, 0], [1392336000000, 0], [1395014400000, 0], [1397692800000, 0], [1400371200000, 0], [1403049600000, 0], [1405728000000, 0]]
var v12 = [[1325376000000, 0], [1328054400000, 0], [1330732800000, 0], [1333411200000, 0], [1336089600000, 0], [1338768000000, 0], [1341446400000, 0], [1344124800000, 0], [1346803200000, 0], [1349481600000, 0], [1352160000000, 0], [1354838400000, 0]]

var vIntList12 = [1325376000000, 1328054400000, 1330732800000, 1333411200000, 1336089600000, 1338768000000, 1341446400000, 1344124800000, 1346803200000, 1349481600000, 1352160000000, 1354838400000]

$(function () {

    //var d1, d2, d3, data, chartOptions;

    //d1 = [
    //    [1325376000000, 1200], [1328054400000, 700], [1330560000000, 1000], [1333238400000, 600],
    //    [1335830400000, 350]
    //];

    //d2 = [
    //    [1325376000000, 800], [1328054400000, 600], [1330560000000, 300], [1333238400000, 350],
    //    [1335830400000, 300]
    //];

    //d3 = [
    //    [1325376000000, 650], [1328054400000, 450], [1330560000000, 150], [1333238400000, 200],
    //    [1335830400000, 150]
    //];

    //data = [{
    //    label: 'Windows',
    //    data: d1
    //}, {
    //    label: 'Android',
    //    data: d2
    //}, {
    //    label: 'Apple',
    //    data: d3
    //}];

    //chartOptions = {
    //    xaxis: {
    //        min: (new Date(2011, 11, 15)).getTime(),
    //        max: (new Date(2012, 04, 18)).getTime(),
    //        mode: "time",
    //        tickSize: [1, "month"],
    //        monthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
    //        tickLength: 0
    //    },
    //    grid: {
    //        hoverable: true,
    //        clickable: false,
    //        borderWidth: 1,
    //        tickColor: $border_color,
    //        borderColor: $grid_color,
    //    },
    //    bars: {
    //        show: true,
    //        barWidth: 12 * 24 * 60 * 60 * 300,
    //        fill: true,
    //        lineWidth: 1,
    //        order: true,
    //        lineWidth: 0,
    //        fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }] }
    //    },
    //    shadowSize: 0,
    //    tooltip: true,
    //    tooltipOpts: {
    //        content: '%s: %y'
    //    },
    //    colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    //}

    //var holder = $('#vertical-chart');

    //if (holder.length) {
    //    $.plot(holder, data, chartOptions);
    //}

});


function YearVer(listData, xNames) {
    GetVerChartForYear(listData, xNames, vIntList12, months)
}
function DayVer(listData, xNames) {
    GetVerChartForDay(listData, xNames, x24Temp, months)
}
function MonthVer31(listData, xNames) {
    GetVerChartForMonth(listData, xNames, monthDays31, 31)
}
function MonthVer30(listData, xNames) {
    GetVerChartForMonth(listData, xNames, monthDays30, 30)
}
function MonthVer29(listData, xNames) {
    GetVerChartForMonth(listData, xNames, monthDays29, 29)
}
function MonthVer28(listData, xNames) {
    GetVerChartForMonth(listData, xNames, monthDays28, 28)
}

function GetChargeVer(listData, xNames, vIntList12, monthNames) {
    var chartOptions;
    var data = new Array();
    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: VerGetData(listData[i], vIntList12)
        }
        data.push(temp);
    }
    chartOptions = {
        xaxis: {
            //min: (new Date(2011, 11, 15)).getTime(),
            //max: (new Date(2012, 04, 18)).getTime(),
            mode: "time",
            tickSize: [1, "month"],
            monthNames: monthNames,
            tickLength: 0
        },
        grid: {
            hoverable: true,
            clickable: false,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        bars: {
            show: true,
            barWidth: 12 * 24 * 60 * 60 * 300,
            fill: true,
            lineWidth: 1,
            order: true,
            lineWidth: 0,
            fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }] }
        },
        shadowSize: 0,
        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    }

    var holder = $('#vertical-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }

}

function GetVerChartForYear(listData, xNames, vIntList12, monthNames) {
    var chartOptions;
    var data = new Array();
    var dateHere = new Date(date)
    var year = dateHere.getFullYear()
    var month = dateHere.getMonth() + 1
    var day = dateHere.getDate()
    var startNo = (new Date(year - 1, 11, 31)).getTime()
    var xN = GetXn(startNo, 2592000000, 12)

    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: VerGetData(listData[i], xN)
        }
        data.push(temp);
    }
    chartOptions = {
        xaxis: {
            min: (new Date(year - 1, 11, 2)).getTime(),
            max: (new Date(year, 12, 1)).getTime(),
            //min: (new Date(2011, 11, 15)).getTime(),
            //max: (new Date(2012, 04, 18)).getTime(),
            mode: "time",
            tickSize: [1, "month"],
            monthNames: monthNames,
            tickLength: 0
        },
        grid: {
            hoverable: true,
            clickable: false,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        bars: {
            show: true,
            barWidth: 12 * 24 * 60 * 60 * 300,
            fill: true,
            lineWidth: 1,
            order: true,
            lineWidth: 0,
            fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }] }
        },
        shadowSize: 0,
        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    }

    var holder = $('#vertical-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }

}

function GetVerChartForDay(listData, xNames, vIntList12, monthNames) {
    var chartOptions;
    var data = new Array();

    //var dateHere = new Date(date)
    //var year = dateHere.getFullYear()
    //var month = dateHere.getMonth() + 1
    //var day = dateHere.getDate()
    //var startNo = (new Date(year - 1, 11, 31)).getTime()
    //var xN = GetXn(startNo, 3600000, 24)
    //console.log(hour);
    var arrHere = new Array();
    //单柱的时候，不加hour小时距离，多柱（2）的时候，加hour小时距离。可，这是为什么呢
    if (listData.length == 1) {
        for (var i = 0; i < vIntList12.length; i++) {
            arrHere.push(vIntList12[i]  - 600000)
        }
    } else {
        for (var i = 0; i < vIntList12.length; i++) {
            arrHere.push(vIntList12[i] + 1000 * 3600 * hour)
        }
    }
        //arrHere = vIntList12

    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: VerGetData1(listData[i], arrHere)
        }
        data.push(temp);
    }

    //var hours = hour + 7;
    //if()

    var minTime = (new Date(2017, 10, 17, 7, 1, 0)).addHours(hour);
    var maxTime = (new Date(2017, 10, 18, 7, 59, 59)).addHours(hour);

    chartOptions = {
        xaxis: {
            min: minTime.getTime(),
            max: maxTime.getTime(),
            mode: "time",
            tickSize: [1, "hour"],
            monthNames: monthNames,
            tickLength: 0
        },
        grid: {
            hoverable: true,
            clickable: false,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        bars: {
            show: true,
            barWidth: 60 * 60 * 300, //圆柱的宽度，300是直观的。其他的因数是60秒*60分钟。。之类的
            fill: true,
            lineWidth: 1,
            order: true,
            lineWidth: 0,
            fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }] }
        },
        shadowSize: 0,
        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    }

    var holder = $('#vertical-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }
}

function GetVerChartForMonth(listData, xNames, monthNames, days) {
    var dateHere = new Date(date)
    var year = dateHere.getFullYear()
    var month = dateHere.getMonth() + 1
    var day = dateHere.getDate()

    var prevMonth;
    if (month == 1) {
        prevMonth = new Date(year - 1, 12, 1);
    } else {
        prevMonth = new Date(year, month - 1, 1);
    }

    var startNo = prevMonth.getTime()
    var xN = GetXn(startNo + 28800000, 86400000, days)//决定折线的起点等。

    if (listData.length == 1)
        xN = GetXn(startNo + 14400000, 86400000, days)//决定折线的起点等。

    var chartOptions;
    var data = new Array();
    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: VerGetData1(listData[i], xN)
        }
        data.push(temp);
    }

    var maxMonth = new Date(year, month, 1);

    chartOptions = {
        xaxis: {
            min: prevMonth.getTime(),//min.max x轴的范围
            max: maxMonth.getTime(),
            mode: "time",
            tickSize: [1, "day"],
            monthNames: monthNames,
            tickLength: 0
        },
        grid: {
            hoverable: true,
            clickable: false,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        bars: {
            show: true,
            barWidth: 24 * 60 * 60 * 300, //圆柱的宽度，300是直观的。其他的因数是60秒*60分钟。。之类的
            fill: true,
            lineWidth: 1,
            order: true,
            lineWidth: 0,
            fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }] }
        },
        shadowSize: 0,
        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    }

    var holder = $('#vertical-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }
}

function VerGetData(data, vIntList12) {
    //console.log(vIntList12);
    var arr = new Array();
    for (var i = 0; i < vIntList12.length; i++) {
        var temp = new Array();
        temp.push(vIntList12[i], data[i])
        arr.push(temp);
    }
    return arr;
}

function VerGetData1(data, xNos) {
    var arr = new Array();
    for (var i = 0; i < xNos.length; i++) {
        var temp = new Array();
        temp.push(xNos[i])
        temp.push(data[i])
        arr.push(temp);
    }
    return arr;
}