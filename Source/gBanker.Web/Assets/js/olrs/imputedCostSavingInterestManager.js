
var btnSyncToPKSFHrml = '';

var imputedCostSavingInterestManager = {
    init: function () {       
        this.initDate();
        //this.GetImputedCostSavingInterestList();
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
    GetImputedCostSavingInterestList: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    //console.log("Loading from custom function...");
                    var mnyr = $("#MNYR").val();
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: '/ImputedCostSavingInterest/GetImputedCostSavingInterestList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr },
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
                    title: 'Date'
                },
                INT_RATE: {
                    title: 'Interest Rate'
                },
                NPK: {
                    title: 'Non PKSF Fund'
                },
                Regular: {
                    title: 'Regular Fund'
                },
                VOL: {
                    title: 'Voluntary Fund'
                },
                Other: {
                    title: 'Other Fund'
                },
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
                        return `<div class="text-center delete-link"><a onclick="imputedCostSavingInterestManager.DeleteData('${data.record.MNYR}')"><i class='fa fa-trash-o'></i></a></div>`;
                    }
                }

            }
        });
        $('#grid').jtable('load');
    },

    performDeleteAction: (mnyr) => {
        const postData = { mnyr };

        fetch('/ImputedCostSavingInterest/DeleteImputedCostSavingInterest', {
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
                imputedCostSavingInterestManager.reloadGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteData: (mnyr) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                imputedCostSavingInterestManager.performDeleteAction(mnyr);
                return true;
            }
            else {
                return false;
            }
        });
    },
    refreshSearchTerm: () => {         
        imputedCostSavingInterestManager.reloadGrid();
    },
    reloadGrid: () => {
        $('#grid').jtable('load');
    },

    syncToPKSF: () => {    
        var url = "/ImputedCostSavingInterest/SyncImputedCostSavingInterestToPKSF"
        fetch(url)
            .then(response => response.json())
            .then(data => {
                $('#btnSyncToPKSF').removeAttr('disabled');
                $('#btnSyncToPKSF').html(btnSyncToPKSFHrml);
                app.showNotification(data);
            });
    }
}

$(function () {
    imputedCostSavingInterestManager.init();

    $('#btnView').on('click', function () {
        imputedCostSavingInterestManager.GetImputedCostSavingInterestList();
    })

    $('#btnSyncToPKSF').on('click', () => {
        console.log('btnSyncToPKSF');

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

                    imputedCostSavingInterestManager.syncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })
});
