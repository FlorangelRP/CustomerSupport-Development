var SessionTimer;    
$(document).ready(function () {    
    setInterval("fnSessionRun()", 1000);    
});    
function fnSessionRun() {    
    SessionTimer -= 1;    
    if (SessionTimer == 0) {    
        window.location.href = $("#hdnAppPath").val() + '/Login';    
    }    
    if ($("#hdnSessionExpNotice").val() == SessionTimer) {    
        $("#SessionExpNotice").modal('show');    
    }    
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