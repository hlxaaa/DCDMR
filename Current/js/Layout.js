var AlarmMeterNo;
var alarmCount;//是报警的总数量，包括已处理和未处理的
//var alarmIng;

var lastAlarmId;//之前报警记录的最后一条id，由于已处理的以后只显示一个月内的了，所以，这个用来检测有没有新警报的变量只能换了

$(document).ready(function () {

    console.log($('#allmap').css('height'));
    console.log($('#allmap').css('width'));

    //定时查询有没有新的报警，有就右下角弹出来提示

    //GetNewAlarm()

    //clearInterval(t);
    //var t = setInterval(GetNewAlarm, 7 * 1000)

    //setInterval(refresh, parseInt(time) * 1000)

    //点击打印报表跳到打印页面
    $('.btn-print').click(function () {
        var printType = $(this).data('printtype');
        location.href = '/Statistics/PrintPage?printType=' + printType;
    });

    //表格的列头双击排序
    $('body').delegate('.table th', 'dblclick', function () {
        var e = $(this);
        if ($(this).hasClass('sorting')) {
            $('.table .th-sort').attr('class', 'th-sort sorting');
            $(this).attr('class', 'th-sort sorting_asc');
            orderField = e.data('sort');
            desc = false;
            changePage(1);
        } else if ($(this).hasClass('sorting_asc')) {
            $('.table .th-sort').attr('class', 'th-sort sorting');
            $(this).attr('class', 'th-sort sorting_desc');
            orderField = e.data('sort');
            desc = true;
            changePage(1);
        }
        else if ($(this).hasClass('sorting_desc')) {
            $('.table .th-sort').attr('class', 'th-sort sorting');
            $(this).attr('class', 'th-sort sorting');
            orderField = defaultField;
            desc = false;
            changePage(1);
        }
    });

    //表格2的列表双击排序
    $('body').delegate('.table2 th', 'dblclick', function () {
        var e = $(this);
        if ($(this).hasClass('sorting')) {
            $('.table2 .th-sort').attr('class', 'th-sort sorting');
            $(this).attr('class', 'th-sort sorting_asc');
            orderField2 = e.data('sort');
            desc2 = false;
            changePage2(1);
        } else if ($(this).hasClass('sorting_asc')) {
            $('.table2 .th-sort').attr('class', 'th-sort sorting');
            $(this).attr('class', 'th-sort sorting_desc');
            orderField2 = e.data('sort');
            desc2 = true;
            changePage2(1);
        }
        else if ($(this).hasClass('sorting_desc')) {
            $('.table2 .th-sort').attr('class', 'th-sort sorting');
            $(this).attr('class', 'th-sort sorting');
            orderField2 = defaultField2;
            desc2 = false;
            changePage2(1);
        }
    });

    //查询按钮点击事件
    $('body').delegate('.btn-search', 'click', function () {
        changePage(1);
    });

    //查询按钮2点击事件
    $('body').delegate('.btn-search2', 'click', function () {
        changePage2(1);
    });

    //关键词查询输入框按键监听
    $('.input-search').keypress(function (e) {
        if (e.keyCode == 13)
            $('.btn-search').click();
    });

    //关键词查询输入框按键监听
    $('.input-search2').keypress(function (e) {
        if (e.keyCode == 13)
            $('.btn-search2').click();
    });

    //右上角头像点击事件
    //$('#drop7').click(function () {

    //    var e = $(this).parent();

    //    $('.user-profile').addClass('class2')

    //    //console.log(e)
    //    //console.log($(this).parent())
    //    //console.log($('.list-box .user-profile'))
    //    //console.log($(this).parent().html())
    //    //console.log($('.user-profile').html())

    //    //console.log(e)
    //    //console.log(e.hasClass('open'));
    //    if (e.hasClass('open'))
    //        $('.list-box .user-profile').removeClass('open')
    //    //e.removeClass('open')
    //    else {
    //        e.addClass('class1')
    //        e.addClass('class3')
    //        //e.addClass('open')//-txy 用e为什么不行。——因为它控制出现消失的是另外地方的，这里的注释掉照样行。
    //        //$('.list-box .user-profile').addClass('open')
    //        //console.log(22323)
    //    }
    //})


    //按ESC关闭一些对话框
    $('body').keypress(function (e) {
        if (e.keyCode == 27)
            $(".pop_bg").fadeOut();
    });


    //报警数量圆盘点击事件
    $('#div-alarm').click(function () {
        location.href = '/alarm/list';
    });
});

