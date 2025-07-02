
var financialDataManager = {
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
    GetfinancialDataList: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    console.log("Loading from custom function...");
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: '/FinancialData/GetfinancialDataList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            data: postData,
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
                IndicatorCode: {
                    title: 'Indicator Code'
                },
                MNYR: {
                    title: 'Date'
                },
                Amount: {
                    title: 'Amount'
                },
                DeleteLink: {
                    title: "Delete",
                    width: '5%',
                    display: function (data) {
                        return '<div class="text-center delete-link"><a href="/FinancialData/DeleteFinancialData?IndicatorCode=' + data.record.IndicatorCode + '&MNYR=' + data.record.MNYR + '" ' + ' OnClick="return confirm(' + "'" + 'Are you sure you want to delete this item?' + "'" + ');"' + '><i class="fa fa-trash-o"></i></a></div>';
                    }
                }
            }
        });
        $('#grid').jtable('load');
    }
}

$(function () {
    financialDataManager.init();   
    $('#btnView').on('click', function () {
        financialDataManager.GetfinancialDataList();
    })
});
