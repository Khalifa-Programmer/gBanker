
var sentLogSMSSummarySMSManager = {
    initFunctions: function () {
        $('.ddl-select-picker').selectpicker();
        sentLogSMSSummarySMSManager.populateOfficeDropdownList();
        sentLogSMSSummarySMSManager.initJtable();
    },
    initJtable: function () {
        if ($('#grid').lenght <= 0) return;

        $('#grid').jtable({
            paging: true,
            pageSize: 25,           
            sorting: false,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: '/SMSReport/GetSentLogSMSSummary'
            },
            fields: {
                DateSent: {
                    title: 'Sent On',
                    width: '50%'
                },
                SMSCount: {
                    title: 'SMS Count',
                    width: '50%'
                }
            }
        });
    },
    populateOfficeDropdownList: function () {

        var $ddlOfficeSelector = $('#OfficeId')

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/office/getofficesfordropdownlist',
            data: {},
            dataType: 'json',
            async: true,
            success: function (data) {
                if (data) $ddlOfficeSelector.html('');

                $.each(data, function (id, option) {
                    var selected = option.Selected ? 'selected="selected"' : '';
                    $ddlOfficeSelector.append($(`<option ${selected}></option>`).val(option.Value).html(option.Text));
                });

                $('.ddl-select-picker').selectpicker('refresh');

            },
        });
    },
    reloadGrid2: function () {
        var dateFrom = $('#DateFromValue').val();
        var dateTo = $('#DateToValue').val();
        var officeIds = $('#OfficeId').val();

        $('#grid').jtable('load', { OptionId: 'NotSend', officeIds: officeIds, DateFromValue: dateFrom, DateToValue: dateTo });
    }
}

$(function () {
    sentLogSMSSummarySMSManager.initFunctions();

    $(".sms-smry-datepicker").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2100"

    });

    $('#btn-search').on('click', function () {
        var officeIds = $('#OfficeId').val();

        $('#grid').jtable('load', { officeIds: officeIds, DateFromValue: $('#StartDateInString').val(), DateToValue: $('#EndDateInString').val() });

    });
});
