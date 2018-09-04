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

$(function () {
    //var xNames = new Array();
    //xNames.push('试试')
    //var listData = new Array();
    //var dataTemp = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
    //listData.push(dataTemp)
    //GetLineChart(listData, xNames, x12, months)
});

function YearLine(listData, xNames) {

    GetLineChartForYear(listData, xNames, x12, months)
}
function MonthLine31(listData, xNames) {
    GetLineChartForMonth(listData, xNames, monthDays31, 31)
}
function MonthLine30(listData, xNames) {
    GetLineChartForMonth(listData, xNames, monthDays30, 30)
}
function MonthLine29(listData, xNames) {
    GetLineChartForMonth(listData, xNames, monthDays29, 29)
}
function MonthLine28(listData, xNames) {
    GetLineChartForMonth(listData, xNames, monthDays28, 28)
}
function DayLine(listData, xNames) {
    //console.log(x24);
    //console.log(date);
    //var dateHere = new Date(date)
    //console.log(dateHere);
    //console.log(dateHere.getFullYear())
    //console.log(dateHere.getMonth() + 1)
    //console.log(dateHere.getDate())
    GetLineChartForDay(listData, xNames, x24Temp, hours)
}

function GetLineChartForDay(listData, xNames, xNos, monthNames) {
    //console.log(listData)

    var dateHere = new Date(date)
    var year = dateHere.getFullYear()
    var month = dateHere.getMonth() + 1
    var day = dateHere.getDate()
    var startNo = dateHere.addMonths(1).addHours(hour-6).getTime()
    var xN = GetXn(startNo, 1000 * 3600, 24)


    var data = new Array();

    var chartOptions;

    var arrHere = new Array();
    for (var i = 0; i < xNos.length; i++) {
        arrHere.push(xNos[i])
        //arrHere.push(xNos[i] + 1000 * 3600 * hour)
    }
    //console.log(arrHere)
    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: LineGetData(listData[i], xN)
        }
        data.push(temp);
    }
    var minTime = dateHere.addDays(1).addHours(hour - 1).addSeconds(1);
    //var minTime = (new Date(2017, 10, 17, 7, 1, 0)).addHours(hour);
    var maxTime = dateHere.addDays(1).addDays(1).addHours(hour).addSeconds(-1);
    //var maxTime = (new Date(2017, 10, 18, 7, 59, 59)).addHours(hour);

    //console.log(data)
    //console.log(minTime.getTime())
    chartOptions = {
        xaxis: {
            min: minTime.getTime(),//时间要提前一个大单位的3分之一左右，迷-txy
            max: maxTime.getTime(),
            mode: "time",
            tickSize: [1, "hour"],
            monthNames: monthNames,
            tickLength: 0
        },
        yaxis: {

        },
        series: {
            lines: {
                show: true,
                fill: false,
                lineWidth: 2
            },
            points: {
                show: true,
                radius: 4.5,
                fill: true,
                fillColor: "#ffffff",
                lineWidth: 2
            }
        },
        grid: {
            hoverable: true,
            clickable: true,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        shadowSize: 0,
        legend: {
            show: true,
            position: 'nw'
        },

        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    };

    var holder = $('#line-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }
}

function GetLineChartForYear(listData, xNames, xNos, monthNames) {
    var dateHere = new Date(date)
    var year = dateHere.getFullYear()
    var month = dateHere.getMonth() + 1
    var day = dateHere.getDate()
    var startNo = (new Date(year - 1, 11, 31)).getTime()
    var xN = GetXn(startNo, 2592000000, 12)

    var data = new Array();

    var chartOptions;
    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: LineGetData(listData[i], xN)
        }
        data.push(temp);
    }

    chartOptions = {
        xaxis: {
            min: (new Date(year - 1, 11, 2)).getTime(),
            max: (new Date(year, 12, 1)).getTime(),
            mode: "time",
            tickSize: [1, "month"],
            monthNames: monthNames,
            tickLength: 0
        },
        yaxis: {

        },
        series: {
            lines: {
                show: true,
                fill: false,
                lineWidth: 2
            },
            points: {
                show: true,
                radius: 4.5,
                fill: true,
                fillColor: "#ffffff",
                lineWidth: 2
            }
        },
        grid: {
            hoverable: true,
            clickable: true,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        shadowSize: 0,
        legend: {
            show: true,
            position: 'nw'
        },

        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    };

    var holder = $('#line-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }
}

function GetLineChartForMonth(listData, xNames, monthNames, days) {
    var dateHere = new Date(date)
    var year = dateHere.getFullYear()
    var month = dateHere.getMonth() + 1
    //console.log(month);
    var day = dateHere.getDate()

    var prevMonth;
    if (month == 1) {
        prevMonth = new Date(year - 1, 12, 1);
    } else {
        prevMonth = new Date(year, month - 1, 1);
    }

    var startNo = prevMonth.getTime()
    var xN = GetXn(startNo + 28800000, 86400000, days)//决定折线的起点等。

    var data = new Array();

    var chartOptions;
    for (var i = 0; i < listData.length; i++) {
        var temp = {
            label: xNames[i],
            data: LineGetData(listData[i], xN)
        }
        data.push(temp);
    }

    var maxMonth = new Date(year, month, 1);

    //console.log(prevMonth);
    //console.log(dateHere);
    chartOptions = {
        xaxis: {
            min: prevMonth.getTime(),//min.max x轴的范围
            max: maxMonth.getTime(),
            mode: "time",
            tickSize: [1, "day"],
            monthNames: monthNames,
            tickLength: 0
        },
        yaxis: {

        },
        series: {
            lines: {
                show: true,
                fill: false,
                lineWidth: 2
            },
            points: {
                show: true,
                radius: 4.5,
                fill: true,
                fillColor: "#ffffff",
                lineWidth: 2
            }
        },
        grid: {
            hoverable: true,
            clickable: true,
            borderWidth: 1,
            tickColor: $border_color,
            borderColor: $grid_color,
        },
        shadowSize: 0,
        legend: {
            show: true,
            position: 'nw'
        },

        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        },
        colors: [$green, $blue, $yellow, $teal, $yellow, $green],
    };

    var holder = $('#line-chart');

    if (holder.length) {
        $.plot(holder, data, chartOptions);
    }
}

function LineGetData(data, xNos) {
    var arr = new Array();
    for (var i = 0; i < xNos.length; i++) {
        var temp = new Array();
        temp.push(xNos[i])
        temp.push(data[i])
        arr.push(temp);
    }
    return arr;
}
