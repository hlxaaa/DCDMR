function gotoPage(pageNum) {
    $(document.forms[0]).append("<input type='hidden' name='pageNum' value='" + pageNum + "'/>");
    document.forms[0].submit();
}

function map_init(e) {
    var markerArr = document.getElementById("mapValue");  //这里是当前页的 Json  
    markerArr = eval(markerArr.value);

    var markerArrOther = document.getElementById("mapOtherValue"); //这里获取的是剩余的 地图Json   
    markerArrOther = eval(markerArrOther.value);

    var map = new BMap.Map("map"); // 创建Map实例  
    var point = new BMap.Point(116.302771, 39.963603); //地图中心点，海淀区   
    map.centerAndZoom(point, 14); // 初始化地图,设置中心点坐标和地图级别。  
    map.enableScrollWheelZoom(true); //启用滚轮放大缩小  
    //地图、卫星、混合模式切换  
    map.addControl(new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP] }));
    //向地图中添加缩放控件  
    var ctrlNav = new window.BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_LEFT,
        type: BMAP_NAVIGATION_CONTROL_LARGE
    });
    map.addControl(ctrlNav);

    //向地图中添加缩略图控件  
    var ctrlOve = new window.BMap.OverviewMapControl({
        anchor: BMAP_ANCHOR_BOTTOM_RIGHT,
        isOpen: 1
    });
    map.addControl(ctrlOve);

    //添加全景 控件   
    var stCtrl = new BMap.PanoramaControl(); //构造全景控件  
    stCtrl.setOffset(new BMap.Size(40, 40));
    map.addControl(stCtrl);//添加全景控件  

    //向地图中添加比例尺控件  
    var ctrlSca = new window.BMap.ScaleControl({
        anchor: BMAP_ANCHOR_BOTTOM_LEFT
    });
    map.addControl(ctrlSca);

    var point = new Array(); //存放标注点经纬信息的数组  
    var marker = new Array(); //存放标注点对象的数组  
    var info = new Array(); //存放提示信息窗口对象的数组  
    var searchInfoWindow = new Array();//存放检索信息窗口对象的数组  

    var pointOther = new Array(); //同上 一样，只不过这里是剩余的查看 Json数组  
    var markerOther = new Array();
    var infoOther = new Array();
    var searchInfoWindowOther = new Array();

    for (var i = 0; i < markerArr.length; i++) {
        var p0 = markerArr[i].longitude; //  
        var p1 = markerArr[i].latitude; //按照原数组的point格式将地图点坐标的经纬度分别提出来  
        point[i] = new window.BMap.Point(p0, p1); //循环生成新的地图点  

        var iconImg = new BMap.Icon('${pageContext.request.contextPath}/images/mapimage/' + (i + 1) + '.png', new BMap.Size(32, 47), {
            anchor: new BMap.Size(10, 30)
        });
        marker[i] = new window.BMap.Marker(point[i], { icon: iconImg }); //按照地图点坐标生成标记  
        map.addOverlay(marker[i]);
        var label = new window.BMap.Label(markerArr[i].name, { offset: new window.BMap.Size(20, -10) });

        // 创建信息窗口对象  
        info[i] = '<p style="width:280px;margin:0;line-height:20px;">市场名称：' + markerArr[i].name + '</br>地址：' + markerArr[i].address + '</br> 电话：' + markerArr[i].contactPhone + '</p>';
        //infoOther[i] = '<p style="width:280px;margin:0;line-height:20px;">市场名称：' + markerArrOther[i].name  + '</br>地址：' + markerArrOther[i].address + '</br> 电话：' + markerArrOther[i].contactPhone + '</p>';  

        //创建百度样式检索信息窗口对象                         
        searchInfoWindow[i] = new BMapLib.SearchInfoWindow(map, info[i], {
            title: markerArr[i].name,      //标题  
            width: 290,             //宽度  
            height: 55,              //高度  
            panel: "panel",         //检索结果面板  
            enableAutoPan: true,     //自动平移  
            searchTypes: [
                BMAPLIB_TAB_SEARCH,   //周边检索  
                BMAPLIB_TAB_TO_HERE,  //到这里去  
                BMAPLIB_TAB_FROM_HERE //从这里出发  
            ]
        });
        //判断当前点击的是否有数据   
        if (e != "" && e == markerArr[i].name) {
            searchInfoWindow[i].open(marker[i]);
        }
        //添加点击事件  
        marker[i].addEventListener("click",
            (function (k) {
                // js 闭包  
                return function () {
                    searchInfoWindow[k].open(marker[k]);
                }
            })(i)
        );
    }
    //从这里写新的Json  
    for (var i = 0; i < markerArrOther.length; i++) {
        var p0 = markerArrOther[i].longitude; //  
        var p1 = markerArrOther[i].latitude; //按照原数组的point格式将地图点坐标的经纬度分别提出来  
        pointOther[i] = new window.BMap.Point(p0, p1); //循环生成新的地图点  

        var iconImgOther = new BMap.Icon('${pageContext.request.contextPath}/images/mapimage/11.png', new BMap.Size(20, 30), {
            anchor: new BMap.Size(10, 30)
        });
        markerOther[i] = new window.BMap.Marker(pointOther[i], { icon: iconImgOther }); //按照地图点坐标生成标记  
        map.addOverlay(markerOther[i]);
        var labelOther = new window.BMap.Label(markerArrOther[i].name, { offset: new window.BMap.Size(20, -10) });
        // 创建信息窗口对象  
        infoOther[i] = '<p style="width:280px;margin:0;line-height:20px;">市场名称：' + markerArrOther[i].name + '</br>地址：' + markerArrOther[i].address + '</br> 电话：' + markerArrOther[i].contactPhone + '</p>';


        //创建百度样式检索信息窗口对象                         
        searchInfoWindowOther[i] = new BMapLib.SearchInfoWindow(map, infoOther[i], {
            title: markerArrOther[i].name,      //标题  
            width: 290,             //宽度  
            height: 55,              //高度  
            panel: "panel",         //检索结果面板  
            enableAutoPan: true,     //自动平移  
            searchTypes: [
                BMAPLIB_TAB_SEARCH,   //周边检索  
                BMAPLIB_TAB_TO_HERE,  //到这里去  
                BMAPLIB_TAB_FROM_HERE //从这里出发  
            ]
        });
        //添加点击事件  
        markerOther[i].addEventListener("click",
            (function (k) {
                // js 闭包  
                return function () {
                    searchInfoWindowOther[k].open(markerOther[k]);
                }
            })(i)
        );

    }

}
//异步调用百度js  
function map_load() {
    var load = document.createElement("script");
    load.src = "http://api.map.baidu.com/api?v=2.0&ak=IDvNBsejl9oqMbPF316iKsXR&callback=map_init";
    document.body.appendChild(load);
}

//显示信息弹框   
function showInfoWindow(obj) {
    var a = $(obj).attr('value');
    document.getElementById("txtContent").value = a;
    map_init(a);
}  