//退出登录
function logout() {
    jQuery.postNL('../login/logout', '', function (data) {
        location.href = '/';
    });

    $("#loading").click(function () {
        $(".loading_area").fadeIn();
        $(".loading_area").fadeOut(1500);
    });
}

//得到页码的html
function getPageHtml(pages, thePage) {
    if (pages <= 5) {
        h = '<a onclick="changePage(1)">第一页</a> ';

        for (var i = 1; i < pages + 1; i++) {
            if (i == thePage) {
                h += '<a class="active">' + i + '</a> ';
            }
            else
                h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
        }

        h += '<a  onclick="changePage(' + pages + ')">最后一页</a> ';
    } else {
        if (thePage <= 2) {
            h = '<a onclick="changePage(1)">第一页</a> ';


            for (var i = 1; i < pages + 1; i++) {
                if (i == pages) {
                    h += '<a onclick="getMidPage(this)">...</a> ';
                }
                if (i <= thePage + 2 || i == pages) {
                    if (i == thePage)
                        h += '<a class="active">' + i + '</a> ';
                    else
                        h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
                }
            }


            h += '<a onclick="changePage(' + pages + ')">最后一页</a> ';
        } else {
            if (thePage < 4) {
                h = '<a onclick="changePage(1)">第一页</a> ';


                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<a onclick="getMidPage(this)">...</a> ';
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages) {
                        if (i == thePage)
                            h += '<a class="active">' + i + '</a> ';
                        else
                            h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
                    }
                }

                h += '<a onclick="changePage(' + pages + ')">最后一页</a> ';
            } else {
                h = '<a onclick="changePage(1)">第一页</a> ';


                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<a onclick="getMidPage(this)">...</a> ';
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages || i == 1) {
                        if (i == thePage)
                            h += '<a class="active">' + i + '</a> ';
                        else
                            h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
                    }
                    if (i == 1 && thePage > 4) {
                        h += '<a onclick="getMidPage(this)">...</a> ';
                    }
                }
                h += '<a onclick="changePage(' + pages + ')">最后一页</a> ';
            }
        }
    }
    if (pages < 1) {
        return "";
    }
    return h;
}

//得到页码的html2
function getPageHtml2(pages, thePage) {
    if (pages <= 5) {
        h = '<a onclick="changePage2(1)">第一页</a> ';

        for (var i = 1; i < pages + 1; i++) {
            if (i == thePage) {
                h += '<a class="active">' + i + '</a> ';
            }
            else
                h += '<a onclick="changePage2(' + i + ')">' + i + '</a> ';
        }

        h += '<a  onclick="changePage2(' + pages + ')">最后一页</a> ';
    } else {
        if (thePage <= 2) {
            h = '<a onclick="changePage2(1)">第一页</a> ';


            for (var i = 1; i < pages + 1; i++) {
                if (i == pages) {
                    h += '<a onclick="getMidPage2(this)">...</a> ';
                }
                if (i <= thePage + 2 || i == pages) {
                    if (i == thePage)
                        h += '<a class="active">' + i + '</a> ';
                    else
                        h += '<a onclick="changePage2(' + i + ')">' + i + '</a> ';
                }
            }


            h += '<a onclick="changePage2(' + pages + ')">最后一页</a> ';
        } else {
            if (thePage < 4) {
                h = '<a onclick="changePage2(1)">第一页</a> ';


                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<a onclick="getMidPage2(this)">...</a> ';
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages) {
                        if (i == thePage)
                            h += '<a class="active">' + i + '</a> ';
                        else
                            h += '<a onclick="changePage2(' + i + ')">' + i + '</a> ';
                    }
                }

                h += '<a onclick="changePage2(' + pages + ')">最后一页</a> ';
            } else {
                h = '<a onclick="changePage2(1)">第一页</a> ';


                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<a onclick="getMidPage2(this)">...</a> ';
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages || i == 1) {
                        if (i == thePage)
                            h += '<a class="active">' + i + '</a> ';
                        else
                            h += '<a onclick="changePage2(' + i + ')">' + i + '</a> ';
                    }
                    if (i == 1 && thePage > 4) {
                        h += '<a onclick="getMidPage2(this)">...</a> ';
                    }
                }
                h += '<a onclick="changePage2(' + pages + ')">最后一页</a> ';
            }
        }
    }
    if (pages < 1) {
        return "";
    }
    return h;
}

