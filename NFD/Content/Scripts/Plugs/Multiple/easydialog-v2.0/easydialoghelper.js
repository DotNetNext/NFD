function ed_alert(title, content) {
    easyDialog.open({
        container: {
            header: title,
            autoClose:2000,
            content: content,
            yesFn: function () { }
        }
    });
}


function ed_alertBox(selector, title,fn) {
    var $selector = $(selector).clone(true);
    $selector.show();
    easyDialog.open({
        container: {
            header: title,
            content: $selector.html(),
            yesFn: function () { if (fn != null) { fn(); } }
        }
    });
}
function ed_alertBoxNoClost(selector, title, fn) {
    var $selector = $(selector).clone(true);
    $selector.show();
    easyDialog.open({
        container: {
            header: title,
            content: $selector.html(),
            yesFn: function () { if (fn != null) { fn(); } return false; }
        }
    });
}