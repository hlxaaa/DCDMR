var r;
var isAdd;
var addFlag = true;
$(document).ready(function () {

    //删除客户
    $('.btn-delCust').click(function () {
        var data = {
            customerNo: $(this).data('customerno')
        }
        jQuery.postNL('../deviceAjax/DeleteCustomer', data, function (data) {
            layer.msg(data.Message, {
                time: 1000,
                end: function () {
                    location.href = '/device/customerList'
                }
            })
        })
    })

    //新增、更新客户
    $('.btn-save').click(function () {
        if (isAdd == 1) {
            if (addFlag) {
                jQuery.postNL('../deviceAjax/Addcustomer', GetData(), function (data) {
                    layer.msg('添加成功,可继续添加', {
                        time: 1000,
                        end: function () {
                            location.reload();
                        }
                    })
                })
                addFlag = false;
            }
        } else {
            jQuery.postNL('../deviceAjax/UpdateCustomer', GetData(), function (data) {
                layer.msg('更新成功', {
                    time: 1000,
                    end: function () {
                        location.href = '/device/customerList'
                    }
                })

            })
        }
    })
})

//获取表单里的data
function GetData() {
    var data = {
        customerNo: $('.customerNo').val(),
        contractNo: $('.contractNo').val(),
        certNo: $('.certNo').val(),
        mobileNo: $('.mobileNo').val(),
        customerType: $('.customerType').val(),
        customerName: $('.customerName').val(),
        telNo: $('.telNo').val(),
        address: $('.address').val(),
        defineNo1: $('.defineNo1').val(),
        defineNo2: $('.defineNo2').val(),
        defineNo3: $('.defineNo3').val(),
        remark: $('.remark').val(),
    }
    return data;
}