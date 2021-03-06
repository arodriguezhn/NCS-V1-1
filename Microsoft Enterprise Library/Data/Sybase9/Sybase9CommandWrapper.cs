//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright � Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright � Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using iAnywhere.Data.UltraLite;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sybase9
{
    /// <summary>
    /// <para>Represents a UL statement or stored procedure to execute against a UL Server database.</para>
    /// </summary>   
    public class Sybase9CommandWrapper : DBCommandWrapper
    {
        private ULCommand command;
        private int rowsAffected;
        private object[] parameterValues;
        private bool needsParameters = false;
        private char parameterToken;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="Sybase9CommandWrapper"/> class with the text of a query and the command type.</para>
        /// </summary>
        /// <param name="commandText"><para>The stored procedure name or UL sting the command represents.</para></param>
        /// <param name="commandType"><para>One of the <see crer="CommandType"/> values.</para></param>
        /// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
        internal Sybase9CommandWrapper(string commandText, CommandType commandType, char parameterToken)
        {
            this.parameterToken = parameterToken;
            this.command = CreateCommand(commandText, commandType);
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="Sybase9CommandWrapper"/> class with the text of a query the command type, and the parameter values.</para>
        /// </summary>        
        /// <param name="commandText"><para>The stored procedure name or UL sting the command represents.</para></param>
        /// <param name="commandType"><para>One of the <see crer="CommandType"/> values.</para></param>
        /// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
        /// <param name="parameterValues"><para>The parameter values to assign in positional order.</para></param>
        internal Sybase9CommandWrapper(string commandText, CommandType commandType, char parameterToken, object[] parameterValues) : this(commandText, commandType, parameterToken)
        {
            //this.command = CreateCommand(commandText, commandType);
            this.parameterValues = parameterValues;
            if (commandType == CommandType.StoredProcedure)
            {
                this.needsParameters = true;
            }
        }

        /// <summary>
        /// <para>Gets the underlying <see cref="IDbCommand"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The underlying <see cref="IDbCommand"/>. The defaULt is <see langword="nULl"/>.</para>
        /// </value>
        /// <remarks>
        /// <para>This command is a <see cref="ULCommand"/></para>        
        /// </remarks>        
        /// <seealso cref="ULCommand"/>
        public override IDbCommand Command
        {
            get { return this.command; }
        }

        /// <summary>
        /// <para>Gets or sets the rows affected by this command.</para>
        /// </summary>
        /// <value>
        /// <para>The rows affected by this command.</para>
        /// </value>
        public override int RowsAffected
        {
            get { return this.rowsAffected; }
            set { this.rowsAffected = value; }
        }

        /// <summary>
        /// <para>Gets or sets the wait time before terminating the attempt to execute a command and generating an error.</para>
        /// </summary>
        /// <value>
        /// <para>The wait time before terminating the attempt to execute a command and generating an error.</para>
        /// </value>
        public override int CommandTimeout
        {
            get { return this.command.CommandTimeout; }
            set { this.command.CommandTimeout = value; }
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nULlable"><para>Avalue indicating whether the parameter accepts nULl values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>       
        public override void AddParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nULlable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, size, direction, nULlable, precision, scale, sourceColumn, sourceVersion, value));
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="ULType"><para>One of the <see cref="ULDbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nULlable"><para>Avalue indicating whether the parameter accepts nULl values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddParameter(string name, ULDbType ULType, int size, ParameterDirection direction, bool nULlable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            ULParameter param = CreateParameter(name, DbType.String, size, direction, nULlable, precision, scale, sourceColumn, sourceVersion, value);
            param.ULDbType = ULType;
            this.command.Parameters.Add(param);
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public override void AddParameter(string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            ULParameter param = CreateParameter(name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
            this.command.Parameters.Add(param);
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command set as <see cref="ParameterDirection"/> value of Output.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public override void AddOutParameter(string name, DbType dbType, int size)
        {
            AddParameter(name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

       /// <summary>
       /// <para>Does nothing in SQL Server.  Cursors are not used.  Only here for compliance with DBCommandWrapper</para>
       /// </summary>
       /// <param name="CursorName"><para>The name of the cursor</para></param>
       public override void AddCursorOutParameter(string CursorName)
       {
          // Do Nothing
          return;
       }


        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command set as <see cref="ParameterDirection"/> value of Input.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object mULtiple times with different values.</para>
        /// </remarks>        
        public override void AddInParameter(string name, DbType dbType)
        {
            AddParameter(name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command set as <see cref="ParameterDirection"/> value of Input.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public override void AddInParameter(string name, DbType dbType, object value)
        {
            AddParameter(name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="ULParameter"/> object to the command set as <see cref="ParameterDirection"/> value of Input.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public override void AddInParameter(string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            AddParameter(name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, DBNull.Value);
        }

        /// <summary>
        /// <para>Returns the value of the parameter for the given <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter to get the value.</para></param>
        /// <returns><para>The value of the parameter.</para></returns>
        public override object GetParameterValue(string name)
        {
            return this.command.Parameters[BuildParameterName(name)].Value;
        }

        /// <summary>
        /// <para>Sets the value of a parameter for the given <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter to set the value.</para></param>
        /// <param name="value"><para>The new value of the parameter.</para></param>
        public override void SetParameterValue(string name, object value)
        {
            this.command.Parameters[BuildParameterName(name)].Value = (value == null) ? DBNull.Value : value;
        }

        /// <summary>
        /// <para>Clean up resources.</para>
        /// </summary>
        public override void Dispose()
        {
            this.command.Dispose();
        }

        /// <summary>
        /// <para>Dicover the parameters for a stored procedure using a separate connection and command.</para>
        /// </summary>
        /// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
        protected override void DoDiscoverParameters(char parameterToken)
        {
//            this.parameterToken = parameterToken;
//            using (ULCommand newCommand = CreateNewCommandAndConnectionForDiscovery())
//            {
//                ULCommandBuilder.DeriveParameters(newCommand);
//
//                foreach (IDataParameter parameter in newCommand.Parameters)
//                {
//                    IDataParameter cloneParameter = (IDataParameter)((ICloneable)parameter).Clone();
//                    cloneParameter.ParameterName = BuildParameterName(cloneParameter.ParameterName);
//                    this.command.Parameters.Add(cloneParameter);
//                }
//                newCommand.Connection.Close();
//            }
        }

        /// <summary>
        /// <para>Assign the values provided by a user to the command parameters discovered in positional order.</para>
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <para>The number of parameters does not match number of values for stored procedure.</para>
        /// </exception>
        protected override void DoAssignParameterValues()
        {
            if (SameNumberOfParametersAndValues() == false)
            {
                throw new InvalidOperationException(SR.ExceptionMessageParameterMatchFailure);
            }

            int returnParameter = 1;
            for (int i = 0; i < this.parameterValues.Length; i++)
            {
                IDataParameter parameter = this.command.Parameters[i + returnParameter];

                // There used to be code here that checked to see if the parameter was input or input/output
                // before assigning the value to it. We took it out because of an operational bug with
                // deriving parameters for a stored procedure. It turns out that output parameters are set
                // to input/output after discovery, so any direction checking was unneeded. ShoULd it ever
                // be needed, it shoULd go here, and check that a parameter is input or input/output before
                // assigning a value to it.
                SetParameterValue(parameter.ParameterName, this.parameterValues[i]);
            }
        }

        /// <summary>
        /// <para>Determine if a stored procedure is using parameter discovery.</para>
        /// </summary>
        /// <returns>
        /// <para><see langword="true"/> if further preparation is needed.</para>
        /// </returns>
        protected override bool DoIsFurtherPreparationNeeded()
        {
            return this.needsParameters;
        }

        /// <devdoc>
        /// Create a parameter.
        /// </devdoc>        
        private ULParameter CreateParameter(string name, DbType type, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            ULParameter param = this.command.CreateParameter();
            param.ParameterName = BuildParameterName(name);
            param.DbType = type;
            param.Size = size;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.Precision = precision;
            param.Scale = scale;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
            param.Value = (value == null) ? DBNull.Value : value;

            return param;
        }

        private bool SameNumberOfParametersAndValues()
        {
            int returnParameterCount = 1;
            int numberOfParametersToStoredProcedure = this.command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = this.parameterValues.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <devdoc>
        /// Discovery has to be done on its own connection to allow for the case of the
        /// connection being used being enrolled in a transaction. The ULCommandBuilder.DeriveParameters
        /// method creates a new ULCommand internally to communicate to the database, and it
        /// reuses the same connection that is passed in on the command object. If this command
        /// object has a connection that is enrolled in a transaction, the DeriveParameters method does not
        /// honor that transaction, and the call fails. To avoid this, create your own connection and
        /// command, and use them. 
        /// 
        /// You then have to clone each of the IDataParameter objects before it can be transferred to 
        /// the original command, or another exception is thrown.
        /// </devdoc>
        private ULCommand CreateNewCommandAndConnectionForDiscovery()
        {
            ULConnection clonedConnection = (ULConnection)((ICloneable)((IDbConnection)this.command.Connection)).Clone();
            clonedConnection.Open();
            ULCommand newCommand = CreateCommand(this.command.CommandText, this.command.CommandType);
            newCommand.Connection = clonedConnection;

            return newCommand;
        }

        private static ULCommand CreateCommand(string commandText, CommandType commandType)
        {
            ULCommand newCommand = new ULCommand();
            newCommand.CommandText = commandText;
            newCommand.CommandType = commandType;

            return newCommand;
        }

        private string BuildParameterName(string name)
        {
            //System.Diagnostics.Debug.Assert(parameterToken != 0x0000);
            if (name[0] != this.parameterToken)
            {
                return name.Insert(0, new string(this.parameterToken, 1));
            }
            return name;
        }
    }
}