
var EmploymentTypeConstants = {
    EMPLOYMENT_UP_TO_LAST_HALF_YEAR: "Employment up to Last Half Year",
    EMPLOYMENT_RETAINED_FROM_LAST_HALF_YEAR: "Employment Retained From Last Half Year",
    NEW_EMLOYEE_IN_CURRENT_HALF_YEAR: "New Emloyee In Current Half Year",
}

var employmentManagerManager = {
    init: function () {
        this.initChoosen();
        this.initDate();
    },
    initChoosen: function () {
        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    },
    initDate: function () {
        $("#MNYR").datepicker({
            dateFormat: "mm/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#MNYR").datepicker('setDate', new Date());
    },    
    syncToPKSF: () => {
        var url = employmentManagerManager.getPKSFSyncUrl();
        fetch(url)
            .then(response => response.json())
            .then(data => {
                $('#btnSyncToPKSF').removeAttr('disabled');
                $('#btnSyncToPKSF').html(btnSyncToPKSFHrml);

                app.showNotification(data)
            });
    },
    getPKSFSyncUrl: () => {
        var employmentType = $('#EmploymentType').val();
        var url = "";
        switch (employmentType) {

            case EmploymentTypeConstants.EMPLOYMENT_UP_TO_LAST_HALF_YEAR:
                url = "/OLRSEmployment/SyncEmploymentAToPKSF";
                break;

            case EmploymentTypeConstants.EMPLOYMENT_RETAINED_FROM_LAST_HALF_YEAR:
                url = "/OLRSEmployment/SyncEmploymentBToPKSF";
                break;

            case EmploymentTypeConstants.NEW_EMLOYEE_IN_CURRENT_HALF_YEAR:
                url = "/OLRSEmployment/SyncEmploymentCToPKSF";
                break;

            default:
                url = "/OLRSEmployment/SyncEmploymentAToPKSF";
                break;
        }

        return url;
    }
}

$(function () {
    employmentManagerManager.init();


    $('#btnSyncToPKSF').on('click', () => {

        var employmentType = $('#EmploymentType').val();
        if (!employmentType || employmentType === '') {
            var data = {
                Result: "Error",
                Message: "Warning! Please Select Employment Type"
            }
            app.showNotification(data)
            return;
        }

        $.alert.open({
            type: 'confirm',
            title: "Confirmation",
            content: `Are you sure you want to sync to pksf?
                        <br> If Yes, Please do not refresh the page until the process completed.
                        <br> <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Sync process will take time depending on amount of data to be sent.
                        `,
            callback: function (button) {
                if (button == 'yes') {
                    btnSyncToPKSFHrml = $('#btnSyncToPKSF').html();
                    $('#btnSyncToPKSF').attr('disabled', 'disabled');
                    $('#btnSyncToPKSF').html('Processing...');

                    employmentManagerManager.syncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })
});
