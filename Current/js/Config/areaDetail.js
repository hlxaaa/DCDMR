$(document).ready(function () {
    $('body').delegate('.sr-div', 'click', function () {
        $('.sr-div2').removeClass('sr-div2');
        $(this).addClass('sr-div2');
        map.clearOverlays();
        var lat = $(this).data('lat')
        var lng = $(this).data('lng')
        var point = new BMap.Point(lng, lat);
        var marker = new BMap.Marker(point);  // 创建标注
        map.addOverlay(marker);
        $('#poi_cur').val(lat)
        $('#addr_cur').val(lng);
    })

    //$('body').delegate('#suggestId', 'change', function () {
    //    layer.msg(1);
    //    var str = $(this).val();
    //    layer.msg(str.length)
    //})

    $('#suggestId').keyup(function () {
        //layer.msg(1);
        var str = $(this).val();
        if (str.length > 1)
            $('#btn-searchMap').click();
    })

    $('#btn-searchMap').click(function () {
        var h = ''

        var local = new BMap.LocalSearch(map, {
            renderOptions: {
                map: map, /*panel: "searchResult"*/
            },
            onSearchComplete: function (results) {
                //console.log(1);
                //可以得到搜索结果且搜索结果不为空 
                if (local.getStatus() == BMAP_STATUS_SUCCESS) {
                    var html = "";
                    var title = new Array();
                    var lats = new Array();
                    var lngs = new Array();
                    var addresses = new Array();
                    //遍历结果第一页的点，自定义结果面板
                    for (var i = 0; i < results.getCurrentNumPois(); i++) {
                        var poi = results.getPoi(i);
                        //下面根据LocalResultPoi对象的值拼html代码，此处略
                        title[i] = poi.title;
                        lats[i] = poi.point.lat;
                        lngs[i] = poi.point.lng;
                        //console.log(poi.address)
                        //if (poi.address)
                        addresses[i] = poi.address;
                        //if (poi.phoneNumber)
                        //    telephone[i] = poi.phoneNumber;
                        html += `   <div class="sr-div" data-lat="${poi.point.lat}" data-lng="${poi.point.lng}">
                    <div class="sr-name">${poi.title}</div>
                    <div class="sr-addr">${poi.address}</div>
                </div>`


                    }
                    $('.sr-div').remove();
                    $('#searchResult').append(html);


                    //console.log(title)
                    //console.log(address)
                    //console.log(telephone)
                    //重新遍历第一页所有点，对结果面板中的每一条记录添加click事件
                    for (var i = 0; i < results.getCurrentNumPois(); i++) {
                        $("#poi" + i).click(function () {
                            //这里用前面title、address、telephone三个数组中存放的值来拼信息窗里的html代码，存在变量content中，然后：
                            var info = new BMap.InfoWindow(content);
                            //利用在第一个问题中的markerArr数组设置触发函数，但注意数组的索引值不能用i，因为函数运行时i已不存在，因此在构造结果面板时，每个节点我添加了一个index属性，并用下面的代码获取，设置属性的代码类似于：<div id='poi5' index='5'></div>                        
                            markerArr[$(this).attr("index")].openInfoWindow(info);
                        })
                    }
                } else {
                    layer.msg('没有结果')
                }
                map.clearOverlays()

            },
            pageCapacity: 8

        });
        local.search($('#suggestId').val());
    })

    $('#suggestId').keypress(function (e) {
        if (e.keyCode == 13)
            $('#btn-searchMap').click();
    })

    $('#select-meterNo').change(function () {
        var meterNo = $(this).val();
        location.href = '/config/deviceLatlng?meterNo=' + meterNo;
    })

    $('.btn-save').click(function () {
        var data = {
            lat: $('#poi_cur').val(),
            lng: $('#addr_cur').val(),
            meterNo: $(this).data('meterno')
        }
        jQuery.postNL('../deviceAjax/updateLatlng', data, function (data) {
            layer.msg(data.Message)
        })
    })
})