using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Security;
using System.Data.SqlClient;
using System.Globalization;
using log4net;

/// <summary>
/// Summary description for VacancyEvaluationManager
/// </summary>
public class VacancyRegistrationAndEvaluationManager : IManager
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(VacancyRegistrationAndEvaluationManager));

    private static string FORM1 = "1";
    private static string FORM2 = "2";

    private static string className = "VacancyRegistrationAndEvaluationManager";

    private Vacancy vacancy;

    private VacancyApplicant vacancyApplicant;

    private VacancyEvaluationForm vacanyEvaluationForm;

    public VacancyRegistrationAndEvaluationManager()
    {
    }

    public VacancyRegistrationAndEvaluationManager(Vacancy vacancy)
    {
        this.vacancy = vacancy;
    }

    public VacancyRegistrationAndEvaluationManager(VacancyApplicant vacancyApplicant)
    {
        this.vacancyApplicant = vacancyApplicant;
    }

    public VacancyRegistrationAndEvaluationManager(VacancyEvaluationForm vacanyEvaluationForm)
    {
        this.vacanyEvaluationForm = vacanyEvaluationForm;
    }


    public TransactionResponse registerApplicants(bool isInternalEmployee, bool isEduQualExist)
    {
        if (isInternalEmployee)
        {
            return registerInternalApplicants(isEduQualExist);
        }

        return registerExternalApplicants();
    }

    public TransactionResponse checkEmployeeInPromotiontable(string empID)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@Emp_ID", empID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spCheckEmployeeRecSelected, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse checkEmployeeInEmployeeRemarktable(string empID, string remarkType)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@Emp_ID", empID);
        statusParams.Add("@remarkType", remarkType);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spCheckEmployeePenalty, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }

        return response;
    }

    private TransactionResponse registerInternalApplicants(bool isEduQualExist)
    {
        //Add Education Qualification into db if Education Qualification is not registered
        if (isEduQualExist)
        {
            addEducationQualification();
        }


        //Add List of Arguments for new employee
        IDictionary<string, object> internalAppicantsParam = new Dictionary<string, object>();

        internalAppicantsParam.Add("@EmpID", vacancyApplicant.EmpId);
        internalAppicantsParam.Add("@vacancyNo", vacancyApplicant.VacancyNo);
        internalAppicantsParam.Add("@vacancyDate", vacancyApplicant.VacancyDate);

        return sendInsertApplicantToDb(DbAccessConstants.spAddNewInternalApplicant, internalAppicantsParam);
    }

    private void addEducationQualification()
    {

        IDictionary<string, object> internalAppicantsParam = new Dictionary<string, object>();

        internalAppicantsParam.Add("@Emp_ID", vacancyApplicant.EmpId);
        internalAppicantsParam.Add("@EducationLevel", vacancyApplicant.EducLevel);
        internalAppicantsParam.Add("@Qualification", vacancyApplicant.Qualification);

        sendInsertApplicantToDb(DbAccessConstants.spAddEducqualificationForInternalApplicant, internalAppicantsParam);
    }

    private TransactionResponse registerExternalApplicants()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> externalAppicantsParam = new Dictionary<string, object>();

        externalAppicantsParam.Add("@EmpID", vacancyApplicant.EmpId);
        externalAppicantsParam.Add("@vacancyNo", vacancyApplicant.VacancyNo);
        externalAppicantsParam.Add("@vacancyDate", vacancyApplicant.VacancyDate);
        externalAppicantsParam.Add("@FName", vacancyApplicant.FirstName);
        externalAppicantsParam.Add("@MName", vacancyApplicant.MiddleName);
        externalAppicantsParam.Add("@LName", vacancyApplicant.LastName);
        externalAppicantsParam.Add("@branch", vacancyApplicant.CurrentBranch);
        externalAppicantsParam.Add("@job_title", vacancyApplicant.JobTitle);
        externalAppicantsParam.Add("@job_Grade", vacancyApplicant.CurrentJGrade);
        externalAppicantsParam.Add("@EducationalLevel", vacancyApplicant.EducLevel);
        externalAppicantsParam.Add("@Qualification", vacancyApplicant.Qualification);

        return sendInsertApplicantToDb(DbAccessConstants.spAddNewExternalApplicant, externalAppicantsParam);
    }

    private TransactionResponse sendInsertApplicantToDb(string spName, IDictionary<string, object> parameters)
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(spName, parameters);

        TransactionResponse response = new TransactionResponse();
        try
        {
            //call store to DB mathod and get reponse. 
            storeToDb.instertNewRecord();
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_REGISTERED_FOR_VACANCY_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_APPLICANT_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_APPLICANT_REGISTER);
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

    /*
     * get list of Vacancy ready to b assigned to HR officers + number of applicants in each vacancy. 
     */
    public static TransactionResponse getVacancyListToBeAssignedToHrOfficerWithApplicantNumber()
    {
        TransactionResponse response = new TransactionResponse();
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(
            DbAccessConstants.spVacancyListToAssignToHrOfficerWithApplicantsNo, null);
        DataTable dataTable = dpOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);
        return response;
    }

    /*
     * get list of Vacancy ready to b assigned to HR officers + number of applicants in each vacancy. 
     */
    public static TransactionResponse getVacancyListToBeCancelledNotAssignedToHrOfficer()
    {
        TransactionResponse response = new TransactionResponse();
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(
            DbAccessConstants.spGetListOfVacancyNotAssigned2HROfficerTobeCancelled, null);
        DataTable dataTable = dpOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);
        return response;
    }

    /**
     * Return list of vacancy which have this status. 
     */
    public static TransactionResponse getAllActiveVacancy(string status)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@status", status);
        statusParams.Add("@districtId", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAllActiveVacancy, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
     * Return list of vacancy which have this status. 
     */
    public static TransactionResponse getAllActiveVacancyByDateInterval(string status, string startDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@status", status);
        statusParams.Add("@startDate", startDate);
        statusParams.Add("@endDate", endDate);
        statusParams.Add("@districtId", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAllActiveVacancyByDateInterval, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }


    /**
     * Return list of vacancy which is assigned to the current loggedIn Officer 
     */
    public static TransactionResponse getVacancyAssignedToProcessorOrAssessor(string LoggedInID, string spName)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@EmpId", LoggedInID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(spName, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }


    /**
     * Return list of all managarial job positions
     */
    public static TransactionResponse getAllManagerialJobPosition()
    {
        TransactionResponse response = new TransactionResponse();

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetAllManagerialJobposition, null);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
     * Return list of vacancy which have this status. 
     */
    public static TransactionResponse getListNotannouncedVacancy()
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@districtId", PageAccessManager.getDistrictID());

        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetListOfNotAnnouncedVacancy, parameters);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
     * Return list of vacancy which have this status. 
     */
    public static TransactionResponse getAllVacancyAssignedToProcessor2ndPhase(string status)
    {
        // Get a reference to the currently logged on user 
        MembershipUser currentUser = Membership.GetUser();

        //get employee detail of the current user. 
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);
        
        //Pass Stored Procedure Name and parameter list. 
        IDictionary<string, object> parameter = new Dictionary<string, object>();
        
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@EmpId", employee.EmpID);
        statusParams.Add("@status", status);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetVacancyListForProcessor2ndPhase, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
     * Return list of vacancy which Is still active
     */
    public static TransactionResponse getAllActiveVacancyWithAppCompNull(string status)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@status", status);
        statusParams.Add("@districtId", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAllActiveVacancyWithApplRegFinishedNull, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
     * Return list of all currently active vacancies those profile date is NULL. 
     */
    public static TransactionResponse getAllActiveVacancyWithProfileDateNull(string status)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@status", status);
        statusParams.Add("@districtId", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list.
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAllActiveVacancyWithProfileDateNull, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
     * Return list of all currently active vacancies those applicant registeration complete is NULL. 
     */
    public static TransactionResponse getAllActiveVacancyWithAppRegComplete(string status)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@status", status);
        statusParams.Add("@districtId", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAllActiveVacancyWithApplRegFinishedNull, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    /**
 * Return list of all currently active vacancies those applicant registeration complete is Not NULL. 
 */
    public static TransactionResponse getAllActiveVacancyWithAppRegCompleteNotNull(string status)
    {
        TransactionResponse response = new TransactionResponse();
        IDictionary<string, object> statusParams = new Dictionary<string, object>();
        statusParams.Add("@status", status);
        statusParams.Add("@districtId", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAllActiveVacancyWithProfileDateNotNull, statusParams);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }


    /**
     * Return list of vacancy which have this status. 
     */
    public static TransactionResponse getNotAnnouncedPromotion()
    {
        // Get a reference to the currently logged on user 
        MembershipUser currentUser = Membership.GetUser();
        
        //get employee detail of the current user. 
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

        TransactionResponse response = new TransactionResponse();

        //Pass Stored Procedure Name and parameter list. 
        IDictionary<string, object> parameter = new Dictionary<string, object>();
        parameter.Add("@hROfficerID", employee.EmpID);
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetNotAnnouncedPromotion, parameter);
        try
        {
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setSuccess(false);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse registerVacancyEvaluation()
    {
        if (FORM1.Equals(vacanyEvaluationForm.FormType))
        {
            return registerEvaluationusingForm1();
        }
        return registerEvaluationusingForm2();
    }

    private TransactionResponse registerEvaluationusingForm1()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> evaluationForm1Params = new Dictionary<string, object>();

        evaluationForm1Params.Add("@EID", vacanyEvaluationForm.EmpId);
        evaluationForm1Params.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
        evaluationForm1Params.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
        evaluationForm1Params.Add("@education_level_mark", vacanyEvaluationForm.EducationLevelMark);
        evaluationForm1Params.Add("@general_work_expr", vacanyEvaluationForm.GeneralWorkExpr);
        evaluationForm1Params.Add("@specific_work_expr", vacanyEvaluationForm.SpecificWorkRxpr);
        evaluationForm1Params.Add("@recommendation_of_mgr_line", vacanyEvaluationForm.RecommendationOfMgrLine);
        evaluationForm1Params.Add("@remark", vacanyEvaluationForm.Remark);
        evaluationForm1Params.Add("@applicantType", vacanyEvaluationForm.ApplicantType);

        //call store to DB mathod and get reponse. 
        return sendInsertApplicantFormToDb(DbAccessConstants.spAddNewEvaluationUsingForm1, evaluationForm1Params);
    }

    private TransactionResponse registerEvaluationusingForm2()
    {

        //Add List of Arguments for new employee
        IDictionary<string, object> evaluationForm2Params = new Dictionary<string, object>();

        evaluationForm2Params.Add("@EID", vacanyEvaluationForm.EmpId);
        evaluationForm2Params.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
        evaluationForm2Params.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
        evaluationForm2Params.Add("@education_level_mark", vacanyEvaluationForm.EducationLevelMark);
        evaluationForm2Params.Add("@related_work_experiance", vacanyEvaluationForm.RelatedWorExperiance);
        evaluationForm2Params.Add("@remark", vacanyEvaluationForm.Remark);
        evaluationForm2Params.Add("@applicantType", vacanyEvaluationForm.ApplicantType);

        //call store to DB mathod and get reponse. 
        return sendInsertApplicantFormToDb(DbAccessConstants.spAddNewEvaluationUsingForm2, evaluationForm2Params);
    }

    private TransactionResponse sendInsertApplicantFormToDb(string spName, IDictionary<string, object> parameters)
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(spName, parameters);

        TransactionResponse response = new TransactionResponse();
        try
        {
            //call store to DB mathod and get reponse. 
            storeToDb.instertNewRecord();
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_EVALUATION_REGISTERED_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_APP_FORM_KEY_ERROR);
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

    /**
     * returns list of employee a processor can Evaluate/process
     */
    public List<String> getListEmpIDForProcessor(string EvaluatedEmployeeID)
    {
        return getListOfEmployeeForResponsibleHROfficer(DbAccessConstants.spGetListOfEployeeForProcessor,
            vacancy.VacancyNo, vacancy.PostedDate, vacancy.ResponsibleProcessorEID, EvaluatedEmployeeID);
    }

    /**
     * returns list of employee an accessor can check
     */
    public List<String> getListEmpIDForChecker(string EvaluatedEmployeeID)
    {
        return getListOfEmployeeForResponsibleHROfficer(DbAccessConstants.spGetListOfEployeeForChecker,
            vacancy.VacancyNo, vacancy.PostedDate, vacancy.ResponsibleAccessorEID, EvaluatedEmployeeID);
    }


    /**
     * returns list of employee a processor can Evaluate/process for the second phase
     */
    public List<String> getListEmpIDForProcessorSecondPhase(string EvaluatedEmployeeID)
    {
        return getListOfEmployeeForResponsibleHROfficer(DbAccessConstants.spGetListOfEployeeForProcessorSecondPhase,
            vacancy.VacancyNo, vacancy.PostedDate, vacancy.ResponsibleProcessorEID, EvaluatedEmployeeID);
    }
    private List<String> getListOfEmployeeForResponsibleHROfficer(string spName, string vacancyNo,
        string vacancyDate, string checkerEmpId, string evaluatedEmployeeID)
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> employeesToBeProcessed = new Dictionary<string, object>();


        employeesToBeProcessed.Add("@proc_EID", checkerEmpId);
        employeesToBeProcessed.Add("@vacancy_No", vacancyNo);
        // date can be a string type
        //the date formst should be : YYYY-MM-DD
        //eg: "2015-02-28"
        employeesToBeProcessed.Add("@vacancyDateTemp", vacancyDate);
        employeesToBeProcessed.Add("@applicantId", evaluatedEmployeeID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(spName, employeesToBeProcessed);

        //call store to DB mathod and get reponse. 
        return dpOperation.getRecordAsListOfString("EID");
    }

    public TransactionResponse addNewVacancy()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> vacancyParameters = new Dictionary<string, object>();

        vacancyParameters.Add("@vacancy_No", vacancy.VacancyNo);
        vacancyParameters.Add("@Vancay_title", vacancy.VancayTitle);
        vacancyParameters.Add("@branch_id", vacancy.BranchId);
        vacancyParameters.Add("@vancy_deadline", vacancy.VancyDeadline);
        vacancyParameters.Add("@vacancy_for_JobGrade", vacancy.VacancyforJobGrade);
        vacancyParameters.Add("@vacancy_opened_for", vacancy.VacancyOpenedFor);
        vacancyParameters.Add("@vacancy_evaluation_form", vacancy.VacancyEvaluationForm);
        vacancyParameters.Add("@year_gen_required", vacancy.YearRequiredforGeneral);
        vacancyParameters.Add("@year_spec_required", vacancy.YearRequiredforSpec);
        vacancyParameters.Add("@general_wrk_expr_percent", vacancy.GeneralWrkExprPercent);
        vacancyParameters.Add("@specific_wrk_expr_percent", vacancy.SpecificWrkExprPercent);
        vacancyParameters.Add("@related_wrk_expr_percent", vacancy.RelatedWrkExprPercent);
        vacancyParameters.Add("@examination_percent", vacancy.ExaminationPercent);
        vacancyParameters.Add("@manager_Recom_percent", vacancy.ManagerRecPercent);
        vacancyParameters.Add("@interview_percent", vacancy.InterviewPercent);
        vacancyParameters.Add("@JobDescription", vacancy.JobDescription);
        vacancyParameters.Add("@JobRequirement", vacancy.JobRequirement);
        vacancyParameters.Add("@JobBenefit", vacancy.SalaryAndBenefit);
        vacancyParameters.Add("@districtId", PageAccessManager.getDistrictID());


        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spAddNewVacancy, vacancyParameters);

        TransactionResponse response = new TransactionResponse();
        try
        {
            //call store to DB mathod and get reponse. 
            storeToDb.instertNewRecord();
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_REGISTER_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_VACANCY_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_VACANCY_REGISTER);
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

    /**
     * Returns list of Vacancy assigned to the HR officer (PROCESSOR | CHECKER)
     */
    public List<string> getVacancyListForHrOfficer(string hrOfficerEmpid, string hrOfficerType)
    {
        if (PageConstants.PROCESSOR.Equals(hrOfficerType))
        {
            return getProcessorVacancyList(hrOfficerEmpid);
        }
        else if (PageConstants.CHECKER.Equals(hrOfficerType))
        {
            return getCheckerVacancyList(hrOfficerEmpid);
        }

        //Otherwise return empty vacancy list. 
        return new List<string>();
    }

    //PROCESSOR
    private List<string> getProcessorVacancyList(string hrOfficerEmpid)
    {
        return getVacancyList(DbAccessConstants.spGetVacancyListForProcessor, hrOfficerEmpid);
    }

    //CHECKER
    private List<string> getCheckerVacancyList(string hrOfficerEmpid)
    {
        return getVacancyList(DbAccessConstants.spGetVacancyListForChecker, hrOfficerEmpid);
    }

    private List<string> getVacancyList(string empid, string spName)
    {
        //pass parameters
        IDictionary<string, object> employeeIDParam = new Dictionary<string, object>();
        employeeIDParam.Add("@EmpId", empid);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(spName, employeeIDParam);
        return dpOperation.getRecordAsListOfString("vacancy_detail");
    }


    /**
     * get list of EMID not evaluated
     */
    public List<string> getNotEvaluatedEmployees(MembershipUser currentUser, string hrOfficerType)
    {
        //get employee detail of the current user. 
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);
        if (employee == null)
        {
            //find away to return Transaction Response always. 
            // return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        if (PageConstants.PROCESSOR.Equals(hrOfficerType))
        {
            return getNotProcessedEmployees(employee);
        }
        else if (PageConstants.CHECKER.Equals(hrOfficerType))
        {
            return getNotcheckedEmployees(employee);
        }

        //Otherwise we have to stop the flow, this call is with invalid data.. 
        throw new InvalidOperationException();
    }

    private List<string> getNotcheckedEmployees(Employee employee)
    {
        return getNotProcessedEmployees(employee, DbAccessConstants.spGetNotCheckedEmployee);
    }

    private List<string> getNotProcessedEmployees(Employee employee)
    {
        return getNotProcessedEmployees(employee, DbAccessConstants.spGetNotProcessedEmployee);
    }

    private List<string> getNotProcessedEmployees(Employee employee, string spName)
    {
        //pass parameters
        IDictionary<string, object> employeeIDParam = new Dictionary<string, object>();
        employeeIDParam.Add("@proc_EID", employee.EmpID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(spName, employeeIDParam);
        return dpOperation.getRecordAsListOfString("EID");
    }

    public TransactionResponse updateVacancyStatusToAssgnedFromReAdvertise()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = sendVacancyUpdateStatus(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_CHANGED_TO_ASSIGNED_FROM_READVERTISE);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_CHANGED_TO_ASSIGNED_FROM_READVERTISE_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_READVERTISE_TO_REASSIGNED_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_CHANGED_TO_ASSIGNED_FROM_READVERTISE_FAIL);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    public TransactionResponse updateVacancyEvaluation()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> vacancyParameters = new Dictionary<string, object>();

        vacancyParameters.Add("@vacancyNo", vacancy.VacancyNo);
        vacancyParameters.Add("@postedDate", vacancy.PostedDate);
        vacancyParameters.Add("@responsibleProcessor_EID", vacancy.ResponsibleProcessorEID);
        vacancyParameters.Add("@reponsibleAccessorEID", vacancy.ResponsibleAccessorEID);
        vacancyParameters.Add("@processorStartDate", vacancy.ProcessorStartDate);
        vacancyParameters.Add("@processorEendDate", vacancy.ProcessorEndDate);
        vacancyParameters.Add("@accessorStartDate", vacancy.AccessorStartDate);
        vacancyParameters.Add("@accessorEndDate", vacancy.AccessorEndDate);
        vacancyParameters.Add("@status", vacancy.Status);


        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateVacancyEvaluators, vacancyParameters);
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
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_UPDATE_VACANCY);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
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

    public TransactionResponse checkIfItIslreadyEvaluatedEmployee(string hrOfficerType, Employee employee)
    {
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@EmpId", vacanyEvaluationForm.EmpId);
        parameters.Add("@vacancyNo", vacanyEvaluationForm.VacancyNo);
        parameters.Add("@vacancyDate", vacanyEvaluationForm.VacancyDate);
        parameters.Add("@formType", vacanyEvaluationForm.FormType);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetAlreadyEvaluatedEmployee, parameters);
        TransactionResponse response = new TransactionResponse();

        //get Record. 
        DataTable dataTable = null;
        VacancyEvaluationForm applicantRatingDetail = null;
        try
        {
            dataTable = dpOperation.getRecord();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    applicantRatingDetail = getAppicantDetailForThisFormType(row, vacanyEvaluationForm.FormType);
                    //there sould only be one record returned
                    break;
                }
            }

            //EMPLOYEE is already evaluated, Now check if employee is also processed and/or checked
            if (applicantRatingDetail != null)
            {
                response.setSuccess(true);
                response.Data = applicantRatingDetail;
                manageProcessingAndCheckingPermission(employee, hrOfficerType, response);
                return response;
            }

            response.setSuccess(false);
            return response;
        }
        catch (SqlException ex)
        {
            response.setSuccess(false);
            return response;
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return response;
        }
    }

    public TransactionResponse checkIfItIslreadyEvaluatedEmployeeForSecondPhase(Employee employee)
    {
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@EmpId", vacanyEvaluationForm.EmpId);
        parameters.Add("@vacancyNo", vacanyEvaluationForm.VacancyNo);
        parameters.Add("@vacancyDate", vacanyEvaluationForm.VacancyDate);
        parameters.Add("@formType", vacanyEvaluationForm.FormType);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetAlreadyEvaluatedEmployeeForSecondPhase, parameters);
        TransactionResponse response = new TransactionResponse();

        //get Record. 
        DataTable dataTable = null;
        VacancyEvaluationForm applicantRatingDetail = null;
        try
        {
            dataTable = dpOperation.getRecord();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    applicantRatingDetail = getAppicantDetailForThisFormType(row, vacanyEvaluationForm.FormType);
                    //there sould only be one record returned
                    break;
                }
            }

            if (applicantRatingDetail.InterviewResult != null || applicantRatingDetail.ExaminationResult != null
                || applicantRatingDetail.InterviewResultRecommendation != null)
            {
                response.setSuccess(true);
                response.Data = applicantRatingDetail;
                response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_ALREADY_PROCESSED_BY_YOU);
                response.setMessageType(TransactionResponse.SeverityLevel.INFO);
                return response;
            }

            response.setSuccess(false);
            return response;
        }
        catch (SqlException ex)
        {
            response.setSuccess(false);
            return response;
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return response;
        }
    }

    /**
     * Checkes if Processor and Checker has a permission to evaluate the current employee. 
     */
    private void manageProcessingAndCheckingPermission(Employee replyEmployee, string hrOfficerType, TransactionResponse response)
    {
        if (PageConstants.PROCESSOR.Equals(hrOfficerType))
        {
            if (replyEmployee.CheckedDate != null && replyEmployee.ProcessedDate != null)
            {
                response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_ALREADY_CHECKED);
                response.setMessageType(TransactionResponse.SeverityLevel.WARNING);

                //log permission refusal reason. 
                LoggerManager.LogInfo("Applicant : " + replyEmployee.EmpID + " can not be processed;  Because it is already checked. Checked date : "
                    + replyEmployee.CheckedDate + "Processed date : " + replyEmployee.ProcessedDate, logger);
            }
            else
            {
                response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_ALREADY_PROCESSED_BY_YOU);
                response.setMessageType(TransactionResponse.SeverityLevel.INFO);
            }
        }

        else if (PageConstants.CHECKER.Equals(hrOfficerType))
        {
            if (replyEmployee.ProcessedDate == null)
            {
                response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_NOT_PROCESSED_YET);
                response.setMessageType(TransactionResponse.SeverityLevel.WARNING);


                LoggerManager.LogInfo("Applicant : " + replyEmployee.EmpID + " can not be checked;  Because it is not processed yet. Checked date : "
                    + replyEmployee.CheckedDate + "Processed date : " + replyEmployee.ProcessedDate, logger);
            }
            else
            {
                //CHECK if the vacancy is still in ASSIGNED STATE
                Vacancy currentVacancy = new Vacancy();
                currentVacancy.VacancyNo = vacanyEvaluationForm.VacancyNo;
                currentVacancy.PostedDate = vacanyEvaluationForm.VacancyDate;
                //initialise before calling method : getCurrentStatusOfVacancy
                this.vacancy = currentVacancy;
                TransactionResponse vacancyStatusResp = getCurrentStatusOfVacancy();

                string vacancyStatus = "";
                if (vacancyStatusResp.Data != null)
                {
                    vacancyStatus = (string)vacancyStatusResp.Data;
                }

                // check if Vacancy is still in assigned state  or processing done state, but not checking done state
                if (!vacancyStatus.Equals("") && (VacancyConstants.VACANCY_ASSIGNED_TO_HR_OFFICERS.Equals(vacancyStatus)
                    || VacancyConstants.VACANCY_PROCESSING_DONE.Equals(vacancyStatus)) && replyEmployee.CheckedDate != null)
                {
                    response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_ALREADY_CHECKED_BY_YOU);
                    response.setMessageType(TransactionResponse.SeverityLevel.INFO);
                }

                //check if the status is neither not-in-Assigned-state nor in processing done state
                //it means the vacancy is either cancelled/readvertised,... OR checking is done. state. 
                else if (!VacancyConstants.VACANCY_ASSIGNED_TO_HR_OFFICERS.Equals(vacancyStatus) 
                    && !VacancyConstants.VACANCY_PROCESSING_DONE.Equals(vacancyStatus))
                {
                    response.setMessage(DBOperationErrorConstants.M_VACANCY_NOT_IN_ASSIGNED_STATE);
                    response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
                }
            }
        }
    }

    private VacancyEvaluationForm getAppicantDetailForThisFormType(DataRow row, string formType)
    {
        //we should return null if forType is not good one. 
        VacancyEvaluationForm applicantRatingDetail = null;
        if (FORM1.Equals(formType))
        {
            applicantRatingDetail = new VacancyEvaluationForm();
            applicantRatingDetail.EmpId = row["EID"].ToString();
            applicantRatingDetail.VacancyNo = row["vacancy_No"].ToString();
            applicantRatingDetail.VacancyDate = row["vacancy_date"].ToString();
            applicantRatingDetail.EducationLevelMark = row["education_level_mark"].ToString();
            applicantRatingDetail.GeneralWorkExpr = row["general_work_expr"].ToString();
            applicantRatingDetail.SpecificWorkRxpr = row["specific_work_expr"].ToString();
            applicantRatingDetail.RecommendationOfMgrLine = row["recommendation_of_mgr_line"].ToString();
            applicantRatingDetail.InterviewResultRecommendation = row["interview_result_recommendation"].ToString();
            applicantRatingDetail.Remark = row["remark"].ToString();
        }

        else if (FORM2.Equals(formType))
        {
            applicantRatingDetail = new VacancyEvaluationForm();
            applicantRatingDetail.EmpId = row["EID"].ToString();
            applicantRatingDetail.VacancyNo = row["vacancy_No"].ToString();
            applicantRatingDetail.VacancyDate = row["vacancy_date"].ToString();
            applicantRatingDetail.EducationLevelMark = row["education_level_mark"].ToString();
            applicantRatingDetail.RelatedWorExperiance = row["related_work_experiance"].ToString();
            applicantRatingDetail.ExaminationResult = row["examination_result"].ToString();
            applicantRatingDetail.InterviewResult = row["interview_result"].ToString();
            applicantRatingDetail.Remark = row["remark"].ToString();
        }
        return applicantRatingDetail;
    }


    public TransactionResponse getCurrentStatusOfVacancy()
    {
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@vacancyNo", vacancy.VacancyNo);
        parameters.Add("@vacancyDate", vacancy.PostedDate);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetCurrentStatusOfVacancy, parameters);
        TransactionResponse response = new TransactionResponse();

        //get Record. 
        DataTable dataTable = null;
        try
        {
            dataTable = dpOperation.getRecord();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    response.Data = row["status"].ToString();
                    //there sould only be one record returned
                    break;
                }
            }
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_STAUS_CHECK_FAIL);
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

    public TransactionResponse updateApplicantRating(int hrOfficerType, string applicantType)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            DBOperationsUtil dpOperation = getDBOperationToUpdateApplicantRating(parameters, hrOfficerType, applicantType);
            dpOperation.updateRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_RATING_UPDATE_SUCCESS);
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

    public TransactionResponse updateApplicantRatingForSecondPhase(string applicantType)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            DBOperationsUtil dpOperation = getDBOperationToUpdateApplicantRating4SecondPhase(parameters, applicantType);
            dpOperation.updateRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_RATING_UPDATE_SUCCESS);
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

    private DBOperationsUtil getDBOperationToUpdateApplicantRating(IDictionary<string, object> parameters, int hrOfficerType, string applicantType)
    {
        DBOperationsUtil dpOperation = null;

        if (FORM1.Equals(vacanyEvaluationForm.FormType))
        {
            parameters.Add("@EID", vacanyEvaluationForm.EmpId);
            parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
            parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
            parameters.Add("@education_level_mark", vacanyEvaluationForm.EducationLevelMark);
            parameters.Add("@general_work_expr", vacanyEvaluationForm.GeneralWorkExpr);
            parameters.Add("@specific_work_expr", vacanyEvaluationForm.SpecificWorkRxpr);
            parameters.Add("@recommendation_of_mgr_line", vacanyEvaluationForm.RecommendationOfMgrLine);
            parameters.Add("@remark", vacanyEvaluationForm.Remark);
            parameters.Add("@applicantType", applicantType);
            parameters.Add("@hrOfficerType", hrOfficerType);

            //Pass Stored Procedure Name and parameter list. 
            dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateApplicantRatingForm1, parameters);
            return dpOperation;
        }

        //ELSE it is form 2
        parameters.Add("@EID", vacanyEvaluationForm.EmpId);
        parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
        parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
        parameters.Add("@education_level_mark", vacanyEvaluationForm.EducationLevelMark);
        parameters.Add("@related_work_experiance", vacanyEvaluationForm.RelatedWorExperiance);
        parameters.Add("@remark", vacanyEvaluationForm.Remark);
        parameters.Add("@applicantType", applicantType);
        parameters.Add("@hrOfficerType", hrOfficerType);

        //Pass Stored Procedure Name and parameter list. 
        dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateApplicantRatingForm2, parameters);
        return dpOperation;
    }


    private DBOperationsUtil getDBOperationToUpdateApplicantRating4SecondPhase(IDictionary<string, object> parameters, string applicantType)
    {
        DBOperationsUtil dpOperation = null;

        if (FORM1.Equals(vacanyEvaluationForm.FormType))
        {
            parameters.Add("@EID", vacanyEvaluationForm.EmpId);
            parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
            parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
            parameters.Add("@interview_result_recommendation", vacanyEvaluationForm.InterviewResult);
            parameters.Add("@applicantType", applicantType);

            //Pass Stored Procedure Name and parameter list. 
            dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateApplicantRatingphase2Form1, parameters);
            return dpOperation;
        }

        //ELSE it is form 2
        parameters.Add("@EID", vacanyEvaluationForm.EmpId);
        parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
        parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
        parameters.Add("@interview_result", vacanyEvaluationForm.InterviewResult);
        parameters.Add("@examination_result", vacanyEvaluationForm.ExaminationResult);
        parameters.Add("@applicantType", applicantType);

        //Pass Stored Procedure Name and parameter list. 
        dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateApplicantRatingphase2Form2, parameters);
        return dpOperation;
    }


    /**
     * Return list of HR Officer processor rating report
     */
    public static TransactionResponse getHROfficerProcessorRatingReport(string EID, string startDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@EID", EID);
        parameters.Add("@startDate", startDate);
        parameters.Add("@endDate", endDate);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spGetHROfficerProcessorRatingReport, parameters);
        DataTable dataTable = gvOperation.getRecord();
        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }

    /**
    * Return list of HR Officer checker rating report
    */
    public static TransactionResponse getHROfficerCheckerRatingReport(string EID, string startDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@EID", EID);
        parameters.Add("@startDate", startDate);
        parameters.Add("@endDate", endDate);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spGetHROfficerCheckerRatingReport, parameters);
        DataTable dataTable = gvOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }


    /**
     * Return list of All HR Officer processor rating report
     */
    public static TransactionResponse getAllHROfficerProcessorRatingReport(string startDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@startDate", startDate);
        parameters.Add("@endDate", endDate);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spGetAllHROfficerProcessorRatingReport, parameters);
        DataTable dataTable = gvOperation.getRecord();
        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }

    /**
    * Return list of All HR Officer checker rating report
    */
    public static TransactionResponse getAllHROfficerCheckerRatingReport(string startDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@startDate", startDate);
        parameters.Add("@endDate", endDate);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spAllGetHROfficerCheckerRatingReport, parameters);
        DataTable dataTable = gvOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }


    /**
 * Return list of Applciant for the selected vacancy.
 */
    public static TransactionResponse getApplicantDetialforProfileRequest(Vacancy vacancy)
    {

        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@vNumber", vacancy.VacancyNo);
        parameters.Add("@vPostDate", vacancy.PostedDate);
        parameters.Add("@status", vacancy.Status);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantDetialForProfileRequest, parameters);
        DataTable dataTable = gvOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }

    /**
     * Return list of Applciant for the selected vacancy.
     */
    public static TransactionResponse getApplicantDetial(Vacancy vacancy)
    {

        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@vNumber", vacancy.VacancyNo);
        parameters.Add("@vPostDate", vacancy.PostedDate);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantDetial, parameters);
        DataTable dataTable = gvOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }

    /**
     * Return list of HR Officer or Clerk
     */
    public static TransactionResponse getHROfficerOrClerk(string SPName)
    {

        TransactionResponse response = new TransactionResponse();

        DBOperationsUtil gvOperation = new DBOperationsUtil(SPName, null);
        DataTable dataTable = gvOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);

        return response;
    }

    /**
     * Return list of Applciant for the selected vacancy.
     */
    public static TransactionResponse getApplicantRegCompVacancy(Vacancy vacancy)
    {

        TransactionResponse response = new TransactionResponse();

        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@vacancy_No", vacancy.VacancyNo);
        parameters.Add("@posted_date", vacancy.PostedDate);

        DBOperationsUtil gvOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantRegCompVacancy, parameters);
        DataTable dataTable = gvOperation.getRecord();

        response.Data = dataTable;
        response.setSuccess(true);
        response.setMessage(DBOperationErrorConstants.M_VACANCY_REVERSE_WARNING);

        return response;
    }

    public TransactionResponse getNotEvaluatedApplicants(int hrOfficerType)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
            parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
            parameters.Add("@hrOfficerType", hrOfficerType);
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetNotEvaluatedApplicants, parameters);
            List<string> listOfNotEvaluatedEmployee = dpOperation.getRecordAsListOfString("EID");
            if (listOfNotEvaluatedEmployee != null && listOfNotEvaluatedEmployee.Count > 0)
            {
                //convert list of string to one string seprated by ','
                response.Data = String.Join(", ", listOfNotEvaluatedEmployee.ToArray());
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETION_NOT_POSSIBLE);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_COMPLETION_NOT_POSSIBLE);
                response.setSuccess(false);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETION_WARNING);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_VERIFY_COMPLETE_VANCANCY);
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

    //The method to get the next applicant for 1st phase evaluation
    public List<string> getNextEvaluatedApplicants1stPhase(int hrOfficerType)
    {
        List<string> listOfNotEvaluatedEmployee = null;
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
            parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
            parameters.Add("@hrOfficerType", hrOfficerType);
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetNotEvaluatedApplicants, parameters);
            listOfNotEvaluatedEmployee = dpOperation.getRecordAsListOfString("EID");
            if (listOfNotEvaluatedEmployee != null && listOfNotEvaluatedEmployee.Count > 0)
            {
                response.Data = listOfNotEvaluatedEmployee.ToArray();
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_VERIFY_COMPLETE_VANCANCY);
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
        return listOfNotEvaluatedEmployee;
    }



    public TransactionResponse getNotEvaluatedApplicantsForSecondPhase(int formType)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
            parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
            parameters.Add("@formType", formType);
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetNotEvaluatedApplicantsforSecondPhase, parameters);
            List<string> listOfNotEvaluatedEmployee = dpOperation.getRecordAsListOfString("EID");
            if (listOfNotEvaluatedEmployee != null && listOfNotEvaluatedEmployee.Count > 0)
            {
                //convert list of string to one string seprated by ','
                response.Data = String.Join(", ", listOfNotEvaluatedEmployee.ToArray());
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETION_NOT_POSSIBLE);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_COMPLETION_NOT_POSSIBLE);
                response.setSuccess(false);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETION_WARNING);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_VERIFY_COMPLETE_VANCANCY);
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

    //The method to get the next applicant for 2nd phase evaluation
    public List<string> getNextEvaluatedApplicants2ndPhase(int formType)
    {
        List<string> listOfNotEvaluatedEmployee = null;
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacanyEvaluationForm.VacancyNo);
            parameters.Add("@vacancy_date", vacanyEvaluationForm.VacancyDate);
            parameters.Add("@formType", formType);
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetNotEvaluatedApplicantsforSecondPhase, parameters);
            listOfNotEvaluatedEmployee = dpOperation.getRecordAsListOfString("EID");
            if (listOfNotEvaluatedEmployee != null && listOfNotEvaluatedEmployee.Count > 0)
            {
                response.Data = listOfNotEvaluatedEmployee.ToArray();
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_VERIFY_COMPLETE_VANCANCY);
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
        return listOfNotEvaluatedEmployee;
    }

    private bool sendVacancyUpdateStatus(TransactionResponse response)
    {
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacancy.VacancyNo.Trim());
            parameters.Add("@posted_date ", vacancy.PostedDate.Trim());
            parameters.Add("@status", vacancy.Status);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateVacancyStatus, parameters);
            return dpOperation.updateRecord();
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
            return false;
        }
    }

    public TransactionResponse updateVacancyStatusToCancelled()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = sendVacancyUpdateStatus(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_CANCELLED_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_CANCELLATION_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_CANCELLATION_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_CANCELLATION_FAIL);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    public TransactionResponse updateVacancyStatusforVacancyCompletion()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = sendVacancyUpdateStatus(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETED_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETING_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_COMPLETION_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_COMPLETING_FAIL);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    public TransactionResponse updateVacancyStatusToVacActiveforVacancyReversal()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = sendVacancyUpdateStatus(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_REVERSED_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_REVERSE_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_REVERSE_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_REVERSE_FAIL);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    //this methode is used to update vacancy status from profile arrived to readvertise
    //and also set profile arrived & applicant registeration to NULL
    public TransactionResponse updateVacancyStatusToReadvertiseFromProfileArrived()
    {
        TransactionResponse response = new TransactionResponse();
        bool isUpdated = false;
        try
        {
            try
            {
                //Add List of Arguments for new employee
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@vacancy_No", vacancy.VacancyNo.Trim());
                parameters.Add("@posted_date ", vacancy.PostedDate.Trim());
                parameters.Add("@status", vacancy.Status);
                parameters.Add("@vancy_deadline", vacancy.VancyDeadline);

                DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spReadvertiseVacFromProfileArr, parameters);
                isUpdated = dpOperation.updateRecord();
            }
            //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
            catch (Exception ex)
            {
                //Write this exception to file for investigation of the issue later. 
                logException(ex);
                LoggerManager.upDateWithGenericErrorMessage(response);
            }

            if (isUpdated)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISED_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISED_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_READVERTISE_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISED_FAIL);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }


    /*
     * Update Profile arrived date. 
     */

    public TransactionResponse updateProfileArrivedDateAndVacancyStatus()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@posted_date ", vacancy.PostedDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateProfileArrivedDate, parameters);
            //COUNT NUMBER OF ROW UPDATED.
            bool updateProfDate = dpOperation.updateRecord();
            if (updateProfDate)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_PROFILE_DATE_REGISTERED);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_PROFILE_DATE_NOT_REGISTERED);
                response.setErrorCode(DBOperationErrorConstants.E_PROFILE_ARRIVED_DATE_UPDATE_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.E_PROFILE_ARRIVED_DATE_UPDATE_FAILED);
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

    /*
    * Update applicant registration complete date. 
    */

    public TransactionResponse updateApplicantRegisterationCompleteDate()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@posted_date ", vacancy.PostedDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateApplicantRegistrationFinishedDate, parameters);
            //COUNT NUMBER OF ROW UPDATED.
            bool updateApplRegComplete = dpOperation.updateRecord();
            if (updateApplRegComplete)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_APPLICANT_REGISTERED_COMPLETE);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_PROFILE_DATE_NOT_REGISTERED);
                response.setErrorCode(DBOperationErrorConstants.E_PROFILE_ARRIVED_DATE_UPDATE_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.E_PROFILE_ARRIVED_DATE_UPDATE_FAILED);
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

    public TransactionResponse getApplicantRegistrationComplete()
    {
        //Add List of Arguments
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@vacancy_No", vacancy.VacancyNo);
        parameters.Add("@posted_date ", vacancy.PostedDate);

        TransactionResponse response = new TransactionResponse();

        DBOperationsUtil operation = new DBOperationsUtil(DbAccessConstants.spIsApplicantRegisterComplete, parameters);
        DataTable dataTable = operation.getRecord();
        if (dataTable.Rows.Count == 0)
        {
            response.Data = dataTable;
            response.setSuccess(true);
        }
        else
        {
            response.setSuccess(false);
            response.setMessage(DBOperationErrorConstants.M_APPLICANT_COMPLETE_REGISTERED_ALREADY);
        }

        return response;
    }

    public TransactionResponse isProfileArrivedDateRegistered()
    {
        //Add List of Arguments
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@vacancy_No", vacancy.VacancyNo);
        parameters.Add("@posted_date ", vacancy.PostedDate);

        TransactionResponse response = new TransactionResponse();

        response = getApplicantRegistrationComplete();

        if (!response.isSuccessful())
        {
            DBOperationsUtil operation = new DBOperationsUtil(DbAccessConstants.spIsProfileDateRegistered, parameters);
            DataTable dataTable = operation.getRecord();
            if (dataTable != null && dataTable.Rows.Count == 0)
            {
                response.Data = dataTable;
                response.setSuccess(true);
            }
            else
            {
                response.setSuccess(false);
                response.setMessage(DBOperationErrorConstants.M_PROFILE_ARRIVED_DATE_REGISTERED_ALREADY);
            }
        }
        else
        {
            response.setSuccess(false);
            response.setMessage(DBOperationErrorConstants.M_APPLICANT_COMPLETE_NOT_REGISTERED);
        }

        return response;
    }

    /**
     * Get complete vacancy evaluation report. - PHASE 1 and PHASE 2
     */
    public TransactionResponse getVacancyEvaluationResult(string reportPhase)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            DBOperationsUtil dpOperation = null;
            if (PageConstants.REPORT_PHASE1.Equals(reportPhase))
            {
                dpOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantPhas1Evaluation, parameters);
            }
            else
            {
                dpOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantPhas2Evaluation, parameters);
            }

            DataTable updateApplRegComplete = dpOperation.getRecord();
            if (updateApplRegComplete == null || updateApplRegComplete.Rows.Count == 0)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
                response.setMessage(DBOperationErrorConstants.M_NO_VACANCY_EVALUTION_RESULT);
                response.setErrorCode(DBOperationErrorConstants.E_NO_EVALUATION_RESULT);
                response.setSuccess(false);
                return response;
            }

            return rankApplicants(updateApplRegComplete, response);
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_GET_VACANCY_EVALUATION_RESULT);
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

    /**
     * Get applicant result after checking done, or while interview or exam result is entering
     */
    public TransactionResponse getApplicantResultAfterEvaluation(Vacancy vacancy)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantResultAfterEvaluation, parameters);

            DataTable appResult = dpOperation.getRecord();

            response.Data = appResult;
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_REPORT_GENERATED_SUCCESS);
            response.setSuccess(true);
            return response;
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_APPLICANT_RESULT);
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

    /**
     * Get applicant result after checking done, or while interview or exam result is entering
     */
    public TransactionResponse getApplicantPassedToSecPhase(Vacancy vacancy)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantPassedToSecPhase, parameters);

            DataTable appResult = dpOperation.getRecord();

            response.Data = appResult;
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_REPORT_GENERATED_SUCCESS);
            response.setSuccess(true);
            return response;
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_APPLICANT_RESULT);
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

    /**
     * Get recently selected, lateral and penality employee
     */
    public TransactionResponse getRecentlySelectedEmpResult()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@vacancy_date", vacancy.PostedDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetRecSelectedLateralPenality, parameters);

            DataTable recSelectedEmployee = dpOperation.getRecord();

            //if (recSelectedEmployee != null && recSelectedEmployee.Rows.Count != 0)
            //{
            response.Data = recSelectedEmployee;
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
            return response;
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_GET_RECENTLY_SELECTED_RESULT);
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

    /**
        * Ranks applicants according to their total evaluation result. 
        * If 2 applicants have the same evaluation result the same rank will be given to them
        * and the next rank will be skipped
        * example: Applicants in their evaluation result order: A, B|C, D 
        * -------> if Applicant B and C have the same evaluation result
        * -------> The Rank will be:
        * -------> A = 1
        * -------> B = 2
        * -------> C = 2
        * -------> D = 4  <- (rank 3 is skipped)	 	 
        */
    private TransactionResponse rankApplicants(DataTable updateApplRegComplete, TransactionResponse response)
    {
        int rankCount = 1;
        int count = 1;
        float subTot = 0;
        float tempSubTot = 0;
        bool firstTimeIteration = true;
        bool isAllApplicantDataValid = true;
        string applicantWithInValidEvaluationData = "";

        foreach (DataRow row in updateApplRegComplete.Rows)
        {
            //case of the 1st rank
            if (firstTimeIteration)
            {
                row["rank"] = rankCount;

                //get the subTotal of the current applicant
                if (row["Total"] != null && !row["Total"].ToString().Equals(""))
                {
                    subTot = float.Parse(row["Total"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                }

                //not first time interaction any more
                firstTimeIteration = false;


                //Evaluation result can not be less than 1. why? 
                if (subTot < 1 || row["Total"] == null || row["Total"].ToString().Equals(""))
                {
                    isAllApplicantDataValid = false;
                    //applicantWithInValidEvaluationData = "Applicant: " + row["Full Name"]
                    //                                     + " doesn't have valid evaluation result = " + row["Total"]
                    //                                     + " Please correct this evaluation and generate the report again ";
                    applicantWithInValidEvaluationData = "Applicant: " + row["First Name"] + " " + row["Middle Name"] + " " + row["Last Name"] + " [" + row["Employee ID"] + "]"
                                                            + " doesn't have valid evaluation result = " + row["Total"]
                                                            + " Please correct this evaluation and generate the report again ";
                    break;
                }
            }

            else
            {
                tempSubTot = 0;
                if (row["Total"] != null && !row["Total"].ToString().Equals(""))
                {
                    tempSubTot = float.Parse(row["Total"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                }

                //Different result, use the next available rank	and update the subTot to the new result. 
                if (tempSubTot > 0 && subTot > 0 && subTot != tempSubTot)
                {
                    rankCount = count;
                    subTot = tempSubTot;
                }

                //Otherwise it is same result, so keep the rank
                row["rank"] = rankCount;

                //Evaluation result can not be less than 1. 
                if (tempSubTot < 1 || row["Total"] == null || row["Total"].ToString().Equals(""))
                {
                    isAllApplicantDataValid = false;
                    //applicantWithInValidEvaluationData = "Applicant: " + row["Full Name"]
                    applicantWithInValidEvaluationData = "Applicant: " + row["First Name"] + " " + row["Middle Name"] + " " + row["Last Name"] + " [" + row["Employee ID"] + "]"
                                                         + " doesn't have valid evaluation result = " + row["Total"]
                                                         + " Please correct this evaluation and generate the report again ";
                    break;
                }
            }
            ++count;
        }

        if (!isAllApplicantDataValid)
        {
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(applicantWithInValidEvaluationData);
            response.setSuccess(false);
            return response;
        }

        //rank given to each applicants and the data is ready. 
        //set it inside the transaction response
        response.Data = updateApplRegComplete;

        response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        response.setMessage(DBOperationErrorConstants.M_VACANCY_EVALUTION_RESULT_GENERATED_SUCCESS);
        response.setSuccess(true);
        return response;
    }


    public TransactionResponse updateApplicantsEvaluationStatus(DataTable dataTable, string newStatus, int ranklimit)
    {
        //update Vacancy status
        bool vacancyStatusUpsateOk = false;
        TransactionResponse response = new TransactionResponse();

        try
        {
            vacancyStatusUpsateOk = sendVacancyUpdateStatus(new TransactionResponse());
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_UPDATE_APP_EVA_STATUS);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            return response;
        }
        //vacancy update is not OK, we stop the flow.
        if (!vacancyStatusUpsateOk)
        {
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_UPDATE_APP_EVA_STATUS);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            return response;
        }



        //convert the data in datatable, specifically in EID column to list of string separated by comma
        string listOfApplicants = DBOperationsUtil.getDataTableColumnAsListOfString(dataTable, "Employee ID", "Rank", ranklimit);

        //Add List of Arguments for new employee
        IDictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@vacancyNo", vacancy.VacancyNo);
        parameters.Add("@vacancyDate", vacancy.PostedDate);
        parameters.Add("@newStatus", newStatus);
        parameters.Add("@listOfApplicants", listOfApplicants);


        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateAppicantsStatus, parameters);

        try
        {
            //call update to DB mathod and get reponse. 
            dpOperation.updateRecord();
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_APPLICANTS_PASSED_SUCCESS);
        }

        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_UPDATE_APP_EVA_STATUS);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
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

    /**
    * Update diploma applicant on vacancy readvertise 
    */
    private bool updateDiplomaApplicantToNull(TransactionResponse response)
    {
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancyNo", vacancy.VacancyNo.Trim());
            parameters.Add("@vacancyDate", vacancy.PostedDate.Trim());

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spAddDiplomaIntoEvaluation, parameters);
            return dpOperation.updateRecord();
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
            return false;
        }
    }

    /**
    * Update status fo Vacancy to Readverised after rating
    */
    private bool readvertiseVacancyAfterRating(TransactionResponse response)
    {
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vacancy_No", vacancy.VacancyNo.Trim());
            parameters.Add("@posted_date ", vacancy.PostedDate.Trim());
            parameters.Add("@status", vacancy.Status);
            parameters.Add("@vancy_deadline", vacancy.VancyDeadline);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateVacancyStatusAfterRating, parameters);
            return dpOperation.updateRecord();
        }
        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
            return false;
        }
    }

    /**
 * Update status fo Vacancy to Readverised after rating
 */
    public TransactionResponse updateVacancyStatusToReAdvertisedAfterRating()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = readvertiseVacancyAfterRating(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISED_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISE_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_READVERTISE_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_CANCELLATION_FAIL);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    /**
     * Update diploma applicant on vacancy readvertise 
     */
    public TransactionResponse updateDiplomaApplicantToIncludeInEvaluation()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = updateDiplomaApplicantToNull(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_DIPLOMA_APPLICANT_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_INCLUDE_DIPLOMA_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_INCLUDE_DIPLOMA_APPLICANT_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_GENERATING_REPORT);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    /**
     * Update status fo Vacancy to Readverised
     */ 
    public TransactionResponse updateVacancyStatusToReAdvertised()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            bool updateStatus = sendVacancyUpdateStatus(response);
            if (updateStatus)
            {
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISED_SUCCESS);
                response.setSuccess(true);
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_VACANCY_READVERTISE_FAIL);
                response.setErrorCode(DBOperationErrorConstants.E_VACANCY_READVERTISE_FAILED);
                response.setSuccess(true);
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_GENERATING_REPORT);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            response.setSuccess(false);
        }
        return response;
    }

    /**
     * Return report of vacancy evluation done by Processor and checker.. 
     */
    public TransactionResponse getHROfficersVacancyEvaluationReport()
    {
        TransactionResponse response = new TransactionResponse();

        //will hold 2 data tables to contain report for the processor and checker. 
        DataTable[] dataTableList = new DataTable[2];

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            //get report for the Processor
            parameters.Add("@hrOfficerEID", vacancy.ResponsibleProcessorEID);
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetApplicantCountProcessor, parameters);
            dataTableList[0] = dpOperation.getRecord();

            //get report for the Checker
            parameters.Remove("@hrOfficerEID"); //first remove processor EID and add checker EID. 
            parameters.Add("@hrOfficerEID", vacancy.ResponsibleAccessorEID);
            DBOperationsUtil dpOperation2 = new DBOperationsUtil(DbAccessConstants.spGetApplicantCountChecker, parameters);
            dataTableList[1] = dpOperation2.getRecord();

            response.Data = dataTableList;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }

        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_GENERATING_REPORT);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
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



    /**
     * Return get detail of Hr officers. 
     */
    public TransactionResponse getHROfficersDetailOfAVacancy()
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spHROfficerDetailOfAVacancy, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }

        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_GENERATING_REPORT);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
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

    /**
     * ReturnsPromotion detail: Minute No + HrOfficer name if this vacany is opened for it.
     */
    public TransactionResponse getPromotionForWhichVacancyOpened()
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spPromotionDetailThisVacancyOpenedFor, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }

        catch (SqlException ex)
        {
            //Other SqlException is catched -  we can keep the same error message as if it is generating report issue
            response.setMessageType(TransactionResponse.SeverityLevel.WARNING);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_GENERATING_REPORT);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
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

    /**
     * Adds new hr officer evaluation. 
     */
    public TransactionResponse addNewHrOfficerEvaluation(string grade, string evaluatedFor, string remark)
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> parameters = new Dictionary<string, object>();

        // Hr officer ID is passed as processor in vacancy object
        parameters.Add("@hrOfficerEID", vacancy.ResponsibleProcessorEID);
        parameters.Add("@vacancyNo", vacancy.VacancyNo);
        parameters.Add("@vacancyDate", vacancy.PostedDate);
        parameters.Add("@grade", grade);
        parameters.Add("@evaluatedFor", evaluatedFor);
        parameters.Add("@remark", remark);


        return sendHrOfficerEvaluationToDb(DbAccessConstants.SpAddNewHrOfficerEvaluation, parameters);
    }

    private TransactionResponse sendHrOfficerEvaluationToDb(string spName, IDictionary<string, object> parameters)
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
            response.setMessage(DBOperationErrorConstants.M_HR_OFFICER_VALUATION_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_HR_EVALUATION_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_HR_OFFICER_REGISTER);
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

    public TransactionResponse getVacancyDetail(Vacancy vacancy)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetVacancyDetail, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse getVacancyDetailByDateInterval(string startDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetVacancyByDateInterval, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse getVacancyToGenerateReportByDateInterval(string startDate, string endDate, string SPName)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(SPName, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse getVacancyDetailtoBindtoDetailView(string vacancyNo, string postedDate)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancyNo);
            parameters.Add("@vacancyDate", postedDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetVacancyDetailToBeEditted, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse DeleteApplicat(Vacancy vacancy, string eid)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee @EID
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@vancacy_Date", vacancy.PostedDate);
            parameters.Add("@EID", eid);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spDeleteApplicant, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse getVacancyToAnnounceToOutside()
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetVacancyDetailToAnnounce, null);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse getNotAnnouncedVacancyToShowPreviewforHROfficer()
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancy.VacancyNo);
            parameters.Add("@vacancyDate", vacancy.PostedDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetNotAnnouncedVacancyForHROfficerPost, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse getVacancyDetailNotAssignedtoBindtoDetailView(string vacancyNo, string postedDate)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancyNo", vacancyNo);
            parameters.Add("@vacancyDate", postedDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetVacancyDetailNotAssignedToBeEditted, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse updateNotAnnouncedVacancyToAnnounced()
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@posted_date", vacancy.PostedDate);

            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateNotAnnouncedVacancyToAnnounced, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_UPDATE_OK);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse updateVacancyDetailAssignedtoHROfficer(Vacancy vacancy, int isProcAccCanUpdate,
                                    int ProcAccUpdateStatus, string oldProc, string oldAcc)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@posted_date", vacancy.PostedDate);
            parameters.Add("@Vancay_title", vacancy.VancayTitle);
            parameters.Add("@branch_id", vacancy.BranchId);
            parameters.Add("@vancy_deadline", vacancy.VancyDeadline);
            parameters.Add("@vacancy_for_JobGrade", vacancy.VacancyforJobGrade);
            parameters.Add("@status", vacancy.Status);
            parameters.Add("@vacStatusCanEditted", isProcAccCanUpdate);
            parameters.Add("@year_gen_required", vacancy.YearRequiredforGeneral);
            parameters.Add("@year_spec_required", vacancy.YearRequiredforSpec);
            parameters.Add("@general_wrk_expr_percent", vacancy.GeneralWrkExprPercent);
            parameters.Add("@specific_wrk_expr_percent", vacancy.SpecificWrkExprPercent);
            parameters.Add("@related_wrk_expr_percent", vacancy.RelatedWrkExprPercent);
            parameters.Add("@examination_percent", vacancy.ExaminationPercent);
            parameters.Add("@manager_Recom_percent", vacancy.ManagerRecPercent);
            parameters.Add("@interview_percent", vacancy.InterviewPercent);
            parameters.Add("@applicant_reg_finished_date", vacancy.ApplicantComplete);
            parameters.Add("@profile_arrived_date", vacancy.ProfileArrivedDate);
            parameters.Add("@processor_end_date", vacancy.ProcessorEndDate);
            parameters.Add("@processor_start_date", vacancy.ProcessorStartDate);
            parameters.Add("@accessor_start_date", vacancy.AccessorStartDate);
            parameters.Add("@accessor_end_date", vacancy.AccessorEndDate);
            parameters.Add("@responsible_processor_EID", vacancy.ResponsibleProcessorEID);
            parameters.Add("@reponsible_accessor_EID", vacancy.ResponsibleAccessorEID);
            parameters.Add("@JobDescription", vacancy.JobDescription);
            parameters.Add("@JobRequirement", vacancy.JobRequirement);
            parameters.Add("@JobBenefit", vacancy.SalaryAndBenefit);
            parameters.Add("@oldProc", oldProc);
            parameters.Add("@oldAcc", oldAcc);
            parameters.Add("@updateAccProc", ProcAccUpdateStatus);


            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateAssignedVacancyDetail, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_UPDATE_OK);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

    public TransactionResponse updateVacancyDetailNotAssignedtoHROfficer(Vacancy vacancy)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@vacancy_No", vacancy.VacancyNo);
            parameters.Add("@posted_date", vacancy.PostedDate);
            parameters.Add("@Vancay_title", vacancy.VancayTitle);
            parameters.Add("@branch_id", vacancy.BranchId);
            parameters.Add("@vancy_deadline", vacancy.VancyDeadline);
            parameters.Add("@vacancy_for_JobGrade", vacancy.VacancyforJobGrade);
            parameters.Add("@year_gen_required", vacancy.YearRequiredforGeneral);
            parameters.Add("@year_spec_required", vacancy.YearRequiredforSpec);
            parameters.Add("@general_wrk_expr_percent", vacancy.GeneralWrkExprPercent);
            parameters.Add("@specific_wrk_expr_percent", vacancy.SpecificWrkExprPercent);
            parameters.Add("@related_wrk_expr_percent", vacancy.RelatedWrkExprPercent);
            parameters.Add("@examination_percent", vacancy.ExaminationPercent);
            parameters.Add("@manager_Recom_percent", vacancy.ManagerRecPercent);
            parameters.Add("@interview_percent", vacancy.InterviewPercent);
            parameters.Add("@JobBenefit", vacancy.SalaryAndBenefit);
            parameters.Add("@JobDescription", vacancy.JobDescription);
            parameters.Add("@JobRequirement", vacancy.JobRequirement);


            //get report for the Processor
            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spUpdateNotAssignedVacancyDetail, parameters);
            DataTable dataTable = dpOperation.getRecord();

            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_VACANCY_UPDATE_OK);
        }
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            LoggerManager.upDateWithGenericErrorMessage(response);
        }
        return response;
    }

   // private void logException()
   // {
   //     ErrorLogManager.LogError
   // }

    public void logException(Exception ex)
    {
        LoggerManager.LogError(ex.ToString(), logger);
    }
}

