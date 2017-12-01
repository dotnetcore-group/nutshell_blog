function changecode() {
    var timestamp = new Date().getTime();
    $("#img_code").attr('src', "/account/getValidCode?timestamp=" + timestamp);
}
function btn_loaded(str) {
    $(".btn-disable").addClass("btn-submit");
    $(".btn-disable").val(str);
    $(".btn-disable").attr("disabled", false);
    $(".btn-disable").removeClass("btn-disable");
}
function btn_loading(str) {
    $(".btn-submit").addClass("btn-disable");
    $(".btn-submit").attr("disabled", true);
    $(".btn-submit").val(str);
    $(".btn-submit").removeClass("btn-submit");
}