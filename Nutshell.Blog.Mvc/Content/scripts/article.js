
// 预览
function preview(id) {
    console.log(id);
    layer.open({
        type: 2,
        title: '文章预览',
        shadeClose: true,
        shade: 0.8,
        maxmin: true, //开启最大化最小化按钮
        area: ['893px', '600px'],
        content: '/admin/article/preview/' + id //iframe的url
    });
}
/*资讯-添加*/
function article_add(title, url, w, h) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url
    });
    layer.full(index);
}
/*资讯-编辑*/
function article_edit(title, url, id, w, h) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url
    });
    layer.full(index);
}
/*资讯-删除*/
function article_delete(id) {
    layer.confirm('确认要删除吗？', function (index) {
        var index = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景
        });
        $.post("/admin/article/delete", { id: id }, function (data) {
            if (data.code == 0) {
                layer.msg('已删除!', { icon: 1, time: 1000 });
                table.ajax.reload();
            } else {
                layer.msg(data.msg, { icon: 2, time: 1000 });
            }
            layer.close(index);
        });
    });
}

/*资讯-审核*/
function article_shenhe(id) {
    layer.confirm('审核文章？', {
        btn: ['通过', '不通过', '取消'],
        shade: false,
        closeBtn: 0
    },
        function () {
            $.post("/admin/article/ExaminePass", { id: id }, function (data) {
                if (data.code == 0) {
                    layer.msg(data.msg, { icon: 6, time: 1000 });
                    table.ajax.reload();
                } else {
                    layer.msg(data.msg, { icon: 5, time: 1000 });
                }
            });

        },
        function () {
            $.post("/admin/article/ExamineOut", { id: id }, function (data) {
                if (data.code == 0) {
                    layer.msg(data.msg, { icon: 6, time: 1000 });
                    table.ajax.reload();
                } else {
                    layer.msg(data.msg, { icon: 5, time: 1000 });
                }
            });
        });
}
/*资讯-下架*/
function article_stop(obj, id) {
    layer.confirm('确认要下架吗？', function (index) {
        $(obj).parents("tr").find(".td-manage").prepend('<a style="text-decoration:none" onClick="article_start(this,id)" href="javascript:;" title="发布"><i class="Hui-iconfont">&#xe603;</i></a>');
        $(obj).parents("tr").find(".td-status").html('<span class="label label-defaunt radius">已下架</span>');
        $(obj).remove();
        layer.msg('已下架!', { icon: 5, time: 1000 });
    });
}

/*资讯-发布*/
function article_start(obj, id) {
    layer.confirm('确认要发布吗？', function (index) {
        $(obj).parents("tr").find(".td-manage").prepend('<a style="text-decoration:none" onClick="article_stop(this,id)" href="javascript:;" title="下架"><i class="Hui-iconfont">&#xe6de;</i></a>');
        $(obj).parents("tr").find(".td-status").html('<span class="label label-success radius">已发布</span>');
        $(obj).remove();
        layer.msg('已发布!', { icon: 6, time: 1000 });
    });
}
/*资讯-申请上线*/
function article_shenqing(obj, id) {
    $(obj).parents("tr").find(".td-status").html('<span class="label label-default radius">待审核</span>');
    $(obj).parents("tr").find(".td-manage").html("");
    layer.msg('已提交申请，耐心等待审核!', { icon: 1, time: 2000 });
}
