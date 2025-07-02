
var btnSyncToPKSFHrml = '';
var headerInformationManager = {
    init: function () {
        this.initDate();
        this.GetHeaderInformation();
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
    GetHeaderInformation: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    console.log("Loading from custom function...");
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var fyMonth = $("#FY_Month").val();
                        $.ajax({
                            url: '/ImputedCost/GetHeaderInformation?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            //data: postData,
                            data: { mnyr: mnyr, fyMonth: fyMonth},
                            success: function (data) {
                                $dfd.resolve(data);
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
                    title: 'Month-Year'
                },
                FY_Month: {
                    title: 'Fin_YearMonth'
                },
                MarketRate: {
                    title: 'Market Rate'
                },
                InflationRate: {
                    title: 'Inflation Rate'
                },                
                imp_fs_npk: {
                    title: 'imp_fs_npk'
                },
                imp_fs_pk: {
                    title: 'imp_fs_pk'
                },
                //imp_fs_total: {
                //    title: 'imp_fs_total'
                //},
                imp_fsi_npk: {
                    title: 'imp_fsi_npk'
                },
                imp_fsi_pk: {
                    title: 'imp_fsi_pk'
                },
                //imp_fsi_total: {
                //    title: 'imp_fsi_total'
                //},
                SYNCED_STATUS: {
                    title: 'SYNCED_STATUS',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            //let mnyr = data.record.MNYR.split('-')[0].trim();
                            return `<div class="text-center delete-link"><a onclick="headerInformationManager.DeleteHeaderInfoAction('${data.record.MNYR}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }

                        return "";
                    }
                }
            }
        });
        $('#grid').jtable('load');
    },
    performHeaderInfoDataDeleteAction: (mnyr) => {
        const postData = { MNYR: mnyr };

        fetch('/ImputedCost/DeleteHeaderInfoData', {
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
                headerInformationManager.GetHeaderInformation();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteHeaderInfoAction: (mnyr) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                headerInformationManager.performHeaderInfoDataDeleteAction(mnyr);
                return true;
            }
            else {
                return false;
            }
        });
    },
    syncToPKSF: () => {
        var url = "/ImputedCost/SyncImputedCostHeaderInfoToPKSF"
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
    headerInformationManager.init();
    $('.CheckNumeric').on('keypress', function () {    
        if (event.which != 8 && isNaN(String.fromCharCode(event.which))){
            event.preventDefault(); 
        }
    })
    $("#btnSearch").on('click', function () {
        headerInformationManager.GetHeaderInformation();
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

                    headerInformationManager.syncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })
});
