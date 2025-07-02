
const memberManager = {
    init: function () {

    },
    initCoApplicant: function () {
        if (!$('#CoApplicantName')) {
            return;
        }

        let gender = $('#Gender').val();
        let maritalStatus = $('#MaritalStatus').val();

        $('#CoApplicantName').val('');

        if (gender === GenderConstants.Male) {
            let memberName = $('#FirstName').val();
            $('#CoApplicantName').val(memberName);
        }
        else if (gender === GenderConstants.Female && maritalStatus === MaritalStatusConstants.Married) {
            let refereeName = $('#RefereeName').val();
            $('#CoApplicantName').val(refereeName);
        }
    },
    populateAgeInfo: function (years, months, days) {

        debugger;
        let age = parseInt(months) + parseInt(days);

        if (parseInt(years) == 0) {

        }
        else {
            age = parseInt(years) * 365 + age;
        }

        //org: 6 for prayash
        var orgid = $("#LoggedInOrg").val();
        var orgid = "6";
        var memberAge = parseInt($("#hdnMemberAge").val()) * 365;

        if (orgid == 6 || orgid == "6" ) { // organizat

            if ((age >= 5840 && age <= 23725)) { //  16 years = 5840 days  if (age >= 18 && age <= 65) {

                $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)").css("color", "green");
                $("#AsOnDateAge").val(years + " year(s) " + months + " month(s) and" + days + "day(s)");
            }
            else {

                $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)").css("color", "red");

                $("#BirthDate").val('');
                $("#dialog-message").html('<p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>Member age is less than 16 or greater than ' + 65 + ' </p>');
                $("#dialog-message").dialog({
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }
        else { // All other Organization

            if ((age >= 5840 && age <= 23725)) { //  18 years = 6570 days  if (age >= 18 && age <= 59) {

                $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)").css("color", "green");
                $("#AsOnDateAge").val(years + " year(s) " + months + " month(s) and" + days + "day(s)");
            }
            else {

                $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)").css("color", "red");

                $("#BirthDate").val('');
                $("#dialog-message").html('<p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>Member age is less than 18 or greater than ' + $("#hdnMemberAge").val() + ' </p>');
                $("#dialog-message").dialog({
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }// END ORG ChECK
    },
    populateAgeInfoPrayas: function (years, months, days) {

        debugger;
        let age = parseInt(months) + parseInt(days);

        if (parseInt(years) == 0) {

        }
        else {
            age = parseInt(years) * 365 + age;
        }

        var memberAge = parseInt($("#hdnMemberAge").val()) * 365;

        if ((age >= 5840 && age <= memberAge)) { //  18 years = 6570 days  if (age >= 18 && age <= 59) {

            $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)").css("color", "green");
            $("#AsOnDateAge").val(years + " year(s) " + months + " month(s) and" + days + "day(s)");
        }
        else {

            $("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)").css("color", "red");

            $("#BirthDate").val('');
            $("#dialog-message").html('<p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>Member age is less than 18 or greater than ' + $("#hdnMemberAge").val() + ' </p>');
            $("#dialog-message").dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    },
    getAgeInfo: function () {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/Member/LoadAge',
            data: { FromDate: $("#BirthDate").val(), ToDate: $("#JoinDate").val() },
            dataType: 'json',
            async: true,
            success: function (data) {
                $.each(data, function (index, data) {
                    if (data != "Error") {

                        var agepart = data.AsOnDateAge.split("-");
                        let years = agepart[0];
                        let months = agepart[1];
                        let days = agepart[2];

                        //populate age info
                        memberManager.populateAgeInfo(years, months, days);
                    }
                });
            },
            error: function (request, status, error) {
                alert("Problem with calculation");
            }
        });
    },
    getAgeInfoPrayas: function () {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Member/LoadAge',
        data: { FromDate: $("#BirthDate").val(), ToDate: $("#JoinDate").val() },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.each(data, function (index, data) {
                if (data != "Error") {

                    var agepart = data.AsOnDateAge.split("-");
                    let years = agepart[0];
                    let months = agepart[1];
                    let days = agepart[2];

                    //populate age info
                    memberManager.populateAgeInfoPrayas(years, months, days);
                }
            });
        },
        error: function (request, status, error) {
            alert("Problem with calculation");
        }
    });
}
}

$(function () {
    $('#Gender').on('change', function () {
        memberManager.initCoApplicant();
    })

    $('#FirstName,#RefereeName').on('keyup', function () {
        memberManager.initCoApplicant();
    });

    $('#OtherIdNo').on('keyup', function () {

        $('#OrderNoCount').val($(this).val().length);
    });

    $('#CardIssueDate').on('blur', function () {
        if ($(this).val().length == 0)
            alert("Please Give Issue Date.");
    });

})