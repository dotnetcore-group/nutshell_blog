$.fn.openWindow = function (options) {
    var s = {
        width: 500,
        height: 300
    };
    var window = $(this);
    var $bg = $("<div class='m-bg'></div>");
    var $close = $("<i class='fa fa-times m-w-close'></i>");
    window.append($close);
    $bg.click(function () {
        window.closeWindow();
    });
    $close.click(function () {
        window.closeWindow();
    });
    window.addClass("mWindow")
    $('body').append($bg);
    var w = options.width ? options.width : s.width;
    var h = options.height ? options.height : s.height;

    window.css({ 'width': w, 'height': h, 'margin-left': w / -2, 'margin-top': h / -2 });

    window.show();

    return this;
}
$.fn.closeWindow = function () {
    $(this).hide();
    $(".m-bg").remove();

    return this;
}