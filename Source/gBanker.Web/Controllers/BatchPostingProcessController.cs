using Kendo.Mvc.UI;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using gBanker.Service;
using gBanker.Web.Controllers;
using gBanker.Service.ReportServies;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.ViewModels;
using OfficeOpenXml;

namespace gBanker.Web.Controllers
{
    public class BatchPostingProcessController : BaseController
    {
        private IBatchPostingProcessService batchPostingProcessService;
        private readonly IUltimateReportService sPQueryService;
        private readonly IOfficeService unitInfoService;
        private readonly IAccChartService accChartService;
        
        public BatchPostingProcessController(IBatchPostingProcessService batchPostingProcessService, IUltimateReportService sPQueryService
            , IOfficeService unitInfoService, IAccChartService accChartService)
        {
            this.batchPostingProcessService = batchPostingProcessService;
            this.sPQueryService = sPQueryService;
            this.unitInfoService = unitInfoService;
            this.accChartService = accChartService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddBatchFile()
        {
            return View();
        }
        public ActionResult UploadBatchFile(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["BatchFile"];
                BatchPostingProcessViewModel returnInformation = new BatchPostingProcessViewModel();
                try
                {
                    if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var batchPostingList = new List<BatchPostingProcess>();
                        var existingList = new List<BatchPostingProcess>();

                        var fileTYpe = Path.GetExtension(file.FileName).GetType();

                          var package = new ExcelPackage(file.InputStream);
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                /*Transaction Date	Office Code	Account Code	Account name 	Voucher Type	Narration	Credit	Debit*/
                                var batchData = new BatchPostingProcess();

                                if (string.IsNullOrEmpty(returnInformation.BatchFileNo) == true)
                                {

                                    returnInformation.BatchFileNo = GenerateFileNo();
                                }
                               var vDate = workSheet.Cells[rowIterator,2].Value;
                                var dd = workSheet.Cells[rowIterator, 2].Value.ToString().Trim().Replace(" ",string.Empty) ;
                                batchData.TransactionDate = Convert.ToDateTime(dd);
                                batchData.OfficeCode = workSheet.Cells[rowIterator, 3].Value == null ? null : workSheet.Cells[rowIterator, 3].Value.ToString().Trim().Replace(" ", string.Empty);
                                batchData.AccountCode = workSheet.Cells[rowIterator, 4].Value.ToString().Trim().Replace(" ", string.Empty);
                                batchData.AccountName = workSheet.Cells[rowIterator, 5].Value.ToString().Trim();
                                batchData.VoucherType = workSheet.Cells[rowIterator, 6].Value.ToString().Trim();
                                batchData.Narration = workSheet.Cells[rowIterator, 7].Value == null ? null : workSheet.Cells[rowIterator, 7].Value.ToString().Trim();
                                batchData.Credit = Convert.ToDecimal(workSheet.Cells[rowIterator, 8].Value.ToString().Trim().Replace(" ", string.Empty));
                                batchData.Debit = Convert.ToDecimal(workSheet.Cells[rowIterator, 9].Value.ToString().Trim().Replace(" ", string.Empty));
                                batchData.CreateBy = LoggedInEmployeeID.Value;
                                batchData.CreateDate = DateTime.Now;
                                batchData.IsRemoved = false;
                                batchData.BatchFileNo = returnInformation.BatchFileNo;
                                batchData.IsPosted = false;
                               
                                batchPostingList.Add(batchData);
                            }
                            var totalVoucher = batchPostingList.Count();

                            if (batchPostingList.Sum(b => b.Debit) == batchPostingList.Sum(b => b.Credit))
                            {
                                int totalRow = batchPostingProcessService.SaveListBatchData(batchPostingList);
                            }
                            else
                            {
                                returnInformation.IsError = 1;
                                returnInformation.ErrorMessage = "Debit And Credit Amount is not same.";
                                return View("AddBatchFile", returnInformation);
                            }

                    }
                } catch(Exception e)
                {
                    returnInformation.ErrorMessage = e.Message;
                    return View("AddBatchFile",returnInformation);
                }
            }
            return RedirectToAction("Index");
        }

        public JsonResult getBatchFileDetailsData([DataSourceRequest]DataSourceRequest request, string BatchFileNo)
        {
            IEnumerable<BatchPostingProcessViewModel> dataList = new List<BatchPostingProcessViewModel>();


             var batchFileList= (from batchFile in  batchPostingProcessService.GetAll().Where(b => b.IsRemoved == false
                                && b.IsPosted == false && b.BatchFileNo == BatchFileNo)
                    join unit in unitInfoService.GetAll().Where(b=>b.IsActive == true) on batchFile.OfficeCode equals unit.OfficeCode
                    join acChart in accChartService.GetAll().Where(b=>b.IsActive== true) on batchFile.AccountCode equals acChart.AccCode
                    select new{BatchData = batchFile,UnitInfo = unit,AcountChart = acChart}).ToList();

            dataList =batchFileList.Select((b,index) => new BatchPostingProcessViewModel
                                {
                                    RowSl = index+1,
                                    BatchId = b.BatchData.BatchId,
                                    TransactionDate = b.BatchData.TransactionDate,
                                    TransactionDateMsg=b.BatchData.TransactionDate.ToString("dd-MMM-yyyy"),
                                    OfficeCode = b.BatchData.OfficeCode,
                                    OfficeName = b.UnitInfo.OfficeName,
                                    AccountCode = b.BatchData.AccountCode,
                                    AccountName = b.AcountChart.AccName,
                                    VoucherType = b.BatchData.VoucherType,
                                    Narration = b.BatchData.Narration,
                                    Credit = b.BatchData.Credit,
                                    Debit = b.BatchData.Debit,
                                    BatchFileNo = b.BatchData.BatchFileNo,
                                    
                                }).ToList();

            DataSourceResult result = dataList.ToDataSourceResult(request);

            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

          [HttpPost]
       public ActionResult CreateNewBatchData([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<BatchPostingProcessViewModel> model)
       {
           if (model != null && ModelState.IsValid)
           {
               foreach (var d in model)
               {
                      BatchPostingProcess up = new BatchPostingProcess();
                       up.AccountCode = d.AccountCode;
                       up.AccountName = d.AccountName;
                       up.Credit = d.Credit;
                       up.Debit = d.Debit;
                       up.Narration = d.Narration;
                       up.OfficeCode = d.OfficeCode;
                       up.TransactionDate = d.TransactionDate;
                       up.VoucherType = d.VoucherType;
                       up.UpdateBy = LoggedInEmployeeID;
                       up.UpdateDate = DateTime.Now;
                       up.CreateBy = LoggedInEmployeeID.Value;
                       up.CreateDate = DateTime.Now;
                       up.IsRemoved = false;
                       up.IsPosted = false;
                       batchPostingProcessService.Create(up);

                   
               }
           }

           return Json(batchPostingProcessService.GetAll().ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
       }
        [HttpPost]
       public ActionResult Destroy([DataSourceRequest] DataSourceRequest request,[Bind(Prefix = "models")]IEnumerable<BatchPostingProcessViewModel> model)
       {
           if (model != null)
           {
               foreach (var d in model)
               {
                   if (d.BatchId > 0)
                   {
                       BatchPostingProcess up = new BatchPostingProcess();
                       up = batchPostingProcessService.getByBatchIdLong(d.BatchId);
                       up.IsRemoved = true;
                       up.UpdateBy = LoggedInEmployeeID;
                       up.UpdateDate = DateTime.Now;
                       batchPostingProcessService.Update(up);
                   }
               }
           }
           return Json(batchPostingProcessService.GetAll().ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
       }
        [HttpPost]
        public ActionResult UpdateBatchData([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<BatchPostingProcessViewModel> model)
        {
            if (model != null && ModelState.IsValid)
            {
                foreach (var d in model)
                {
                    if (d.BatchId > 0)
                    {
                        BatchPostingProcess up = new BatchPostingProcess();
                        up = batchPostingProcessService.getByBatchIdLong(d.BatchId);
                        
                        up.AccountCode = d.AccountCode;
                        up.AccountName = d.AccountName;
                        up.Credit = d.Credit;
                        up.Debit = d.Debit;
                        up.Narration = d.Narration;
                        up.OfficeCode = d.OfficeCode;
                        up.TransactionDate = d.TransactionDate;
                        up.VoucherType = d.VoucherType;
                        up.UpdateBy = LoggedInEmployeeID;
                        up.UpdateDate = DateTime.Now;

                        batchPostingProcessService.Update(up);
                    }
           
                }
            }
            else
            {
                var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            }

            return Json(batchPostingProcessService.GetAll().ToDataSourceResult(request, ModelState),JsonRequestBehavior.AllowGet);
        }

        public string GenerateFileNo()
        {
            var existingList = new List<BatchPostingProcess>();
            existingList = batchPostingProcessService.GetAll().Where(b => b.IsRemoved == false)
                                .Select(b => 
                                    new BatchPostingProcess { CreateDate=b.CreateDate
                                                             ,BatchFileNo=b.BatchFileNo
                                                             ,TransactionDate = b.TransactionDate
                                                            }).ToList();
            var fileNo = existingList
                                                    .Where(b => b.CreateDate.Year == DateTime.Now.Year && b.CreateDate.Month == DateTime.Now.Month)
                                                 .GroupBy(x => new { x.CreateDate.Year, x.CreateDate.Month, x.BatchFileNo })
                                                   .Count();
            if (fileNo != null)
            {
                fileNo = fileNo + 1;
            }
            else
            {
                fileNo = 1;
            }
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.ToString("MMM");
            string returnFileName = year + '-' + month + '-' + fileNo.ToString();
            return returnFileName;
        }

        public JsonResult getDateBatchFileNoList(DateTime BatchFileDate)
        {
            var fileList = batchPostingProcessService.GetAll()
                .Where(b => b.IsPosted == false );//&& b.CreateDate.Date == BatchFileDate
            var nFileList=fileList
                .Select(b => new {
                BatchFileNo = b.BatchFileNo
            }).Distinct()
            .OrderBy(b=>b.BatchFileNo);

            var returnList = nFileList
                 .Select(b => new SelectListItem
                 {
                     Value= b.BatchFileNo,
                     Text=b.BatchFileNo
                 });
            return Json(new { Data = returnList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostBatchFileToVoucher(string BatchFileNo)
        {
            int Result = 0; string Message = "";
            try
            {

                var param = new { BatchFileNo = BatchFileNo, CreateBy = LoggedInEmployeeID, OfficeID = LoginUserOfficeID, TranDate = TransactionDate };
                var postVoucher = sPQueryService.GetDataWithParameter(param, "SP_SET_PostBatchFileToVoucher");

                var batchPosting = postVoucher.Tables[0].AsEnumerable()
                 .Select(row => new
                 {
                     Message = row.Field<string>("Msg"),
                     Result = row.Field<int>("Result"),
                 }).FirstOrDefault();
                Result = batchPosting.Result;
                Message = batchPosting.Message;
            }
            catch (Exception e)
            {
                Result = 0;
                Message = e.Message;
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteBatch(int BatchID)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime UpdateDate = DateTime.Now;

                var param = new { @BatchId = BatchID };
                var alldata = sPQueryService.GetDataWithParameter(param, "deleteBatchPosting");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}