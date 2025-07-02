using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ISmsConfigService : IServiceBase<SmsConfig>
    {
        IEnumerable<ValidationResult> IsValidSmsConfig(SmsConfig note);
        SmsConfig GetByOrgID(int orgId);
        string Encrypt(string clearText);
        string Decrypt(string cipherText);
    }
    public class SmsConfigService : ISmsConfigService
    {
        private readonly ISmsConfigRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SmsConfigService(ISmsConfigRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SmsConfig> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.SmsID);
            return entities;
        }

        public SmsConfig GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public SmsConfig GetByOrgID(int orgId)
        {
            var entity = repository.Get(p => p.OrgID == orgId);
            return entity;
        }

        //public string GetNoteDetail(int note_Id)
        //{
        //    var result = repository.GetById(note_Id);
        //    var note = "";
        //    if (result != null)
        //        note = result.NoteNo.ToString(); //+ " - " + result.NoteName;
        //    return note;
        //}

        public SmsConfig Create(SmsConfig objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SmsConfig objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        IEnumerable<ValidationResult> ISmsConfigService.IsValidSmsConfig(SmsConfig note)
        {
            var entity = repository.Get(p => p.SmsID == note.SmsID);
            if (entity != null)
            {
                yield return new ValidationResult("SmsID", "Duplicate SMS Credential.");

            }
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }



        public SmsConfig GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsConfig> GetMany(Expression<Func<SmsConfig, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
