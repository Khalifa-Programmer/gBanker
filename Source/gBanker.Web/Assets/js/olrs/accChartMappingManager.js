
var accChartMappingManager = {
    init: function () {
        this.initChoosen();
        this.initChoosenOLRSAacc();
        this.initChoosenConfigAccChart();
        this.initChoosenAccCode();
    },
    populateAccChartByLavel: function () {
        var ddlAccChartCode = $("#AccChartCode");
        var accChartLevel = $('#AccChartLevel').val();
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/OLRSAccChartMapping/GetAccChartDDLByLavel',
            data: { accChartLevel: accChartLevel },
            dataType: 'json',
            async: true,
            success: function (result) {
                ddlAccChartCode.html('');
                $.each(result.listItems, function (id, option) {
                    ddlAccChartCode.append($('<option></option>').val(option.Value).html(option.Text));
                });

                accChartMappingManager.initChoosenAccCode();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    populateConfigAccChart: function () {
        var ddlConfigAccChartCode = $("#ConfigAccChartCode");
        var accChartCode = $("#AccChartCode").val();
        var accChartLevel = $('#AccChartLevel').val();
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/OLRSAccChartMapping/GetConfigAccChartDDLByLavelAndCode',
            data: { accChartLevel: accChartLevel, accCode: accChartCode },
            dataType: 'json',
            async: true,
            success: function (result) {
                ddlConfigAccChartCode.html('');
                $.each(result.listItems, function (id, option) {
                    ddlConfigAccChartCode.append($('<option></option>').val(option.Value).html(option.Text));
                });

                accChartMappingManager.initChoosenConfigAccChart();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    populateAccChartMappingHtml: function () {
        var accChartLevel = $("#AccChartLevel").val();
        var accChartCode = $('#AccChartCode').val();
        var dtTable = $('#tblAvailableAccChartMapping');
        $.ajax({
            type: 'POST',
            url: '/OLRSAccChartMapping/GetAccChartMappingHtml',
            data: { AccChartLevel: accChartLevel, AccCode: accChartCode },
            dataType: 'json',
            async: true,
            success: function (data) {
                var tableBody = dtTable.find('tbody');
                tableBody.empty();
                tableBody.append(data);
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    initChoosen: function () {
        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    },
    initChoosenAccCode: function () {
        if ($(".chosen-acc-chart-code").length > 0) {
            $(".chosen-acc-chart-code").val('').trigger("liszt:updated");
            jQuery(".chosen-acc-chart-code").chosen();
        }
    },
    initChoosenOLRSAacc: function () {
        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }

        if ($(".chosen-olrs-acc").length > 0) {
            $(".chosen-olrs-acc").val('').trigger("liszt:updated");
            jQuery(".chosen-olrs-acc").chosen();
        }
    },
    initChoosenConfigAccChart: function () {
        if ($(".chosen-config-acc-chart").length > 0) {
            $(".chosen-config-acc-chart").val('').trigger("liszt:updated");
            jQuery(".chosen-config-acc-chart").chosen();
        }
    },
    GetAccChartWithOLRSAccCodeMapList: function () {

        $('#grid').jtable({
            paging: true,
            pageSize: 25,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        let searchTerm = '';
                        if ($('#SearchTerm').length > 0)
                            searchTerm = $('#SearchTerm').val();
                        else
                            searchTerm = $('#AccCodeOLRS').val();

                        $.ajax({
                            url: '/OLRSAccChartMapping/GetAccChartWithOLRSAccCodeMapList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            //data: postData,
                            data: { searchTerm: searchTerm },
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
                Id: {
                    key: true,
                    list: false,
                    create: false,
                    edit: false
                },
                AccCodeOLRS: {
                    title: 'Acc Code OLRS'
                },
                AccChartCode: {
                    title: 'Acc Code'
                },
                
                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        return `<div class="text-center delete-link"><a onclick="accChartMappingManager.DeleteAccMap('${data.record.Id}')"><i class='fa fa-trash-o'></i></a></div>`;
                    }
                }
                
            }
        });
        $('#grid').jtable('load');
    },
    performDeleteAction: (id) => {
        const postData = { Id:id };

        fetch('/OLRSAccChartMapping/DeleteAccMap', {
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
                accChartMappingManager.reloadGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteAccMap: (id) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                accChartMappingManager.performDeleteAction(id);
                return true;
            }
            else {
                return false;
            }
        });
    },
    refreshSearchTerm: () => {
        $('#SearchTerm').val('');
        accChartMappingManager.reloadGrid();
    },
    reloadGrid: () => {
        $('#grid').jtable('load', { searchTerm: $("#SearchTerm").val() });
    },
}

$(function () {
    accChartMappingManager.init();

    $('#AccChartLevel').on('change', function () {
        var accChartLevel = $(this).val();
        if (!accChartLevel) {
            var ddlAccChartCode = $("#AccChartCode");
            ddlAccChartCode.html('');
            ddlAccChartCode.append($('<option></option>').val("").html("Select One"));
            $('#tblAvailableAccChartMapping').find('tbody').empty();
            return;
        }
        accChartMappingManager.populateAccChartByLavel();
    })

    $('#AccChartCode').on('change', function () {
        var accChartLevel = $('#AccChartLevel').val();
        var accChartCode = $(this).val();
        if (!accChartLevel || !accChartCode) {
            $('#tblAvailableAccChartMapping').find('tbody').empty();
            return;
        }
        accChartMappingManager.populateConfigAccChart();
    })

    $('#AccCodeOLRS').on('change', function () {
        
        accChartMappingManager.GetAccChartWithOLRSAccCodeMapList();
    })
    $('#btnView').on('click', function () {
        accChartMappingManager.GetAccChartWithOLRSAccCodeMapList();
    })
    $('#btnRefresh').on('click', () => {
        accChartMappingManager.refreshSearchTerm();
    })
});
