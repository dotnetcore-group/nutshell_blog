$(function () {
    $.each($(".tabs-nav li"), function (i, e) {
        $(this).click(function () {
            if ($(this).hasClass("active")) {
                return;
            }
            $(".active").removeClass("active");
            $(this).addClass("active");
            $($(".tab-panel")[i]).addClass("active");
        });
    });
});

