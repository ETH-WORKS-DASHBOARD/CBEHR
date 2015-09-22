using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BranchManager
/// </summary>
public class BranchManager
{
    private Branch branch;
    private static string className = "BranchManager";

	public BranchManager(Branch branch)
	{
        this.branch = branch;
	}

    public TransactionResponse addNewBranch()
    {
        //
        IDictionary<string, object> branchParameters = new Dictionary<string, object>();
        branchParameters.Add("@BranchName", branch.branchName);
        branchParameters.Add("@DistrictCode", branch.district);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spAddNewcbeBranch, branchParameters);
        TransactionResponse response = new TransactionResponse();

        //call sote to DB method. 
        
        try
        {
            response.setSuccess(storeToDb.instertNewRecord());
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_REGISTERING_NEW_BRANCH);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            return response;
        }

        if (!response.isSuccessful())
        {
            //Write this exception to file for investigation of the issue later. 
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_REGISTERING_NEW_BRANCH);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            return response;
        }

        return response;
    }
}