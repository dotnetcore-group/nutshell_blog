var hub = $.connection.pushHub;
$.connection.hub.start().done(function () {
    console.log("connected!")
});
hub.client.receive = function (msg) {
    console.log(msg);
}

hub.client.sayHello = function (msg) {
    console.log(msg);
}