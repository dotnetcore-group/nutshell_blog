$(function () {
    var $menu = $("#menu");

    // <li class="sidebar-nav-link">
    //      <a data-href="chart.html">
    //          <i class="am-icon-bar-chart sidebar-nav-link-logo"></i> 图表
    //      </a>
    // </li>
    // <li class="sidebar-nav-link"><a data-href="chart.html"><i class="am-icon-bar-chart sidebar-nav-link-logo"></i> 图表</a></li>
    // <li class="sidebar-nav-link"><a href="javascript:;" class="sidebar-nav-sub-title"><i class="am-icon-table sidebar-nav-link-logo"></i> 数据列表<span class="am-icon-chevron-down am-fr am-margin-right-sm sidebar-nav-sub-ico"></span></a><ul class="sidebar-nav sidebar-nav-sub">
    // <li class="sidebar-nav-link"> <a href="table-list.html"> <span class="am-icon-angle-right sidebar-nav-link-logo"></span> 文字列表 </a> </li>
    $.get("/admin/home/getmenu", function (data) {
        var menu = data.menu;
        var child = new Array();

        if (menu == null) {
            return;
        }
        $.each(menu, function (i) {
            //console.log(menu[i].Parent_Id);
            // 一级菜单
            var parentId = menu[i].Parent_Id;
            if (parentId == 0) {
                // 无子项
                if (menu[i].IsLast) {
                    $menu.append('<li class="sidebar-nav-link"><a data-href="' + menu[i].Url + '"><i class="am-icon-home sidebar-nav-link-logo"></i> ' + menu[i].Name + '</a></li>');
                } else {
                    $menu.append('<li class="sidebar-nav-link"><a href="javascript:;" class="sidebar-nav-sub-title"><i class="am-icon-home sidebar-nav-link-logo"></i> ' + menu[i].Name + '<span class="am-icon-chevron-down am-fr am-margin-right-sm sidebar-nav-sub-ico"></span></a><ul class="sidebar-nav sidebar-nav-sub" id="menu_' + menu[i].Id + '"></ul>');
                }
            } else {
                child.push(menu[i]);
            }
        });
        //console.log(child);
        $.each(child, function (i) {
            console.log(child[i]);
            $("#menu_" + child[i].Parent_Id).append('<li class="sidebar-nav-link"> <a href="table-list.html"> <span class="am-icon-angle-right sidebar-nav-link-logo"></span> ' + child[i].Name + ' </a> </li>');
        });
        //var target = $.getUrlParam('target');

        //$menu.collapse();

    });
});