$(document).ready(function () {
    GetOfficeListTree();
});


function GetOfficeListTree() {
    $.ajax({
        url: '/Home/GetOfficeListTree',
        type: "GET",
        dataType: "json",
        success: function (resp) {
            var msg = resp.msg;
            if (msg && msg.length > 0) {
                alert(msg);
            }
            else {
                var officeList = resp.officeList;
                if (officeList && officeList.length > 0) {
                    generateTreeView(officeList);
                    $('#officeSelectDialog').modal('show');
                }
                else
                    alert('No office assigned. Please ask your support engineer to help.');
            }
        },
        error: function (err) {
            loader.hide();
            toastr.error(err.responseText, 'Error');
        }
    });
}


var selectedNode = null;
function redirectToOrganizaton() {
    debugger;
    if (selectedNode === null)
        $("#search-output").html('Please select a branch to work.');
    else {
        var officeId = selectedNode.id; //$("#officeid").val();
        if (officeId && officeId.length > 0) {
            $('#officeSelectDialog').modal('hide');
            loader.show();
            //Get redirect URL for this user:
            $.ajax({
                url: '/Account/GetRedirectUrl?selectedOfficeId=' + officeId,
                type: "GET",
                dataType: "json",
                success: function (resp) {
                    loader.hide();
                    var msg = resp.msg;
                    if (msg && msg.length > 0) {
                        alert(msg);
                    }
                    else {
                        var url = resp.url;
                        window.location.href = url;
                    }
                },
                error: function (err) {
                    loader.hide();
                    toastr.error(err.responseText, 'Error');
                }
            });
        }
    }
}

function generateTreeView(officeList) {
    var allOffliceList = officeList;
    var $searchableTree = getTreeView(officeList);
    var search = function (e) {
        //debugger;
        var pattern = $('#input-search').val();
        var tvData = $.grep(allOffliceList, function (element, index) {
            return element.Text.toLowerCase().indexOf(pattern) >= 0;
        });
        $searchableTree = getTreeView(tvData);
        var options = {
            ignoreCase: true,
            exactMatch: false,
            revealResults: true
        };
        var results = $searchableTree.treeview('search', [pattern, options]);
        var output = '<p>' + results.length + ' matches found</p>';
        $.each(results, function (index, result) {
            output += '<p>- ' + result.text + '</p>';
        });
        $('#search-output').html(output);
    }

    $('#btn-search').on('click', search);
    $('#input-search').on('keyup', search);

    $('#btn-clear-search').on('click', function (e) {
        $searchableTree.treeview('clearSearch');
        $('#input-search').val('');
        $('#search-output').html('');
    });
}

function getTreeView(data) {
    var defaultData = generateTreeviewData(data);
    var $searchableTree = $('#treeview-searchable').treeview({
        data: defaultData,
        onNodeSelected: function (event, data) {
            if (data.selectable)
                console.log(data);
            else
                console.log("Not selectable");
            selectedNode = data;
            var output = '<p>- ' + data.text + '</p>';
            $('#search-output').html(output);
            $("#btnSelectOffice").show();
        },
        onNodeUnselected: function (event, data) {
            selectedNode = null;
            $("#btnSelectOffice").hide();
            $('#search-output').html('');
        },
    });
    return $searchableTree;
}
function generateTreeviewData(officeList) {
    var treeViewData = [];
    if (officeList) {
        var autoSelect = (officeList.length === 1);
        $.each(officeList, function (index, value) {
            var currentHo = value.HoName;
            var currentZo = value.ZoneName;
            var currentAo = value.AreaName;
            var currentOffice = value.Text;
            var currentOfficeId = value.Value;
            var ho = searchOffice(1, currentHo, treeViewData);
            //var ho = $.grep(treeViewData, function (val) {
            //    var level = val.level;               
            //    return val.text === currentHo && level === 1;
            //});
            if (ho.length === 0) {
                var hoObj = { text: currentHo, level: 1, selectable: false, nodes: [] };
                if (autoSelect)
                    hoObj.state = { expanded: true };
                var zoObj = { text: currentZo, level: 2, selectable: false, nodes: [] };
                if (autoSelect)
                    zoObj.state = { expanded: true };
                var aoObj = { text: currentAo, level: 3, selectable: false, nodes: [] };
                if (autoSelect)
                    aoObj.state = { expanded: true };
                var offObj = {
                    text: currentOffice, id: currentOfficeId, level: 4, selectable: true
                };
                if (autoSelect) {
                    selectedNode = offObj;
                    offObj.state = { selected: true, expanded: true };
                }
                zoObj.nodes.push(aoObj);
                aoObj.nodes.push(offObj);
                hoObj.nodes.push(zoObj);
                treeViewData.push(hoObj);
            }
            else {
                var hoObj = ho[0];
                var zoObj = { text: currentZo, level: 2, selectable: false, nodes: [] };
                var zoOffice = searchOffice(2, currentZo, hoObj.nodes);
                if (zoOffice.length > 0)
                    zoObj = zoOffice[0];
                else
                    hoObj.nodes.push(zoObj);
                var aoObj = { text: currentAo, level: 3, selectable: false, nodes: [] };
                var aoOffice = searchOffice(3, currentAo, zoObj.nodes);
                if (aoOffice.length > 0)
                    aoObj = aoOffice[0];
                else
                    zoObj.nodes.push(aoObj);
                var offObj = { text: currentOffice, id: currentOfficeId, level: 4, selectable: true };
                if (autoSelect) {
                    offObj.state = { selected: true, expanded: true };
                    selectedNode = offObj;
                }
                aoObj.nodes.push(offObj);
                // treeViewData.push(hoObj);
            }
        });
    }
    return treeViewData;
}
function searchOffice(level, text, treeViewData) {
    var obj = $.grep(treeViewData, function (val) {
        return val.text === text && val.level === level;
    });
    return obj;
}