//之前用116模板的时候，错误提示html
function ErrorTipsHtml(str) {
    return '<span class="errorTips">' + str + '</span>';
}

//为116模板清除错误提示
function ClearErrorTips() {
    $('.errorTips').remove();
}

//点击页码的省略号 跳转中间页码
function getMidPage(node) {
    var prev = node.previousSibling.previousSibling.innerText;
    var next = node.nextSibling.nextSibling.innerText;
    var thePage = (parseInt(prev) + parseInt(next)) / 2;
    changePage(parseInt(thePage));
}

//点击页码的省略号 跳转中间页码2
function getMidPage2(node) {
    var prev = node.previousSibling.previousSibling.innerText;
    var next = node.nextSibling.nextSibling.innerText;
    var thePage = (parseInt(prev) + parseInt(next)) / 2;
    changePage2(parseInt(thePage));
}

//单独触发控件中控件的点击事件，不触发母控件的点击事件
function stopBubbling(e) {
    e = window.event || e;
    if (e.stopPropagation) {
        e.stopPropagation();      //阻止事件 冒泡传播
    } else {
        e.cancelBubble = true;   //ie兼容
    }
}


//layui.js
layui.use('layer', function () { //独立版的layer无需执行这一句
    var $ = layui.jquery, layer = layui.layer; //独立版的layer无需执行这一句

    //触发事件
    var active = {
        setTop: function () {
            var that = this;
            //多窗口模式，层叠置顶
            layer.open({
                type: 2 //此处以iframe举例
                , title: '当你选择该窗体时，即会在最顶端'
                , area: ['390px', '260px']
                , shade: 0
                , maxmin: true
                , offset: [ //为了演示，随机坐标
                    Math.random() * ($(window).height() - 300)
                    , Math.random() * ($(window).width() - 390)
                ]
                , content: 'http://layer.layui.com/test/settop.html'
                , btn: ['继续弹出', '全部关闭'] //只是为了演示
                , yes: function () {
                    $(that).click();
                }
                , btn2: function () {
                    layer.closeAll();
                }

                , zIndex: layer.zIndex //重点1
                , success: function (layero) {
                    layer.setTop(layero); //重点2
                }
            });
        }
        , confirmTrans: function () {
            //配置一个透明的询问框
            layer.msg('大部分参数都是可以公用的<br>合理搭配，展示不一样的风格', {
                time: 20000, //20s后自动关闭
                btn: ['明白了', '知道了', '哦']
            });
        }
        , notice: function () {
            //示范一个公告层
            layer.open({
                type: 1
                , title: false //不显示标题栏
                , closeBtn: false
                , area: '300px;'
                , shade: 0.8
                , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                , btn: ['火速围观', '残忍拒绝']
                , btnAlign: 'c'
                , moveType: 1 //拖拽模式，0或者1
                , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">你知道吗？亲！<br>layer ≠ layui<br><br>layer只是作为Layui的一个弹层模块，由于其用户基数较大，所以常常会有人以为layui是layerui<br><br>layer虽然已被 Layui 收编为内置的弹层模块，但仍然会作为一个独立组件全力维护、升级。<br><br>我们此后的征途是星辰大海 ^_^</div>'
                , success: function (layero) {
                    var btn = layero.find('.layui-layer-btn');
                    btn.find('.layui-layer-btn0').attr({
                        href: '/alarm/alarmInfo?meterNo=' + AlarmMeterNo
                        , target: '_blank'
                    });
                }
            });
        }
        , offset: function (othis) {
            var type = othis.data('type')
                , text = '表' + AlarmMeterNo + '警报';

            layer.open({
                type: 1
                , offset: type //具体配置参考：http://www.layui.com/doc/modules/layer.html#offset
                , id: 'layerDemo' + type //防止重复弹出
                , content: '<div style="padding: 20px 100px;">' + text + '</div>'
                , btn: '前往'
                , btnAlign: 'c' //按钮居中
                , shade: 0 //不显示遮罩
                , yes: function () {
                    //layer.closeAll();
                    window.open('/alarm/alarmInfo?meterNo=' + AlarmMeterNo, "_blank");
                    //location.href='/alarm/alarmInfo?meterNo=' + AlarmMeterNo
                }
            });
        }
    };

    $('#layerDemo .layui-btn').on('click', function () {
        var othis = $(this), method = othis.data('method');
        active[method] ? active[method].call(this, othis) : '';
    });

});
//layui.js 结束

