

function gBankerDatePicker(controlid) {
    if (controlid == null) {
        return;
    }
    $("#" + controlid).datepicker(
           {
               dateFormat: "dd-M-yy",
               showAnim: "scale",
           });
}