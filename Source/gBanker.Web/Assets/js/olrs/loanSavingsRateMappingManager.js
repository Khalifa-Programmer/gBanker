
var loanSavingsRateMappingManager = {
    init: function () {
        this.initChoosen();
        this.initProductChoosen();
    },

    initChoosen: function () {

        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    },

    initProductChoosen: function () {
        if ($(".chosen-product").length > 0) {
            $(".chosen-product").val('').trigger("liszt:updated");
            jQuery(".chosen-product").chosen();
        }
    },
    populateProductByType: function () {
        var loanSavingRateType = $("#LoanSavingRateType").val();
        var ddlProduct = $("#ProductName");

        ddlProduct.html('').append($('<option></option>').val('').html("Select One"));

        if (!loanSavingRateType || loanSavingRateType.length <= 0) {
            loanSavingsRateMappingManager.initProductChoosen();
            return;
        }

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/LoanSavingsRateMapping/GetProductByType',
            data: { loanSavingRateType: loanSavingRateType },
            dataType: 'json',
            async: true,
            success: function (result) {
                ddlProduct.html('');
                $.each(result, function (id, option) {
                    ddlProduct.append($('<option></option>').val(option.Value).html(option.Text));
                });

                loanSavingsRateMappingManager.initProductChoosen();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    GetLoanSavingsRateMappingList: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    console.log("Loading from custom function...");
                    return $.Deferred(function ($dfd) {
                        var loanSavingRateType = $("#LoanSavingRateType").val();
                        $.ajax({
                            url: '/LoanSavingsRateMapping/GetLoanSavingsRateMappingList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            //data: postData,
                            data: { LoanSavingRateType: loanSavingRateType },
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
                ProductName: {
                    title: 'Product Name'
                },
                Rate: {
                    title: 'Rate'
                },               
            }
        });
        $('#grid').jtable('load');
    }
}

$(function () {
    loanSavingsRateMappingManager.init();

    $('#LoanSavingRateType').on('change', function () {
        loanSavingsRateMappingManager.populateProductByType();
        loanSavingsRateMappingManager.GetLoanSavingsRateMappingList();
    })
});
