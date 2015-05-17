function bindTrader(selector) {
    $(selector).html("");
    $.ajax({
        url: "/Select/Trader",
        dataType: "json",
        cache: false,
        success: function (msg) {
            $(msg).each(function (i, v) {
                $(selector).append("<option value='" + v.key + "'>" + v.val + "</option>");
            });
        }
    })
}

function bindFactory(selector) {
    $(selector).html("");
    $.ajax({
        url: "/Select/Factory",
        dataType: "json",
        cache: false,
        success: function (msg) {
            $(msg).each(function (i, v) {
                $(selector).append("<option value='" + v.key + "'>" + v.val + "</option>");
            });
        }
    })
}