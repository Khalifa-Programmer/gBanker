var btnSyncToPKSFHrml = '';
var infEquityInformationManager = {
    init: function () {
        this.initDate();
        if ($('#grid').length > 0) {
            this.GetHeaderInformation();
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
                        var posting_flag = $("#SYNCED_STATUS").val();
                        $.ajax({
                            url: '/ImputedCost/GetINFEquityInformation?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            //data: postData,
                            data: { mnyr, posting_flag },
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
                    title: 'Month/Year'
                },
                NPK_LAST_YR: {
                    title: 'NPK LAST YR'
                },
                PK_LAST_YR: {
                    title: 'PK LAST YR'
                },
                NPK_THIS_MONTH: {
                    title: 'NPK THIS MONTH'
                },
                PK_THIS_MONTH: {
                    title: 'PK THIS MONTH'
                },

                NPK_AVG: {
                    title: 'NPK AVG'
                },
                PK_AVG: {
                    title: 'PK AVG'
                },

                NPK_INF_ADJ: {
                    title: 'NPK INF ADJ'
                },
                PK_INF_ADJ: {
                    title: 'PK INF ADJ'
                },
                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                DeleteLink: {
                    title: "Delete",
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            return `<div class="text-center delete-link"><a onclick="infEquityInformationManager.DeleteInfEquity('${data.record.MNYR}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }
                    }
                }
            }
        });
        $('#grid').jtable('load');
    },
    performInfEquityDeleteAction: (mmYYYY) => {
        const postData = { mmYYYY: mmYYYY };

        fetch('/ImputedCost/DeleteInfEquity', {
            method: 'POST',
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
                infEquityInformationManager.reloadGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteInfEquity: (mmYYYY) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                infEquityInformationManager.performInfEquityDeleteAction(mmYYYY);
                return true;
            }
            else {
                return false;
            }
        });
    },
    refreshSearchTerm: () => {
        $('#MNYR').val('');
        infEquityInformationManager.reloadGrid();
    },
    reloadGrid: () => {
        $('#grid').jtable('load', { mnyr: $("#MNYR").val() });
    },
    syncToPKSF: () => {
        var url = "/ImputedCost/SyncINFEquityToPKSF"
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
    infEquityInformationManager.init();
    $('.CheckNumeric').on('keypress', function () {
        if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    })

    $('#btnSearch').on('click', () => {
        infEquityInformationManager.reloadGrid();
    })

    $('#btnRefresh').on('click', () => {
        infEquityInformationManager.refreshSearchTerm();
    })

    $('#btnSyncToPKSF').on('click', () => {
        $.alert.open({
            type: 'confirm',
            title:"Confirmation",
            content: `Are you sure you want to sync to pksf?
                        <br> If Yes, Please do not refresh the page until the process completed.
                        <br> <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Sync process will take time depending on amount of data to be sent.
                        `,
            callback: function (button) {
                if (button == 'yes') {
                    btnSyncToPKSFHrml = $('#btnSyncToPKSF').html();
                    $('#btnSyncToPKSF').attr('disabled', 'disabled');
                    $('#btnSyncToPKSF').html('Processing...');

                    infEquityInformationManager.syncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })
});
