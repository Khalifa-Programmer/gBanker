using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
using System;
using Utility;
using Utility.Exceptions;
using Utility.Enums;
using Utility.Attributes;

namespace BasicDataAccess.Data
{
    public abstract class DataAccessBase : IDisposable
    {
        #region Constructors and destructors

        public DataAccessBase()
        {
        }
      

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    this.UseTransaction = false;
                    ConnectionClose();
                    _connection = null;
                    _transaction = null;
                }
                // Note disposing has been done.
                disposed = true;

            }
        }

        ~DataAccessBase()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        #region Private Members

        private ConnectionStringSettings _dbConnectionStringSetting;
        private DbConnection _connection;
        private DbTransaction _transaction;
        private bool disposed = false;
        private bool _isConnected;
        private bool _transactionCreated;

        private DbTransaction Transaction
        {
            get { return _transaction; }
        }
        private bool Connect()
        {
            try
            {
                if (_isConnected)
                {
                    return true;
                }
                if (_connection == null)
                    _connection = Factory.CreateConnection();

                _connection.ConnectionString = ConnectionStringSetting.ConnectionString;

                if (_connection.ConnectionString != "")
                {
                    _connection.StateChange += new StateChangeEventHandler(cnn_StateChange);
                    _connection.Open();
                    _isConnected = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Connect()"));
            }
        }
        #endregion

        #region Protected Members

        protected bool UseTransaction
        {
            get;
            set;
        }    
        /// <summary>
        /// Method commiting transaction, if useTransaction-property is set.
        /// </summary>
        protected void Commit()
        {
            try
            {
                if (UseTransaction && _transaction != null)
                    _transaction.Commit();
                _transactionCreated = false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Commit()"));
            }
        }
        /// <summary>
        /// Method rollbacking transaction, if useTransaction-property is set.
        /// </summary>
        protected void Rollback()
        {
            try
            {
                if (UseTransaction)
                    _transaction.Rollback();
                _transactionCreated = false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Rollback()"));
            }
        }        
        void cnn_StateChange(object sender, StateChangeEventArgs e)
        {
            switch (e.CurrentState)
            {
                case ConnectionState.Broken:
                case ConnectionState.Closed:
                case ConnectionState.Connecting:
                    _isConnected = false; break;
                case ConnectionState.Open:
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                    _isConnected = true; break;
            }
        }
        protected DbProviderFactory Factory
        {
            get
            {
                if (_dbConnectionStringSetting != null)
                    return DbProviderFactories.GetFactory(_dbConnectionStringSetting.ProviderName);
                else
                {
                    _dbConnectionStringSetting = this.LoadConnectionStringSetting();
                    if (_dbConnectionStringSetting == null)
                        throw new DataAccessException(" No connection setting found.");
                    else
                        return DbProviderFactories.GetFactory(_dbConnectionStringSetting.ProviderName); ;
                }
            }
        }
        /// <summary>
        /// Override this property to set connectionstring
        /// </summary>
        protected ConnectionStringSettings ConnectionStringSetting
        {

            get
            {
                if (_dbConnectionStringSetting != null)
                    return _dbConnectionStringSetting;
                else
                {
                    _dbConnectionStringSetting = this.LoadConnectionStringSetting();
                    if (_dbConnectionStringSetting == null)
                        throw new DataAccessException("No connection setting found.");
                    else
                        return _dbConnectionStringSetting;
                }
            }
        }
        /// <summary>
        /// Override this method with connection string setting information. Based on this connection string setting, database operation is performed.
        /// </summary>
        /// <returns></returns>
        protected abstract ConnectionStringSettings LoadConnectionStringSetting();


        protected long SetData<TTarget>(TTarget target, DBEnums.DbTransactionOption option, string storeProcedureName) where TTarget : new()
        {
            try
            {

                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                long returnValue = this.ExecuteNonQuery(storeProcedureName, parameters.ToArray());
                return returnValue;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (BasicException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "SetData()"));
            }

        }

        protected long SetData(string storeProcedureName, params DbParameter[] parameters)
        {
            try
            {
                long returnValue = this.ExecuteNonQuery(storeProcedureName, parameters);
                return returnValue;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (BasicException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "SetData()"));
            }
        }
        /// <summary>
        /// Sets data with optional parameters...
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <param name="option"></param>
        /// <param name="storeProcedureName"></param>
        /// <param name="optionalParameters"></param>
        /// <returns></returns>
        protected long SetDataWithOptionalParameter<TTarget>(TTarget target, DBEnums.DbTransactionOption option, string storeProcedureName, params DbParameter[] optionalParameters) where TTarget : new()
        {
            try
            {
                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                if (optionalParameters != null && optionalParameters.Length > 0)
                {
                    foreach (DbParameter param in optionalParameters)
                    {
                        if (param != null)
                            parameters.Add(param);
                    }
                }

                long returnValue = this.ExecuteNonQuery(storeProcedureName, parameters.ToArray());
                return returnValue;

            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (BasicException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "SetDataWithOptionalParameter()"));
            }

        }
        /// <summary>
        /// Sets data with only optional parameters. It does not require any entity class.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <param name="option"></param>
        /// <param name="storeProcedureName"></param>
        /// <param name="optionalParameters"></param>
        /// <returns></returns>
        protected long SetDataWithOptionalParameter(DBEnums.DbTransactionOption option, string storeProcedureName, params DbParameter[] optionalParameters)
        {

            try
            {
                List<DbParameter> parameters = new List<DbParameter>();

                if (optionalParameters != null && optionalParameters.Length > 0)
                {
                    foreach (DbParameter param in optionalParameters)
                    {
                        if (param != null)
                            parameters.Add(param);
                    }
                }

                long returnValue = this.ExecuteNonQuery(storeProcedureName, parameters.ToArray());
                return returnValue;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (BasicException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "SetDataWithOptionalParameter()"));
            }

        }

        #endregion

        #region Public Members
        public void ConnectionClose()
        {
            try
            {
                if (_connection != null)
                {
                    if (_connection.State == ConnectionState.Open || _connection.State == ConnectionState.Fetching || _connection.State == ConnectionState.Executing)
                    {
                        _connection.Close();
                        _connection.Dispose();
                        _isConnected = false;
                        _transactionCreated = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "ConnectionClose()"));
            }
        }
        /// <summary>
        /// Inserts data in parent child tables, maintaining the transaction.
        /// </summary>
        /// <typeparam name="TParentType"> Parent type </typeparam>
        /// <typeparam name="TChildType"> child type</typeparam>
        /// <param name="parentStoredProcedureName"> Parent stored procedure name</param>
        /// <param name="parentTarget"> Parent object</param>
        /// <param name="childStoredProcedureName"> Child stored procedure name</param>
        /// <param name="childObjectNames"> Child objects</param>
        /// <returns> Message indicates the status of the transaction.</returns>
        public  Message InsertParentChild<TParentType, TChildType>(string parentStoredProcedureName, TParentType parentTarget, string childStoredProcedureName, IList<TChildType> childTargets, string parentReferenceParameterName)
            where TParentType : new()
            where TChildType : new()
        {
            var msg = new Message();
            bool needCommit = true;
            try
            {
                this.UseTransaction = true;  
                long returnValue = 0;
                //Parent sp calls parameter.
                List<DbParameter> parentParameters = this.CreateDbParmetersFromEntity(parentTarget);

                returnValue = this.SetData(parentStoredProcedureName, parentParameters.ToArray());
                long outputParamValue = -1;

                //Get the output parameter value for 
                foreach (DbParameter p in parentParameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //Output parameter type should be long..
                        outputParamValue = long.Parse(p.Value.ToString());
                    }
                }
                //Executes child stored procedure.
                foreach (TChildType target in childTargets)
                {
                    List<DbParameter> childParameters = this.CreateDbParmetersFromEntity(target);
                    //Update the child parameter reference param value.
                    foreach (DbParameter p in childParameters)
                    {
                        if (p.ParameterName == parentReferenceParameterName)
                        {
                            p.Value = outputParamValue;
                            break;
                        }
                    }
                    returnValue = this.SetData(childStoredProcedureName, childParameters.ToArray());
                }

                msg.Code = Constants.OperationStatus.SUCCESS;
                msg.Source = string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Insert()");
                msg.Text = Constants.MessageTexts.Insert_Success;
                //Add return value in the additional message list.
                msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());               
            }
            catch (DataAccessException ex) { needCommit = false; this.Rollback(); throw ex; }
            catch (BasicException ex) { needCommit = false; this.Rollback(); throw ex; }
            catch (Exception ex) { needCommit = false; this.Rollback(); throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Insert()")); }
            if (needCommit)
                this.Commit();
            return msg;
        }

        /// <summary>
        /// Inserts and object in the  database. This methods automatically creates stored procedure parameters based on the entity provided.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Target object type that will be inserted.
        /// </typeparam>
        /// <param name="target">
        /// Target object  that will be inserted.
        /// </param>
        /// <param name="storedProcedureName">
        /// Stored Procedure name that will be executed.
        /// </param>       
        /// <returns>
        /// A message object containing the details about the operation. Message contains code, response text, return value, output parameter value etc.
        /// </returns>
        public virtual Message Insert<TTarget>(TTarget target, string storedProcedureName)
        {
            try
            {
                var msg = new Message();
                long returnValue = 0;

                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);               

                returnValue = this.SetData(storedProcedureName, parameters.ToArray());

                msg.Code = Constants.OperationStatus.SUCCESS;
                msg.Source = this.GetType().FullName;
                msg.Text = Constants.MessageTexts.Insert_Success;
                //Add return value in the additional message list.
                msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());
                foreach (DbParameter p in parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                    {

                        msg.AdditionalMessages.Add(p.ParameterName, p.Value);
                    }
                }
                return msg;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) { throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Insert()")); }
        }

        /// <summary>
        /// Inserts and object in the database. This methods automatically creates stored procedure parameters based on the entity provided.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Target object type that will be inserted.
        /// </typeparam>
        /// <param name="target">
        /// A list of Target objects  that will be inserted.
        /// </param>
        /// <param name="storedProcedureName">
        /// Stored Procedure name that will be executed.
        /// </param>       
        /// <returns>
        /// A list of message objects containing the details about the operation. Message contains code, response text, return value, output parameter value etc.
        /// </returns>
        public virtual List<Message> Insert<TTarget>(IList<TTarget> targets, string storedProcedureName)
        {
            var msgLists = new List<Message>();
            long returnValue = 0;

            //Use transaction to insert multiple rows.

            this.UseTransaction = true;
            bool needCommit = true;
            foreach (TTarget target in targets)
            {
                var msg = new Message();
                try
                {

                    List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                    returnValue = this.SetData(storedProcedureName, parameters.ToArray());

                    msg.Code = Constants.OperationStatus.SUCCESS;
                    msg.Source = this.GetType().FullName;
                    msg.Text = Constants.MessageTexts.Insert_Success;
                    //Add return value in the additional message list.
                    msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());
                    foreach (DbParameter p in parameters)
                    {
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                        {
                            msg.AdditionalMessages.Add(p.ParameterName, p.Value);
                        }
                    }
                    msgLists.Add(msg);
                }
                catch (DataAccessException ex) { needCommit = false; this.Rollback(); break; throw ex; }
                catch (BasicException ex) { needCommit = false; this.Rollback(); break; throw ex; }
                catch (Exception ex) { needCommit = false; this.Rollback(); break; throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Insert()")); }

               
            }
            if (needCommit)
                this.Commit();
            return msgLists;

        }
        /// <summary>
        /// Updates an object in the  database. This methods automatically creates stored procedure parameters based on the entity provided.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Target object type that will be updated.
        /// </typeparam>
        /// <param name="target">
        /// Target object  that will be updated.
        /// </param>
        /// <param name="storedProcedureName">
        /// Stored Procedure name that will be executed.
        /// </param>        
        /// <returns>
        /// A message object containing the details about the operation. Message contains code, response text, return value, output parameter value etc.
        /// </returns>
        public virtual Message Update<TTarget>(TTarget target, string storedProcedureName)
        {
            try
            {
                var msg = new Message();
                long returnValue = 0;

                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);
                returnValue = this.SetData(storedProcedureName, parameters.ToArray());

                msg.Code = Constants.OperationStatus.SUCCESS;
                msg.Source = this.GetType().FullName;
                msg.Text = Constants.MessageTexts.Update_Success;
                //Add return value in the additional message list.
                msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());
                foreach (DbParameter p in parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                    {
                        msg.AdditionalMessages.Add(p.ParameterName, p.Value);
                    }
                }
                return msg;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) { throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Update()")); }

        }

        /// <summary>
        /// Updates an object in the  database. This methods automatically creates stored procedure parameters based on the entity provided.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Target object type that will be updated.
        /// </typeparam>
        /// <param name="target">
        /// Target object  that will be updated.
        /// </param>
        /// <param name="storedProcedureName">
        /// Stored Procedure name that will be executed.
        /// </param>        
        /// <returns>
        /// A message object containing the details about the operation. Message contains code, response text, return value, output parameter value etc.
        /// </returns>
        public virtual List<Message> Update<TTarget>(IList<TTarget> targets, string storedProcedureName)
        {
            var msgLists = new List<Message>();
            long returnValue = 0;

            //Use transaction to update multiple rows.

            this.UseTransaction = true;
            bool needCommit = true;
            foreach (TTarget target in targets)
            {
                var msg = new Message();
                try
                {

                    List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                    returnValue = this.SetData(storedProcedureName, parameters.ToArray());

                    msg.Code = Constants.OperationStatus.SUCCESS;
                    msg.Source = this.GetType().FullName;
                    msg.Text = Constants.MessageTexts.Insert_Success;
                    //Add return value in the additional message list.
                    msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());
                    foreach (DbParameter p in parameters)
                    {
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                        {
                            msg.AdditionalMessages.Add(p.ParameterName, p.Value);
                        }
                    }
                    msgLists.Add(msg);
                }
                catch (DataAccessException ex) { needCommit = false; this.Rollback(); break; throw ex; }
                catch (BasicException ex) { needCommit = false; this.Rollback(); break; throw ex; }
                catch (Exception ex) { needCommit = false; this.Rollback(); break; throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Update()")); }

            }
            if (needCommit)
                this.Commit();
            return msgLists;
        }

        /// <summary>
        /// Deletes an object from the  database. This methods automatically creates stored procedure parameters based on the entity provided.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Target object type that will be deleted.
        /// </typeparam>
        /// <param name="target">
        /// Target object  that will be deleted.
        /// </param>
        /// <param name="storedProcedureName">
        /// Stored Procedure name that will be executed.
        /// </param>       
        /// <returns>
        /// A message object containing the details about the operation. Message contains code, response text, return value, output parameter value etc.
        /// </returns>
        public virtual Message Delete<TTarget>(TTarget target, string storedProcedureName)
        {
            try
            {
                var msg = new Message();
                long returnValue = 0;

                List<DbParameter> parameters = this.CreateDeleteParmetersFromEntity(target);
                returnValue = this.SetData(storedProcedureName, parameters.ToArray());

                msg.Code = Constants.OperationStatus.SUCCESS;
                msg.Source = this.GetType().FullName;
                msg.Text = Constants.MessageTexts.Delete_Success;
                //Add return value in the additional message list.
                msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());
                //Add output parameter values
                foreach (DbParameter p in parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                    {
                        msg.AdditionalMessages.Add(p.ParameterName, p.Value);
                    }
                }
                return msg;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) {  throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Delete()")); }

        } 

        /// <summary>
        /// Deletes an object from the  database. This methods automatically creates stored procedure parameters based on the entity provided.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Target object type that will be deleted.
        /// </typeparam>
        /// <param name="target">
        /// A list of Target objects  that will be deleted.
        /// </param>
        /// <param name="storedProcedureName">
        /// Stored Procedure name that will be executed.
        /// </param>       
        /// <returns>
        /// A list of message object containing the details about the operation. Message contains code, response text, return value, output parameter value etc.
        /// </returns>
        public virtual List<Message> Delete<TTarget>(IList<TTarget> targets, string storedProcedureName)
        {
            var msgLists = new List<Message>();
            long returnValue = 0;

            //Use transaction to update multiple rows.

            this.UseTransaction = true;
            bool needCommit = true;
            foreach (TTarget target in targets)
            {
                var msg = new Message();
                try
                {

                    List<DbParameter> parameters = this.CreateDeleteParmetersFromEntity(target);

                    returnValue = this.SetData(storedProcedureName, parameters.ToArray());

                    msg.Code = Constants.OperationStatus.SUCCESS;
                    msg.Source = this.GetType().FullName;
                    msg.Text = Constants.MessageTexts.Insert_Success;
                    //Add return value in the additional message list.
                    msg.AdditionalMessages.Add(Constants.MessageKey.ReturnValue, returnValue.ToString());
                    foreach (DbParameter p in parameters)
                    {
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                        {
                            msg.AdditionalMessages.Add(p.ParameterName, p.Value);
                        }
                    }
                    msgLists.Add(msg);
                }
                catch (DataAccessException ex) { 
                    needCommit = false;
                    this.Rollback();
                    break; //No need to preceed for next delete... 
                    throw ex;                    
                }
                catch (BasicException ex) {
                     needCommit = false;
                    this.Rollback();
                    break; //No need to preceed for next delete...
                    throw ex;
                }
                catch (Exception ex)
                {                  

                    needCommit = false;
                    this.Rollback();
                    break;
                    throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "Delete()"));                   
                }
              
            }
            if (needCommit)
                this.Commit();
            return msgLists;
        }
        #endregion

        #region parameters

        protected DbParameter CreateParameter(string parameterName, DbType parameterType, int parameterSize)
        {
            DbParameter parameter = Factory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = parameterType;
            parameter.Size = parameterSize;
            return parameter;
        }

        protected DbParameter CreateParameter(string parameterName, object parameterValue)
        {
            DbParameter parameter = Factory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
        protected DbParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection direction)
        {
            DbParameter parameter = Factory.CreateParameter();
            parameter.Direction = direction;
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
        protected DbParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection direction, int size)
        {
            DbParameter parameter = Factory.CreateParameter();
            parameter.Direction = direction;
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.Size = size;
            return parameter;
        }
        protected DbParameter CreateParameter(string parameterName)
        {
            DbParameter parameter = Factory.CreateParameter();
            parameter.ParameterName = parameterName;
            return parameter;
        }

        #endregion

        #region Command Handlers
        protected DbCommand CreateCommand(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            try
            {
                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server. Connectionstring is not specified.");
                    }
                }

                DbCommand command = Factory.CreateCommand();
                command.Connection = _connection;
                command.CommandType = commandType;
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);

                return command;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "CreateCommand()"));
            }
        }
        protected object ExecuteScalar(string storeProcedure, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server. Connectionstring is not specified.");
                    }
                }
                command = Factory.CreateCommand();
                command.Connection = _connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);

                if (UseTransaction)
                {
                    if (!_transactionCreated)
                    {
                        _transaction = _connection.BeginTransaction();
                        _transactionCreated = true;
                    }
                    command.Transaction = _transaction;
                }

                object returnValue = command.ExecuteScalar();
                return returnValue;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "ExecuteScalar()"));
            }
            finally
            {
                //Necessary to execute multiple calls to avoid the exception: System.ArgumentException: The SqlParameter is already contained by another SqlParameterCollection
                if (command != null)
                    command.Parameters.Clear();

            }

        }
        protected int ExecuteNonQuery(string storeProcedure, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server. Connectionstring is not specified.");
                    }
                }

                command = Factory.CreateCommand();
                command.Connection = _connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);


                if (UseTransaction)
                {
                    if (!_transactionCreated)
                    {
                        _transaction = _connection.BeginTransaction();
                        _transactionCreated = true;
                    }
                    command.Transaction = _transaction;
                }

                int returnValue = command.ExecuteNonQuery();
                return returnValue;

            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "ExecuteNonQuery()"));
            }
            finally
            {
                //Necessary to execute multiple calls to avoid the exception: System.ArgumentException: The SqlParameter is already contained by another SqlParameterCollection
                if (command != null)
                    command.Parameters.Clear();
            }

        }
        protected IDataReader ExecuteReader(string storeProcedure, params DbParameter[] parameters)
        {
            try
            {

                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server. Connectionstring is not specified.");
                    }
                }

                DbCommand command = Factory.CreateCommand();
                command.Connection = _connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "ExecuteReader()"));
            }

        }

        /// <summary>
        /// Used in creating DbParameter array from a target entity type object.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        protected List<DbParameter> CreateDeleteParmetersFromEntity<TTarget>(TTarget target)
        {
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
                //Get all properties for the given type.
                List<PropertyInfo> props = new List<PropertyInfo>(DataWrapper.GetSourceProperties(typeof(TTarget)));
                foreach (PropertyInfo p in props)
                {
                    //default property name is column name for the parameter.
                    string columnName = p.Name;
                    object value = p.GetValue(target, null);

                    object[] attr = p.GetCustomAttributes(true);

                    foreach (object o in attr)
                    {
                        //If DbConfiguration attrigute is specified.
                        DBConfigurationAttribute dbAttr = o as DBConfigurationAttribute;
                        if (dbAttr != null && !dbAttr.IsChild)
                        {
                            if (dbAttr.IsPrimaryKey || dbAttr.IncludeInDeleteParameter)
                            {
                                //default direction.
                                ParameterDirection direction = dbAttr.Direction;
                                //if (!string.IsNullOrEmpty(dbAttr.MappingColumnName))
                                //    columnName = dbAttr.MappingColumnName;

                                parameters.Add(CreateParameter(columnName, value, direction));

                            }
                            break;
                        }
                    }
                }
                return parameters;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "CreateDeleteParmetersFromEntity()"));
            }
        }

        /// <summary>
        /// Used in creating DbParameter array from a target entity type object.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        protected List<DbParameter> CreateDbParmetersFromEntity<TTarget>(TTarget target)
        {
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
                //Get all properties for the given type.
                List<PropertyInfo> props = new List<PropertyInfo>(DataWrapper.GetSourceProperties(typeof(TTarget)));

                //for anonymous type, Object is passed which does not have any properties. So work with the anonymous type itselt.
                if (props.Count == 0)
                    props = new List<PropertyInfo>(DataWrapper.GetSourceProperties(target.GetType()));

                foreach (PropertyInfo p in props)
                {
                    //default property name is column name for the parameter.
                    string columnName = p.Name;
                    object value = p.GetValue(target, null);

                    object[] attr = p.GetCustomAttributes(true);
                    bool added = false;
                    foreach (object o in attr)
                    {
                        //If DbConfiguration attrigute is specified.
                        DBConfigurationAttribute dbAttr = o as DBConfigurationAttribute;
                        if (dbAttr != null)
                        {
                            if (!dbAttr.IsChild && !dbAttr.ExcludeFromInputParam)
                            {
                                //default direction.
                                ParameterDirection direction = dbAttr.Direction;

                                var param = CreateParameter(columnName, value, direction);

                                if (param.Value == null)
                                {
                                    param.Value = DBNull.Value;
                                }
                                parameters.Add(param);
                                added = true;
                                break;
                            }
                            else
                            {
                                added = true;
                                break;
                            }
                        }
                    }
                    //If still not added in the parameter list, then add as the default input parameter type.
                    if (!added)
                    {
                        if (p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                            parameters.Add(CreateParameter(columnName, value));
                    }
                }
                return parameters;
            }
            catch (Exception ex) { throw new BasicException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "CreateDbParmetersFromEntity()")); }
        }

        #endregion

        #region Common Data Access method

        /// <summary>
        /// Returns a list of entities executing a select stored procedures for a specific type.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Return object types.
        /// </typeparam>
        /// <param name="storedProcedureName">
        /// A Select stored procedure name that returns the data to fill the object.
        /// </param>
        /// <param name="startRowIndex">
        /// If paging is enabled, the start row index of the row collection to return.
        /// </param>
        /// <param name="pageSize">
        /// If paging is enabled, it specifies the number of rows that will be returned per page.
        /// </param>
        /// <param name="sortExpression">
        /// If sorting is enable, this parameter specifies the sorting column name.
        /// </param>
        /// <param name="filterExpression">
        /// If filtering is enable, this parameter specifies the filter expression that will be used to filter data.
        /// </param>
        /// <param name="option">
        ///  An option that specifies whether the select operation will contain additional information like paging, sorting, filter etc.
        /// This parameter expects that the appropriate parameter values are supplied in the startRowIndex, pageSize, sortExpression and filterExpression.
        /// </param>
        /// <returns>
        /// A list of target objects.
        /// </returns>
        /// 
        public List<TTarget> GetData<TTarget>(string storedProcedureName, int? startRowIndex, int? pageSize, string sortExpression, string filterExpression, DBEnums.DataLoadingOption option, out int totalRowCount) where TTarget : new()
        {
            try
            {
                List<TTarget> entities;
                DbParameter totalRowCountParam = this.CreateParameter(Constants.CommonDbParameterNames.TOTAL_ROW_COUNT, 0);
                totalRowCountParam.Direction = ParameterDirection.Output;

                using (IDataReader reader = this.ExecuteReader(storedProcedureName,
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.START_ROW_INDEX,
                                                                                                startRowIndex),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.PAGE_SIZE, pageSize),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.SORT_EXPRESSION,
                                                                                                sortExpression),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.FILTER_EXPRESSION,
                                                                                                filterExpression),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.OPTION, (int)option),
                                                                      totalRowCountParam))
                {
                    entities = DataWrapper.MapReader<TTarget>(reader);
                }

                totalRowCount = Convert.ToInt32(totalRowCountParam.Value);

                return entities;
            }
            catch (DataAccessException ex){ throw ex; }
            catch (Exception ex){ throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()")); }
        }
        public List<TTarget> GetData<TTarget>(string storedProcedureName) where TTarget : new()
        {

            try
            {
                List<TTarget> entities;
                DbParameter totalRowCountParam = this.CreateParameter(Constants.CommonDbParameterNames.TOTAL_ROW_COUNT, 0);
                //totalRowCountParam.Direction = ParameterDirection.Output;

                using (IDataReader reader = this.ExecuteReader(storedProcedureName))
                {
                    entities = DataWrapper.MapReader<TTarget>(reader);
                }
                
                return entities;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (Exception ex)  {  throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()"));  }
        }
        /// <summary>
        /// Gets data in a dataset. 
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="optionalParams"></param>
        /// <returns></returns>
        public object GetScalarResult<TParamOType>(string storeProcedureName, TParamOType target) where TParamOType : class
        {
            try
            {

                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server.Connectionstring is not specified.");
                    }
                }


                DbCommand command = Factory.CreateCommand();

                command.Connection = _connection;
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedureName;
                command.Parameters.Clear();

                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                var obj = command.ExecuteScalar();

                return obj;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) { throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()")); }
            finally
            {

                _connection.Close();
                _connection.Dispose();

            }
        }
        /// <summary>
        /// Gets data in a dataset. 
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="optionalParams"></param>
        /// <returns></returns>
        public DataSet GetDataOnDateset<TParamOType>(string storeProcedureName, TParamOType target) where TParamOType : class
        {
            try
            {

                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server.Connectionstring is not specified.");
                    }
                }

                DataSet ds = new DataSet();
                DbCommand command = Factory.CreateCommand();

                command.Connection = _connection;
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedureName;
                command.Parameters.Clear();

                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                DbDataAdapter da = Factory.CreateDataAdapter();
                da.SelectCommand = command;
                da.Fill(ds);

                return ds;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) { throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()")); }
            finally
            {
               
                _connection.Close();
                _connection.Dispose();
                
            }
        }
        public int ExecuteNonQuery<TParamOType>(string storeProcedure, TParamOType target) where TParamOType : class
        {
            DbCommand command = null;
            try
            {
                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server. Connectionstring is not specified.");
                    }
                }
                command = Factory.CreateCommand();
                command.Connection = _connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedure;
                command.Parameters.Clear();
                //command.Parameters.AddRange(parameters);
                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                if (UseTransaction)
                {
                    if (!_transactionCreated)
                    {
                        _transaction = _connection.BeginTransaction();
                        _transactionCreated = true;
                    }
                    command.Transaction = _transaction;
                }
                int returnValue = command.ExecuteNonQuery();
                return returnValue;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "ExecuteNonQuery()"));
            }
            finally
            {
                //Necessary to execute multiple calls to avoid the exception: System.ArgumentException: The SqlParameter is already contained by another SqlParameterCollection
                if (command != null)
                    command.Parameters.Clear();
            }

        }
        /// <summary>
        /// Gets data in a list. Get the param as an object and returns another of object. 
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="optionalParams"></param>
        /// <returns></returns>
        public IList<TReturnType> GetData<TReturnType, TParamType>(string storeProcedureName, TParamType paramObject) 
            where TParamType : class
            where TReturnType : new()
        {
            try
            {

                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server.Connectionstring is not specified.");
                    }
                }              

                List<DbParameter> parameters = this.CreateDbParmetersFromEntity(paramObject);


                List<TReturnType> entities;

                using (IDataReader reader = this.ExecuteReader(storeProcedureName, parameters.ToArray()))
                {
                    entities = DataWrapper.MapReader<TReturnType>(reader);
                }               

                return entities;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) { throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()")); }
        }
        /// <summary>
        /// Returns a list of entities executing a select stored procedures for a specific type.
        /// </summary>
        /// <typeparam name="TTarget">
        /// Return object types.
        /// </typeparam>
        /// <param name="storedProcedureName">
        /// A Select stored procedure name that returns the data to fill the object.
        /// </param>
        /// <param name="startRowIndex">
        /// If paging is enabled, the start row index of the row collection to return.
        /// </param>
        /// <param name="pageSize">
        /// If paging is enabled, it specifies the number of rows that will be returned per page.
        /// </param>
        /// <param name="sortExpression">
        /// If sorting is enable, this parameter specifies the sorting column name.
        /// </param>
        /// <param name="filterExpression">
        /// If filtering is enable, this parameter specifies the filter expression that will be used to filter data.
        /// </param>
        /// <param name="option">
        ///  An option that specifies whether the select operation will contain additional information like paging, sorting, filter etc.
        /// This parameter expects that the appropriate parameter values are supplied in the startRowIndex, pageSize, sortExpression and filterExpression.
        /// </param>
        /// <returns>
        /// A list of target objects.
        /// </returns>
        /// 
        public List<TTarget> GetData<TTarget>(string storedProcedureName, int? startRowIndex, int? pageSize, string sortExpression, DBEnums.DataLoadingOption option, out int totalRowCount) where TTarget : new()
        {
            try
            {
                List<TTarget> entities;
                DbParameter totalRowCountParam = this.CreateParameter(Constants.CommonDbParameterNames.TOTAL_COUNT, 0);
                totalRowCountParam.Direction = ParameterDirection.Output;

                using (IDataReader reader = this.ExecuteReader(storedProcedureName,
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.PAGE_NO,
                                                                                                startRowIndex),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.PAGE_SIZE, pageSize),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.SORT_EXPRESSION,
                                                                                                sortExpression),
                                                                                                totalRowCountParam))
                {
                    entities = DataWrapper.MapReader<TTarget>(reader);
                }

                totalRowCount = Convert.ToInt32(totalRowCountParam.Value);

                return entities;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()"));
            }
        }
        public List<TTarget> GetData<TTarget>(string storedProcedureName, int? startRowIndex, int? pageSize, string sortExpression, string filterExpression, out int totalRowCount) where TTarget : new()
        {
            try
            {

                List<TTarget> entities;
                DbParameter totalRowCountParam = this.CreateParameter(Constants.CommonDbParameterNames.TOTAL_COUNT, 0);
                totalRowCountParam.Direction = ParameterDirection.Output;

                using (IDataReader reader = this.ExecuteReader(storedProcedureName,
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.PAGE_NO,
                                                                                                startRowIndex),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.PAGE_SIZE, pageSize),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.SORT_EXPRESSION,
                                                                                                sortExpression),
                                                                     this.CreateParameter(Constants.CommonDbParameterNames.FILTER_EXPRESSION,
                                                                     filterExpression),
                                                                                                totalRowCountParam))
                {
                    entities = DataWrapper.MapReader<TTarget>(reader);
                }

                totalRowCount = Convert.ToInt32(totalRowCountParam.Value);

                return entities;
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }           
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()"));
            }
        }
        /// <summary>
        /// Gets data with optional parameters.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="optionalParameters"></param>
        /// <returns></returns>
        public List<TTarget> GetDataWithOptionalParameters<TTarget>(string storedProcedureName, params DbParameter[] optionalParameters) where TTarget : new()
        {
            try
            {
                List<TTarget> entities;
                using (IDataReader reader = this.ExecuteReader(storedProcedureName, optionalParameters))
                {
                    entities = DataWrapper.MapReader<TTarget>(reader);
                }
                return entities;

            }
            catch (DataAccessException ex)
            {
                throw ex;
            }

            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetDataWithOptionalParameters()"));
            }
        }
        /// <summary>
        /// Makes an CRUD operation on the database based on provided object and transaction option.
        /// </summary>
        /// <typeparam name="TTarget">
        /// A target object type which holds the data to make CRUD operation.
        /// </typeparam>
        /// <param name="target">
        /// A target object which holds the data to make CRUD operation.
        /// </param>
        /// <param name="option">
        /// Specifies a database operation, Insert, Update and Delete.
        /// </param>
        /// <param name="storeProcedureName">
        /// Stored procedure name which will be executed for Insert, Update and Delete operation.
        /// </param>
        /// <returns>
        /// Returns a value from a parameter value named 'ReturnKey'.
        /// </returns>
        /// 
        public DataSet GetDataOnDatesetWithoutParam(string storeProcedureName)
        {
            try
            {

                if (!_isConnected)
                {
                    if (!Connect())
                    {
                        throw new DataAccessException("Cannot connect with database server.Connectionstring is not specified.");
                    }
                }

                DataSet ds = new DataSet();
                DbCommand command = Factory.CreateCommand();

                command.Connection = _connection;
                command.CommandTimeout = 300;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedureName;
                command.Parameters.Clear();

                //List<DbParameter> parameters = this.CreateDbParmetersFromEntity(target);

                //if (parameters != null && parameters.Count > 0)
                //{
                //    command.Parameters.AddRange(parameters.ToArray());
                //}
                DbDataAdapter da = Factory.CreateDataAdapter();
                da.SelectCommand = command;
                da.Fill(ds);

                return ds;
            }
            catch (DataAccessException ex) { throw ex; }
            catch (BasicException ex) { throw ex; }
            catch (Exception ex) { throw new DataAccessException(ex.Message, ex, string.Format("Type = '{0}, Method = '{1}'", this.GetType().FullName, "GetData()")); }
        }
        #endregion

    }
}