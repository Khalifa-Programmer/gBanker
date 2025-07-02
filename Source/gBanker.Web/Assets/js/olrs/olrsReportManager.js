
var isCalculateTotal = true;
var totalCount = 0;

var olrsReportManager = {
    init: function () {
        this.initDate();
        if ($('#gridFd').length > 0) {
            this.GetFinancialData();
        }
        if ($('#gridBd').length > 0) {
            this.GetBasicData();
        }
        if ($('#gridPd').length > 0) {
            this.GetProgramData();
        }
        if ($('#gridUl').length > 0) {
            olrsReportManager.initDistrictChoosen();
            olrsReportManager.initThanaChoosen();

            this.GetUpazillaLoanData();
        }
        if ($('#gridAd').length > 0) {
            this.GetAccountingData();
        }
        if ($('#gridTopSheet').length > 0) {
            this.GetTopSheetData();
        }
    },
    initDistrictChoosen: function () {
        if ($(".chosen-district").length > 0) {
            $(".chosen-district").val('').trigger("liszt:updated");
            jQuery(".chosen-district").chosen();
        }
    },
    initThanaChoosen: function () {
        if ($(".chosen-thana").length > 0) {
            $(".chosen-thana").trigger("liszt:updated");
            jQuery(".chosen-thana").chosen();
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
    },
    getGenderFlag: function (flag) {
        let flagName = '';
        switch (flag) {
            case MFFlagConstants.Male:
                flagName = `<span class='label label-primary'>Male</span>`;
                break;

            case MFFlagConstants.Female:
                flagName = `<span class='label label-info'>Female</span>`;
                break;

            case MFFlagConstants.Neutral:
                flagName = `<span class='label label-success'>Neutral</span>`;
                break;
        }

        return flagName;
    },

    GetFinancialData: function () {
        $('#gridFd').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var ind_code = $("#IND_CODE").val();
                        var m_f_flag = $("#M_F_flag").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/OLRSReport/GetFinancialData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, ind_code: ind_code, m_f_flag: m_f_flag, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                IND_CODE: {
                    title: 'Ind Code'
                },
                M_F_FLAG: {
                    title: 'M/F Flag'
                },
                FD_PKSF_FUND: {
                    title: 'PKSF Fund'
                },
                FD_NON_PKSF_FUND: {
                    title: 'Non PKSF Fund'
                },

                FD_TOTAL_FUND: {
                    title: 'TOTAL FUND'
                },
                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            let indCode = data.record.IND_CODE.split('-')[0].trim();
                            return `<div class="text-center delete-link"><a onclick="olrsReportManager.DeleteFinancialAction('${data.record.MNYR}','${indCode}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }

                        return "";
                    }
                }
            }
        });
        $('#gridFd').jtable('load');
    },
    refreshSearchTerm: () => {
        $("#MNYR").val('');
        $("#IND_CODE").val('');
        $("#M_F_flag").val('');
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadFdGrid();
    },
    reloadFdGrid: () => {
        var mnyr = $("#MNYR").val();
        var ind_code = $("#IND_CODE").val();
        var m_f_flag = $("#M_F_flag").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridFd').jtable('load', { mnyr: mnyr, ind_code: ind_code, m_f_flag: m_f_flag, syncedStatus: syncedStatus });
    },
    performFinancialDataDeleteAction: (mnyr, ind_code) => {
        const postData = { MNYR: mnyr, IndicatorCode: ind_code };

        fetch('/FinancialData/DeleteFinancialData', {
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
                olrsReportManager.reloadFdGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteFinancialAction: (mnyr, ind_code) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsReportManager.performFinancialDataDeleteAction(mnyr, ind_code);
                return true;
            }
            else {
                return false;
            }
        });
    },

    GetBasicData: function () {
        $('#gridBd').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var ind_code = $("#IND_CODE").val();
                        var m_f_flag = $("#M_F_flag").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/OLRSReport/GetBasicData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, ind_code: ind_code, m_f_flag: m_f_flag, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;

                                console.log(data);
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                IND_INF: {
                    title: 'Indicator'
                },
                M_F_FLAG: {
                    title: 'M/F Flag',
                    display: function (data) {
                        var mfFlag = olrsReportManager.getGenderFlag(data.record.M_F_FLAG);
                        return mfFlag;
                    }
                },
                BD_PKSF_FUND: {
                    title: 'PKSF Fund'
                },
                BD_NON_PKSF_FUND: {
                    title: 'Non PKSF Fund'
                },

                BD_TOTAL_FUND: {
                    title: 'TOTAL FUND'
                },
                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            return `<div class="text-center delete-link"><a onclick="olrsReportManager.DeleteBasicDataAction('${data.record.MNYR}','${data.record.IND_CODE}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }

                        return "";
                    }
                }
            }
        });
        $('#gridBd').jtable('load');
    },
    refreshBdSearchTerm: () => {
        $("#MNYR").val('');
        $("#IND_CODE").val('');
        $("#M_F_flag").val('');
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadBdGrid();
    },
    reloadBdGrid: function () {
        var mnyr = $("#MNYR").val();
        var ind_code = $("#IND_CODE").val();
        var m_f_flag = $("#M_F_flag").val();
        var m_f_flag = $("#M_F_flag").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridBd').jtable('load', { mnyr: mnyr, ind_code: ind_code, m_f_flag: m_f_flag, syncedStatus: syncedStatus });
    },
    performBasicDataDeleteAction: (mnyr, ind_code) => {
        const postData = { MNYR: mnyr, IndicatorCode: ind_code };

        fetch('/BasicData/DeleteBasicData', {
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
                olrsReportManager.reloadBdGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteBasicDataAction: (mnyr, ind_code) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsReportManager.performBasicDataDeleteAction(mnyr, ind_code);
                return true;
            }
            else {
                return false;
            }
        });
    },

    GetProgramData: function () {
        $('#gridPd').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var ind_code = $("#IND_CODE").val();
                        var m_f_flag = $("#M_F_flag").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/OLRSReport/GetProgramData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, ind_code: ind_code, m_f_flag: m_f_flag, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                IND_INFO: {
                    title: 'Indicator'
                },
                M_F_FLAG: {
                    title: 'M/F Flag',
                    display: function (data) {                        
                        var mfFlag = olrsReportManager.getGenderFlag(data.record.M_F_FLAG);
                        return mfFlag;
                    }
                },
                TOTAL: {
                    title: 'Total'
                },
                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                Action: {
                    title: 'Details',
                    display: function (data) {
                        var mmyr = data.record.MNYR.replace("/", "_");
                        var detailLink = `<a class="btn btn-sm btn-primary" target='_blank' href="/olrsreport/programdata/mmyr/${mmyr}/indcode/${data.record.IND_CODE}/mfflag/${data.record.M_F_FLAG}"><i class='fa fa-info-circle' aria-hidden='true'></i> Details</a>`;
                        return detailLink;
                    }
                },
                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            return `<div class="text-center delete-link"><a onclick="olrsReportManager.DeleteProgramDataAction('${data.record.MNYR}','${data.record.IND_CODE}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }

                        return "";
                    }
                }
            }
        });
        $('#gridPd').jtable('load');
    },    
    refreshPdSearchTerm: () => {
        $("#MNYR").val('');
        $("#IND_CODE").val('');
        $("#M_F_flag").val('');
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadPdGrid();
    },
    reloadPdGrid: function () {
        var mnyr = $("#MNYR").val();
        var ind_code = $("#IND_CODE").val();
        var m_f_flag = $("#M_F_flag").val();
        var m_f_flag = $("#M_F_flag").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridPd').jtable('load', { mnyr: mnyr, ind_code: ind_code, m_f_flag: m_f_flag, syncedStatus: syncedStatus });
    },
    performProgramDataDeleteAction: (mnyr, ind_code) => {
        const postData = { MNYR: mnyr, IndicatorCode: ind_code };

        fetch('/ProgramData/DeleteProgramData', {
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
                olrsReportManager.reloadPdGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteProgramDataAction: (mnyr, ind_code) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsReportManager.performProgramDataDeleteAction(mnyr, ind_code);
                return true;
            }
            else {
                return false;
            }
        });
    },


    GetUpazillaLoanData: function () {
        $('#gridUl').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var mfi_District_Code = $("#MFI_District_Code").val();
                        var mfi_Thana_Code = $("#MFI_Thana_Code").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/OLRSReport/GetUpazillaLoan?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, mfi_District_Code, mfi_Thana_Code, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                MFI_DISTRICT_NAME: {
                    title: 'District'
                },
                MFI_THANA_NAME: {
                    title: 'Thana'
                },
                PRODUCTNAME: {
                    title: 'Product'
                },
                CUM_DISB: {
                    title: 'CUM_DISB'
                }, MEMBER: {
                    title: 'MEMBER'
                }, BORROWER: {
                    title: 'BORROWER'
                }, LOAN_FY: {
                    title: 'LOAN_FY'
                }, CUM_BORR: {
                    title: 'CUM_BORR'
                },               
                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                EditLink: {
                    title: "Edit",
                    display: function (data) {

                        var syncedStatusOption = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? '' : '<div class="text-center"><a href="#" OnClick="popupDiv(' + "'" + data.record.PO_CODE + "'" + ',' + "'" + data.record.MNYR + "'" + ',' + data.record.CUM_DISB + ',' + data.record.MEMBER + ',' + data.record.BORROWER + ',' + data.record.LOAN_FY + ',' + data.record.CUM_BORR + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                        return syncedStatusOption;

                       // return '<div class="text-center"><a href="#" OnClick="popupDiv(' + data.record.PO_CODE + ','+ "'" + data.record.MNYR + "'" + ',' + data.record.CUM_DISB + ',' + data.record.MEMBER + ',' + data.record.BORROWER + ',' + data.record.LOAN_FY + ',' + data.record.CUM_BORR + ');"><i class="fa fa-pencil-square-o"></i></a></div>';

                    }
                }
            }
        });
        $('#gridUl').jtable('load');
    },
    refreshUlSearchTerm: () => {
        $("#MNYR").val('');
        $("#MFI_District_Code").val('');
        $("#MFI_Thana_Code").val('');
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadUlGrid();
    },
    reloadUlGrid: function () {
        var mnyr = $("#MNYR").val();
        var mfi_District_Code = $("#MFI_District_Code").val();
        var mfi_Thana_Code = $("#MFI_Thana_Code").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridUl').jtable('load', { mnyr: mnyr, mfi_District_Code, mfi_Thana_Code, syncedStatus: syncedStatus });
    },
    populateThanaByDistrict: function () {
        var districtCode = $("#MFI_District_Code").val();
        var ddlThana = $("#MFI_Thana_Code");

        ddlThana.html('').append($('<option></option>').val('').html("Select One"));

        if (!districtCode || districtCode.length <= 0) {
            olrsReportManager.initThanaChoosen();
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

                olrsReportManager.initThanaChoosen();

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },

    GetAccountingData: function () {
        $('#gridAd').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var accGROUP = $("#ACCGROUP").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();
                        var searchTerm = $("#SearchTerm").val();

                        $.ajax({
                            url: '/OLRSReport/GetAccountingData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, accGroup: accGROUP, syncedStatus: syncedStatus, searchTerm: searchTerm },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;

                                //get accounting data summary
                                olrsReportManager.getAccountingDataSummary(mnyr, accGROUP, syncedStatus, searchTerm);                              

                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                ACCHEADNAME: {
                    title: 'Acc Head'
                },
                ACCGROUP: {
                    title: 'Acc Group'
                },
                //BAL_DR: {
                //    title: 'BAL_DR'
                //},
                //BAL_CR: {
                //    title: 'BAL_CR'
                //},
                //CUM_BAL_DR: {
                //    title: 'CUM_BAL_DR'
                //},
                //CUM_BAL_CR: {
                //    title: 'CUM_BAL_CR'
                //},
                THIS_MONTH_CASH: {
                    title: 'THIS_MONTH_CASH'
                },
                THIS_FY_CASH: {
                    title: 'THIS_FY_CASH'
                },
                THIS_MONTH_NONCASH: {
                    title: 'THIS_MONTH_NONCASH'
                },

                THIS_FY_NONCASH: {
                    title: 'THIS_FY_NONCASH'
                },
                LAST_JUNE: {
                    title: 'LAST_JUNE'
                },

                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                DeleteLink: {
                    title: "Delete",
                    //width: '5%',
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            return `<div class="text-center delete-link"><a onclick="olrsReportManager.DeleteAccountingDataAction('${data.record.MNYR}','${data.record.L1_CODE}','${data.record.L2_CODE}','${data.record.L3_CODE}','${data.record.L4_CODE}','${data.record.L5_CODE}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }

                        return "";
                    }
                }
            }
        });
        $('#gridAd').jtable('load');
    },
    refreshAdSearchTerm: () => {
        $("#MNYR").val('');
        $("#ACCGROUP").val('BS');
        $("#SearchTerm").val('');
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadAdGrid();
    },
    reloadAdGrid: function () {
        var mnyr = $("#MNYR").val();
        var accGROUP = $("#ACCGROUP").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridAd').jtable('load', { mnyr: mnyr, accGroup: accGROUP, syncedStatus: syncedStatus });
    },
    performAccountingDataDeleteAction: (MNYR, l1_code, l2_code, l3_code, l4_code, l5_code ) => {
        const postData = { MNYR, l1_code, l2_code, l3_code, l4_code, l5_code };

        fetch('/OLRSReport/DeleteAccountingData', {
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
                olrsReportManager.reloadAdGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteAccountingDataAction: (MNYR, l1_code, l2_code, l3_code, l4_code, l5_code ) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsReportManager.performAccountingDataDeleteAction(MNYR, l1_code, l2_code, l3_code, l4_code, l5_code );
                return true;
            }
            else {
                return false;
            }
        });
},
    getAccountingDataSummary: (mnyr, accGROUP, syncedStatus, searchTerm) => {

        $('.BAL_DR').text(0);
        $('.BAL_CR').text(0);
        $('.CUM_BAL_DR').text(0);
        $('.CUM_BAL_CR').text(0);
        $('.THIS_MONTH_CASH').text(0);
        $('.THIS_FY_CASH').text(0);
        $('.THIS_MONTH_NONCASH').text(0);
        $('.THIS_FY_NONCASH').text(0);
        $('.LAST_JUNE').text(0);

        let url = `/OLRSReport/GetAccountingDataSummary`;
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: url,
            data: { mnyr, accGROUP, syncedStatus, searchTerm },
            dataType: 'json',
            async: true,
            success: function (res) {
                if (res.Data) {
                    $('.BAL_DR').text(res.Data.BAL_DR);
                    $('.BAL_CR').text(res.Data.BAL_CR);
                    $('.CUM_BAL_DR').text(res.Data.CUM_BAL_DR);
                    $('.CUM_BAL_CR').text(res.Data.CUM_BAL_CR);
                    $('.THIS_MONTH_CASH').text(res.Data.THIS_MONTH_CASH);
                    $('.THIS_FY_CASH').text(res.Data.THIS_FY_CASH);
                    $('.THIS_MONTH_NONCASH').text(res.Data.THIS_MONTH_NONCASH);
                    $('.THIS_FY_NONCASH').text(res.Data.THIS_FY_NONCASH);
                    $('.LAST_JUNE').text(res.Data.LAST_JUNE);
                }

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });        
         
    },

    GetEmploymentData: function () {
        $('#gridEd').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();
                        var employmentType = $("#EmploymentType").val();
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/OLRSReport/GetEmploymentData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, employmentType: employmentType, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
                PRODUCT_NAME: {
                    title: 'Product_Name'
                },
                SELF_FULL_M: {
                    title: 'SELF_FULL_M'
                },
                SELF_FULL_F: {
                    title: 'SELF_FULL_F'
                },
                SELF_PART_M: {
                    title: 'SELF_PART_M'
                },
                SELF_PART_F: {
                    title: 'SELF_PART_F'
                },
                WAGE_FULL_M: {
                    title: 'WAGE_FULL_M'
                },
                WAGE_FULL_F: {
                    title: 'WAGE_FULL_F'
                },
                WAGE_PART_M: {
                    title: 'WAGE_PART_M'
                },
                WAGE_PART_F: {
                    title: 'WAGE_PART_F'
                },
                SYNCED_STATUS: {
                    title: 'Synced_Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                },
                DeleteLink: {
                    title: "Delete",                    
                    //width: '5%',
                    display: function (data) {
                        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                            return `<div class="text-center delete-link"><a onclick="olrsReportManager.DeleteEmploymentAction('${data.record.MNYR}','${data.record.EmploymentType}')"><i class='fa fa-trash-o'></i></a></div>`;
                        }

                        return "";                       
                    }                    
                }
            }
        });
        $('#gridEd').jtable('load');
    },
    refreshEdSearchTerm: () => {
        $("#MNYR").val('');
        $("#EmploymentType").val('Employment up to Last Half Year');
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadEdGrid();
    },
    reloadEdGrid: function () {
        var mnyr = $("#MNYR").val();
        var employmentType = $("#EmploymentType").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridEd').jtable('load', { mnyr: mnyr, employmentType: employmentType, syncedStatus: syncedStatus });
    },
    performEmploymentDeleteAction: (mnyr, type) => {
        const postData = { mnyr, type };

        fetch('/OLRSEmployment/DeleteEmploymentData', {
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
                olrsReportManager.reloadEdGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteEmploymentAction: (mnyr,type) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsReportManager.performEmploymentDeleteAction(mnyr, type);
                return true;
            }
            else {
                return false;
            }
        });
    },

    GetTopSheetData: function () {
        $('#gridTopSheet').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            defaultSorting: 'Name ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        var mnyr = $("#MNYR").val();                       
                        var syncedStatus = $("#SYNCED_STATUS").val();

                        $.ajax({
                            url: '/OLRSReport/GetTopSheetData?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + `&isCalculateTotal=${isCalculateTotal}&totalCount=${totalCount}`,
                            type: 'POST',
                            dataType: 'json',
                            data: { mnyr: mnyr, syncedStatus: syncedStatus },
                            success: function (data) {
                                $dfd.resolve(data);
                                isCalculateTotal = false;
                                totalCount = data.TotalRecordCount;

                                console.log(data);
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                },
            },
            fields: {
                MNYR: {
                    title: 'Month/Year'
                },
               
                MEMBER_MALE: {
                    title: 'MEMBER_MALE'
                },
                MEMBER_FEMALE: {
                    title: 'MEMBER_FEMALE'
                },

                BORROWER_MALE: {
                    title: 'BORROWER_MALE'
                },
                BORROWER_FEMALE: {
                    title: 'BORROWER_FEMALE'
                },

                CUM_LOAN_DISB: {
                    title: 'CUM_LOAN_DISB'
                },
                CUM_LOAN_RCVD_PRN: {
                    title: 'CUM_LOAN_RCVD_PRN'
                },

                LOAN_OUT_PRN: {
                    title: 'LOAN_OUT_PRN'
                },

                LOAN_OD_PRN: {
                    title: 'LOAN_OD_PRN'
                },

                LOAN_ADV_PRN: {
                    title: 'LOAN_ADV_PRN'
                },
                LOAN_OUT_PRN_OPB_YR: {
                    title: 'LOAN_OUT_PRN_OPB_YR'
                },

                LOAN_OVERDUE: {
                    title: 'LOAN_OVERDUE'
                },

                SYNCED_STATUS: {
                    title: 'Synced Status',
                    display: function (data) {
                        var syncedStatus = data.record.SYNCED_STATUS == SyncedStatusConstants.SYNCED ? `<span class='label label-success'>${SyncedStatusConstants.SYNCED}</span>` : `<span class='label label-warning'>${SyncedStatusConstants.NOT_SYNCED}</span>`;
                        return syncedStatus;
                    }
                }
                //,
                //DeleteLink: {
                //    title: "Delete",
                //    //width: '5%',
                //    display: function (data) {
                //        if (data.record.SYNCED_STATUS !== SyncedStatusConstants.SYNCED) {
                //            return `<div class="text-center delete-link"><a onclick="olrsReportManager.DeleteTopSheetDataAction('${data.record.MNYR}','${data.record.IND_CODE}')"><i class='fa fa-trash-o'></i></a></div>`;
                //        }

                //        return "";
                //    }
                //}
            }
        });
        $('#gridTopSheet').jtable('load');
    },
    refreshTopSheetSearchTerm: () => {
        $("#MNYR").val('');       
        $("#SYNCED_STATUS").val('');
        olrsReportManager.reloadTopSheetGrid();
    },
    reloadTopSheetGrid: function () {
        var mnyr = $("#MNYR").val();
        var syncedStatus = $("#SYNCED_STATUS").val();
        $('#gridTopSheet').jtable('load', { mnyr: mnyr, syncedStatus: syncedStatus });
    },
    performTopSheetDataDeleteAction: (mnyr) => {
        const postData = { MNYR: mnyr};

        fetch('/TopSheetData/DeleteTopSheetData', {
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
                olrsReportManager.reloadTopSheetGrid();
                $.alert.open("Success", data.Message);
            })
            .catch((error) => {
                $.alert.open("Error", error);
            });
    },
    DeleteTopSheetDataAction: (mnyr) => {
        $.alert.open('confirm', 'Are you sure you want to delete this one?', function (button) {
            if (button == 'yes') {
                olrsReportManager.performTopSheetDataDeleteAction(mnyr);
                return true;
            }
            else {
                return false;
            }
        });
    },
}



