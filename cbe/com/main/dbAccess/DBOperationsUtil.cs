using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using log4net;

public class DBOperationsUtil
{
    private string storedProcedureName;

    private IDictionary<string, object> parametersMap;

    private SqlConnection connection;

    private static readonly ILog logger = LogManager.GetLogger(typeof(DBOperationsUtil));

    // CONSTRACTOR
    public DBOperationsUtil(string storedProcedureName, IDictionary<string, object> parametersMap)
    {
        this.storedProcedureName = storedProcedureName;
        this.parametersMap = parametersMap;
        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DbAccessConstants.connStringTagName].ToString());
    }

    //---------------------------------------------------
    // update record, Utility
    // upate can be handles the same way insert works
    // NOTE: this method might throw SqlException, the caller should catch SqlException
    //---------------------------------------------------
    public bool updateRecord()
    {
        return instertNewRecord();
    }

    //---------------------------------------------------
    // delete record, Utility
    // delete can be handles the same way insert works
    // NOTE: this method might throw SqlException, the caller should catch SqlException 
    //---------------------------------------------------
    public bool deleteRecord()
    {
        return instertNewRecord();
    }

    //---------------------------------------------------
    // StoreToBD, Utility
    // NOTE: this method might throw SqlException, the caller should catch SqlException
    //---------------------------------------------------
    public bool instertNewRecord()
    {
        // TransactionResponse response = new TransactionResponse();
        try
        {
            SqlCommand cmd = getSqlCommand();
            //Open connection
            openConnection();
            cmd.ExecuteNonQuery();

            //log every DB operation
            string result = DBOperationsUtil.getDictionaryAsListOfString(parametersMap);
            LoggerManager.LogInfo("SP Name: " + storedProcedureName + " Parameters [" + result + "]" , logger);

            return true;
        }
        catch (SqlException ex)
        {
            //convert Dictionay to list of KEY:VALUE
            string result = DBOperationsUtil.getDictionaryAsListOfString(parametersMap);
            LoggerManager.LogError("SP Name: " + storedProcedureName + " Parameters [" + result + "]" + ex.ToString(), logger);

            //rethrow the exception, it has to be handled by the manager class
            throw;
        }
        finally
        {
            closeConnection();
        }
    }


    //Record and conver it to list of string for the specific column name passed.
    // NOTE: this method might throw SqlException, the caller should catch SqlException
    public List<string> getRecordAsListOfString(string columnName)
    {
        DataTable dataTable = getRecord();
        List<string> listOfString = null;
        if (dataTable != null)
        {
            listOfString = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                listOfString.Add(row[columnName].ToString());
            }
        }
        return listOfString;
    }

    //---------------------------------------------------
    // get Record from DB, Utility
    // NOTE: this method might throw SqlException, the caller should catch SqlException
    //---------------------------------------------------
    public DataTable getRecord()
    {
        DataTable dataTable = null;
        try
        {
            SqlCommand command = getSqlCommand();

            //Open connection
            openConnection();

            //Use SqlDataAdapter, because SqlDataReader can not be used after connection is closed            
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            //fill the result into DataTable,
            sqlDataAdapter.Fill(dataTable);

            //log all DB operations
            string result = DBOperationsUtil.getDictionaryAsListOfString(parametersMap);
            LoggerManager.LogInfo("SP Name: " + storedProcedureName + " Parameters [" + result + "] ; Number of results: " 
                +  dataTable.Rows.Count, logger);
        }
        catch (SqlException ex)
        {
            //convert Dictionay to list of KEY:VALUE
            string result = DBOperationsUtil.getDictionaryAsListOfString(parametersMap);

            //Write the exception catched to a trace file. 
            //Show only meaningfull error to a user. 
            LoggerManager.LogError("SP Name: " + storedProcedureName + " Parameters [" + result + "]" + ex.ToString(), logger);

            //rethrow the exception to indicate that operation was not sucessful
            throw;
        }
        finally
        {
            closeConnection();
        }
        return dataTable;
    }

    // Close an already open connection. 
    private void closeConnection()
    {
        connection.Close();
    }

    //Open Connection using the connection String
    private void openConnection()
    {
        connection.Open();
    }

    /**
     * Prepare and return SQl Command.
     */
    private SqlCommand getSqlCommand()
    {
        // create a command object identifying the stored procedure
        SqlCommand cmd = new SqlCommand(storedProcedureName, connection);

        //set the command object so it knows to execute a stored procedure
        cmd.CommandType = CommandType.StoredProcedure;

        // add parameter to command, which will be passed to the stored procedure
        // first check if paramter list exists
        if (parametersMap != null && parametersMap.Count > 0)
        {
            foreach (var paramMap in parametersMap)
            {
                cmd.Parameters.AddWithValue(paramMap.Key, paramMap.Value);
            }
        }
        return cmd;
    }


    //Converts Dictionary to list of string: format KEY1:VALUE1, KEY2:VALUE2
    //and returns as one sngle string. 
    private static string getDictionaryAsListOfString(IDictionary<string, object> parametersDictionary)
    {
        string paramsList = "NULL";
        if (parametersDictionary != null)
        {
            paramsList = string.Join(", ", parametersDictionary.Select(x => string.Format("{0}:{1}", x.Key, x.Value)).ToArray());
        }
        return paramsList;
    }

    /**
     * Convert Datatable column to list of strings. 
     * it collects string as long as the data in columnName2 is not greater than 'limit' passed in param. 
     */
    public static string getDataTableColumnAsListOfString(DataTable dataTable, string columnName, string columnName2, int limit)
    {
        string result = "";
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            bool firstTime = true;
            string COMMA = ",";
            foreach (DataRow row in dataTable.Rows)
            {
                int currentData;
                if (int.TryParse(row[columnName2].ToString(), out currentData))
                {
                    if (currentData > limit)
                    {
                        break;
                    }
                }
                else
                {
                    //if there is invalid data it will be skept. 
                    continue;
                }

                if (firstTime)
                {
                    result += row[columnName].ToString().Trim();
                    firstTime = false;
                }
                else
                {
                    //in case array index out of bound exception happens, 
                    //for example the user entered rank limit that is greater than the max rank
                    //in this case we have to catch the exception and simple exit this operation. 
                    try
                    {
                        result += COMMA + row[columnName].ToString().Trim();
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }
        }
        return result;
    }
}
