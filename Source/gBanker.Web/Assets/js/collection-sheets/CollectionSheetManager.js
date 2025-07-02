
//var isCalculateTotal = false;
//var totalCount = 0;
var TransTypeConstants = {
    Loan: 'L',
    Savings: "S"
}

var NTypeConstants = {
    GSavings: "G",
    SSP: "SSP",
    VS: "VS"
}

var collectionSheetModel = {
    TrxID: '',
    TransType: '',
    nType: '',
    TotalAmount: 0,

    PrincipalLoan: 0,
    LoanRepaid: 0,
    CumIntCharge: 0,
    CumIntPaid: 0,

    LoanDue: 0,
    IntDue: 0,
    DurationOverLoanDue: 0,
    DurationOverIntDue: 0,
    DueSavingInstallment: 0,
    Recoverable: 0,
}

var OrgID = 0;
var CollectionSheetManager = {
    init: function () {
        OrgID = $('#LoggedInOrganizationID').val();
        this.DestryPopupModal();
    },
    DestryPopupModal: () => {
        //destroy send email popup modal upon cancel button click        
        $GlobalPopupModal = $('#GlobalPopupModal');
        $GlobalPopupModal.on('hidden.bs.modal', function (e) {
            $(this).data('bs.modal', null);
        });
    },
    ApproveBalanceSheet: (memberID) => {
        var model = {
            memberID: memberID
        };
        var requestWithBody = fetch('/CollectionSheet/ApproveCollectionSheet', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', },
            body: JSON.stringify(model),
        })

        requestWithBody.then(response => response.json())
            .then(data => {
                if (data.Result !== 'OK') {
                    $.alert.open("Error", data.Message);
                    return;
                }

                //reload grid data
                CollectionSheetManager.GetCollectionSheetList();
                //$.alert.open("Success", data.Message);
                //app.showNotification(data)
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    GetCollectionSheetList: () => {

        $('#grid').jtable({
            paging: false,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var CenterID = $("#CenterID").val();
                        $.ajax({
                            url: '/CollectionSheet/GetCollectionSheetList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&filterValue=${$('#filterValue').val()}`, // + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            //data: postData,
                            data: { CenterID: CenterID },
                            success: function (data) {
                                /*isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;
                                console.log(totalCount);
                                */

                                //populate collection sheet total
                                CollectionSheetManager.populateCollectionSheetTotal(data.Records);

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
                Id: {
                    key: true,
                    list: false,
                    create: false,
                    edit: false
                },
                MemberCode: {
                    title: 'Member Code'
                },
                MemberName: {
                    title: 'Member Name'
                },
                TotalLoanPaid: {
                    title: 'Total L.Paid'
                },
                LoanPaid: {
                    title: 'Principal Paid'
                },
                IntPaid: {
                    title: 'Sc.Paid'
                },
                GSavings: {
                    title: 'G.Savings'
                },
                SSP: {
                    title: 'SSP'
                },
                VS: {
                    title: 'VS'
                },
                Total: {
                    title: 'Total'
                },
                CollectionStatus: {
                    title: 'Paid'
                },
                Approve: {
                    title: "Approve",
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        return `<div class="text-center"><a href="javascript:void(0);" OnClick="CollectionSheetManager.ApproveBalanceSheet(${data.record.MemberID});"><i class="fa fa-check"></i></a></div>`;
                    }
                },
                Details: {
                    title: "Details",
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        return `<div class="text-center"><a href="/CollectionSheet/GetCollectionSheetDetails?CenterID=${data.record.CenterID}&memberCode=${data.record.MemberCode}" data-toggle="modal" data-target="#GlobalPopupModal" ><i class="fa fa-search"></i></a></div>`;
                    }
                }
            }
        });
        $('#grid').jtable('load');
    },
    populateCollectionSheetTotal: (data) => {
        if (!data || data.length <= 0) {
            $('#totalLoanPaid').val(0);
            $('#totalPrincipalPaid').val(0);
            $('#totalScPaid').val(0);
            $('#totalGSavings').val(0);
            $('#totalSSP').val(0);
            $('#totalCS').val(0);
            $('#grandTotal').val(0);

            return;
        }
        let totalLoanPaid = _.sumBy(data, (item) => {
            if (item.CollectionStatus === 'YES')
                return item.TotalLoanPaid;
        });
        let totalPrincipalPaid = _.sumBy(data, (item) => {
            if (item.CollectionStatus === 'YES')
                return item.LoanPaid;
        });

        let totalScPaid = _.sumBy(data, (item) => {
            if (item.CollectionStatus === 'YES')
                return item.IntPaid;
        });

        let totalGSavings = _.sumBy(data, (item) => {
            if (item.CollectionStatus === 'YES')
                return item.GSavings;
        });

        let totalSSP = _.sumBy(data, function (item) {
            if (item.CollectionStatus === 'YES')
                return item.SSP;
        });

        let totalVS = _.sumBy(data, function (item) {
            if (item.CollectionStatus === 'YES')
                return item.VS;
        });

        let grandTotal = _.sumBy(data, function (item) { return item.Total; });

        $('#totalLoanPaid').val(totalLoanPaid);
        $('#totalPrincipalPaid').val(totalPrincipalPaid);
        $('#totalScPaid').val(totalScPaid);
        $('#totalGSavings').val(totalGSavings);
        $('#totalSSP').val(totalSSP);
        $('#totalVS').val(totalVS);
        $('#grandTotal').val(grandTotal);
    },
    /*
    PopupDiv: function (TrxId) {

        GetDataPopUp(TrxId);
        $('#hdnTrxId').val(TrxId);
        $('html, body').animate({ scrollTop: $('#ProgramDiv').offset().top }, 'slow');

        $("#ProgramDiv").dialog({
            autoOpen: false,
            height: 750,
            width: 830,
            modal: true,
            scroll: false,
            buttons: {
                "Close": function () {
                    $(this).dialog("close");
                }
            }
        });

        var showPopup = "True";
        if (showPopup == "True") {
            $("#ProgramDiv").dialog('open');
        }
    },
    */

    /*
    populateTotalPaid: ($tr) => {
        var principalPaid = $tr.find('.input-loan-paid').val();
        principalPaid = parseInt(principalPaid ?? 0);

        var intPaid = $tr.find('.input-int-paid').val();
        intPaid = parseInt(intPaid ?? 0);

        var gsavings = $tr.find('.input-gsavings').val();
        gsavings = parseInt(gsavings ?? 0);

        var cs = $tr.find('.input-cs').val();
        cs = parseInt(cs ?? 0);

        var total = principalPaid + intPaid + gsavings + cs;
        //console.log(total);

        $tr.find('.input-total-lPaid').val(total);
    },
    */
    distributeCollectionAmount: (model = collectionSheetModel) => {
        let total = Number(Math.round((model.TotalAmount)));
        let vLoanInstallment = 0;
        let vInterestInstallment = 0;

        if (model.TransType === TransTypeConstants.Savings) {
            if (model.nType === NTypeConstants.GSavings) {
                $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_GSavings`).val(total);                
            }
            else if (model.nType === NTypeConstants.SSP) {
                $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_SSP`).val(Math.round(total));               
            }
            else if (model.nType === NTypeConstants.VS) {

                console.log('*****************model.nType === NTypeConstants.VS****************');
                console.log(model.Recoverable);
               // console.log(total % model.Recoverable );

                if (total == model.Recoverable || total % model.Recoverable === 0) {
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_TotalLPaid`).val(total);
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_VS`).val(Math.round(total));
                }
                else {
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_VS`).val(0);
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_TotalLPaid`).val(0);
                }
            }

            return;
        }

        var vBuroLoanBal = Number(model.PrincipalLoan) - Number(model.LoanRepaid)
        var vBuroIntBal = Number(Math.round(model.CumIntCharge)) - Number(Math.round(model.CumIntPaid))
        var vBuroActualBal = Number(vBuroLoanBal) + Number(vBuroIntBal)

        if (Number(total) >= Number(vBuroActualBal)) {
            $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_TotalLPaid`).val(Math.round(vBuroActualBal));
            $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_LoanPaid`).val(Math.round(vBuroLoanBal));
            $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_IntPaid`).val(Math.round(vBuroIntBal));            
        }
        else {
            var vLoan = Number(model.LoanDue);
            var vInt = Number(model.IntDue);

            if (Number(total) == Number(vLoan) + Number(vInt)) {
                $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_TotalLPaid`).val(Math.round(total));
                $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_LoanPaid`).val(Math.round(vLoan));
                $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_IntPaid`).val(Math.round(vInt));
            }
            else {
                var vTotalInstallBuro = (parseInt(model.DurationOverLoanDue) + parseInt(model.DurationOverIntDue))
                var instMod = (Number(total) % Number(vTotalInstallBuro));

                vLoanInstallment = (parseInt(model.DurationOverLoanDue) * parseInt(total)) / parseInt(vTotalInstallBuro);
                vInterestInstallment = (parseInt(model.DurationOverIntDue) * parseInt(total)) / parseInt(vTotalInstallBuro);

                if (instMod === 0) { 
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_TotalLPaid`).val(Math.round(total));
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_LoanPaid`).val(Math.round(vLoanInstallment));
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_IntPaid`).val(Math.round(vInterestInstallment));
                }
                else {
                   

                    let loanPaid = Math.round((vLoanInstallment / (vLoanInstallment + vInterestInstallment))* total);
                    let intPaid = Number(total) - loanPaid;

                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_TotalLPaid`).val(total);
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_LoanPaid`).val(loanPaid);
                    $(`#GlobalPopupModal #CSTrxIdXTransTypes_${model.TrxID}${model.TransType}_IntPaid`).val(intPaid);
                }
            }
        }
    },
    goNextRow: function (e, tabIndex) {
        console.log(tabIndex);

        //up arrow 38
        if (e.keyCode == 38) {
            var $newClass = $(`.totallpaid_${parseInt(tabIndex) - 1}`);
            $newClass.focus();
            $newClass.select();
        }
        else if (e.keyCode == 40) {
            var $newClass = $(`.totallpaid_${parseInt(tabIndex) + 1}`);
            $newClass.focus();
            $newClass.select();
        }
    }
}

