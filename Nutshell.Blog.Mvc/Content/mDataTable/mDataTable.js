/**************************************************************************************************************
* 
* 
* 
**************************************************************************************************************/
// 数据格式
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
        $("div.mLoading").hide();
        $("div.mFooter").remove();
        $table.parent().append($div);
        //$table.parent().append($footer);

        // 获取配置
        var url = options.ajax.url;
        var data = options.ajax.data ? options.ajax.data : {};
        var columns = options.columns;
        var type = options.ajax.type ? "POST" : options.ajax.type;
        var pagesize = options.pagesize && (Number)(options.pagesize) ? (Number)(options.pagesize) : 20;
        var pageindex = 1;
        var ePager = options.pager ? true : false;

        if (ePager) {
            // 添加参数
            data.pageindex = pageindex;
            data.pagesize = pagesize;
        }

        //console.log(data);

        // 数据填充对象
        var $tbody = $("<tbody></tbody>");


        // 发送请求
        var _ajax = function () {
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
        }
        _ajax();

        // ajax 成功返回
        var _success = function (res) {
            if (res.code == 0) {
                $tbody.empty();
                if (res.res.length > 0) {
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
                } else {
                    $tbody.append("<tr><td colspan='999'>表中数据为空</td></tr>")
                }
                $table.append($tbody);
                if (ePager) {
                    _pager(res.total, pagesize, res.index);
                }
                $("div.mLoading").remove();
            }
            else {
                $div.text(res.msg);
            }
        };

        var _pager = function (total, size, index) {
            $footer.empty();
            //$(".mFooter").remove();

            var temp = total % size == 0 ? total / size : total / size + 1;
            var totalPage = parseInt(temp);
            var $pager = $('<div class="mPager"></div>');
            var $text = $("<span>" + total + "条 共" + totalPage + "页</span>");

            $pager.append($text);

            if (totalPage == 1) {
                $pager.append('<a href="javascript:void(0);" class="active">1</a>');
                $footer.append($pager);
                $table.after($footer);
                return;
            }

            // 起始位置 页面上显示10个数字页码.
            var start = index - 5;
            if (start < 1) {
                start = 1;
            }
            // 终止位置
            var end = start + 9;
            if (end > totalPage) {
                end = totalPage;
                start = end - 9 < 1 ? 1 : end - 9;
            }
            // 上一页
            if (index > 1) {

            }
            for (var i = start; i <= end; i++) {
                var $link = $('<a href="javascript:void(0);"></a>');
                $link.text(i);
                if (i == index) {
                    $link.addClass('active');
                }
                $pager.append($link);
                $link.on('click', function () {
                    var current = isNaN(parseInt($(this).text())) ? 1 : parseInt($(this).text());
                    if (pageindex == current) {
                        return;
                    }
                    $table.children("tbody").remove();
                    _pager(total, size, current);
                    //$table.parent().append($div);
                    $('.mFooter').before($div);
                    pageindex = current;
                    data.pageindex = current;
                    _ajax();
                });
            }

            // 下一页
            if (index < totalPage) {

            }
            $table.after($footer);
            $footer.append($pager);

        }

        //return options;
    }//,
    //reload: function (options) {
    //    return $(this).mDataTable(options);
    //}
});
