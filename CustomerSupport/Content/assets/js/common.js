var SessionTimer = parseInt($("#hdnSessionTimeout").val());

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    setInterval("fnSessionRun()", 1000);
});
function fnSessionRun() {
    SessionTimer -= 1;
    if (SessionTimer == 0) {
        window.location.href = $("#hdnAppPath").val() + '/User/Login';
    }
    if ($("#hdnSessionExpNotice").val() == SessionTimer) {
        $("#SessionExpNotice").modal('show');
    }
    $("#spanTime").text(SessionTimer);
}

function SessionOKClick() {
    //ajax call to reset session    
    $.ajax({
        url: $("#hdnAppPath").val() + '/Common/ResetSession/',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        success: function (data) {
            $("#hdnSessionTimeout").val(parseInt(data));
            $("#SessionExpNotice").modal('hide');
        }
    });
}
$(document).ajaxStart(function (event, jqxhr, settings) {
    SessionTimer = parseInt($("#hdnSessionTimeout").val());
});