//右下角提示框，好像不用了
function RightBottomTips(no) {
    AlarmMeterNo = no;
    $('#layerDemo .layui-btn').click();
}

reset = function () {
    $("toggleCSS").href = "~/base/css/alertify.core.css";
    alertify.set({
        labels: {
            ok: "OK",
            cancel: "Cancel"
        },
        delay: 9999999999,
        buttonReverse: false,
        buttonFocus: "ok"
    });
};

//右下角弹出提示
function rbTips008(no) {
    AlarmMeterNo = no;
    reset();
    var h = '<p>表' + no + '警报 <a href="/alarm/alarminfo?meterNo=' + AlarmMeterNo + '" target="_blank">前往</a></p>';
    alertify.error(h);
}

//将时间字符串格式化为"yyyy-MM-dd HH:mm:ss"
function ChangeTimeFormat(date) {
    if (date == '')
        return '';
    var dt = new Date(date);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    month = month < 10 ? `0${month}` : month;
    var day = dt.getDate();
    day = day < 10 ? `0${day}` : day;
    var hour = dt.getHours();
    hour = hour < 10 ? `0${hour}` : hour;
    var minute = dt.getMinutes();
    minute = minute < 10 ? `0${minute}` : minute;
    var second = dt.getSeconds();
    second = second < 10 ? `0${second}` : second;
    return `${year}-${month}-${day} ${hour}:${minute}:${second}`;
}

//将时间字符串格式化为"yyyy-MM-dd"
function ChangeTimeMonth(date) {
    if (date == '')
        return '';
    var dt = new Date(date);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    month = month < 10 ? `0${month}` : month;
    var day = dt.getDate();
    day = day < 10 ? `0${day}` : day;

    return `${year}-${month}-${day}`;
}

//将时间字符串格式化为"yyyy-MM"
function ChangeTimeYear(date) {
    if (date == '')
        return '';
    var dt = new Date(date);
    var year = dt.getFullYear();
    var month = dt.getMonth() + 1;
    month = month < 10 ? `0${month}` : month;

    return `${year}-${month}`;
}

//信东。获取最小一级公司的名字
function GetLastName() {
    //-txy 目前好像没问题，可优化
    lastName = '';
    var i99 = $('.input-lv99');
    var i98 = $('.input-lv98');

    if (i99.length > 0 && i98.length > 0) {
        if (i98.val().length > 0)
            lastName = i98.val();
        else if (i99.val().length > 0)
            lastName = i99.val();
    } else {
        if (i98.length > 0) {
            if (i98.val().length > 0)
                lastName = i98.val();
        }
    }
}

//不用了
function SetChargeToCookie(h) {
    setCookie("chargeTableHtml", h, 360);
}


function GetChargeToCookie() {
    return getCookie("chargeTableHtml");
}


function SetSSRToCookie(h) {
    //console.log(h);
    setCookie("SSRTableHtml", h, 360);
}
function GetSSRToCookie() {
    return getCookie("SSRTableHtml");
}

function setCookie(c_name, value, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = c_name + "=" + escape(value) +
        ((expiredays == null) ? "" : "; expires=" + exdate.toGMTString()) + "; path=/;";
}

//获取cookie
function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");

        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) c_end = document.cookie.length;
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}

//删除cookie
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

//时间加天
Date.prototype.addDays = function (number) {
    var adjustDate = new Date(this.getTime() + 24 * 60 * 60 * 1000 * number);
    //alert("Date" + adjustDate.getFullYear() + "-" + adjustDate.getMonth() + "-" + adjustDate.getDate());
    return adjustDate;
};
//时间加小时
Date.prototype.addHours = function (number) {
    var date = new Date(this.getTime() + 60 * 60 * 1000 * number);
    return date;
};
//时间加月
Date.prototype.addMonths = function (number) {
    var date = new Date(this.getTime() + 60 * 60 * 1000 * 30 * number);
    return date;
};
//时间加秒
Date.prototype.addSeconds = function (number) {
    var date = new Date(this.getTime() + 1000 * number);
    return date;
};


