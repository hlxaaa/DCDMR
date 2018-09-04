var orderField = 'id'
var desc = true;
var listSort;
var defaultField = 'id'
var isAdd = true;
var areaId;
var page = 0
var ths=''

$(document).ready(function () {
    changePage(1);
})

function changePage(index) {
    page = index;
    listSort = new Array();
    $('.table th').each(function () {
        listSort.push($(this).attr('class'))
    })
    var data = {
        search: $('.input-search').val(),
        pageIndex: index,
        orderField: orderField,
        isDesc: desc,
        listSort: listSort
    }
    jQuery.postNL('../RecordAjax/GetOperRecordList', data, function (data) {
        var index = data.index;
        var pages = data.pages;
        var listSort = data.sort;
        var data = data.ListData;
        if (data.length == 0) {
            //$('.rt_content .table tr:first').nextAll().remove();
            if ($('.rt_content .table th').length >1) {
                ths = $('.rt_content .table tr:first').html();
            }

            $('.rt_content .table tr').remove();
            $('.rt_content .table tbody').append('<tr class="tr-del"><th>没有相关信息</th></tr>')
            $('.rt_content .paging').children().remove();
            return;
        }
        //else {
        //    if ($('.rt_content .table tr th').length < 2)
        //        $('.rt_content .table tr').remove();
        //}

        var h = ''
        if ($('.rt_content .table th').length < 2) {
            h +=`<tr>${ths}</tr>`;
        }
        //console.log(h);
            //h = '<table class="table" id="fixtable"><tr><th class="' + listSort[0] + '" data-sort="id">记录ID</th><th class="' + listSort[1] + '" data-sort="content">操作内容</th><th class="' + listSort[2] + '" data-sort="operatorName">操作者</th><th class="' + listSort[3] + '" data-sort="operateTime">操作时间</th></tr>';
        //h=''
        //<th>操作</th>
        for (var i = 0; i < data.length; i++) {
            //var item = data[i]
            //name = item.name

            var item = data[i]
            var id = item.id
            var content = item.content
            var operatorName = item.operatorName
            var operateTime = ChangeTimeFormat(item.operateTime)
            h += '<tr><td class="center tds td-id" title="' + id + '">' + id + '</td><td class="center tds td-content" title="' + content + '">' + content + '</td><td class="center tds td-operatorName" title="' + operatorName + '">' + operatorName + '</td><td class="center tds td-operateTime" title="' + operateTime + '">' + operateTime + '</td></tr>'
            //<td class="center "  data-id="' + id +'"></td>
        }
        h += ' </table>'
        //$('.rt_content .table').remove();
        $('.tr-del').remove();
        $('.rt_content .table tr:first').nextAll().remove();
        $('.rt_content .table').append(h);
        //$('.rt_content .paging').before(h);
        $('.rt_content .paging').children().remove();
        var h = getPageHtml(pages, index);
        //console.log(h);
        $('.rt_content .paging').append(h);

        tableFixedTest("fixtable")
    })
}