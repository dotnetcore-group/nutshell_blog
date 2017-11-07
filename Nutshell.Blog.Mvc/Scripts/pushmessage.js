var hub = $.connection.pushHub;
$.connection.hub.start().done(function () {
    console.log("connected!")
});
hub.client.receive = function (msg) {
    console.log(msg);
    toastr.success(msg, '系统消息')
}

hub.client.sayHello = function (msg) {
    console.log(msg);
}