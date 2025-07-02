

const ProcessTypeConstants = {
    Process_All: "Process_All",
    Program_Data: "Program_Data",
    Basic_Data: "Basic_Data",
    Financial_Data: "Financial_Data",
    Upazilla_Loan: "Upazilla_Loan",
    Balance_Sheet: "Balance_Sheet",
    Trial_Balance: "Trial_Balance",
    Trial_Balance: "Employment_Data",
    Top_Sheet: "Top_Sheet",
}

var SyncToPKSFConstants = {
    Sync_All: "Sync_All",
    Program_Data: "Program_Data",
    Financial_Data: "Financial_Data",
    Basic_Data: "Basic_Data",
    Upazilla_Loan: "Upazilla_Loan",

    Accounting_BS_IE_RP: "Accounting_BS_IE_RP",

    /*
    Balance_Sheet: "Balance_Sheet",
    Income_Expenditure: "Income_Expenditure",
    Receipt_Payment: "Receipt_Payment",
    */
    ImputedCost_Header_Info: "Imputed_Cost_Header_Info",
    ImputedCost_Loan_Code_Wise_Service_Change: "ImputedCost_Loan_Code_Wise_Service_Change",
    ImputedCost_Savings_Interest_Info: "ImputedCost_Savings_Interest_Info",
    ImputedCost_Inflation_Equity_Info: "ImputedCost_Inflation_Equity_Info",

    Employment_Related_Last_Data: "Employment_Related_Last_Data",
    Employment_Related_Last_Retained_Data: "Employment_Related_Last_Retained_Data",
    Employment_Related_Current_Year_New_Data: "Employment_Related_Current_Year_New_Data",
}

var NotSyncToPKSFConstants = {
    Not_Sync_All: "Not_Sync_All",
    Program_Data: "Program_Data",
    Financial_Data: "Financial_Data",
    Basic_Data: "Basic_Data",
    Upazilla_Loan: "Upazilla_Loan",

    Accounting_BS_IE_RP: "Accounting_BS_IE_RP",

    /*
    Balance_Sheet: "Balance_Sheet",
    Income_Expenditure: "Income_Expenditure",
    Receipt_Payment: "Receipt_Payment",
    */
    ImputedCost_Header_Info: "Imputed_Cost_Header_Info",
    ImputedCost_Loan_Code_Wise_Service_Change: "ImputedCost_Loan_Code_Wise_Service_Change",
    ImputedCost_Savings_Interest_Info: "ImputedCost_Savings_Interest_Info",
    ImputedCost_Inflation_Equity_Info: "ImputedCost_Inflation_Equity_Info",

    Employment_Related_Last_Data: "Employment_Related_Last_Data",
    Employment_Related_Last_Retained_Data: "Employment_Related_Last_Retained_Data",
    Employment_Related_Current_Year_New_Data: "Employment_Related_Current_Year_New_Data",
}
 

let btnSyncToPKSFHrml = '';
let btnProcessHrml = '';
let btnNotSyncToPKSFHrml = ''; 

