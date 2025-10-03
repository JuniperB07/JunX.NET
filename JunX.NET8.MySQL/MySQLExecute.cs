using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides a modular wrapper for executing MySQL commands, managing connections, and retrieving data using <see cref="MySqlDataReader"/>.
    /// </summary>
    /// <remarks>
    /// This class encapsulates connection handling, command execution, and reader lifecycle management for MySQL operations.  
    /// It supports both default and parameterized initialization, and exposes methods for opening, closing, resetting, and disposing resources.  
    /// Use <see cref="CommandString"/> to assign the SQL query, <see cref="Open"/> to establish the connection, and <see cref="DataReader"/> to execute and retrieve results.  
    /// Caller is responsible for consuming and disposing the reader appropriately.  
    /// <b>Note:</b> This class is not thread-safe and assumes single-command execution per instance.
    /// </remarks>
    public class MySQLExecute
    {
        private MySqlConnection _connection;
        private MySqlCommand _cmd;
        private MySqlDataReader _reader;
        private MySqlDataAdapter _adapter;
        private string _connSTR;
        private string _cmdSTR;
        private List<string> _read;
        private DataSet _dataset;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLExecute"/> class with default MySQL connection, command, and adapter objects.
        /// </summary>
        public MySQLExecute()
        {
            _connection = new MySqlConnection();
            _cmd = new MySqlCommand();
            _adapter = new MySqlDataAdapter();
            _connSTR = "";
            _cmdSTR = "";
            _read = new List<string>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLExecute"/> class using the specified MySQL connection.
        /// </summary>
        /// <param name="Connection">
        /// The <see cref="MySqlConnection"/> to associate with the command and adapter.
        /// </param>
        public MySQLExecute(MySqlConnection Connection)
        {
            _connection = Connection;
            _cmd = new MySqlCommand();
            _adapter = new MySqlDataAdapter();
            _connSTR = "";
            _cmdSTR = "";
            _read = new List<string>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLExecute"/> class using the specified connection string.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string used to configure the MySQL connection.
        /// </param>
        public MySQLExecute(string connectionString)
        {
            _connSTR = connectionString;
            _cmdSTR = "";
        }

        #region PUBLIC PROCEDURES
        
        #endregion

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the MySQL connection string used to establish a database connection.
        /// </summary>
        public string ConnectionString { get { return _connSTR; } set { _connSTR = value; } }
        /// <summary>
        /// Gets the active <see cref="MySqlConnection"/> instance associated with this class.
        /// </summary>
        public MySqlConnection Connection { get { return _connection; } }
        /// <summary>
        /// Gets the current <c>MySqlDataAdapter</c> instance used for executing SELECT queries.
        /// </summary>
        /// <returns>
        /// A configured <c>MySqlDataAdapter</c> bound to the active SQL command and connection.
        /// </returns>
        /// <remarks>
        /// This property exposes the internal data adapter prepared by <c>ExecuteSelectToAdapter()</c>.  
        /// It can be used to fill <c>DataTable</c> or <c>DataSet</c> objects for reporting, binding, or further processing.
        /// </remarks>
        public MySqlDataAdapter Adapter { get { return _adapter; } }
        /// <summary>
        /// Gets a <c>MySqlDataReader</c> instance for executing the current command string.
        /// </summary>
        /// <returns>
        /// A <c>MySqlDataReader</c> containing the result set of the executed command.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if the command string is not set or the connection is not open.
        /// </exception>
        public MySqlDataReader Reader
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_cmdSTR))
                {
                    if (_connection.State != System.Data.ConnectionState.Open)
                        throw new Exception("No open connections.");

                    _cmd = new MySqlCommand();
                    _cmd.Connection = _connection;
                    _cmd.CommandType = System.Data.CommandType.Text;
                    _cmd.CommandText = _cmdSTR;
                    _reader = _cmd.ExecuteReader();
                    _cmd.Dispose();
                    return _reader;
                }
                else
                {
                    throw new Exception("Command String not set.");
                }
            }
        }
        /// <summary>
        /// Gets the current <c>DataSet</c> instance populated by the most recent SELECT query.
        /// </summary>
        /// <returns>
        /// A <c>DataSet</c> containing the result of the executed SQL command.
        /// </returns>
        /// <remarks>
        /// This property exposes the internal dataset prepared by <c>ExecuteSelectToDataSet()</c>.  
        /// It can be used for multi-table data binding, reporting, or offline data manipulation workflows.
        /// </remarks>
        public DataSet DataSet { get { return _dataset; } }
        /// <summary>
        /// Sets the SQL command string to be executed by this instance.
        /// </summary>
        public string CommandString { set { _cmdSTR = value; } }
        /// <summary>
        /// Gets the list of string values retrieved from the most recent read operation.
        /// </summary>
        /// <returns>
        /// A <see cref="List{String}"/> containing the extracted column values.
        /// </returns>
        public List<string> GetValues { get { return _read; } }
        /// <summary>
        /// Determines whether the result set contains any data.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the result set has at least one value; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks whether any values have been collected from a previously executed query.  
        /// It is typically used to verify that a SELECT operation returned data before attempting to access it.
        /// </remarks>
        public bool HasRows
        {
            get
            {
                if (_read.Count > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Opens a new MySQL connection using the configured connection string.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the connection attempt fails.
        /// </exception>
        public void Open()
        {
            _connection = new MySqlConnection(_connSTR);

            try
            {
                _connection.Open();
            }
            catch(Exception e)
            {
                throw new Exception("An error occured while trying to open connection to database." + e.Message.ToString());
            }
        }
        /// <summary>
        /// Closes the active MySQL data reader and connection if currently open.
        /// </summary>
        /// <remarks>
        /// <b>Warning:</b> This method will silently return if the connection is not open.
        /// Ensure the reader is fully consumed or explicitly closed before invoking this method to avoid incomplete disposal or runtime exceptions.
        /// </remarks>
        public void Close()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                return;

            if (_reader.IsClosed == false)
                _reader.Close();
            _connection.Close();
        }
        public void CloseReader()
        {
            _reader.Close();
        }
        /// <summary>
        /// Asynchronously releases all resources used by the current instance of <see cref="MySQLExecute"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous dispose operation.
        /// </returns>
        /// <remarks>
        /// Disposes the command, adapter, and connection synchronously.  
        /// Closes the reader asynchronously if it is still open.  
        /// </remarks>
        public async Task DisposeAsync()
        {
            _cmd.Dispose();
            _adapter.Dispose();
            _connection.Dispose();
            _adapter.Dispose();
            _dataset.Dispose();

            if (_reader != null && !_reader.IsClosed)
                await _reader.CloseAsync();

            _read.Clear();
        }
        /// <summary>
        /// Resets the internal MySQL connection string and applies it to the active connection.
        /// </summary>
        /// <param name="connectionString">
        /// The new connection string to assign. Defaults to an empty string.
        /// </param>
        /// <remarks>
        /// Closes the current connection before updating the connection string.
        /// Caller must reopen the connection manually if needed.
        /// </remarks>
        public void ResetConnectionString(string connectionString = "")
        {
            _connection.Close();
            _connSTR = connectionString;
            _connection.ConnectionString = connectionString;
        }
        /// <summary>
        /// Executes a SQL SELECT command without parameters and collects all column values from all rows into a flat list of strings.
        /// </summary>
        /// <remarks>
        /// Validates that the database connection is open and the command string is a valid SELECT statement.  
        /// Executes the query and reads each row returned by the result set.  
        /// For every row, each column value is converted to a string and added to a sequential list.  
        /// This method is suitable for simple queries where all data can be flattened into a single list of string values.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if the connection is closed, the command string is missing or invalid, the query returns no rows, or execution fails.
        /// </exception>
        public void ExecuteSelect()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (string.IsNullOrWhiteSpace(_cmdSTR))
                throw new Exception("No command string.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _cmd = new MySqlCommand();
            _cmd.Connection = _connection;
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = _cmdSTR;
            try
            {
                _reader = _cmd.ExecuteReader();

                if (!_reader.HasRows)
                {
                    _reader.Close();
                    _read.Clear();
                }
                else
                {
                    _read.Clear();
                    while (_reader.Read())
                    {
                        for (int i = 0; i < _reader.FieldCount; i++)
                            _read.Add(_reader[i].ToString());
                    }

                }
            }
            catch(Exception ex)
            {
                throw new Exception("Query Execution Error:\n\n" + ex.Message.ToString());
            }
            finally
            {
                if (_reader.IsClosed == false)
                    _reader.Close();
                _cmd.Dispose();
            }
        }
        /// <summary>
        /// Executes a SQL SELECT command with a single parameter and collects all column values from all rows into a flat list of strings.
        /// </summary>
        /// <param name="Parameter">
        /// The parameter to bind to the SQL query, including its name and value.
        /// </param>
        /// <remarks>
        /// Validates that the database connection is open and the command string is a valid SELECT statement.  
        /// Executes the query and reads each row returned by the result set.  
        /// For every row, each column value is converted to a string and added sequentially to a list.  
        /// This method is suitable for simple queries where all data can be flattened into a single list of string values.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if the connection is closed, the command string is missing or invalid, the query returns no rows, or execution fails.
        /// </exception>
        public void ExecuteSelect(ParametersMetadata Parameter)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (string.IsNullOrWhiteSpace(_cmdSTR))
                throw new Exception("No command string.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _cmd = new MySqlCommand();
            _cmd.Connection = _connection;
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = _cmdSTR;

            _cmd.Parameters.Clear();
            _cmd.Parameters.AddWithValue(Parameter.Name, Parameter.Value);

            try
            {
                _reader = _cmd.ExecuteReader();

                if (!_reader.HasRows)
                {
                    _reader.Close();
                    _read.Clear();
                }
                else
                {
                    _read.Clear();
                    while (_reader.Read())
                    {
                        for (int i = 0; i < _reader.FieldCount; i++)
                            _read.Add(_reader[i].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error:\n\n" + ex.Message.ToString());
            }
            finally
            {
                if (_reader.IsClosed == false)
                    _reader.Close();
                _cmd.Dispose();
            }
        }
        /// <summary>
        /// Executes a SQL SELECT command with multiple parameters and collects all column values from all rows into a flat list of strings.
        /// </summary>
        /// <param name="Parameters">
        /// An array of parameter definitions, each containing a name and value to be bound to the query.
        /// </param>
        /// <remarks>
        /// Validates that the database connection is active and the command string represents a valid SELECT statement.  
        /// Binds all provided parameters to the query before execution.  
        /// If the query returns rows, each column value from every row is converted to a string and added sequentially to a list.  
        /// This method is suitable for queries where the result set can be flattened into a single list of string values.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if the connection is closed, the command string is missing or invalid, the query returns no rows, or execution fails.
        /// </exception>
        public void ExecuteSelect(ParametersMetadata[] Parameters)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (string.IsNullOrWhiteSpace(_cmdSTR))
                throw new Exception("No command string.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _cmd = new MySqlCommand();
            _cmd.Connection = _connection;
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = _cmdSTR;

            _cmd.Parameters.Clear();
            foreach (ParametersMetadata par in Parameters)
                _cmd.Parameters.AddWithValue(par.Name, par.Value);

            try
            {
                _reader = _cmd.ExecuteReader();

                if (!_reader.HasRows)
                {
                    _reader.Close();
                    _read.Clear();
                }
                else
                {
                    _read.Clear();
                    while (_reader.Read())
                    {
                        for (int i = 0; i < _reader.FieldCount; i++)
                            _read.Add(_reader[i].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error:\n\n" + ex.Message.ToString());
            }
            finally
            {
                if (_reader.IsClosed == false)
                    _reader.Close();
                _cmd.Dispose();
            }
        }
        /// <summary>
        /// Executes the configured SQL command that does not return any result set, such as INSERT, UPDATE, or DELETE.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid syntax, or runtime errors.
        /// </exception>
        /// <remarks>
        /// Initializes a new <see cref="MySqlCommand"/> using the configured command string and connection.  
        /// Executes the command using <see cref="MySqlCommand.ExecuteNonQuery"/> and disposes the command afterward.  
        /// This method is intended for SQL operations that modify data but do not return rows.
        /// </remarks>
        public void ExecuteNonQuery()
        {
            _cmd = new MySqlCommand();
            _cmd.Connection = _connection;
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = _cmdSTR;

            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception("Query Execution Error\n\n" + ex.Message.ToString());
            }
            finally
            {
                _cmd.Dispose();
            }
        }
        /// <summary>
        /// Executes the configured SQL command with a single parameter and does not return any result set.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> instance containing the name and value of the SQL parameter to bind.
        /// </param>
        public void ExecuteNonQuery(ParametersMetadata Parameter)
        {
            _cmd = new MySqlCommand();
            _cmd.Connection = _connection;
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = _cmdSTR;
            _cmd.Parameters.Clear();
            _cmd.Parameters.AddWithValue(Parameter.Name, Parameter.Value);

            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error\n\n" + ex.Message.ToString());
            }
            finally
            {
                _cmd.Dispose();
            }
        }
        /// <summary>
        /// Executes the configured SQL command with multiple parameters and does not return any result set.
        /// </summary>
        /// <param name="Parameters">
        /// An array of <see cref="ParametersMetadata"/> instances, each containing a parameter name and value to bind to the command.
        /// </param>
        public void ExecuteNonQuery(ParametersMetadata[] Parameters)
        {
            _cmd = new MySqlCommand();
            _cmd.Connection = _connection;
            _cmd.CommandType = System.Data.CommandType.Text;
            _cmd.CommandText = _cmdSTR;
            _cmd.Parameters.Clear();

            for (int a = 0; a < Parameters.Length; a++)
                _cmd.Parameters.AddWithValue(Parameters[a].Name, Parameters[a].Value);

            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error\n\n" + ex.Message.ToString());
            }
            finally
            {
                _cmd.Dispose();
            }
        }
        /// <summary>
        /// Prepares and resets the data adapter for executing a SQL SELECT command against the active database connection.
        /// </summary>
        /// <remarks>
        /// Validates that the connection is open and the command string represents a valid SELECT statement.  
        /// Disposes any previously assigned <c>MySqlDataAdapter</c> instance to release resources,  
        /// then initializes a new adapter and assigns the current command string as its <c>SelectCommand</c>.  
        /// This method is typically used for populating datasets or binding data to UI components.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if the connection is closed or the command string is not a valid SQL SELECT statement.
        /// </exception>
        public void ExecuteSelectToAdapter()
        {
            if (_connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _adapter = new MySqlDataAdapter();
            _adapter.SelectCommand = new MySqlCommand(_cmdSTR, _connection);
        }
        /// <summary>
        /// Executes a SQL SELECT command and fills a new <c>DataSet</c> with the result.
        /// </summary>
        /// <remarks>
        /// Validates that the database connection is open and the command string is a valid SELECT statement.  
        /// Disposes any previously assigned <c>MySqlDataAdapter</c> and <c>DataSet</c> instances to release resources.  
        /// Initializes a new adapter and dataset, assigns the current command string to the adapter,  
        /// and populates the dataset with the query result.  
        /// This method is typically used for multi-table data binding, reporting, or offline data manipulation.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if the connection is closed or the command string is not a valid SQL SELECT statement.
        /// </exception>
        public void ExecuteSelectToDataSet()
        {
            if (_connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _adapter = new MySqlDataAdapter();
            _dataset = new DataSet();
            _adapter.SelectCommand = new MySqlCommand(_cmdSTR, _connection);
            _adapter.Fill(_dataset);
        }
        /// <summary>
        /// Removes all values from the collection, resetting it to an empty state.
        /// </summary>
        public void ClearValues()
        {
            _read.Clear();
        }
        #endregion

        #region PRIVATE FUNCTIONS
        /// <summary>
        /// Determines whether the current SQL command string represents a SELECT statement.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command string contains the keyword "SELECT"; otherwise, <c>false</c>.
        /// </returns>
        private bool IsSelectCommand()
        {
            if (_cmdSTR.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) != -1)
                return true;
            else
                return false;
        }
        #endregion
    }
}