function popupDiv( PO_CODE, MNYR,CUM_DISB, MEMBER, BORROWER, LOAN_FY, CUM_BORR ) {

   // alert(CUM_BORR);
     
    $("#hdnPO_CODE").val(PO_CODE);
    $("#hdnMNYR").val(MNYR);

    $("#CUM_DISB").val(CUM_DISB);
    $("#MEMBER").val(MEMBER);
    $("#BORROWER").val(BORROWER);
    $("#LOAN_FY").val(LOAN_FY);
    $("#CUM_BORR").val(CUM_BORR);

    $("#CUM_DISB_HDN").val(CUM_DISB);
    $("#MEMBER_HDN").val(MEMBER);
    $("#BORROWER_HDN").val(BORROWER);
    $("#LOAN_FY_HDN").val(LOAN_FY);
    $("#CUM_BORR_HDN").val(CUM_BORR);


    $('html, body').animate({ scrollTop: $('#PopupDiv').offset().top }, 'slow');
    $("#PopupDiv").dialog({
        autoOpen: false,
        height: 460,
        width: 390,
        modal: true,
        buttons: {
            "Save": function () {
                 
                var po_code = $("#hdnPO_CODE").val();
                var mnyr = $("#hdnMNYR").val();

                var cum_disb    =   $("#CUM_DISB").val();
                var member      =   $("#MEMBER").val();
                var borrower    =   $("#BORROWER").val();
                var loan_fy     =   $("#LOAN_FY").val();
                var cum_borr    =   $("#CUM_BORR").val();

                var cum_disb_hdn    =   $("#CUM_DISB_HDN").val();
                var member_hdn      =   $("#MEMBER_HDN").val();
                var borrower_hdn    =   $("#BORROWER_HDN").val();
                var loan_fy_hdn     =   $("#LOAN_FY_HDN").val();
                var cum_borr_hdn    =   $("#CUM_BORR_HDN").val();

                 
                $.ajax({
                    url: '/OLRSReport/UpdateUpazillaLoan',
                    type: 'POST',
                    dataType: 'json',
                    data: { po_code: po_code, mnyr: mnyr, cum_disb: cum_disb, member: member, borrower: borrower, loan_fy: loan_fy, cum_borr: cum_borr, cum_disb_hdn: cum_disb_hdn, member_hdn: member_hdn, borrower_hdn: borrower_hdn, loan_fy_hdn: loan_fy_hdn, cum_borr_hdn: cum_borr_hdn  },
                    success: function (data) {
                        olrsReportManager.GetUpazillaLoanData();
                        alert(data.Result);
                        $(this).dialog("close");

                    },
                    error: function () {
                       
                    }
                });




                
            },
            "Close": function () {
                
                $(this).dialog("close");
            }
        }
    });

    var showPopup = "True";
    if (showPopup == "True") {
        $("#PopupDiv").dialog('open');
    }

}//End of Show Popup		

