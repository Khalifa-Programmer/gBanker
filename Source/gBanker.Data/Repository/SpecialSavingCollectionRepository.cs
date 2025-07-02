using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ISpecialSavingCollectionRepository : IRepository<DailySavingTrx>
    {
        int setUpdateSavingBalance(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> monthlyInt);
        int setDailySavingTrx(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> dailySavinInstallment, Nullable<decimal> withDrawal, Nullable<System.DateTime> lcl_BusinessDate, string createUser, Nullable<System.DateTime> createDate, Nullable<int> transType);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate,Nullable<int> OrgID);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetsavingCollectionDetailForBankInterest(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<getSavingLastBalance_Result> getSavingLastBalance(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> dailySavinInstallment, Nullable<decimal> withDrawal, Nullable<System.DateTime> lcl_BusinessDate, Nullable<int> transType);
        IEnumerable<Proc_get_SavingLastBalance> Proc_getSavingLastBalance(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> dailySavinInstallment, Nullable<decimal> withDrawal, Nullable<System.DateTime> lcl_BusinessDate, Nullable<int> transType);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWiseAdjustment(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise_ForDormant(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID);

    }
    public class SpecialSavingCollectionRepository: RepositoryBaseCodeFirst<DailySavingTrx>, ISpecialSavingCollectionRepository
    {
        public SpecialSavingCollectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise_ForDormant(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var EmpIDParameter = new SqlParameter("@EmployeeId", EmpID);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollectionEmpWise_ForDormant @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue,@EmployeeId", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue, EmpIDParameter);

        }
        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollection @officeId,@CollectionDay", officeIdParameter, vcolday);

        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollection @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue", orgIdParameter,officeIdParameter, vcolday, filColName, filColvalue);

        }
        public int delVoucher(int? officeID, DateTime? businessDate,int? OrgID)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var date = new SqlParameter("@BusinessDate", businessDate);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("delVoucher @OfficeID,@BusinessDate,@OrgID", officeIdParameter, date, orgIdParameter);
        }


        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetsavingCollectionDetailForBankInterest(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SavingCollectionBankInterest @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue);

        }

        public int setDailySavingTrx(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? dailySavinInstallment, decimal? withDrawal, DateTime? lcl_BusinessDate, string createUser, DateTime? createDate, int? transType)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var CenterIDParameter = new SqlParameter("@CenterID",centerID);
            var MemberIDParameter = new SqlParameter("@MemberID", memberID);
            var ProductIDParameter = new SqlParameter("@ProductID", productID);
            var NoOfAccountParameter = new SqlParameter("@NoOfAccount", noOfAccount);
            var DailySavinInstallmentParameter = new SqlParameter("@DailySavinInstallment", dailySavinInstallment);
            var WithDrawalParameter = new SqlParameter("@WithDrawal", withDrawal);
            var lcl_BusinessDateParameter = new SqlParameter("@lcl_BusinessDate", lcl_BusinessDate);
            var CreateUserParameter = new SqlParameter("@CreateUser", createUser);
            var CreateDateParameter = new SqlParameter("@CreateDate", createDate);
            var TransTypeParameter = new SqlParameter("@TransType", transType);
            return DataContext.Database.ExecuteSqlCommand("setDailySavingTrx @OfficeID,@CenterID,@MemberID,@ProductID,@NoOfAccount,@DailySavinInstallment,@WithDrawal,@lcl_BusinessDate,@CreateUser,@CreateDate,@TransType", officeIdParameter, CenterIDParameter, MemberIDParameter,ProductIDParameter,NoOfAccountParameter,DailySavinInstallmentParameter,WithDrawalParameter,lcl_BusinessDateParameter,CreateUserParameter,CreateDateParameter,TransTypeParameter);

        }


        public IEnumerable<getSavingLastBalance_Result> getSavingLastBalance(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? dailySavinInstallment, decimal? withDrawal, DateTime? lcl_BusinessDate, int? transType)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var CenterIDParameter = new SqlParameter("@CenterID", centerID);
            var MemberIDParameter = new SqlParameter("@MemberID", memberID);
            var ProductIDParameter = new SqlParameter("@ProductID", productID);
            var NoOfAccountParameter = new SqlParameter("@NoOfAccount", noOfAccount);
            var DailySavinInstallmentParameter = new SqlParameter("@DailySavinInstallment", dailySavinInstallment);
            var WithDrawalParameter = new SqlParameter("@WithDrawal", withDrawal);
            var lcl_BusinessDateParameter = new SqlParameter("@lcl_BusinessDate", lcl_BusinessDate);
            var TransTypeParameter = new SqlParameter("@TransType", transType);
            return DataContext.Database.SqlQuery<getSavingLastBalance_Result>("getSavingLastBalance @OfficeID,@CenterID,@MemberID,@ProductID,@NoOfAccount,@DailySavinInstallment,@WithDrawal,@lcl_BusinessDate,@TransType", officeIdParameter, CenterIDParameter, MemberIDParameter, ProductIDParameter, NoOfAccountParameter, DailySavinInstallmentParameter, WithDrawalParameter, lcl_BusinessDateParameter, TransTypeParameter);

        }

        public int setUpdateSavingBalance(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? monthlyInt)
        {

            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var CenterIDParameter = new SqlParameter("@CenterID", centerID);
            var MemberIDParameter = new SqlParameter("@MemberID", memberID);
            var ProductIDParameter = new SqlParameter("@ProductID", productID);
            var NoOfAccountParameter = new SqlParameter("@NoOfAccount", noOfAccount);
            var DailymonthlyIntParameter = new SqlParameter("@monthlyInt", monthlyInt);
            return DataContext.Database.ExecuteSqlCommand("setUpdateSavingBalance @OfficeID,@CenterID,@MemberID,@ProductID,@NoOfAccount,@monthlyInt", officeIdParameter, CenterIDParameter, MemberIDParameter, ProductIDParameter, NoOfAccountParameter, DailymonthlyIntParameter);

        }


        public IEnumerable<Proc_get_SavingLastBalance> Proc_getSavingLastBalance(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? dailySavinInstallment, decimal? withDrawal, DateTime? lcl_BusinessDate, int? transType)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var CenterIDParameter = new SqlParameter("@CenterID", centerID);
            var MemberIDParameter = new SqlParameter("@MemberID", memberID);
            var ProductIDParameter = new SqlParameter("@ProductID", productID);
            var NoOfAccountParameter = new SqlParameter("@NoOfAccount", noOfAccount);
            var DailySavinInstallmentParameter = new SqlParameter("@DailySavinInstallment", dailySavinInstallment);
            var WithDrawalParameter = new SqlParameter("@WithDrawal", withDrawal);
            var lcl_BusinessDateParameter = new SqlParameter("@lcl_BusinessDate", lcl_BusinessDate);
            var TransTypeParameter = new SqlParameter("@TransType", transType);
            return DataContext.Database.SqlQuery<Proc_get_SavingLastBalance>("Proc_get_getSavingLastBalance @OfficeID,@CenterID,@MemberID,@ProductID,@NoOfAccount,@DailySavinInstallment,@WithDrawal,@lcl_BusinessDate,@TransType", officeIdParameter, CenterIDParameter, MemberIDParameter, ProductIDParameter, NoOfAccountParameter, DailySavinInstallmentParameter, WithDrawalParameter, lcl_BusinessDateParameter, TransTypeParameter);

        }


        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var EmpIDParameter = new SqlParameter("@EmployeeId", EmpID);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollectionEmpWise @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue,@EmployeeId", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue,EmpIDParameter);

        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWiseAdjustment(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var EmpIDParameter = new SqlParameter("@EmployeeId", EmpID);
            return DataContext.Database.SqlQuery<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollectionEmpWiseAdjustment @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue,@EmployeeId", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue, EmpIDParameter);


        }
    }
}
