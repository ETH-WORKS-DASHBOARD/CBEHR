using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BranchEmployeeStatus
/// </summary>
public class BranchEmployeeStatusManager
{
	public BranchEmployeeStatusManager()
	{
	}

    public static DataTable getSpecBranchEmpStatus(int branchID)
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@branch", branchID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetBranchEmployeeStatus, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public static DataTable getAllBranchEmpStatus()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetAllBranchEmployeeStatus, null);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public static DataTable BindGrid()
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetListOfAssignedEmployeeStatus, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public static DataTable getAssignedEmployeeFormOtherDistrict()
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetListOfAssignedEmployeefromOtherDistrict, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }
}