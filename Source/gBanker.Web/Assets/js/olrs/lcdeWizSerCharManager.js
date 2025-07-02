var isCalculateTotal = true;
var totalCount = 0;

var btnSyncToPKSFHrml = '';
var lcdeWizSerCharManager = {
    init: function () {
        this.initChoosen();
        this.initDate();        
        this.loadServiceChargeDropdown();
        if ($('#gridIMPLNSERAMT').length > 0) {
            this.GetLNSerCharAmountData();
        }
    },
    GetLNSerCharAmountData: function () {
        $('#gridIMPLNSERAMT').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var ind_code = $("#IND_CODE").val();
                        var m_f_flag = $("#M_F_flag").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/LCdeWizSerChar/GetIMPLoanSCAmountData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                SC_RATE: {
                    title: 'SC_RATE'
                },              
                LOAN_TOTAL: {
                    title: 'Total'
                },
                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                Action: {
                    title: 'Details',
                    display: function (data) {
                        var mmyr = data.record.MNYR.replace("/", "_");
                        var detailLink = `<a class="btn btn-sm btn-primary" target='_blank' href="/lcdewizserchar/loanserviceamountdata/mmyr/${mmyr}/sc/${data.record.SC_RATE}"><i class='fa fa-info-circle' aria-hidden='true'></i> Details</a>`;
                        return detailLink;
                    }
                },   
                  DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        return `<div class="text-center delete-link"><a onclick="lcdeWizSerCharManager.DeleteData('${data.record.MNYR}', '${data.record.SC_RATE}')"><i class='fa fa-trash-o'></i></a></div>`;
                    }
                }
            }
        });
        $('#gridIMPLNSERAMT').jtable('load');
    },
    performDeleteAction: (mnyr, SC_RATE) => {       
        const postData = { mnyr: mnyr, SC_RATE: SC_RATE };
        fetch('/LCdeWizSerChar/DeleteImputedCostSavingInterest', {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(postData),
        }).then(response => response.json())
            .then(data => {
                if (data.Result !== 'OK') {
                    $.alert.open("Error", data.Message);
                    return;
                }
                //reload grid data
                lcdeWizSerCharManager.reloadGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteData: (mnyr, SC_RATE) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                lcdeWizSerCharManager.performDeleteAction(mnyr, SC_RATE);
                return true;
            }
            else {
                return false;
            }
        });
    },
    refreshSearchTerm: () => {
        lcdeWizSerCharManager.reloadGrid();
    },
    reloadGrid: () => {
        $('#gridIMPLNSERAMT').jtable('load');
    },


    refreshLNSerCharAmountSearchTerm: () => {
        $("#MNYR").val('');
        $("#SYNCED_STATUS").val('');
        lcdeWizSerCharManager.reloadLNSerCharAmountGrid();
    },
    reloadLNSerCharAmountGrid: function () {
        var mnyr = $("#MNYR").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridIMPLNSERAMT').jtable('load', { mnyr: mnyr, syncedStatus: syncedStatus });
    },

    initChoosen: function () {
        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    },
    loadServiceChargeDropdown: function () {
        let dropdownid = 'ServiceChargeRate';
        $.ajax({
            type: "GET",
            url: "/LCdeWizSerChar/GetServiceRateList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $("#" + dropdownid).append($("<option     />").val('0').text('Select One'));
                $.each(data.Options, function () {
                    $("#" + dropdownid).append($("<option     />").val(this.Value).text(this.Text));
                });
                $(".chosen").val('').trigger("liszt:updated");
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }, 
    initDate: function () {
        if ($("#LoanDate").length > 0) {
            $("#LoanDate").datepicker({
                dateFormat: "mm/yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true,
                yearRange: "1920:2100"
            });
            $("#LoanDate").datepicker('setDate', new Date());
        }

        if ($("#MNYR").length > 0) {
            $("#MNYR").datepicker({
                dateFormat: "mm/yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true,
                yearRange: "1920:2100"
            });
            $("#MNYR").datepicker('setDate', new Date());
        }

        $("#LoanDate").datepicker({
            dateFormat: "mm/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"
        });
        $("#LoanDate").datepicker('setDate', new Date());
    },
    syncToPKSF: () => {
        var url = "/LCdeWizSerChar/SyncImputedCostLoanServiceChargeToPKSF"
        fetch(url)
            .then(response => response.json())
            .then(data => {
                $('#btnSyncToPKSF').removeAttr('disabled');
                $('#btnSyncToPKSF').html(btnSyncToPKSFHrml);

                app.showNotification(data)
            });
    }
}

$(function () {
    lcdeWizSerCharManager.init();

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            if ($('#btnRefreshIMPLNSERAMT').length > 0) {
                $('#btnRefreshIMPLNSERAMT').trigger('click');
            }
        }
    });

    $('#btnSearchIMPLNSERAMT').on('click', () => {
        isCalculateTotal = true;
        lcdeWizSerCharManager.GetLNSerCharAmountData();
    })

    $('#btnRefreshIMPLNSERAMT').on('click', () => {
        isCalculateTotal = true;
        lcdeWizSerCharManager.refreshLNSerCharAmountSearchTerm();
    })

    $('#btnSyncToPKSF').on('click', () => {
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

                    lcdeWizSerCharManager.syncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })
});
