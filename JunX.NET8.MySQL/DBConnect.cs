using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides additional functionalities for connecting, querying, and manipulating MySQL Databases.
    /// </summary>
    public class DBConnect
    {
        private MySqlConnection _conn = new MySqlConnection();
        private MySqlDataReader _reader;
        private MySqlDataAdapter _adapter = new MySqlDataAdapter();
        private MySqlCommand _cmd;
        private DataSet _dataset = new DataSet();
        private string _connSTR = "";
        private string _cmdSTR = "";
        private List<string> _values = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <c>DBConnect</c> class using the specified connection string,
        /// and attempts to open the database connection immediately.
        /// </summary>
        /// <param name="ConnString">
        /// A valid MySQL connection string used to configure and open the internal <see cref="MySqlConnection"/>.
        /// </param>
        /// <remarks>
        /// This constructor sets both the internal connection and command string to the provided value.
        /// It attempts to open the connection immediately, and throws a descriptive exception if the connection fails.
        /// Use this overload when you want automatic connection initialization upon instantiation.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection attempt fails. The inner exception contains the original error details.
        /// </exception>
        public DBConnect(string ConnString)
        {
            _conn.ConnectionString = ConnString;
            _connSTR = ConnString;
            try
            {
                _conn.Open();
            }
            catch(Exception ex)
            {
                throw new Exception("DB Connection Error:\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Initializes a new instance of the <c>DBConnect</c> class using an existing <see cref="MySqlConnection"/>.
        /// </summary>
        /// <param name="Connection">
        /// A preconfigured <see cref="MySqlConnection"/> instance to be used by this class.
        /// </param>
        /// <remarks>
        /// This constructor allows external injection of a connection object, enabling reuse across components or testing scenarios.
        /// It does not open the connection or modify its state. Ensure the connection is properly initialized and opened before use.
        /// </remarks>
        public DBConnect(MySqlConnection Connection)
        {
            _conn = Connection;
        }

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the database connection string used by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the connection string for the underlying database connection.
        /// </value>
        /// <remarks>
        /// Setting this property updates the internal connection string reference but does not automatically reconfigure or reopen the connection.
        /// Use this in conjunction with connection lifecycle methods to ensure consistency.
        /// </remarks>
        public string ConnectionString { get { return _connSTR; } set { _connSTR = value; } }
        /// <summary>
        /// Gets the active <see cref="MySqlConnection"/> instance used by this class.
        /// </summary>
        /// <value>
        /// A read-only <see cref="MySqlConnection"/> object representing the current database connection.
        /// </value>
        /// <remarks>
        /// This property exposes the underlying connection for advanced operations such as transaction handling,
        /// command execution, or diagnostics. It does not enforce connection state checks; ensure the connection is open
        /// before use.
        /// </remarks>
        public MySqlConnection Connection { get { return _conn; } }
        /// <summary>
        /// Gets the current <see cref="ConnectionState"/> of the underlying database connection.
        /// </summary>
        /// <value>
        /// A <see cref="ConnectionState"/> value indicating whether the connection is open, closed, connecting, executing, or broken.
        /// </value>
        /// <remarks>
        /// Use this property to check the readiness of the <c>MySqlConnection</c> before executing commands or transactions.
        /// It reflects the live state of the connection and can be used for defensive programming or logging.
        /// </remarks>
        public ConnectionState State { get { return _conn.State; } }
        /// <summary>
        /// Gets or sets the SQL command text used by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the raw SQL command to be executed against the database.
        /// </value>
        /// <remarks>
        /// This property stores the command string independently of the <c>MySqlCommand</c> object.
        /// Use it to configure or inspect the intended query before execution.
        /// </remarks>
        public string CommandString { get { return _cmdSTR; } set { _cmdSTR = value; } }
        /// <summary>
        /// Gets the current <see cref="MySqlDataReader"/> instance used to traverse query results.
        /// </summary>
        /// <value>
        /// A read-only <see cref="MySqlDataReader"/> object representing the active result set from the last executed SQL command.
        /// </value>
        /// <remarks>
        /// This property exposes the internal reader for advanced result traversal, field inspection, or manual iteration.
        /// Ensure the reader is properly initialized and not closed before accessing its members.
        /// </remarks>
        public MySqlDataReader Reader { get { return _reader; } }
        /// <summary>
        /// Gets the current <see cref="MySqlDataAdapter"/> instance used for data population and synchronization.
        /// </summary>
        /// <value>
        /// A read-only <see cref="MySqlDataAdapter"/> object configured with the current SQL SELECT command and connection.
        /// </value>
        /// <remarks>
        /// This property exposes the internal adapter for operations such as filling a <c>DataSet</c> or <c>DataTable</c>.
        /// Ensure that the adapter is properly initialized via <c>ExecuteAdapter()</c> before use.
        /// </remarks>
        public MySqlDataAdapter Adapter { get { return _adapter; } }
        /// <summary>
        /// Gets the current <see cref="DataSet"/> instance used to store query results retrieved via the data adapter.
        /// </summary>
        /// <value>
        /// A read-only <see cref="DataSet"/> object containing the in-memory representation of one or more result tables.
        /// </value>
        /// <remarks>
        /// This property exposes the internal dataset for inspection, binding, or transformation.
        /// Ensure it has been populated via the <c>ExecuteDataSet()</c> method before accessing its contents.
        /// </remarks>
        public DataSet DataSet { get { return _dataset; } }
        /// <summary>
        /// Indicates whether the internal <see cref="MySqlDataReader"/> of the current
        /// instance of <see cref="DBConnect"/> contains one or more rows.
        /// </summary>
        /// <value>
        /// <c>true</c> if the reader has at least one row; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property wraps the <see cref="MySqlDataReader.HasRows"/> member to expose row presence status.
        /// Useful for pre-checking before attempting to read or extract data from the result set.
        /// </remarks>
        public bool HasRows { get { return _reader.HasRows; } }
        /// <summary>
        /// Gets the list of string values extracted from the current data context.
        /// </summary>
        /// <value>
        /// A <see cref="List{String}"/> containing the string representations of the extracted values.
        /// </value>
        /// <remarks>
        /// This property exposes the internally populated value list, typically hydrated from a data reader or query result.
        /// Use this to access the raw string values after execution or traversal logic has completed.
        /// </remarks>
        public List<string> Values { get { return _values; } }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Opens the database connection if it is not already open.
        /// </summary>
        /// <remarks>
        /// This method checks the current <see cref="ConnectionState"/> of the underlying <c>MySqlConnection</c>.
        /// If the connection is closed or broken, it reassigns the connection string and attempts to open the connection.
        /// Any exceptions encountered during the open operation are wrapped in a new <see cref="Exception"/> with a descriptive message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection attempt fails. The inner exception contains the original error details.
        /// </exception>
        public void Open()
        {
            if(_conn.State != ConnectionState.Open)
            {
                _conn.ConnectionString = _connSTR;

                try
                {
                    _conn.Open();
                }
                catch(Exception e)
                {
                    throw new Exception("An error occurred while trying to open DB Connection.", e);
                }
            }
        }
        /// <summary>
        /// Closes the active database connection and associated data reader if they are open.
        /// </summary>
        /// <remarks>
        /// This method checks the current <see cref="ConnectionState"/> of the underlying <c>MySqlConnection</c>.
        /// If the connection is open, it first closes the <c>MySqlDataReader</c> (if not already closed),
        /// then closes the connection itself. This ensures proper resource cleanup and avoids potential leaks.
        /// </remarks
        public void CloseAll()
        {
            if (_conn.State == ConnectionState.Closed)
                return;

            if (_reader.IsClosed == false)
                _reader.Close();

            _conn.Close();
        }
        /// <summary>
        /// Closes the active <see cref="MySqlDataReader"/> if it is open.
        /// </summary>
        /// <remarks>
        /// This method calls <c>Close()</c> on the internal <c>MySqlDataReader</c> instance.
        /// Ensure that the reader is not already closed or disposed before invoking this method to avoid runtime exceptions.
        /// </remarks>
        public void CloseReader()
        {
            _reader.Close();
        }
        /// <summary>
        /// Asynchronously disposes of all managed database resources used by this instance.
        /// </summary>
        /// <remarks>
        /// This method disposes the command, adapter, connection, and dataset objects.
        /// If the <see cref="MySqlDataReader"/> is not null and still open, it is closed asynchronously using <c>CloseAsync()</c>.
        /// Use this method to ensure proper cleanup of database-related resources in asynchronous workflows.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous dispose operation.
        /// </returns>
        public async Task Dispose()
        {
            _cmd.Dispose();
            _adapter.Dispose();
            _conn.Dispose();
            _dataset.Dispose();

            if (_reader != null && !_reader.IsClosed)
                await _reader.CloseAsync();
        }
        /// <summary>
        /// Resets the database connection by closing it if open, reassigning the connection string, and reopening it.
        /// </summary>
        /// <remarks>
        /// This method ensures the connection is refreshed with the current internal connection string.
        /// If the connection is already open, it is closed first to avoid state conflicts. Then the connection string is reassigned,
        /// and the connection is reopened. Use this to reinitialize the connection after configuration changes or transient failures.
        /// </remarks>
        /// <exception cref="Exception">
        /// May propagate exceptions from <c>Open()</c> if the connection fails to reopen.
        /// </exception>
        public void Reset()
        {
            if (_conn.State == ConnectionState.Open)
                _conn.Close();

            _conn.ConnectionString = _connSTR;
            _conn.Open();
        }
        /// <summary>
        /// Executes the current SQL SELECT command and initializes the <see cref="MySqlDataReader"/> for result traversal.
        /// </summary>
        /// <remarks>
        /// This method performs several validations before execution:
        /// <list type="bullet">
        /// <item>Ensures the database connection is open.</item>
        /// <item>Validates that the command string is not null or empty.</item>
        /// <item>Checks that the command is a valid SQL SELECT statement.</item>
        /// </list>
        /// If all checks pass, it creates a new <see cref="MySqlCommand"/> and executes it using <c>ExecuteReader()</c>.
        /// Any exceptions during execution are wrapped with a descriptive message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is closed, the command string is invalid, the command is not a SELECT,
        /// or the query execution fails. The inner exception contains the original error details.
        /// </exception>
        public void ExecuteReader()
        {
            if (_conn.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (string.IsNullOrEmpty(_cmdSTR))
                throw new Exception("No command string.");

            if (IsSelectCommand() == false)
                throw new Exception("Command String is not an SQL SELECT command.");

            _cmd = new MySqlCommand(_cmdSTR, _conn);
            try
            {
                _reader = _cmd.ExecuteReader();
            }
            catch(Exception ex)
            {
                throw new Exception("Query Execution Error\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Executes a parameterized SQL SELECT command and initializes the <see cref="MySqlDataReader"/> for result traversal.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> object containing the parameter name and value to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method performs several validations before execution:
        /// <list type="bullet">
        /// <item>Ensures the database connection is open.</item>
        /// <item>Validates that the command string is not null or empty.</item>
        /// <item>Checks that the command is a valid SQL SELECT statement.</item>
        /// </list>
        /// If all checks pass, it creates a new <see cref="MySqlCommand"/>, clears existing parameters, adds the provided parameter,
        /// and executes the command using <c>ExecuteReader()</c>. Any exceptions are wrapped with a descriptive message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is closed, the command string is invalid, the command is not a SELECT,
        /// or the query execution fails. The inner exception contains the original error details.
        /// </exception>
        public void ExecuteReader(ParametersMetadata Parameter)
        {
            if (_conn.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (string.IsNullOrEmpty(_cmdSTR))
                throw new Exception("No command string.");

            if (IsSelectCommand() == false)
                throw new Exception("Command String is not an SQL SELECT command.");

            _cmd = new MySqlCommand(_cmdSTR, _conn);
            _cmd.Parameters.Clear();
            _cmd.Parameters.AddWithValue(Parameter.Name, Parameter.Value);
            try
            {
                _reader = _cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Executes a parameterized SQL SELECT command using the provided array of parameters,
        /// and initializes the <see cref="MySqlDataReader"/> for result traversal.
        /// </summary>
        /// <param name="Parameters">
        /// An array of <see cref="ParametersMetadata"/> objects representing the parameter names and values
        /// to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method performs several validations before execution:
        /// <list type="bullet">
        /// <item>Ensures the database connection is open.</item>
        /// <item>Validates that the command string is not null or empty.</item>
        /// <item>Checks that the command is a valid SQL SELECT statement.</item>
        /// </list>
        /// If all checks pass, it creates a new <see cref="MySqlCommand"/>, clears existing parameters,
        /// adds each parameter from the array, and executes the command using <c>ExecuteReader()</c>.
        /// Any exceptions are wrapped with a descriptive message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is closed, the command string is invalid, the command is not a SELECT,
        /// or the query execution fails. The inner exception contains the original error details.
        /// </exception>
        public void ExecuteReader(ParametersMetadata[] Parameters)
        {
            if (_conn.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (string.IsNullOrEmpty(_cmdSTR))
                throw new Exception("No command string.");

            if (IsSelectCommand() == false)
                throw new Exception("Command String is not an SQL SELECT command.");

            _cmd = new MySqlCommand(_cmdSTR, _conn);
            _cmd.Parameters.Clear();
            for(int i=0; i< Parameters.Length; i++)
                _cmd.Parameters.AddWithValue(Parameters[i].Name, Parameters[i].Value);

            try
            {
                _reader = _cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Executes the current SQL command that does not return any result set, such as INSERT, UPDATE, or DELETE.
        /// </summary>
        /// <remarks>
        /// This method validates that the command string is not null or whitespace before execution.
        /// It creates a new <see cref="MySqlCommand"/> using the current connection and command string,
        /// and executes it using <c>ExecuteNonQuery()</c>. Any exceptions encountered during execution
        /// are wrapped with a descriptive error message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the command string is missing or the query execution fails.
        /// The inner exception contains the original error details.
        /// </exception>
        public void ExecuteNonQuery()
        {
            if (string.IsNullOrWhiteSpace(_cmdSTR))
                throw new Exception("No command string.");

            _cmd = new MySqlCommand(_cmdSTR, _conn);
            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception("Query Execution Error:\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Executes a parameterized SQL command that does not return a result set, such as INSERT, UPDATE, or DELETE.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> object containing the name and value of the parameter to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method validates that the command string is not null or whitespace before execution.
        /// It creates a new <see cref="MySqlCommand"/>, clears any existing parameters, adds the provided parameter,
        /// and executes the command using <c>ExecuteNonQuery()</c>. Any exceptions encountered during execution
        /// are wrapped with a descriptive error message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the command string is missing or the query execution fails.
        /// The inner exception contains the original error details.
        /// </exception>
        public void ExecuteNonQuery(ParametersMetadata Parameter)
        {
            if (string.IsNullOrWhiteSpace(_cmdSTR))
                throw new Exception("No command string.");

            _cmd = new MySqlCommand(_cmdSTR, _conn);
            _cmd.Parameters.Clear();
            _cmd.Parameters.AddWithValue(Parameter.Name, Parameter.Value);
            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error:\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Executes a parameterized SQL command that does not return a result set, such as INSERT, UPDATE, or DELETE.
        /// </summary>
        /// <param name="Parameters">
        /// An array of <see cref="ParametersMetadata"/> objects representing the parameter names and values
        /// to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method validates that the command string is not null or whitespace before execution.
        /// It creates a new <see cref="MySqlCommand"/>, clears any existing parameters, adds each parameter from the array,
        /// and executes the command using <c>ExecuteNonQuery()</c>. Any exceptions encountered during execution
        /// are wrapped with a descriptive error message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the command string is missing or the query execution fails.
        /// The inner exception contains the original error details.
        /// </exception>
        public void ExecuteNonQuery(ParametersMetadata[] Parameters)
        {
            if (string.IsNullOrWhiteSpace(_cmdSTR))
                throw new Exception("No command string.");

            _cmd = new MySqlCommand(_cmdSTR, _conn);
            _cmd.Parameters.Clear();
            for (int i = 0; i < Parameters.Length; i++)
                _cmd.Parameters.AddWithValue(Parameters[i].Name, Parameters[i].Value);

            try
            {
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Query Execution Error:\n\n" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// Initializes a <see cref="MySqlDataAdapter"/> with the current SQL SELECT command and active connection.
        /// </summary>
        /// <remarks>
        /// This method validates that the database connection is open and that the command string represents a valid SQL SELECT statement.
        /// It then creates a new <see cref="MySqlDataAdapter"/> and assigns a <see cref="MySqlCommand"/> as its <c>SelectCommand</c>.
        /// Use this method to prepare the adapter for data population into a <c>DataSet</c> or <c>DataTable</c>.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is not open or the command string is not a valid SQL SELECT statement.
        /// </exception>
        public void ExecuteAdapter()
        {
            if (_conn.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _adapter = new MySqlDataAdapter();
            _adapter.SelectCommand = new MySqlCommand(_cmdSTR, _conn);
        }
        /// <summary>
        /// Initializes a <see cref="MySqlDataAdapter"/> with a parameterized SQL SELECT command and the current connection.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> object containing the name and value of the parameter to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method ensures the connection is open and the command string is a valid SQL SELECT statement.
        /// It creates a new <see cref="MySqlCommand"/>, clears any existing parameters, adds the provided parameter,
        /// and assigns it as the <c>SelectCommand</c> of a new <see cref="MySqlDataAdapter"/>.
        /// Use this method to prepare the adapter for data population into a <c>DataSet</c> or <c>DataTable</c> with parameterized queries.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is not open or the command string is not a valid SQL SELECT statement.
        /// </exception>
        public void ExecuteAdapter(ParametersMetadata Parameter)
        {
            if (_conn.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _adapter = new MySqlDataAdapter();
            _cmd = new MySqlCommand(_cmdSTR, _conn);
            _cmd.Parameters.Clear();
            _cmd.Parameters.AddWithValue(Parameter.Name, Parameter.Value);
            _adapter.SelectCommand = _cmd;
        }
        /// <summary>
        /// Initializes a <see cref="MySqlDataAdapter"/> with a parameterized SQL SELECT command and the current connection.
        /// </summary>
        /// <param name="Parameters">
        /// An array of <see cref="ParametersMetadata"/> objects representing the parameter names and values
        /// to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method ensures the connection is open and the command string is a valid SQL SELECT statement.
        /// It creates a new <see cref="MySqlCommand"/>, clears any existing parameters, adds each parameter from the array,
        /// and assigns it as the <c>SelectCommand</c> of a new <see cref="MySqlDataAdapter"/>.
        /// Use this method to prepare the adapter for data population into a <c>DataSet</c> or <c>DataTable</c>
        /// with dynamic, parameterized queries.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is not open or the command string is not a valid SQL SELECT statement.
        /// </exception>
        public void ExecuteAdapter(ParametersMetadata[] Parameters)
        {
            if (_conn.State != ConnectionState.Open)
                throw new Exception("Connection is not open.");

            if (!IsSelectCommand())
                throw new Exception("Command string is not an SQL SELECT command.");

            _adapter = new MySqlDataAdapter();
            _cmd = new MySqlCommand(_cmdSTR, _conn);
            _cmd.Parameters.Clear();
            for(int i=0; i<Parameters.Length; i++)
                _cmd.Parameters.AddWithValue(Parameters[i].Name, Parameters[i].Value);

            _adapter.SelectCommand = _cmd;
        }
        /// <summary>
        /// Executes the current SQL SELECT command and fills the internal <see cref="DataSet"/> with the result.
        /// </summary>
        /// <remarks>
        /// This method ensures the database connection is open and the command string is a valid SQL SELECT statement.
        /// It initializes a new <see cref="MySqlDataAdapter"/>, assigns a <see cref="MySqlCommand"/> as its <c>SelectCommand</c>,
        /// and populates the internal <see cref="DataSet"/> using <c>Fill()</c>.
        /// Use this method to hydrate the dataset for binding, transformation, or inspection.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the connection is not open or the command string is not a valid SQL SELECT statement.
        /// </exception>
        public void ExecuteDataSet()
        {
            _dataset = new DataSet();
            ExecuteAdapter();
            _adapter.Fill(_dataset);
        }
        /// <summary>
        /// Executes a parameterized SQL SELECT command and fills the internal <see cref="DataSet"/> with the result.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> object containing the name and value of the parameter to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method delegates to <c>ExecuteAdapter(Parameter)</c> to configure the <see cref="MySqlDataAdapter"/> with the parameterized command.
        /// It then populates the internal <see cref="DataSet"/> using <c>Fill()</c>.
        /// Use this method to hydrate the dataset for binding, transformation, or inspection with parameterized queries.
        /// </remarks>
        /// <exception cref="Exception">
        /// May propagate exceptions from adapter execution or dataset filling if the query fails or the connection is invalid.
        /// </exception>
        public void ExecuteDataSet(ParametersMetadata Parameter)
        {
            _dataset = new DataSet();
            ExecuteAdapter(Parameter);
            _adapter.Fill(_dataset);
        }
        /// <summary>
        /// Executes a parameterized SQL SELECT command and fills the internal <see cref="DataSet"/> with the result.
        /// </summary>
        /// <param name="Parameters">
        /// An array of <see cref="ParametersMetadata"/> objects representing the parameter names and values
        /// to be injected into the SQL command.
        /// </param>
        /// <remarks>
        /// This method delegates to <c>ExecuteAdapter(Parameters)</c> to configure the <see cref="MySqlDataAdapter"/> with the parameterized command.
        /// It then populates the internal <see cref="DataSet"/> using <c>Fill()</c>.
        /// Use this method to hydrate the dataset for binding, transformation, or inspection with dynamic, parameterized queries.
        /// </remarks>
        /// <exception cref="Exception">
        /// May propagate exceptions from adapter execution or dataset filling if the query fails or the connection is invalid.
        /// </exception>
        public void ExecuteDataSet(ParametersMetadata[] Parameters)
        {
            _dataset = new DataSet();
            ExecuteAdapter(Parameters);
            _adapter.Fill(_dataset);
        }
        /// <summary>
        /// Extracts all field values from the current <see cref="MySqlDataReader"/> and stores them as strings in the internal value list.
        /// </summary>
        /// <remarks>
        /// Clears the existing value list before reading. If the reader is closed or contains no rows, an exception is thrown.
        /// Iterates through each row and field, converting each value to a string and appending it to the internal list.
        /// Automatically closes the reader after traversal to ensure proper resource cleanup.
        /// Use this method to hydrate <see cref="Values"/> after executing a query.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown if the reader is closed, contains no rows, or if an error occurs during value extraction.
        /// </exception>
        public void GetValues()
        {
            if (_values.Count > 0)
                _values.Clear();

            if (_reader.IsClosed == true || _reader.HasRows == false)
                throw new Exception("Reader did not return any values or is closed.");
            else
            {
                try
                {
                    while (_reader.Read())
                        for (int i = 0; i < _reader.FieldCount; i++)
                            _values.Add(_reader[i].ToString());
                }
                catch (Exception e)
                {
                    throw new Exception("Reader Content Retrieval Failure:\n\n" + e.Message.ToString());
                }
                finally
                {
                    _reader.Close();
                }
            }
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
