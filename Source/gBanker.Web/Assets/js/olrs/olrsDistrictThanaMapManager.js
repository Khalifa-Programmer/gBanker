
var olrsDistrictThanaMapManager = {
    init: function () {
        this.initChoosen();
        this.initThanaChoosen();
        this.initOlrsThanaChoosen();
    },

    initChoosen: function () {
        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    }, 
    initThanaChoosen: function () {
        if ($(".chosen-thana").length > 0) {
            $(".chosen-thana").trigger("liszt:updated");
            jQuery(".chosen-thana").chosen();
        }
    },
    initOlrsThanaChoosen: function () {
        if ($(".chosen-OlrsThana").length > 0) {
            $(".chosen-OlrsThana").trigger("liszt:updated");
            jQuery(".chosen-OlrsThana").chosen();
        }
    },

    populateThanaByDistrict: function () {
        var districtCode = $("#DistrictCode").val();
        var ddlThana = $("#ThanaCode");

        ddlThana.html('').append($('<option></option>').val('').html("Select One"));

        if (!districtCode || districtCode.length <= 0) {
            olrsDistrictThanaMapManager.initThanaChoosen();
            return;
        }
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/OlrsDistrictThanaMap/GetThanaByDistrict',
            data: { districtCode: districtCode },
            dataType: 'json',
            async: true,
            success: function (result) {
               
                ddlThana.html('');
                $.each(result, function (id, option) {
                    ddlThana.append($('<option></option>').val(option.Value).html(option.Text));
                });

                olrsDistrictThanaMapManager.initThanaChoosen();

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },

    populateOlrsThanaByOlrsDistrict: function () {
        var olrsDistrictCode = $("#OlrsDistrictCode").val();
        var ddlOlrsThana = $("#OlrsThanaCode");

        ddlOlrsThana.html('').append($('<option></option>').val('').html("Select One"));

        if (!olrsDistrictCode || olrsDistrictCode.length <= 0) {
            olrsDistrictThanaMapManager.initOlrsThanaChoosen();
            return;
        }
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/OlrsDistrictThanaMap/GetOlrsThanaByOlrsDistrict',
            data: { DistrictCode: olrsDistrictCode },
            dataType: 'json',
            async: true,
            success: function (result) {
                ddlOlrsThana.html('');
                $.each(result, function (id, option) {
                    ddlOlrsThana.append($('<option></option>').val(option.Value).html(option.Text));
                });

                olrsDistrictThanaMapManager.initOlrsThanaChoosen();

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
    GetDistrictThanaMappingData: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    console.log("Loading from custom function...");
                    return $.Deferred(function ($dfd) {
                        var DistrictCode = $('#DistrictCode').val();
                        var OlrsDistrictCode = $('#OlrsDistrictCode').val();
                        var ThanaCode = $('#ThanaCode').val();
                        var OlrsThanaCode = $('#OlrsThanaCode').val();

                        $.ajax({
                            url: '/OlrsDistrictThanaMap/GetDistrictThanaMappingData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            data: { DistrictCode: DistrictCode, OlrsDistrictCode: OlrsDistrictCode, ThanaCode: ThanaCode, OlrsThanaCode: OlrsThanaCode },
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
                DistrictName: {
                    title: 'District'
                },
                OlrsDistrictCode: {
                    title: 'Olrs District'
                },
                ThanaName: {
                    title: 'Thana'
                },
                OlrsThanaCode: {
                    title: 'Olrs Thana'
                },

                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        return `<div class="text-center delete-link"><a onclick="olrsDistrictThanaMapManager.DeleteData('${data.record.Id}')"><i class='fa fa-trash-o'></i></a></div>`;
                    }
                }

            }
        });
        $('#grid').jtable('load');
    },

    performDeleteAction: (id) => {
        const postData = { Id: id };

        fetch('/OlrsDistrictThanaMap/DeleteDistrictMap', {
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
                olrsDistrictThanaMapManager.reloadGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteData: (id) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsDistrictThanaMapManager.performDeleteAction(id);
                return true;
            }
            else {
                return false;
            }
        });
    },
    refreshSearchTerm: () => {
         
        olrsDistrictThanaMapManager.reloadGrid();
    },
    reloadGrid: () => {
        $('#grid').jtable('load');
    },




}

$(function () {
    olrsDistrictThanaMapManager.init();  

    $('#DistrictCode').on('change', function () {
        olrsDistrictThanaMapManager.populateThanaByDistrict();
    })
    $('#OlrsDistrictCode').on('change', function () {
        olrsDistrictThanaMapManager.populateOlrsThanaByOlrsDistrict();
    })
    $('#btnView').on('click', function () {
        olrsDistrictThanaMapManager.GetDistrictThanaMappingData();
    })
});
