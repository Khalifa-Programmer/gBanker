
//scaffolding model
var dailyWorkProcessManager = { }

var bulkSMSAPIBaseUrl = $('#hideenApiBaseUrl').val();

dailyWorkProcessManager.displayLoanSummaryReport=()=>{

    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    var reportParam = startDate + ',' + endDate;
    var url = '/Report/Print?reportId=LoanSummary&reportparams=' + reportParam;
    window.open(url, "_blank");
}

dailyWorkProcessManager.displayProductReport = () => {

    var processtDate = $("#processtDate").val();
    var endDate = $("#endDate").val();
    var reportParam = startDate + ',' + endDate;
    var url = '/Report/Print?reportId=Product&reportparams=' + reportParam;
    window.open(url, "_blank");
}

dailyWorkProcessManager.executeToSendSMSToMembers = (resp) => {
    var officeId = $('#hiddenOfficeId').val();
    var organizationCode = $('#hiddenOrganizationCode').val();
    var loginUserOrganizationID = $('#hiddenLoginUserOrganizationID').val();

    var dateSMS = $.datepicker.formatDate('dd-M-yy', new Date());
    const formData = {
        OfficeIDs: [officeId],
        SearchTerm:'',
        StartDateInString: dateSMS,
        EndDateInString: dateSMS,
        OrganizationCode: organizationCode,
        LoginUserOrganizationID: loginUserOrganizationID,
    }

    const url = `${bulkSMSAPIBaseUrl}/api/SendSMS/sendbulksms`;

    $.ajax({
        url: url,
        type: "POST",
        headers: {
            'Authorization': `Bearer ${resp.token}`,
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(formData),
        //contentType: "application/json",
        success: function (res) {
            //TODO: need to display message 
        },
        error: function (err) {
            //TODO: need to display message
        }
    })
}

dailyWorkProcessManager.sendSMSToMembers = () => {   
    debugger;
    //authenticate and get access token
    const clientInfo = $.cookie(BulkSMSAuthConstants.BulkSMSAuthClientKey);

    if (clientInfo == 'undefined') {
        return;
    }

    const client = clientInfo.split('@');
    const clientId = client[0];
    const clientSecret = client[1];

    const formData = { ClientId: clientId, ClientSecret: clientSecret }
    const url = `${bulkSMSAPIBaseUrl}/api/Authorize/Token`;
  
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(formData),
        contentType:"application/json",
        success: function (resp) {
            //let's send sms to members
            dailyWorkProcessManager.executeToSendSMSToMembers(resp);
        },
        error: function (err) {
            //TODO: need to display message
        }
    })
}

dailyWorkProcessManager.rundayEnd = () => {
    //  e.preventDefault();
    var officeId = $("#OfficeId").val();
    var businessDate = $("#BusinessDate").val();

    $("#loading").show();

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/DayEnd/DayEndProcess',
        data: { officeId: officeId, businessDate: businessDate },
        dataType: 'json',
        async: true,
        success: function (data) {
            alert(data.Message);
            $("#loading").hide();
            var msg = "Work process successful";
            var css = "success";
            if (data.Result === 'ERROR') {
                msg = data.Message;
                css = "failed"
            }

            $("#dvMessage").attr('class', css);
            $("#dvMessage").html(msg);
            $("#dvMessage").show();
            if (data.Result === 'OK') {
                $("#dvMessage").toggle('fade', 1500);

                //let's send sms to members
                var hiddenSendAutoSMS = $('#hiddenSendAutoSMS').val();
                if (hiddenSendAutoSMS === '1') {
                    dailyWorkProcessManager.sendSMSToMembers();
                }
            }

        },
        error: function (request, status, error) {
            alert(data.Message);
            $("#loading").hide();
            $("#dvMessage").attr('class', 'failed');
            $("#dvMessage").html(request.statusText);
            $("#dvMessage").show();
        }
    });
}

$(function () {

    confirmDayInitiated();

    gBankerDatePicker("BusinessDate");

    $("#confirmDlg").dialog({
        autoOpen: false,
        height: 150,
        width: 400,
        modal: true,
        title: 'Confirmation',
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                dailyWorkProcessManager.rundayEnd();
               
            },
            "No": function () {               
                $(this).dialog("close");
            }
        }
    });

    $('#BusinessDate').bind('focus', function (e) {
        $("#dvMessage").hide();
    });

    $("#OfficeId").change(function () {
        var officeId = $("#OfficeId").val();
    });

    $('#btnProcess').click(function (e) {
        e.preventDefault();
        var bdate = $("#BusinessDate").val();
        var msg = "Do you want to run the Process? " + bdate.toString("dd-MMM-yy");
        $("#lblMsg").text(msg);
        $("#confirmDlg").dialog('open');
    });
})
