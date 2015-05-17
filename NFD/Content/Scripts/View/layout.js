
var ztreeSetting = {
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: ztreeOnClick
    }
};

function ztreeOnClick(event, treeId, treeNode, clickFlag) {

}
$(document).ready(function () {
    $.fn.zTree.init($(".zTree_v3 #divLayoutMenu"), ztreeSetting, zNodes);
    $("#divLayoutMenu").height($(window).height()-200)
});