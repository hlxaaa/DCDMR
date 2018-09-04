var type = 'year';
var date = ''
var customerNo = ''
var hour = 0;
var tabIndex = 0;
var lastName;

var orderField = 'Id'
var desc = true;
var listSort;
var defaultField = 'Id'
var page;

// #region params
var months = ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"];

var monthDays31 = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"]
var monthDays30 = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30"]
var monthDays29 = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29"]
var monthDays28 = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28"]
var hours = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24"]

//var data1 = ["15", "300", "1205", "1129", "2163", "1205", "2002", "2217", "1200", "2900", "2600", "1900"]

var x12 = [[1262304000000, 0], [1264924000000, 0], [1267544000000, 0], [1270164000000, 0], [1272784000000, 0], [1275404000000, 0], [1278024000000, 0], [1280644000000, 0], [1283264000000, 0], [1285884000000, 0], [1288504000000, 0], [1291124000000, 0]]

var x24Temp = [1510876800000, 1510880400000, 1510884000000, 1510887600000, 1510891200000, 1510894800000, 1510898400000, 1510902000000, 1510905600000, 1510909200000, 1510912800000, 1510916400000, 1510920000000, 1510923600000, 1510927200000, 1510930800000, 1510934400000, 1510938000000, 1510941600000, 1510945200000, 1510948800000, 1510952400000, 1510956000000, 1510959600000]

// #endregion

$(document).ready(function () {

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

    })

    $('.input-lv98').change(function () {
        changePage(1);
    })

    $('.day-startTime').change(function () {
        hour = $(this).val();
        change();
    })

    $('.custNo').change(function () {
        if (tabIndex == 2)
            changeVertical();
        else if (tabIndex == 1)
            changeLine()
        else
            changePage(page);
    })

    $('.btn-addExcel').click(function () {
        $('#upImg').click();
    })
    changePage(1);
    //changeLine();
    //changeVertical()
    //YearVer()
    $('body').delegate('.layui-form-radioed', 'click', function () {
        type = $(this).prev().val()
        //alert(type);

        if (type == 'day') {
            $('.day-startTime').removeClass('hidden')
        } else {
            $('.day-startTime').addClass('hidden')
        }


        switch (type) {
            case "year":
                console.log(1);
                ClearSelects()
                $('#iYear').css('display', 'block')
                break;
            case "month":
                ClearSelects()
                $('#iMonth').css('display', 'block')
                break;
            case "day":
                ClearSelects()
                $('#test5').css('display', 'block')
                break;
        }

        if (tabIndex == 2)
            changeVertical();
        else if (tabIndex == 1)
            changeLine()
        else
            changePage(page);
    })
    $('.btn-search1').click(function () {
        //changeLine()
        //changeVertical()
        if (tabIndex == 2)
            changeVertical();
        else if (tabIndex == 1)
            changeLine()
        else
            changePage(page);
    })

    $('#test5').keypress(function (e) {
        if (e.keyCode == 13)
            $('.btn-search1').click();
    })


    $('.btn-getExcel').click(function () {
        jQuery.postNL('../StatisticsAjax/GetExcelCountSSR', "", function (data) {
            if (data.Message == "1") {
                TestExcel();
            } else {
                layer.msg('没有数据可以导出')
            }
        })
    })
})

function ClearSelects() {
    $('#iYear').css('display', 'none')
    $('#iMonth').css('display', 'none')
    $('#test5').css('display', 'none')
}

function changePage(index) {
    GetLastName();
    changePieType();

    changePieTime();

    page = index;

    //listSort = new Array();
    //$('.table th').each(function () {
    //    listSort.push($(this).attr('class'))
    //})

    date = $('#test5').val();
    var data = {
        type: type,
        date: $('#test5').val(),
        customerNo: $('.custNo').val(),
        startTime: $('.day-startTime').val(),
        lastName: lastName,
        iYear: $('#iYear').val(),
        iMonth: $('#iMonth').val()
    }
    jQuery.postNL('../StatisticsAjax/GetStdSumReport', data, function (data) {
        var index = 1;
        var pages = 1;
        //var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            $('.rt_content .table tr').remove();
            $('.rt_content .table tbody').append('<tr><th>没有相关信息</th></tr></tr>')
            $('.rt_content .paging').children().remove();
            return;
        }

        var h = '<table class="table"><tr><th class=" tds-3x" data-sort="dt">时间</th><th class="tds-3x" data-sort="sumVolume">使用气量</th></tr>';
        //<th class="tds-x">操作</th>
        for (var i = 0; i < data.length; i++) {
            var item = data[i]
            var dt = item.dt == null ? "" : item.dt
            if (type == "year")
                dt = ChangeTimeYear(dt);
            else if (type == "month")
                dt = ChangeTimeMonth(dt);
            else
                dt = ChangeTimeFormat(dt);
            var sumVolume = item.span == null ? "" : item.span
            //var sumMoney = item.sumMoney == null ? "" : item.sumMoney
            h += '<tr><td class="center tds td-dt" title="' + dt + '">' + dt + '</td><td class="center tds td-sumVolume" title="' + sumVolume + '">' + sumVolume + '</td></tr>'

        }
        h += ' </table>'
        $('.rt_content .table').remove();
        $('.rt_content').append(h);
        //$('.rt_content .paging').before(h);
        //$('.rt_content .paging').children().remove();
        //var h = getPageHtml(pages, index);
        //$('.rt_content .paging').append(h);
    })
}