function AddEditable(str) {
    $(str).editableSelect({
        effects: 'fade',  //下拉列表出来的方式
        duration: 200//时间
        //appendTo: 'body'  //添加到何处，此处省略就是添加到输入框下边
    });
}

//study




//function hmDistance(a, b) {
//    if (a.length !== b.length)
//        throw new Error('strings must be of the same length')
//    let distance = 0;
//    for (let i = 0; i < a.length; i++) {
//        if (a[i] !== b[i])
//            distance++;
//    }
//    return distance;
//}

//function cartesianProduct(setA, setB) {
//    if (!setA || !setB || !setA.length || !setB.length)
//        return null
//    const product = [];
//    for (let indexA = 0; indexA < setA.length; indexA++) {
//        for (let indexB = 0; indexB < setB.length; indexB++) {
//            product.push([setA[indexA], setB[indexB]]);
//        }
//    }
//    return product;
//}

//var arr = [4, 5, 6]

//function powerSet(arr) {
//    const subSets = [];
//    const numberOfC = 2 ** arr.length;
//    for (let i = 0; i < numberOfC; i++) {
//        const subSet = [];
//        for (let j = 0; j < arr.length; j++) {
//            var log = i & (1 << j)
//            console.log(`i=${i} -- 1<<j =  ${1 << j} -- result ${log}`)
//            if (i & (1 << j))
//                subSet.push(arr[j])
//        }
//        subSets.push(subSet)
//    }
//    return subSets;
//}

//class LinkedListNode {
//    constructor(value, next = null) {
//        this.value = value;
//        this.next = next;
//    }
//    toString(callback) {
//        return callback ? callback(this.value) : `${this.value}`
//    }
//    calc() {
//        return this.value * this.next;
//    }
//    static calc2(value,next) {
//        return value ** next;
//    }
//}

//class LinkedList {
//    constructor(func) {
//        this.head = null;
//        this.tail = null;
//        this.compare = new Comparator(func);
//    }
//}



function getPageHtml3333(pages, thePage) {
    var h = '';
    if (pages <= 5) {
        if (thePage == 1)
            h = `  <li class="paginate_button disabled">
                                <a onclick="changePage(1)"><i class="fa fa-angle-left"></i></a>
                            </li>`
        else
            h = `  <li class="paginate_button">
                                <a onclick="changePage(1)"><i class="fa fa-angle-left"></i></a>
                            </li>`
        //h = '<a onclick="changePage(1)">第一页</a> ';

        for (var i = 1; i < pages + 1; i++) {
            if (i == thePage) {
                h += '<a class="active">' + i + '</a> ';
            }
            else
                h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
        }

        h += '<a  onclick="changePage(' + pages + ')">最后一页</a> ';
    } else {
        if (thePage <= 2) {
            h = '<a onclick="changePage(1)">第一页</a> ';


            for (var i = 1; i < pages + 1; i++) {
                if (i == pages) {
                    h += '<a onclick="getMidPage(this)">...</a> ';
                }
                if (i <= thePage + 2 || i == pages) {
                    if (i == thePage)
                        h += '<a class="active">' + i + '</a> ';
                    else
                        h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
                }
            }


            h += '<a onclick="changePage(' + pages + ')">最后一页</a> ';
        } else {
            if (thePage < 4) {
                h = '<a onclick="changePage(1)">第一页</a> ';


                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<a onclick="getMidPage(this)">...</a> ';
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages) {
                        if (i == thePage)
                            h += '<a class="active">' + i + '</a> ';
                        else
                            h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
                    }
                }

                h += '<a onclick="changePage(' + pages + ')">最后一页</a> ';
            } else {
                h = '<a onclick="changePage(1)">第一页</a> ';


                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<a onclick="getMidPage(this)">...</a> ';
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages || i == 1) {
                        if (i == thePage)
                            h += '<a class="active">' + i + '</a> ';
                        else
                            h += '<a onclick="changePage(' + i + ')">' + i + '</a> ';
                    }
                    if (i == 1 && thePage > 4) {
                        h += '<a onclick="getMidPage(this)">...</a> ';
                    }
                }
                h += '<a onclick="changePage(' + pages + ')">最后一页</a> ';
            }
        }
    }
    if (pages < 1) {
        return "";
    }
    return `<ul class="pagination">${h}</ul>`;
}