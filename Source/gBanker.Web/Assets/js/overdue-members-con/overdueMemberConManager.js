
//scaffolding model
var overdueMemberConManager = {
    init: () => {
        overdueMemberConManager.initDates();
        overdueMemberConManager.getProduct();
        overdueMemberConManager.getDueTypes();
    },
    initDates: () => {
        $("#txtFromDt").datepicker({
            dateFormat: "dd-M-yy",
            showAnim: "scale"
        });

        $("#txtToDt").datepicker({
            dateFormat: "dd-M-yy",
            showAnim: "scale"
        });
    },
    getProduct: () => {
        let $ddlProd = $("#ddlProd");
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/OverdueMemberList/GetProductListForOverdue',
            data: {},
            dataType: 'json',
            async: true,
            success: function (data) {
                $ddlProd.html('');
                $.each(data, function (id, option) {
                    $ddlProd.append($('<option></option>').val(option.Value).html(option.Text));
                });
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    getDueTypes:() => {
        const dueTypes = [
            { value: DueTypeConstants.Current_Due, text: DueTypeConstants.Current_Due.replace('_', ' ') },
            { value: DueTypeConstants.Duration_Over_Due, text: DueTypeConstants.Duration_Over_Due.replace('_', ' ') }
        ];

        let $dueType = $("#DueType");
        $dueType.html('');
        $dueType.append($('<option></option>').val('').html('Select All'));
        $.each(dueTypes, function (id, option) {
            $dueType.append($('<option></option>').val(option.value).html(option.text));
        });
    },
    toggleCheckProduct: (selector,type) => {
        if (type === omConDDLType.Product) {
            let $ddlProd = $("#ddlProd");
            $ddlProd.val('');

            if ($(`#${selector}`).is(':checked'))
                $ddlProd.attr("disabled", "disabled");
            else {
                $ddlProd.removeAttr("disabled");
            }
        }
        else if (type === omConDDLType.DueType) {
            let $ddlDueType = $("#DueType");
            $ddlDueType.val('');

            if ($(`#${selector}`).is(':checked'))
                $ddlDueType.attr("disabled", "disabled");
            else {
                $ddlDueType.removeAttr("disabled");
            }
        }
    }
}

//on load
$(function () {
    overdueMemberConManager.init();

    $('#chkProd').on('change', () => {
        const selector = 'chkProd';
        overdueMemberConManager.toggleCheckProduct(selector,omConDDLType.Product);
    })
    $('#chkDueType').on('change', () => {
        const selector = 'chkDueType';
        overdueMemberConManager.toggleCheckProduct(selector,omConDDLType.DueType);
    })

    $("#btnView").on('click',(e)=> {
        e.preventDefault();
        //var DateFrom = $("#txtFromDt").val();
        const dateTo = $("#txtToDt").val();
        const mfiOrg = $('#HiddenForOrg').val();
        const productMainCode = $('#ddlProd').val();
        const dueType = $('#DueType').val();
        let url = '/OverdueMemberList/GenerateOverdueMemberListAllReportMainProduct?DateTo=' + dateTo;

        if (parseInt(mfiOrg) === MFIConstants.Society_For_Social_Service_SSS) {
            url = `/OverdueMemberList/GenerateOverdueMemberListAllReportMainProduct?DateTo=${dateTo}&productMainCode=${productMainCode}&dueType=${dueType}`;
        } 
        
        window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    });
    $('#btnExport').on('click',()=> {

        const dateTo = $("#txtToDt").val();
        const mfiOrg = $('#HiddenForOrg').val();
        const productMainCode = $('#ddlProd').val();
        const dueType = $('#DueType').val();
        let url = '/OverdueMemberList/GenerateOverdueMemberListAllReportExportMainProduct?DateTo=' + dateTo;

        //var center = $("#ddlCenter").val();

        if (parseInt(mfiOrg) === MFIConstants.Society_For_Social_Service_SSS) {
            url = `/OverdueMemberList/GenerateOverdueMemberListAllReportExportMainProduct?DateTo=${dateTo}&productMainCode=${productMainCode}&dueType=${dueType}`;
        } 

        //if (center > 0) {
        window.location = url;
        //}
        //else if (center == 0) {
        //    window.location = '/GroupwiseReport/GenerateDailySavingCollectionReportExport?Qtype=1&Center=0';
        //}

    });

})

const omConDDLType = {
    Product: 'Product',
    DueType:'DueType'
}
