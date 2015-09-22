using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;

/// <summary>
/// Summary description for PageAccessManager
/// </summary>
public class PageAccessManager : IManager
{

    private static readonly ILog logger = LogManager.GetLogger(typeof(PageAccessManager));

    private static string className = "PageAccessManager";

    private static string PARAMETER_NOT_DEFINED = "THIS PARAMETER NOT DEFINED ";

    public PageAccessManager()
    {
    }


    public static TransactionResponse getDistrictSettingValue(string paramTag)
    {
        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@paramter_tag", paramTag);
        argumentsMap.Add("@districtID", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spGetDistrictSetting, argumentsMap);
        try
        {
            DataTable dataTable = dbOperation.getRecord();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    //put the data on Transaction reponse
                    response.Data = row["parameter_value"].ToString();
                    response.setSuccess(true);
                    return response;
                }
            }
            response.Data = PARAMETER_NOT_DEFINED + DBOperationErrorConstants.CONTACT_ADMIN;
            response.setSuccess(false);
            return response;
        }
        catch (SqlException ex)
        {
            response.Data = PARAMETER_NOT_DEFINED + DBOperationErrorConstants.CONTACT_ADMIN;
            response.setSuccess(false);
            return response;
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            response.Data = PARAMETER_NOT_DEFINED + DBOperationErrorConstants.CONTACT_ADMIN;
            response.setSuccess(false);
            return response;
        }
    }

    //get session data. 
    public static string getSessionData(string tag)
    {
        return (string)(System.Web.HttpContext.Current.Session[tag]);
    }

    //read property file
    public static string getProperty(string tag)
    {
        var data = new Dictionary<string, string>();
        foreach (var row in System.IO.File.ReadAllLines(PageConstants.DISTRICT_PROPERTY_PATH))
        {
            data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
        }
        return data[tag];
    }

    //read district setting from property file. 
    public static string getDistrictID()
    {
        return getProperty(PageConstants.DISTRICT_ID);
    }

    public void logException(Exception ex)
    {
        LoggerManager.LogError(ex.ToString(), logger);
    }
}