$(document).ready(function (e) {
    // GetSerialChart();
    //layer.msg(111)
    //MakeChart(json);
});

//var json = [
//    { "name": "数据1", "value": "35" },
//    { "name": "数据2", "value": "0" },
//    { "name": "数据3", "value": "22" },
//    { "name": "数据4", "value": "65" },
//    { "name": "数据5", "value": "35" },
//    { "name": "数据6", "value": "22" },
//    { "name": "数据7", "value": "43" },
//    { "name": "数据8", "value": "55" }
//]

function MakeChart(value) {
    //console.log(1);
    //console.log(value);
    chartData = eval(value);
    //饼状图
    chart = new AmCharts.AmPieChart();
    chart.dataProvider = chartData;
    //标题数据
    chart.titleField = "name";
    //值数据
    chart.valueField = "value";
    //边框线颜色
    chart.outlineColor = "#fff";
    //边框线的透明度
    chart.outlineAlpha = .8;
    //边框线的狂宽度
    chart.outlineThickness = 1;
    chart.depth3D = 20;
    chart.angle = 30;
    chart.write("pie");
    //chart.write("pie2");
    $('.amcharts-chart-div a').remove();
}

function MakeChartTime(value) {
    chartData = eval(value);
    //饼状图
    chart = new AmCharts.AmPieChart();
    chart.dataProvider = chartData;

    //标题数据
    chart.titleField = "name";
    //值数据
    chart.valueField = "value";
    //边框线颜色
    chart.outlineColor = "#fff";
    //边框线的透明度
    chart.outlineAlpha = .8;
    //边框线的狂宽度
    chart.outlineThickness = 1;
    chart.depth3D = 20;
    chart.angle = 30;
    chart.write("piee");
    //chart.write("pie2");
    $('.amcharts-chart-div a').remove();
}