var olrsProcessManager = {
    init: function () {
        this.initChoosen();
        this.initDate();
        this.initSelectPicker();
    },
    initSelectPicker: () => {
        if ($('.selectpicker').length > 0) {
            $('.selectpicker').selectpicker('refresh');
        }
    },
    initChoosen: function () {
        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    },
    initDate: function () {
        $("#ProcessMonth").datepicker({
            dateFormat: "mm/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#ProcessMonth").datepicker('setDate', new Date());

        $("#SyncMonth").datepicker({
            dateFormat: "mm/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#SyncMonth").datepicker('setDate', new Date());

        $("#NotSyncMonth").datepicker({
            dateFormat: "mm/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#NotSyncMonth").datepicker('setDate', new Date());
    },
    ProcessConfirmation: function () {
        var processType = $("#ProcessType").val();
        var processMonth = $("#ProcessMonth").val();

        $.alert.open('confirm', 'Are you sure you want to start the process?', function (button) {
            if (button == 'yes') {

                var btnProcessHrml = $('#btnProcess').html();
                $('#btnProcess').attr('disabled', 'disabled');
                $('#btnProcess').html('Processing...');

                $.ajax({
                    type: 'POST',
                    url: '/OlrsProcess/Process',
                    data: { ProcessType: processType, ProcessMonth: processMonth },
                    dataType: 'json',
                    success: function (data) {
                        $('#btnProcess').removeAttr('disabled');
                        $('#btnProcess').html(btnProcessHrml);
                        if (data.Result == "OK") {
                            $.alert.open("Success", data.Message);
                        } else {
                            $.alert.open("Error", data.Message);
                        }
                    },
                });
                return true;
            }
            else {
                return false;
            }
        });
    },
    processMFIDataToPKSF: () => {
        var processTypes = $('#ProcessType').val();

        let filteredProcessTypes = _.filter(processTypes, (processType) => {
            return processType != ProcessTypeConstants.Process_All
        });

        let totalProcess = filteredProcessTypes.length;
        let init = 1;
        let processResponseHtml = `
            <div class="raw">
                <div class="col-md-12">
                    <table class="table table-striped table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>Process For</th>
                                <th>Response</th>
                            </tr>
                        </thead>
                        <tbody> 
        `;

        _.forEach(processTypes, function (processType, sKey) {
            if (processType != ProcessTypeConstants.Process_All) {
                var url = `/OlrsProcess/ProcessMFIDataToPKSF?processType=${processType}`;
                var postedData = {
                    ProcessType: processType,
                    ProcessMonth: $('#ProcessMonth').val(),
                };

                const headers = { 'Content-Type': 'application/json' };

                axios.post(url, postedData, { headers: headers })
                    .then(function (res) {

                        let data = res.data;

                        let badgeBSClass = data.Result != "OK" ? "alert alert-danger" : "";

                        processResponseHtml = processResponseHtml + `
                            <tr class='${badgeBSClass}'>
                                <td>${processType}</td>
                                <td>${data.Message}</td>
                            </tr>
                        `;

                        //when process completed
                        if (totalProcess == init) {
                            $('#btnProcess').removeAttr('disabled');
                            $('#btnProcess').html(btnProcessHrml);

                            processResponseHtml = processResponseHtml + `
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        `;

                            $('section-process-response').html(processResponseHtml);
                        }

                        init = init + 1;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        });
    },
    syncToPKSF: () => {
        var syncToPKSFTypes = $('#SyncToPKSFType').val();
        var syncMonth = $('#SyncMonth').val();

        let filteredSyncToPKSFTypes = _.filter(syncToPKSFTypes, (syncToPKSFType) => {
            return syncToPKSFType != SyncToPKSFConstants.Sync_All
        });

        let totalSyncToPKSF = filteredSyncToPKSFTypes.length;
        let init = 1;
        let syncResponseHtml = `
            <div class="raw">
                <div class="col-md-12">
                    <table class="table table-striped table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>Sync For</th>
                                <th>Response</th>
                            </tr>
                        </thead>
                        <tbody> 
        `;

        _.forEach(syncToPKSFTypes, function (syncToPKSFType, sKey) {
            if (syncToPKSFType != SyncToPKSFConstants.Sync_All) {
                var url = `/OlrsProcess/SyncProcessDataToPKSF`;

                var postedData = {
                    syncToPKSFType,
                    syncMonth,
                };

                const headers = { 'Content-Type': 'application/json' };

                axios.post(url, postedData, { headers: headers })
                    .then(function (res) {

                        let data = res.data;
                        //fetch(url)
                        //    .then(response => response.json())
                        //    .then(data => {

                        let badgeBSClass = data.Result != "OK" ? "alert alert-danger" : "";

                        syncResponseHtml = syncResponseHtml + `
                            <tr class='${badgeBSClass}'>
                                <td>${syncToPKSFType}</td>
                                <td>${data.Message}</td>
                            </tr>
                        `;

                        //when process completed
                        if (totalSyncToPKSF == init) {
                            $('#btnSyncToPKSF').removeAttr('disabled');
                            $('#btnSyncToPKSF').html(btnSyncToPKSFHrml);

                            syncResponseHtml = syncResponseHtml + `
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        `;

                            $('section-sync-response').html(syncResponseHtml);
                        }

                        init = init + 1;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        });
    },
    NotsyncToPKSF: () => {
        var NotsyncToPKSFTypes = $('#NotSyncToPKSFType').val();
        var NotsyncMonth = $('#NotSyncMonth').val();

        let filteredNotSyncToPKSFTypes = _.filter(NotsyncToPKSFTypes, (NotsyncToPKSFType) => {
            return NotsyncToPKSFType != NotSyncToPKSFConstants.Not_Sync_All
        });

        let totalNotSyncToPKSF = filteredNotSyncToPKSFTypes.length;
        let init = 1;
        let NotsyncResponseHtml = `
            <div class="raw">
                <div class="col-md-12">
                    <table class="table table-striped table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>Not Sync For</th>
                                <th>Response</th>
                            </tr>
                        </thead>
                        <tbody> 
        `;

        _.forEach(NotsyncToPKSFTypes, function (NotsyncToPKSFType, sKey) {
            if (NotsyncToPKSFType != NotSyncToPKSFConstants.Not_Sync_All) {
                var url = `/OlrsProcess/NotSyncProcessDataToPKSF`;

                var postedData = {
                    NotsyncToPKSFType,
                    NotsyncMonth,
                };

                const headers = { 'Content-Type': 'application/json' };

                axios.post(url, postedData, { headers: headers })
                    .then(function (res) {

                        let data = res.data;
                        //fetch(url)
                        //    .then(response => response.json())
                        //    .then(data => {

                        let badgeBSClass = data.Result != "OK" ? "alert alert-danger" : "";

                        NotsyncResponseHtml = NotsyncResponseHtml + `
                            <tr class='${badgeBSClass}'>
                                <td>${NotsyncToPKSFType}</td>
                                <td>${data.Message}</td>
                            </tr>
                        `;

                        //when process completed
                        if (totalNotSyncToPKSF == init) {
                            $('#btnNotSyncToPKSF').removeAttr('disabled');
                            $('#btnNotSyncToPKSF').html(btnNotSyncToPKSFHrml);

                            NotsyncResponseHtml = NotsyncResponseHtml + `
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        `;

                            $('section-not-sync-response').html(NotsyncResponseHtml);
                        }

                        init = init + 1;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        });
    },
}

$(function () {
    olrsProcessManager.init();
    //$("#btnProcess").on('click', function () {
    //    olrsProcessManager.ProcessConfirmation();
    //})

    $('#ProcessType').on('change', () => {
        let processTypes = $('#ProcessType').val();
        let processAll = _.some(processTypes, (processType) => { return processType === ProcessTypeConstants.Process_All })
        if (processAll) {
            $('#ProcessType').selectpicker('selectAll');
        }
        //else {
        //    $('#ProcessType option').attr("selected", false);
        //    $('#ProcessType').selectpicker('refresh');
        //    $('#ProcessType').selectpicker('deselectAll');
        //}
    });
    $('#btnProcess').on('click', () => {
        $('section-process-response').html('');
        var processTypes = $('#ProcessType').val();
        if (!processTypes || processTypes.length === 0) {
            var data = {
                Result: "Error",
                Message: "Warning! Please Select process Type"
            }
            app.showNotification(data)
            return;
        }

        var processMonth = $('#ProcessMonth').val();
        if (!processMonth || processMonth.length === 0) {
            var data = {
                Result: "Error",
                Message: "Warning! Date is Required"
            }
            app.showNotification(data)
            return;
        }

        $.alert.open({
            type: 'confirm',
            title: "Confirmation",
            content: `Are you sure you want to process?
                        <br> If Yes, Please do not refresh the page until the process completed.
                        <br> <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Data Process will take time depending on amount of data.
                        `,
            callback: function (button) {
                if (button == 'yes') {
                    btnProcessHrml = $('#btnProcess').html();
                    $('#btnProcess').attr('disabled', 'disabled');
                    $('#btnProcess').html('Processing...');

                    olrsProcessManager.processMFIDataToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })

    $('#SyncToPKSFType').on('change', () => {
        let syncTypes = $('#SyncToPKSFType').val();
        let syncAll = _.some(syncTypes, (syncType) => { return syncType === SyncToPKSFConstants.Sync_All })
        if (syncAll) {
            $('#SyncToPKSFType').selectpicker('selectAll');
        }
        //else {
        //    $('#SyncToPKSFType option').attr("selected", false);
        //    $('#SyncToPKSFType').selectpicker('refresh');
        //    $('#SyncToPKSFType').selectpicker('deselectAll');
        //}
    }); 
    $('#btnReset').on('click', () => {
        var disabledSyncToPKSF = $('#btnSyncToPKSF').attr('disabled');
        if (disabledSyncToPKSF != "disabled") {
            $('.selectpicker').selectpicker('deselectAll');
            $('section-sync-response').html('');
        }
    })
    $('#btnSyncToPKSF').on('click', () => {
        $('section-sync-response').html('');
        var syncToPKSFTypes = $('#SyncToPKSFType').val();
        if (!syncToPKSFTypes || syncToPKSFTypes.length === 0) {
            var data = {
                Result: "Error",
                Message: "Warning! Please Select sync Type"
            }
            app.showNotification(data)
            return;
        }

        var syncMonth = $('#SyncMonth').val();
        if (!syncMonth || syncMonth.length === 0) {
            var data = {
                Result: "Error",
                Message: "Warning! Date is Required"
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

                    olrsProcessManager.syncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })


    $('#NotSyncToPKSFType').on('change', () => {
        let NotSyncTypes = $('#NotSyncToPKSFType').val();
        let notSyncAll = _.some(NotSyncTypes, (NotSyncType) => { return NotSyncType === NotSyncToPKSFConstants.Not_Sync_All })
        if (NotSyncAll) {
            $('#NotSyncToPKSFType').selectpicker('selectAll');
        }
        //else {
        //    $('#SyncToPKSFType option').attr("selected", false);
        //    $('#SyncToPKSFType').selectpicker('refresh');
        //    $('#SyncToPKSFType').selectpicker('deselectAll');
        //}
    });
    $('#btnResetNotSync').on('click', () => {
        var disabledNotSyncToPKSF = $('#btnNotSyncToPKSF').attr('disabled');
        if (disabledNotSyncToPKSF != "disabled") {
            $('.selectpicker').selectpicker('deselectAll');
            $('section-Not-sync-response').html('');
        }
    })
    $('#btnNotSyncToPKSF').on('click', () => {
        $('section-not-sync-response').html('');
        var NotsyncToPKSFTypes = $('#NotSyncToPKSFType').val();
        if (!NotsyncToPKSFTypes || NotsyncToPKSFTypes.length === 0) {
            var data = {
                Result: "Error",
                Message: "Warning! Please Select sync Type"
            }
            app.showNotification(data)
            return;
        }

        var NotsyncMonth = $('#NotSyncMonth').val();
        if (!NotsyncMonth || NotsyncMonth.length === 0) {
            var data = {
                Result: "Error",
                Message: "Warning! Date is Required"
            }
            app.showNotification(data)
            return;
        }

        $.alert.open({
            type: 'confirm',
            title: "Confirmation",
            content: `Are you sure you want to Not sync to pksf?
                        <br> If Yes, Please do not refresh the page until the process completed.
                        <br> <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Not Sync process will take time depending on amount of data to be sent.
                        `,
            callback: function (button) {
                if (button == 'yes') {
                    btnNotSyncToPKSFHrml = $('#btnNotSyncToPKSF').html();
                    $('#btnNotSyncToPKSF').attr('disabled', 'disabled');
                    $('#btnNotSyncToPKSF').html('Processing...');

                    olrsProcessManager.NotsyncToPKSF();
                    return true;
                }
                else {
                    return false;
                }
            },
        });
    })
});
