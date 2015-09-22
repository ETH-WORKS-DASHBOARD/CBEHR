using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using log4net;

/// <summary>
/// Summary description for PromotionManager
/// </summary>
public class PromotionManager : IManager
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(PromotionManager));

    private static string className = "PromotionManager";

    Promotion promotion;
    PromotionAssigment promotionAssigment;

    public PromotionManager()
    {
    }

    public PromotionManager(Promotion promotion)
    {
        this.promotion = promotion;
    }

    public PromotionManager(PromotionAssigment promotionAssigment)
    {
        this.promotionAssigment = promotionAssigment;
    }
    /**
     * Add New Promotion. 
     */
    public TransactionResponse addNewPromotion()
    {
        //create parameters map to pass
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", promotion.EmpID);
        argumentsMap.Add("@prev_branch", promotion.PrevBranch);
        argumentsMap.Add("@branch", promotion.Branch);
        argumentsMap.Add("@Minute_No", promotion.MinuteNo);
        argumentsMap.Add("@Post", promotion.Post);
        argumentsMap.Add("@promotionDate", promotion.PromotionDate);
        argumentsMap.Add("@status", promotion.Status);
        argumentsMap.Add("@destrictID", PageAccessManager.getDistrictID());

        //call sote to DB method. 
        return sendInsertPromotionToDb(DbAccessConstants.spAddNewPromotion, argumentsMap);
    }

    /**
     * Add New Promotion for other district. 
     */
    public TransactionResponse addNewPromotionForOtherDistrict()
    {
        //create parameters map to pass
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        //argumentsMap.Add("@Emp_ID", promotion.EmpID);
        argumentsMap.Add("@prev_branch", promotion.PrevBranch);
        argumentsMap.Add("@branch", promotion.Branch);
        argumentsMap.Add("@Minute_No", promotion.MinuteNo);
        argumentsMap.Add("@Post", promotion.Post);
        argumentsMap.Add("@promotionDate", promotion.PromotionDate);
        argumentsMap.Add("@status", promotion.Status);
        argumentsMap.Add("@isAccounced", promotion.IsAnnounced);
        argumentsMap.Add("@destrictID", PageAccessManager.getDistrictID());

        //call sote to DB method. 
        return sendInsertPromotionToDb(DbAccessConstants. spAddNewPromotionForOtherDistrict, argumentsMap);
    }    

    private TransactionResponse sendInsertPromotionToDb(string spName, IDictionary<string, object> parameters)
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(spName, parameters);

        TransactionResponse response = new TransactionResponse();
        try
        {
            //call store to DB mathod and get reponse. 
            storeToDb.instertNewRecord();
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_PROMOTION_REGISTER_OK);
        }

        catch (SqlException ex) 
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_PROMOTION_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_APP_RATING_REGISTER);
                response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            }
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {            
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }

        return response;
    }

    public TransactionResponse addNewPromotionAssignment()
    {
        //Add List of Arguments for new promotion Assigment
        IDictionary<string, object> promotionAssigmentParameters = new Dictionary<string, object>();

        promotionAssigmentParameters.Add("@minNo", promotionAssigment.MinuteNo);
        promotionAssigmentParameters.Add("@hROfficerID", promotionAssigment.HROfficerID);
        promotionAssigmentParameters.Add("@deadLine", promotionAssigment.DeadLine);
        promotionAssigmentParameters.Add("@remark", promotionAssigment.Remark);


        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAddPromotionAssignmentToHROfficer, promotionAssigmentParameters);
        TransactionResponse response = new TransactionResponse();
        try
        {
            //call update to DB mathod and get reponse. 
            dpOperation.updateRecord();
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_ASSIGNED_SUCCESS);
        }

        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR + ". "+ DBOperationErrorConstants.M_DUPLICATE_PROMOTION_ASSIGNEMENT);
            response.setErrorCode(DBOperationErrorConstants.E_PROMOTION_ASSIGNEMETN_FAILED);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }


    public DataTable getUnassignedPromotion()
    {
        //Pass Stored Procedure Name and parameter list. 
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@districtID", PageAccessManager.getDistrictID());
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetPromotionAssignedToHROfficer, parameters);

        DataTable dataTable = null;
        try
        {
            return dataTable = storeToDb.getRecord();
        }
        catch (SqlException ex)
        {
            //return emptylist
            return dataTable;
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return dataTable;
        }
    }


    public DataTable getmanagerialPosition(string branch)
    {
        //Pass Stored Procedure Name and parameter list. 
        //Add List of Arguments TO GET managerial position
        IDictionary<string, object> branchID = new Dictionary<string, object>();

        branchID.Add("@branch", branch);
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmpAtManagerialPosition, branchID);

        DataTable dataTable = null;
        try
        {
            return dataTable = storeToDb.getRecord();
        }
        catch (SqlException ex)
        {
            //return emptylist
            return dataTable;
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return dataTable;
        }
    }

    private DBOperationsUtil getDBOperationToUpdatePromotionStatus(IDictionary<string, object> parameters, Promotion promotion, string vacNo)
    {
        DBOperationsUtil dpOperation = null;

        //Pass Stored Procedure Name and parameter list. 
        parameters.Add("@Emp_ID", promotion.EmpID);
        parameters.Add("@Minute_No", promotion.MinuteNo);
        parameters.Add("@vacancy_No", vacNo);
        parameters.Add("@forAssOrAnnounce", PromotionConstants.VACANCY_ANNOUNCED_FOR_PROMOTION); 

        dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdatePromotionStatus, parameters);
        return dpOperation;
    }

    public TransactionResponse updatePromotion(Promotion promotion, string vacNo)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            DBOperationsUtil dpOperation = getDBOperationToUpdatePromotionStatus(parameters, promotion, vacNo);
            dpOperation.updateRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_UPDATE_VACANCY);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }


    private DBOperationsUtil getDBOperationToUpdatePromotionStatus4Assignment(IDictionary<string, object> parameters, Promotion promotion, string promoStatus)
    {
        DBOperationsUtil dpOperation = null;

        //Pass Stored Procedure Name and parameter list.
        parameters.Add("@Emp_ID", 0);
        parameters.Add("@Minute_No", promotion.MinuteNo);
        parameters.Add("@vacancy_No", 0);
        parameters.Add("@forAssOrAnnounce", promoStatus);

        dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdatePromotionStatus, parameters);
        return dpOperation;
    }

    public TransactionResponse updatePromotionStatus(Promotion promotion, string promoStatus)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            DBOperationsUtil dpOperation = getDBOperationToUpdatePromotionStatus4Assignment(parameters, promotion, promoStatus);
            dpOperation.updateRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_UPDATE_VACANCY);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    //this methode return a list of promoted or Assigned Employee for specified Post
    public DataTable getSpecifiedPromotionResult(int postID, string promotionOrAssigned)
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Post", postID);
        argumentsMap.Add("@status", promotionOrAssigned);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetPromotedEmpBasedOnPostAndStatus, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }


    public DataTable getAllPromotedEmployee(string promotionStatus)
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Promostatus", promotionStatus);

        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetAllPromotedEmp, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    //Update Promoted Employee Status
    public DataTable UpdatePromotedemployee()
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@prev_branch", promotion.PrevBranch);
        argumentsMap.Add("@branch", promotion.Branch);
        argumentsMap.Add("@Post", promotion.Post);
        argumentsMap.Add("@status", promotion.Status);
        argumentsMap.Add("@Minute_No", promotion.MinuteNo);
        argumentsMap.Add("@promotionDate", promotion.PromotionDate);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdatePromotedEmp, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public void logException(Exception ex){
        LoggerManager.LogError(ex.ToString(), logger);
    }
}