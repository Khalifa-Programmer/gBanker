$(function () {
    let Message = "";
   
    //$("#txtMessage").val("Message");
    var myVar = setInterval(myTimer, 1000);
});
let iner = 1;

function myTimer() {
    //iner = iner + 1;
    //Message = "Message " + iner.toString();
    //$("#txtMessage").val(Message);
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        //GetMessage(int OfficeID)
        url: '/ProcessConfig/GetMessage',
        data: { OfficeID: 0},  //GetMessage(int OfficeID)
        
        dataType: 'json',
        async: true,
        success: function (data) {
            if (data == "Running") { $("#dvrunning").show(); $("#dvcompleted").hide(); }
            else { $("#dvrunning").hide(); $("#dvcompleted").show(); }

            $("#txtMessage").text("Process " + data);
        },
        error: function (request, status, error) {
            $.alert.open("Message", error);
        }
    });   // END ajax

}

 

