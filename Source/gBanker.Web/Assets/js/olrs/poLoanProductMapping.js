
var poLoanProductMapping = {
    init: function () {         
        this.availableProductWiseLoanCodeList();
    },

    initChoosen: function () {
        if ($(".chosen").length > 0) {             
            jQuery(".chosen").chosen();
        }
    },
    availableProductWiseLoanCodeList: function () {
        var dtTable = $('#tblAvailableProduct');
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/ProductMappingWithPO/GetAvailableProductList',
            data: { OfficeID: 0 },
            dataType: 'json',
            async: true,
            success: function (data) {
                var tableBody = dtTable.find('tbody');
                tableBody.empty();
                tableBody.append(data);

                //init choosen
                poLoanProductMapping.initChoosen();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }

}

$(function () {
    poLoanProductMapping.init();
});
