$(function () {
    $.each($(".tabs-nav li"), function (i, e) {
        $(this).click(function () {
            if ($(this).hasClass("active")) {
                return;
            }
            $("li.active").removeClass("active");
            $("div.active").removeClass("active");
            $(this).addClass("active");
            $($(".tab-panel")[i]).addClass("active");
        });
    });

    var hash = location.hash;
    $(hash).trigger("click");
});