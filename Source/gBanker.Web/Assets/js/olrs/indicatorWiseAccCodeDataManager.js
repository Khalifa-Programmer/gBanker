
var indicatorWiseAccCodeDataManager = {
    init: function () {         
        this.availableLoanCodeWiseAccList();
    },

    initChoosen: function () {
        if ($(".chosen").length > 0) {             
            jQuery(".chosen").chosen();
        }
    },
    availableLoanCodeWiseAccList: function () {
        var dtTable = $('#tblAvailableProduct');
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/IndXAccCodeMap/GetToPopulateIndicatorWiseAcc',
            data: { },
            dataType: 'json',
            async: true,
            success: function (data) {
                var tableBody = dtTable.find('tbody');
                tableBody.empty();
                tableBody.append(data);

                indicatorWiseAccCodeDataManager.initChoosen()

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }

}

$(function () {
    indicatorWiseAccCodeDataManager.init();
});
