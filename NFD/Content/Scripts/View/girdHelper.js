function JQgridSaved(formId, rollback) {
    setInterval(function () {
        var fdoForm = $("#" + formId);
        if (fdoForm.size() > 0) {
            rollback(fdoForm);
        }
    }, 1000)
}


function JQgridSaveBindByFirst(gridId, formObj, id) {
    var data = $("#"+gridId).jqGrid("getRowData");
    if ($(data).size() >= 1) {
        var rowFirst = data[0];
        var jno = formObj.find("#"+id);
        if (jno.val() == "") {
            jno.val(rowFirst[id]);
        }
    }
}