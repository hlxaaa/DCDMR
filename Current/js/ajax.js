/// <reference path="../../Current/layer/layer.js" />
/*****************************************************************
                  jQuery Ajax封装通用类  (linjq)       
*****************************************************************/
$(function () {
    var baseUrl = "/";
    /**
     * ajax封装
     * url 发送请求的地址
     * data 发送到服务器的数据，数组存储，如：{"date": new Date().getTime(), "state": 1}
     * async 默认值: true。默认设置下，所有请求均为异步请求。如果需要发送同步请求，请将此选项设置为 false。
     *       注意，同步请求将锁住浏览器，用户其它操作必须等待请求完成才可以执行。
     * type 请求方式("POST" 或 "GET")， 默认为 "GET"
     * dataType 预期服务器返回的数据类型，常用的如：xml、html、json、text
     * successfn 成功回调函数
     * errorfn 失败回调函数
     */
    jQuery.ax = function (url, data, async, type, dataType, successfn) {
        async = (async == null || async == "" || typeof (async) == "undefined") ? "true" : async;
        type = (type == null || type == "" || typeof (type) == "undefined") ? "post" : type;
        dataType = (dataType == null || dataType == "" || typeof (dataType) == "undefined") ? "json" : dataType;
        data = (data == null || data == "" || typeof (data) == "undefined") ? { "date": new Date().getTime() } : data;
        $.ajax({
            type: type,
            async: async,
            data: data,
            url: baseUrl + url,
            dataType: dataType,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.HttpCode != 200) {
                    if (data.Message == null || data.Message == "") {
                        layer.msg("程序出现问题！");
                    }
                    else {
                        layer.msg(data.Message);
                    }
                }
                else {
                    successfn(data);
                }
            },
            error: function (e) {
                layer.msg('请求处理出错');
            }
        });
    };
    /**
     * ajax封装
     * url 发送请求的地址
     * data 发送到服务器的数据，数组存储，如：{"date": new Date().getTime(), "state": 1}
     * successfn 成功回调函数
     */
    jQuery.axpost = function (url, data, successfn) {
        $(".loading_area").fadeIn();
        data = (data == null || data == "" || typeof (data) == "undefined") ? { "date": new Date().getTime() } : data;
        $.ajax({
            type: "post",
            data: data,
            url: baseUrl + url,
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".loading_area").fadeOut();
                if (data.HttpCode != 200) {
                    if (data.HttpCode == 300) {
                        location.href = '/';
                    } else if (data.Message == null || data.Message == "") {
                        layer.msg("程序出现问题！");
                    }
                    else {
                        layer.msg(data.Message);
                    }
                }
                else {
                    successfn(data);
                }
            },
            error: function (e) {
                $(".loading_area").fadeOut();
                layer.msg('请求处理出错!');
            }
        });
    };

    //没有loading效果的
    jQuery.postNL = function (url, data, successfn) {
        data = (data == null || data == "" || typeof (data) == "undefined") ? { "date": new Date().getTime() } : data;
        $.ajax({
            type: "post",
            data: data,
            url: baseUrl + url,
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.HttpCode != 200) {
                    if (data.HttpCode == 300) {
                        location.href = '/';
                    } else if (data.Message == null || data.Message == "") {
                        layer.msg("程序出现问题！");
                    }
                    else {
                        layer.msg(data.Message);
                    }
                }
                else {
                    successfn(data);
                }
            },
            error: function (e) {
                layer.msg('请求处理出错!');
            }
        });
    };

    //用于errortips
    jQuery.axpost2 = function (url, data, successfn) {
        $(".loading_area").fadeIn();
        data = (data == null || data == "" || typeof (data) == "undefined") ? { "date": new Date().getTime() } : data;
        $.ajax({
            type: "post",
            data: data,
            url: baseUrl + url,
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".loading_area").fadeOut();
                if (data.HttpCode != 200) {
                    if (data.HttpCode == 300) {
                        location.href = '/';
                    } else if (data.Message == null || data.Message == "") {
                        layer.msg("程序出现问题！");
                    }
                    else {
                        var m = data.Message;
                        var i = m.indexOf(',');
                        if (i == -1) {
                            layer.msg(m);
                        } else {
                            var i2 = m.substring(i + 1).indexOf(',');
                            var c = m.substring(0, i)
                            var t = "";
                            if (i2 == -1) {
                                t = m.substring(i + 1)
                            } else {
                                t = m.substring(i + 1, i2 + i + 1)
                            }
                            ClearErrorTips();
                            $('.' + c).after(ErrorTipsHtml(t))
                        }
                    }
                }
                else {
                    successfn(data);
                }
            },
            error: function (e) {
                $(".loading_area").fadeOut();
                layer.msg('请求处理出错!');
            }
        });
    };

    /**
     * ajax封装
     * url 发送请求的地址
     * data 发送到服务器的数据，数组存储，如：{"date": new Date().getTime(), "state": 1}
     * successfn 成功回调函数
     */
    jQuery.axpostlayer = function (LayerTips, url, data, successfn) {
        data = (data == null || data == "" || typeof (data) == "undefined") ? { "date": new Date().getTime() } : data;

        $.ajax({
            type: "post",
            data: data,
            url: baseUrl + url,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.HttpCode != 200) {

                    if (data.HttpCode == 300) {
                        location.href = '/';
                    } else if (data.Message == null || data.Message == "") {
                        LayerTips.alert("程序出现问题！");
                    }
                    else {
                        LayerTips.alert(data.Message);
                    }
                }
                else {
                    successfn(data);
                }
            },
            error: function (e) {
                LayerTips.alert('请求处理出错!');
            }
        });
    };
});