$(function () {
    olrsReportManager.init();

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            if ($('#btnSearchPd').length > 0) {
                $('#btnSearchPd').trigger('click');
            }
            if ($('#btnRefreshFd').length > 0) {
                $('#btnRefreshFd').trigger('click');
            }
            if ($('#btnSearchBd').length > 0) {
                $('#btnSearchBd').trigger('click');
            }
            if ($('#btnSearchTopSheet').length > 0) {
                $('#btnSearchTopSheet').trigger('click');
            }
        }
    });

    $('#MFI_District_Code').on('change', function () {
        olrsReportManager.populateThanaByDistrict();
    })

    $('#btnSearchFd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetFinancialData();
    })

    $('#btnRefreshFd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshSearchTerm();
    })

    $('#btnSearchBd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetBasicData();
    })

    $('#btnRefreshBd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshBdSearchTerm();
    })

    $('#btnSearchPd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetProgramData();
    })

    $('#btnRefreshPd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshPdSearchTerm();
    })

    $('#btnSearchUl').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetUpazillaLoanData();
    })

    $('#btnRefreshUl').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshUlSearchTerm();
    })

    $('#btnSearchAd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetAccountingData();
    })

    $('#btnRefreshAd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshAdSearchTerm();
    })

    $('#btnSearchEd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetEmploymentData();
    })

    $('#btnRefreshEd').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshEdSearchTerm();
    })

    $('#btnSearchTopSheet').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.GetTopSheetData();
    })

    $('#btnRefreshTopSheet').on('click', () => {
        isCalculateTotal = true;
        olrsReportManager.refreshTopSheetSearchTerm();
    })
});
