function GenerateAjaxRequist(url, paramObject, type) {
    //debugger
    var objData = {};
    if (paramObject === null)
    {
        $.ajax({
            type: type,
            contentType: "application/json; charset=utf-8",
            url: url,
            //data: paramObject,
            dataType: 'json',
            cache: false,
            async: false,
            success: function (data) {
                //debugger
                objData = data;
            },
            error: function (request, status, error) {
                console.log(status.statusText);
                objData = {};
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
    else {
        $.ajax({
            type: type,
            contentType: "application/json; charset=utf-8",
            url: url,
            data: paramObject,
            dataType: 'json',
            cache: false,
            async: false,
            success: function (data) {
                //debugger
                objData = data;
            },
            error: function (request, status, error) {
                console.log(status.statusText);
                objData = {};
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
    return objData;
}
function ClearFields() {
    $('input[type="text"]').val('');
    $('input[type="number"]').val('');
    $('input[type="hidden"]').val('');
    $('input[type="date"]').val('');
    $('textarea').val('');
    $("select").each(function () { this.selectedIndex = 0 });
}
function isNullOrEmpty(val) {
    return (typeof val === 'undefined' || val === undefined || val === null || val === "");
}

function NumericTextboxOnly(placeholder) {
    
    $(placeholder).keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}