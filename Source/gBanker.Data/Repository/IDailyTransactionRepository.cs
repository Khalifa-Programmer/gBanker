using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IDailyTransactionRepository : IRepository<DailyTransaction>
    {
        DailyTransaction AddNewDailyTransaction(DailyTransaction dailyTransaction);
        bool UpdateDailyTransaction(DailyTransaction dailyTransaction);
    }
    public class DailyTransactionRepository : RepositoryBaseCodeFirst<DailyTransaction>, IDailyTransactionRepository
    {
        public DailyTransactionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public DailyTransaction AddNewDailyTransaction(DailyTransaction dailyTransaction)
        {
            DataContext.DailyTransaction.Add(dailyTransaction);
            DataContext.SaveChanges();
            return dailyTransaction;
        }
        public bool UpdateDailyTransaction(DailyTransaction dailyTransaction)
        {
            try
            {
                var updateDailyTransaction = DataContext.DailyTransaction.FirstOrDefault(f => f.DailyTransactionId == dailyTransaction.DailyTransactionId);

                if (updateDailyTransaction == null)
                    return false;
                else
                {
                    updateDailyTransaction.TransactionDate = dailyTransaction.TransactionDate;
                    updateDailyTransaction.AssetGroupID = dailyTransaction.AssetGroupID;
                    updateDailyTransaction.AssetID = dailyTransaction.AssetID;
                    updateDailyTransaction.AssetSerial = dailyTransaction.AssetSerial;
                    updateDailyTransaction.AssetDescription = dailyTransaction.AssetDescription;
                    updateDailyTransaction.PurchasePrice = dailyTransaction.PurchasePrice;
                    updateDailyTransaction.AssetClientId = dailyTransaction.AssetClientId;
                    updateDailyTransaction.TransactionType = dailyTransaction.TransactionType;
                    updateDailyTransaction.AssetUser = dailyTransaction.AssetUser;
                    updateDailyTransaction.Usable = dailyTransaction.Usable;
                    //updateDailyTransaction.OrgID = dailyTransaction. ;
                    updateDailyTransaction.OfficeID = dailyTransaction.OfficeID;
                    updateDailyTransaction.DepCalcDate = dailyTransaction.DepCalcDate;
                    updateDailyTransaction.PurchaseDate = dailyTransaction.PurchaseDate;
                    updateDailyTransaction.OperationDate = dailyTransaction.OperationDate;
                    updateDailyTransaction.InstallationCost = dailyTransaction.InstallationCost;
                    updateDailyTransaction.CarringCost = dailyTransaction.CarringCost;
                    updateDailyTransaction.OtherCost = dailyTransaction.OtherCost;
                    updateDailyTransaction.TotalCost = dailyTransaction.TotalCost;
                    updateDailyTransaction.ProjectID = dailyTransaction.ProjectID;
                    updateDailyTransaction.DepriciationRate = dailyTransaction.DepriciationRate;
                    updateDailyTransaction.DepriciationMethod = dailyTransaction.DepriciationMethod;
                    updateDailyTransaction.IsCapitalizedAsset = dailyTransaction.IsCapitalizedAsset;
                    updateDailyTransaction.IsInstallmentAsset = dailyTransaction.IsInstallmentAsset;
                    updateDailyTransaction.InsuranceValue = dailyTransaction.InsuranceValue;
                    updateDailyTransaction.WarrantyGurantee = dailyTransaction.WarrantyGurantee;
                    updateDailyTransaction.InsuranceExpDate = dailyTransaction.InsuranceExpDate;
                    updateDailyTransaction.UsefulLifeYear = dailyTransaction.UsefulLifeYear;
                    updateDailyTransaction.PurchaseOrderNo = dailyTransaction.PurchaseOrderNo;
                    updateDailyTransaction.PurchaseOrderDate = dailyTransaction.PurchaseOrderDate;
                    updateDailyTransaction.Remarks = dailyTransaction.Remarks;
                    //updateDailyTransaction.IsActive = dailyTransaction. ;
                    updateDailyTransaction.AssetStatus = dailyTransaction.AssetStatus;
                    updateDailyTransaction.StatusDate = dailyTransaction.StatusDate;
                    updateDailyTransaction.UpdateUser = dailyTransaction.UpdateUser;
                    updateDailyTransaction.UpdateDate = dailyTransaction.UpdateDate;
                    updateDailyTransaction.DownPayment = dailyTransaction.DownPayment;
                    updateDailyTransaction.InstallmentNumber = dailyTransaction.InstallmentNumber;
                    updateDailyTransaction.InstallmentAmount = dailyTransaction.InstallmentAmount;
                    updateDailyTransaction.BankName = dailyTransaction.BankName;

                    DataContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