$(function () {
    CollectionSheetManager.init();

    $('#CenterID').on('change', () => {
        CollectionSheetManager.GetCollectionSheetList();
    })

    $("#GlobalPopupModal").on('click', '.input-total-lPaid', function () {
        var $that = $(this);
        $that.select();
    });

    $('#GlobalPopupModal').on('keyup', '.input-ssp', function () {
        let gTotalSSP = 0.00;
        $('#GlobalPopupModal').find('td.ssp').each(function () {
            var $tr = $(this).parents('tr')
            if ($tr.find('.input-ssp').length <= 0) return false;

            let ssp = $tr.find('.input-ssp').val();
            if (ssp && ssp.length > 0) {
                gTotalSSP = parseInt(gTotalSSP) + parseInt(ssp);
            }
        });

        $('#GlobalPopupModal #GTotalSSP').html(gTotalSSP);
    })

    $('#GlobalPopupModal').on('keyup', '.input-vs', function () {
        let gTotalVS = 0.00;
        $('#GlobalPopupModal').find('td.vs').each(function () {
            var $tr = $(this).parents('tr')
            if ($tr.find('.input-vs').length <= 0) return false;

            let vs = $tr.find('.input-vs').val();
            if (vs && vs.length > 0) {
                gTotalVS = parseInt(gTotalVS) + parseInt(vs);
            }
        });

        $('#GlobalPopupModal #GTotalVS').html(gTotalVS);
    })

    $('#GlobalPopupModal').on('keyup', '.input-gsavings', function () {
        var $tr = $(this).closest('tr');
        let gTotalGSavings = 0.00;
        $('#GlobalPopupModal').find('td.gsavings').each(function () {
            $tr = $(this).parents('tr')
            if ($tr.find('.input-gsavings').length <= 0) return false;

            let gSavings = $tr.find('.input-gsavings').val();
            if (gSavings && gSavings.length > 0) {
                gTotalGSavings = parseInt(gTotalGSavings) + parseInt(gSavings);
            }
        });

        $('#GlobalPopupModal #GTotalGSavings').html(gTotalGSavings);
    })

    $('#GlobalPopupModal').on('keyup', '.input-int-paid', function () {
        let gTotalIntPaid = 0.00;
        $('#GlobalPopupModal').find('td.int-paid').each(function () {
            var $tr = $(this).parents('tr')
            if ($tr.find('.input-int-paid').length <= 0) return false;

            let intPaid = $tr.find('.input-int-paid').val();
            if (intPaid && intPaid.length > 0) {
                gTotalIntPaid = parseInt(gTotalIntPaid) + parseInt(intPaid);
            }
        });

        $('#GlobalPopupModal #GTotalIntPaid').html(gTotalIntPaid);
    })

    $('#GlobalPopupModal').on('keyup', '.input-loan-paid', function () {
        var $tr = $(this).closest('tr');

        let gTotalPPaid = 0.00;
        $('#GlobalPopupModal').find('td.loan-paid').each(function () {
            $tr = $(this).parents('tr')
            if ($tr.find('.input-loan-paid').length <= 0) return false;

            let loanPaid = $tr.find('.input-loan-paid').val();
            if (loanPaid && loanPaid.length > 0) {
                gTotalPPaid = parseInt(gTotalPPaid) + parseInt(loanPaid);
            }
        });

        $('#GlobalPopupModal #GTotalPrincipalPaid').html(gTotalPPaid);
    })

    $('#GlobalPopupModal').on('blur', '.input-total-lPaid', function () {

        var $tr = $(this).parents('tr');

        collectionSheetModel.TrxID = $tr.find('.trxid').val();
        collectionSheetModel.TransType = $tr.find('.trans-type').val();
        collectionSheetModel.nType = $tr.find('.ntype').val();
        collectionSheetModel.TotalAmount = $tr.find('.input-total-lPaid').val();


        collectionSheetModel.PrincipalLoan = $tr.find('.principalloan').val();
        collectionSheetModel.LoanRepaid = $tr.find('.loanrepaid').val();
        collectionSheetModel.CumIntCharge = $tr.find('.cumintcharge').val();
        collectionSheetModel.CumIntPaid = $tr.find('.cumintpaid').val();

        collectionSheetModel.LoanDue = $tr.find('.loandue').val();
        collectionSheetModel.IntDue = $tr.find('.intdue').val();

        collectionSheetModel.Recoverable = $tr.find('.input-recoverable').val();
        collectionSheetModel.DurationOverLoanDue = $tr.find('.durationoverloandue').val();
        collectionSheetModel.DurationOverIntDue = $tr.find('.durationoverintdue').val();

        CollectionSheetManager.distributeCollectionAmount(collectionSheetModel);

        let gTotalLPaid = 0.00;
        $('#GlobalPopupModal').find('td.total-lPaid').each(function () {
            $tr = $(this).parents('tr')
            if ($tr.find('.input-total-lPaid').length <= 0) return false;

            let totalLPaid = $tr.find('.input-total-lPaid').val();

            if (totalLPaid && totalLPaid.length > 0) {
                gTotalLPaid = parseInt(gTotalLPaid) + parseInt(totalLPaid);
            }
        });

        $('#GlobalPopupModal #GTotalLPaid').html(gTotalLPaid);

        $('#GlobalPopupModal .input-loan-paid').trigger('keyup');
        $('#GlobalPopupModal .input-int-paid').trigger('keyup');
        $('#GlobalPopupModal .input-gsavings').trigger('keyup');
        $('#GlobalPopupModal .input-ssp').trigger('keyup');
        $('#GlobalPopupModal .input-vs').trigger('keyup');
    })

    $("#GlobalPopupModal").on("submit", '#formApproveBatchCollectSheet', function (event) {
        event.preventDefault();

        var _currentForm = $(this).closest('form');
        if (_currentForm.valid()) {
            let $prevApproveButton = $('#GlobalPopupModal .btn-approve-batch-cs');
            let $prevApproveButtonHtml = $prevApproveButton.html();
            $prevApproveButton.html('Processing...');
            $prevApproveButton.attr('disabled', 'disabled');

            var url = $(this).attr("action");
            var formData = $(this).serialize();
            $.ajax({
                url: url,
                type: "POST",
                data: formData,
                dataType: "json",
                success: function (resp) {
                    $prevApproveButton.html($prevApproveButtonHtml);
                    $prevApproveButton.removeAttr('disabled');

                    if (resp.Result !== 'OK') { $.alert.open("Error", resp.Message); return; }

                    $('#GlobalPopupModal #btnCloseModal').trigger('click');
                    //$.alert.open("Success", resp.Message);

                    //Get collection sheet list
                    CollectionSheetManager.GetCollectionSheetList();
                },
                error: function (err) {
                    $prevApproveButton.html($prevApproveButtonHtml);
                    $prevApproveButton.removeAttr('disabled');
                    $.alert.open("Error", err);
                }
            })
        }
    });

    $('#btnView').on('click', function () {
        CollectionSheetManager.GetCollectionSheetList();
    })
});
