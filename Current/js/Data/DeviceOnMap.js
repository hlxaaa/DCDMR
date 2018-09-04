var flag = true;
var sonId = 0;
var time;
var flag2 = true;
$(document).ready(function () {

    $('.tools').click(function () {
        $('#info').hide();
    })

    setInterval(changePage3, parseInt(time) * 1000)

    //地图第2+次changePage的坐标和第一次会有点差别，所以直接查2次
    changePage3();
    changePage3();

    $('#select-son').change(function () {
        sonId = $(this).val();
        flag2 = true;
        //$('#info').hide();
        changePage3();
    })

    $('#btn-search').click(function () {
        flag2 = true;
        //$('#info').hide();
        changePage3();
    })

    $('#input-search').keypress(function (e) {
        if (e.keyCode == 13)
            $('#btn-search').click();
    })

    //$('body').delegate('.mapLabel', 'click', function () {
    //    var no = $(this).data('id')
    //    console.log($(this));
    //})

})

function changePage3() {
    //-txy 地图偏移是因为这里
    //$('#info').hide();
    var data = {
        search: $('#input-search').val(),
        sonId: sonId
    }
    jQuery.postNL('../DataAjax/GetDeviceOnMap', data, function (data) {
        var allLat = data.lat;
        var allLng = data.lng;
        var level = data.level;
        var data = data.ListData;

        map.clearOverlays(); //
        if (data.length != 0) {
            flag = true;
            if (flag2) {
                var point = new BMap.Point(allLng, allLat);
                map.centerAndZoom(point, level);
                flag2 = false;
            }

            var index = 1;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                //if (flag) {
                //    if (item.lat != null) {
                //        var point = new BMap.Point(allLng, allLat);

                //        //if (flag2) {
                //        //    map.centerAndZoom(point, level);
                //        //    flag2 = false;
                //        //} else {
                //        //    map.centerAndZoom(point);
                //        //}
                //        flag = false;
                //    }
                //}
                if (item.lat != null) {

                    var imgStr = '/current/images/marker1.png'
                    if (item.FMStateMsg != '')
                        imgStr = '/current/images/marker2.png'
                    else if (item.LoginState == '0')
                        imgStr = '/current/images/marker0.png'
                    var label
                    if (i > 9)
                        label = new BMap.Label(index, { offset: new BMap.Size(9, 6) });
                    else
                        label = new BMap.Label(index, { offset: new BMap.Size(11, 6) });//个位数

                    if (i < 10)
                        label = new BMap.Label(index, { offset: new BMap.Size(11, 6) });//个位数
                    else if (i < 100)
                        label = new BMap.Label(index, { offset: new BMap.Size(9, 6) });
                    else
                        label = new BMap.Label(index, { offset: new BMap.Size(5, 6) });


                    if (index == 1) {
                        //console.log(item)
                    }
                    index = index + 1;

                    //console.log(index)
                    var point = new BMap.Point(item.lng, item.lat);

                    var marker = new BMap.Marker(point);  // 创建标注

                    marker.setIcon(new BMap.Icon(imgStr, new BMap.Size(32, 32), {
                        // imageOffset:new BMap.Size(0,(value.length-1)*-30)
                        //anchor: new BMap.Size(16, 16),
                        //imageOffset: new BMap.Size(0, (i.length - 1) * -0)
                        imageOffset: new BMap.Size(0, 0)
                    }))

                    map.addOverlay(marker);              // 将标注添加到地图中

                    (function () {
                        var meterNo = item.FLMeterNo
                        marker.addEventListener("click", function (e) {
                            var data = {
                                meterNo: meterNo
                            }
                            //alert(e.overlay.point.lng)
                            jQuery.postNL('../DataAjax/GetOneDeviceByMeterNo', data, function (data) {
                                var data = data.ListData[0];
                                SetData(data);
                            })
                        })
                    })();


                    label.setStyle({
                        background: 'none', color: '#fff', border: 'none', zIndex: 100000
                    });
                    marker.setLabel(label);

                    var opts = {
                        position: point,    // 指定文本标注所在的地理位置
                        offset: new BMap.Size(-45, -92)    //设置文本偏移量
                    }
                    //var opts2 = {
                    //    width: 250,     // 信息窗口宽度    
                    //    height: 100,     // 信息窗口高度    
                    //    title: "Hello",  // 信息窗口标题   
                    //           position: point,    // 指定文本标注所在的地理位置
                    //    //offset: new BMap.Size(-45, -92)    //设置文本偏移量
                    //}    
                    //var infoWindow = new BMap.InfoWindow("地址：北京市东城区王府井大街88号乐天银泰百货八层", opts);  // 创建信息窗口对象 
                    //map.openInfoWindow(infoWindow,point)
                    //console.log(item.FLMeterNo)
                    var label2 = new BMap.Label('<div class="mapLabel" ><div>累积量 ' + item.StdSum + '</div><br/><div>流量 ' + item.StdFlow + '</div><br/><div>温度 ' + item.Temperature + ' 压力 ' + item.Pressure + '</div></div>', opts);
                    label2.setStyle({
                        //"borderColor": "#5bc0de",
                        //backgroundColor: '#5bc0de',
                        //borderRadius:"0.1",
                        "color": "#333",
                        "cursor": "pointer",
                        maxWidth: 'none'
                    });


                    //map.addOverlay(label2);



                    ComplexCustomOverlay.prototype = new BMap.Overlay();
                    ComplexCustomOverlay.prototype.initialize = function (map1) {
                        this._map = map1;
                        var div = this._div = document.createElement("div");
                        div.className = 'mapLabel'
                        div.style.position = "absolute";
                        div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
                        div.style.backgroundColor = "white";
                        div.style.border = "1px solid white";
                        div.style.color = "black";
                        div.style.height = "74px";
                        div.style.display = "block"
                        div.style.borderRadius = "10px"
                        div.style.fontWeight = "bold"
                        //div.style.marl="36px"
                        div.style.padding = "8px";
                        div.style.lineHeight = "18px";
                        div.style.whiteSpace = "nowrap";
                        div.style.MozUserSelect = "none";
                        div.style.fontSize = "12px";
                        div.style.borderRadius = "1px"
                        var span = this._span = document.createElement("span");

                        var i2 = this._span = document.createElement("i");
                        i2.style.marginRight = "4px"
                        i2.className = 'fa fa-tachometer text-info'
                        span.appendChild(i2);


                        div.appendChild(span);
                        span.appendChild(document.createTextNode('累积量 ' + item.StdSum));

                        var span2 = this._span = document.createElement("p");
                        span2.className = "mapP";

                        var i1 = this._span = document.createElement("i");
                        i1.className = 'fa fa-exchange text-info'
                        i1.style.marginRight = "4px"
                        span2.appendChild(i1);
                        //span2.style.marginBottom = '18px';
                        div.appendChild(span2);
                        span2.appendChild(document.createTextNode('流量 ' + item.StdFlow));

                        var span3 = this._span = document.createElement("p");
                        span3.className = "mapP";
                        //span3.style.marginBottom = '18px';

                        var i3 = this._span = document.createElement("i");
                        i3.className = 'fa fa-bullseye text-info'
                        i3.style.marginRight = "4px"
                        span3.appendChild(i3);

                        div.appendChild(span3);
                        span3.appendChild(document.createTextNode('温度 ' + item.Temperature + ' 压力 ' + item.Pressure));

                        var that = this;

                        var arrow = this._arrow = document.createElement("div");
                        arrow.className = "mapLabelImg"
                        arrow.style.background = "url(../current/images/down.png) no-repeat";
                        arrow.style.position = "absolute";
                        arrow.style.width = "16px";
                        arrow.style.height = "16px";
                        arrow.style.top = "68px";
                        arrow.style.left = "60px";
                        arrow.style.overflow = "hidden";
                        div.appendChild(arrow);

                        //div.onmouseover = function () {
                        //    this.style.backgroundColor = "#6BADCA";
                        //    this.style.borderColor = "#0000ff";
                        //    this.getElementsByTagName("span")[0].innerHTML = that._overText;
                        //    arrow.style.backgroundPosition = "0px -20px";
                        //}

                        //div.onmouseout = function () {
                        //    this.style.backgroundColor = "#EE5D5B";
                        //    this.style.borderColor = "#BC3B3A";
                        //    this.getElementsByTagName("span")[0].innerHTML = that._text;
                        //    arrow.style.backgroundPosition = "0px 0px";
                        //}

                        map.getPanes().labelPane.appendChild(div);

                        return div;
                    }
                    ComplexCustomOverlay.prototype.draw = function () {
                        var map = this._map;
                        var pixel = map.pointToOverlayPixel(this._point);
                        this._div.style.left = pixel.x - parseInt(this._arrow.style.left) + "px";
                        this._div.style.top = pixel.y - 100 + "px";
                    }
                    var txt = "银湖海岸城";

                    var myCompOverlay = new ComplexCustomOverlay(point, "3213213", '');

                    map.addOverlay(myCompOverlay);

                    //SetHtml('mapLabel-' + item.FLMeterNo)

                }
                //map.enableAutoResize();
            }
        } else {
            $('#info').css('display', 'none');
            map.clearOverlays(); //
            layer.msg('没有设备')
        }
    })
}

function SetData(data) {
    $('#info').css('display', 'block')
    $('.deviceNo').html(data.deviceNo)
    $('.communicateNo').html(data.communicateNo)
    $('.StdSum').html(data.StdSum)
    $('.WorkSum').html(data.WorkSum)
    $('.StdFlow').html(data.StdFlow)
    $('.WorkFlow').html(data.WorkFlow)
    $('.Temperature').html(data.Temperature)
    $('.Pressure').html(data.Pressure)
    $('.RemainMoney').html(data.RemainMoney)
    $('.RemainVolume').html(data.RemainVolume)
    $('.PowerVoltage').html(data.PowerVoltage)
    data.FMStateMsg = data.FMStateMsg == '' ? '正常' : data.FMStateMsg
    $('.FMStateMsg').html(data.FMStateMsg)
    var time = ChangeTimeFormat(data.InstantTime);
    $('.InstantTime').html(time)
    $('.customerName').html(data.customerName)
    $('.address').html(data.address)
}

function ComplexCustomOverlay(point, text, mouseoverText) {
    this._point = point;
    this._text = text;
    //this._overText = mouseoverText;
}

function SetHtml(className) {
    $('mapLabel').append('<div>' + className + '</div>')
}