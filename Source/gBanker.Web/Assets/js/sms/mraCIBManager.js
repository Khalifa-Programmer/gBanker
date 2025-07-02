
var mraCIBManager = {
    initFunctions: function () {
        $('.ddl-select-picker').selectpicker();
        mraCIBManager.populateOfficeDropdownList();
        mraCIBManager.initJtable();
    },
    initJtable: function () {
        //if ($('#grid').lenght <= 0) return;

        //$('#grid').jtable({
        //    paging: true,
        //    pageSize: 500,
        //    sorting: false,
        //    defaultSorting: 'Name ASC',
        //    actions: {
        //        listAction: '/BulkSMS/GetSMSListNew'
        //    },
        //    fields: {
        //        MessageDetails: {
        //            title: 'Message Details',
        //            width: '30%'
        //        },
        //        MemberCode: {
        //            title: 'Member Code',
        //            width: '5%'
        //        },
        //        PhoneNo: {
        //            title: 'Mobile Number',
        //            width: '5%'
        //        },

        //        Length: {
        //            title: 'Message Length',
        //            width: '5%'
        //        },
        //        SMSCount: {
        //            title: 'SMS Count(s)',
        //            width: '5%'
        //        },
        //    }
        //});
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
    }
}

$(function () {
    mraCIBManager.initFunctions();
});
