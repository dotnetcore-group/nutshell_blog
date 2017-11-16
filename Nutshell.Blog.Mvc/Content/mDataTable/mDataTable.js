/**************************************************************************************************************
* 
* 
* 
**************************************************************************************************************/
// 数据格式
//pageindex: [Number]
//pagesize: [Number]
//ajax: {
//    url: [String],
//        type: [String],
//        data:[Object]
//},
//columns: [
//    { data: [String], hidden:[Boolean], render: [Function], class: [String] }
//]

// 后台返回格式：
//{
//    code: [Number] 0（成功） / 1（失败）,
//    res: [Array] 数据,
//    msg:[String] 只有在错误的时候返回
//    total:[Number] 总数据
//    index:[Number] 当前页
//    size: [Number] 每页条数
//}

$.fn.extend({
    mDataTable: function (options) {
        // 表格对象
        var $table = $(this);
        // 提示信息框
        var $div = $("<div class='mLoading'></div>");
        var $footer = $("<div class='mFooter'></div>");
        $div.text("数据加载中...");
        $table.children("tbody").remove();
        $table.children("div.mLoading").remove();
        $table.children("div.mFooter").remove();
        $table.append($div);

        // 获取配置
        var url = options.ajax.url;
        var data = options.ajax.data ? options.ajax.data : {};
        var columns = options.columns;
        var type = options.ajax.type ? "POST" : options.ajax.type;
        var pagesize = options.pagesize && (Number)(options.pagesize) ? (Number)(options.pagesize) : 10;
        var pageindex = options.pageindex && (Number)(options.pageindex) ? (Number)(options.pageindex) : 1;
        // 添加参数
        data.pageindex = pageindex;
        data.pagesize = pagesize;

        //console.log(data);

        // 数据填充对象
        var $tbody = $("<tbody></tbody>");

        // 发送请求
        $.ajax({
            type: type,
            url: url,
            data: data,
            dataType: "json",
            success: function (res) {
                _success(res);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $div.text("数据加载失败 (" + errorThrown + ")！");
                $table.append($div);
            }
        });

        // ajax 成功返回
        var _success = function (res) {
            if (res.code == 0) {
                $.each(res.res, function (i, e) {
                    var $tr = $('<tr></tr>');
                    if (i % 2 != 0) $tr.addClass("altitem");
                    for (var i = 0; i < columns.length; i++) {
                        var column = columns[i];
                        var render = column.render;
                        var className = column.class;
                        var $td = $("<td></td>");
                        if (render != undefined) {
                            $td.append(render(e[column.data], e));
                        } else {
                            $td.html(e[column.data]);
                        }
                        if (column.hidden) {
                            $td.addClass("hidden");
                        }
                        if (className != undefined) {
                            $td.addClass(className);
                        }
                        $tr.append($td);
                        $tbody.append($tr);
                    }
                });
                $footer.text('共有' + res.total + '条数据');
                $table.children("div.mLoading").remove();
                $table.append($tbody);
                $table.append($footer);
            }
            else {
                $div.text(res.msg);
            }
        }
    }
});