//折线图变化
function changeLine() {
    GetLastName();
    date = $('#test5').val();
    var data = {
        type: type,
        date: $('#test5').val(),
        customerNo: $('.custNo').val(),
        startTime: $('.day-startTime').val(),
        lastName: lastName,
        iYear: $('#iYear').val(),
        iMonth: $('#iMonth').val()
    }
    jQuery.postNL('../statisticsAjax/GetStdSumLine', data, function (data) {
        var days = data.level
        var list = data.ListData;
        var datas1 = new Array();
        for (var i = 0; i < list.length; i++) {
            datas1.push(parseFloat(list[i].span))
        }
        var listData = new Array();
        listData.push(datas1);
        var xNames = new Array();
        xNames.push("使用气量")
        //console.log(type)
        //console.log(date+'111');
        if (type == 'month') {
            if (days == 31)
                MonthLine31(listData, xNames);
            if (days == 30)
                MonthLine30(listData, xNames);
            if (days == 29)
                MonthLine29(listData, xNames);
            if (days == 28)
                MonthLine28(listData, xNames);
        } else if (type == 'day') {
            DayLine(listData, xNames);
        } else
            YearLine(listData, xNames)
    })
}

//柱形图变化
function changeVertical() {
    GetLastName();
    date = $('#test5').val();

    var data = {
        type: type,
        date: $('#test5').val(),
        customerNo: $('.custNo').val(),
        startTime: $('.day-startTime').val(),
        lastName: lastName,
        iYear: $('#iYear').val(),
        iMonth: $('#iMonth').val()
    }
    jQuery.postNL('../statisticsAjax/GetStdSumLine', data, function (data) {
        var days = data.level
        var list = data.ListData;
        var datas1 = new Array();
        // var datas2 = new Array();
        for (var i = 0; i < list.length; i++) {
            datas1.push(parseFloat(list[i].span))
            //  datas2.push(parseFloat(list[i].span))
        }
        var listData = new Array();
        listData.push(datas1);
        // listData.push(datas2);

        var xNames = new Array();
        //xNames.push("充值金额")
        xNames.push("使用气量")

        if (type == 'month') {
            if (days == 31)
                MonthVer31(listData, xNames);
            if (days == 30)
                MonthVer30(listData, xNames);
            if (days == 29)
                MonthVer29(listData, xNames);
            if (days == 28)
                MonthVer28(listData, xNames);
        } else if (type == 'day') {
            DayVer(listData, xNames);
        } else
            YearVer(listData, xNames);
    })
}

//获取 xn = [1262304000000, ……
//interval 小时3600000   天86400000 月86400000*（31，30，29，28） 
function GetXn(startNo, interval, amount) {
    var arr = new Array();
    for (var i = 0; i < amount; i++) {
        arr.push(startNo + interval * i);
    }
    return arr;
}

function test() {

}

//上传文件部分
function filechange(event) {
    var obj = document.getElementById("upImg");
    var length = obj.files.length;
    var isPic = true;
    for (var i = 0; i < obj.files.length; i++) {
        var temp = obj.files[i].name;
        var fileTarr = temp.split('.');
        var filetype = fileTarr[fileTarr.length - 1];


    }


    $("#formid").ajaxSubmit(function (res) {

        $('#btn-img').attr('src', res)

        //var h = '<img class="img-show" src="' + res + '" alt="img">';
        //$('.img-show').remove();
        //$('#divImgs').prepend(h);
    });
}

function TestExcel() {
    location.href = '/StatisticsAjax/GetChargeExcelSSR';
}

function change() {
    if (tabIndex == 2)
        changeVertical();
    else if (tabIndex == 1)
        changeLine()
    else
        changePage(page);
}

//客户类型圆饼图变化
function changePieType() {
    date = $('#test5').val()
    var data = {
        type: type,
        date: $('#test5').val(),
        customerNo: $('.custNo').val(),
        startTime: $('.day-startTime').val(),
        lastName: lastName,
        iYear: $('#iYear').val(),
        iMonth: $('#iMonth').val()
    }
    jQuery.postNL('../statisticsAjax/GetStdSumPieByType', data, function (data) {
        //var days = data.level
        var list = data.ListData;
        //console.log(list);
        //var datas1 = new Array();
        //for (var i = 0; i < list.length; i++) {
        //    datas1.push(parseFloat(list[i].sumMoney))
        //}
        MakeChart(list);
        //MakeChartTime(list);
    })
}

//抄表时间圆饼图变化
function changePieTime() {
    date = $('#test5').val()
    var data = {
        type: type,
        date: $('#test5').val(),
        customerNo: $('.custNo').val(),
        startTime: $('.day-startTime').val(),
        lastName: lastName,
        iYear: $('#iYear').val(),
        iMonth: $('#iMonth').val()
    }
    jQuery.postNL('../statisticsAjax/GetStdSumPieByTime', data, function (data) {
        //var days = data.level
        var list = data.ListData;
        //console.log(list);
        //var datas1 = new Array();
        //for (var i = 0; i < list.length; i++) {
        //    datas1.push(parseFloat(list[i].sumMoney))
        //}
        //MakeChart(list);
        MakeChartTime(list);
    })
}

//修改能看到的设备的列表
function ChangeDeviceList() {
    GetLastName();
    var data1 = {
        name: lastName
    }
    jQuery.postNL('../deviceAjax/GetDeviceListByCompanyName', data1, function (data) {
        var list = data.ListData;
        $('.custNo').children().remove();
        var h = ''
        h += '<option value="0">全部客户</option>'
        for (var i = 0; i < list.length; i++) {
            h += `<option value="${list[i].customerNo}">${list[i].customerName}</option>`
        }

        $('.custNo').append(h);
        //AddEditable('#s-custNo')
    })
}