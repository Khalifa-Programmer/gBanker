
var bulkSMSManager = {
    initFunctions: function () {
        $('.ddl-select-picker').selectpicker();
        bulkSMSManager.populateOfficeDropdownList();
        bulkSMSManager.initJtable();
    },
    initJtable: function () {
        if ($('#grid2').lenght <= 0) return;

        $('#grid2').jtable({
            paging: true,
            pageSize: 500,
            sorting: false,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: '/BulkSMS/GetSMSListNew'
            },
            fields: {
                MessageDetails: {
                    title: 'Message Details',
                    width: '30%'
                },
                MemberCode: {
                    title: 'Member Code',
                    width: '5%'
                },
                PhoneNo: {
                    title: 'Mobile Number',
                    width: '5%'
                },

                Length: {
                    title: 'Message Length',
                    width: '5%'
                },
                SMSCount: {
                    title: 'SMS Count(s)',
                    width: '5%'
                },
            }
        });
    },
    SendSMSExecute: function () {
        var filterType = $('input[type=radio][name=Filter]:checked').val();

        if (filterType !== 'NotSend') {
            $.alert.open("Message", "Please check Not Send option");
            return;
        }

        var officeIds = $('#OfficeId').val();
        var searchKey = $('#SearchKey').val();

        var model = {
            officeIds: officeIds,
            SearchKey: searchKey,
            DateFromValue: $('#DateFromValue').val(),
            DateToValue: $('#DateToValue').val()
        };

        var sendSMSDefaultHtml = $('#btnSendSMS').html();
        var sendSMSNewHtml = `<i class="fa fa-circle-o-notch fa-spin"></i> Sending`;
        $('#btnSendSMS').html(sendSMSNewHtml);
        $('#btnSendSMS').attr('disabled','disabled');

        $.ajax({
            url: '/BulkSMS/SMSEventExecute',
            type: 'POST',
            data: model,
            dataType: 'json',
            async: true,
            success: function (result) {
                $.alert.open("Message", result);
                $('#btnSendSMS').html(sendSMSDefaultHtml);
                $('#btnSendSMS').removeAttr('disabled');
                bulkSMSManager.reloadGrid2();
            },
            error: function (request, status, error) {
                $.alert.open("Message", "SMS Error");
            }
        });
    },
    GenerateSMSList: function () {
        var Option = 0;
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/BulkSMS/SMSEventGenerate',
            data: { SMSType: Option },
            dataType: 'json',
            async: true,
            success: function (data) {
                $.alert.open("Message", "Message Sent Successfully");
            },
            error: function (request, status, error) {
                $.alert.open("Message", "SMS Error");
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
                    var selected = option.Selected?'selected="selected"':'';
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
        var searchKey = $('#SearchKey').val();
        
        $('#grid2').jtable('load', { OptionId: 'NotSend', officeIds: officeIds, SearchKey: searchKey, DateFromValue: dateFrom, DateToValue: dateTo });
    }
}

$(function () {
    bulkSMSManager.initFunctions();
    
    $('input[type=radio][name=Filter]').change(function () {
        $('#btnSendSMS').hide();
        if (this.value === 'All') {
            $('#btnSendSMS').hide();
        }
        else if (this.value === 'NotSend') {
            $('#btnSendSMS').show();
        }
        else if (this.value === 'Success') {
            $('#btnSendSMS').hide();
        }
        else if (this.value === 'Greetings') {
            $('#btnSendSMS').hide();
        }
    });    

    $("#DateFromValue").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2100"

    });

    $("#DateToValue").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2100"

    });

    $("#btnGenerateSMS").click(function () { // Reset form
        $.alert.open('confirm', 'Are you sure Generate SMS?', function (button) {
            if (button === 'yes') {
                bulkSMSManager.GenerateSMSList();
                return true;
            }
            else {

                return false;
            }
        });
    });

    $("#btnSendSMS").click(function () { // Reset form
        $.alert.open('confirm', 'Are you sure Execute SMS? If Yes, Please do not refresh the page until sms send completed.', function (button) {
            if (button === 'yes') {
                bulkSMSManager.SendSMSExecute();                
                return true;
            }
            else {

                return false;
            }
        });

    });

    $("#filterColumn").change(function () {
        if ($(this).val() === "ViewAll") {
            $("#filterValue").val('');
        }
    });

    $('#btnFilter').on('click', function () {

        var filterType = $('input[type=radio][name=Filter]:checked').val();

        var officeIds = $('#OfficeId').val();
        var searchKey = $('#SearchKey').val();

        if (filterType === 'All') {
            $('#btnSendSMS').hide();

            $('#grid2').jtable('load', { OptionId: 'All', officeIds: officeIds, SearchKey: searchKey ,DateFromValue: $('#DateFromValue').val(), DateToValue: $('#DateToValue').val() });
        }
        else if (filterType === 'NotSend') {
            $('#btnSendSMS').show();
            $('#grid2').jtable('load', { OptionId: 'NotSend', officeIds: officeIds, SearchKey: searchKey, DateFromValue: $('#DateFromValue').val(), DateToValue: $('#DateToValue').val() });
        }
        else if (filterType === 'Success') {
            $('#btnSendSMS').hide();

            $('#grid2').jtable('load', { OptionId: 'Success', officeIds: officeIds, SearchKey: searchKey,DateFromValue: $('#DateFromValue').val(), DateToValue: $('#DateToValue').val() });
        }
        else if (filterType === 'Greetings') {
            $('#btnSendSMS').hide();

            $('#grid2').jtable('load', { OptionId: 'Greetings', officeIds: officeIds, SearchKey: searchKey, DateFromValue: $('#DateFromValue').val(), DateToValue: $('#DateToValue').val() });
        }

        else {
            $('#grid2').jtable('load', { OptionId: 'NotSend', officeIds: officeIds, SearchKey: searchKey,DateFromValue: $('#DateFromValue').val(), DateToValue: $('#DateToValue').val() });
        }
       
    });
});
