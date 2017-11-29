function changecode() {
    var timestamp = new Date().getTime();
    $("#img_code").attr('src', "/account/getValidCode?timestamp=" + timestamp);
}