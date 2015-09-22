using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using log4net;

/// <summary>
///EmployeeManager - manages transcations and opration on Employee. 
/// </summary>
public class EmployeeManager : IManager
{

    private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeeManager)); 

    private Employee employee;
    private InactiveEmployee inactiveEmployee;
    private EmployeeRemark employeeRemark;

    private static string className = "EmployeeManager";

    //EConstractor..
    public EmployeeManager()
    {
    }
    
    public EmployeeManager(Employee employeeData)
    {
        this.employee = employeeData;
    }

    public EmployeeManager(EmployeeRemark employeeRemark)
    {
        this.employeeRemark = employeeRemark;
    }

    public EmployeeManager(Employee employeeData, InactiveEmployee inactiveEmployee)
    {
        this.employee = employeeData;
        this.inactiveEmployee = inactiveEmployee;
    }

    /**
     * calls Store to DB utility for the current employee.
     */
    public TransactionResponse storeEmployee()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> employeeArgumentaMap = new Dictionary<string, object>();

        employeeArgumentaMap.Add("@Emp_ID", employee.EmpID);
        employeeArgumentaMap.Add("@UserId", employee.UserID);
        employeeArgumentaMap.Add("@First_Name", employee.FName);
        employeeArgumentaMap.Add("@Middle_Name", employee.MName);
        employeeArgumentaMap.Add("@Last_Name", employee.LName);
        employeeArgumentaMap.Add("@Sex", employee.Sex);
        employeeArgumentaMap.Add("@Prev_Job_Tittle", employee.PrevJob);
        employeeArgumentaMap.Add("@Job_Tittle", employee.JobTitle);
        employeeArgumentaMap.Add("@Grade", employee.JobGrade);
        employeeArgumentaMap.Add("@Salary", employee.Salary);
        employeeArgumentaMap.Add("@branch", employee.Branch);
        DateTime HDatefinal = DateTime.ParseExact(employee.Hdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        employeeArgumentaMap.Add("@HDate", HDatefinal);
        employeeArgumentaMap.Add("@MajorCategory", employee.MajorCategory);
        employeeArgumentaMap.Add("@district", employee.District);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spAddNewEmployee, employeeArgumentaMap);

        TransactionResponse response = new TransactionResponse();

        //call store to DB mathod and get reponse. 
        try
        {
            storeToDb.instertNewRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_REGISTER_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_EID_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_EMP_REGISTER);
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
     * calls Store to DB utility for the current employee.
     */
    public TransactionResponse AddHROfficerOrClerk(string SPName, string EmpID)
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> paramater = new Dictionary<string, object>();

        paramater.Add("@Emp_ID", EmpID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(SPName, paramater);

        TransactionResponse response = new TransactionResponse();

        //call store to DB mathod and get reponse. 
        try
        {
            storeToDb.instertNewRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_HR_OFFICER_REGISTER_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_EID_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_EMP_REGISTER);
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
     * Checks if Employee is duplicate by selecting and checking returned result.
     */
    public Boolean isEmployeeAlreadyExist()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> employeeIDArgumentaMap = new Dictionary<string, object>();
        employeeIDArgumentaMap.Add("@Emp_ID", employee.EmpID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmployeeDetail, employeeIDArgumentaMap);

        //get Record.
        DataTable dataTable = null;
        try
        {
            dataTable = storeToDb.getRecord();
            //if there is anything returned it means employee is duplicate.
            return dataTable != null && dataTable.Rows.Count > 0;
        }
        catch (SqlException ex)
        {
            return false;
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return false;
        }
    }

    /**
     * Get list String containing:  of Employee FName + MName+ LName +EmpId, using empId or First name.
     */
    public static List<string> getListOfStringOFEmloyeeForAutoComplete(string empIdOrFirstName)
    {
        //get list of Employee using Employee manager.
        List<Employee> listOfEmployee = EmployeeManager.getListOfEmployeeByFirstNameOrEmpId(empIdOrFirstName);

        //format list of employees for diplay.
        List<string> result = new List<string>();
        if (listOfEmployee != null && listOfEmployee.Count() > 0)
        {
            foreach (Employee employee in listOfEmployee)
            {
                result.Add(employee.FName + " " + employee.MName + " " + employee.LName + " [" + employee.EmpID + "]");
            }
        }
        return result;
    }

    /**
     * gets list of employee for a given EmpId or Fname 
     */
    public static List<Employee> getListOfEmployeeByFirstNameOrEmpId(string empId)
    {
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@txtEmpID", empId);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmployeeByFirstNameAutoComplete, argumentsMap);

        //call getRecord method and get SqlDataReader
        DataTable dataTable = null;
        try
        {
            dataTable = storeToDb.getRecord();

            //format the Result to return to presentation layer.
            List<Employee> listOfEmployee = null;

            if (dataTable != null)
            {
                listOfEmployee = new List<Employee>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Employee employee = new Employee();
                    employee.FName = row["First_Name"].ToString();
                    employee.MName = row["Middle_Name"].ToString();
                    employee.LName = row["Last_Name"].ToString();
                    employee.EmpID = row["Emp_ID"].ToString();
                    listOfEmployee.Add(employee);
                }
            }
            return listOfEmployee;
        }
        catch (SqlException ex)
        {
            //return emptylist
            return new List<Employee>();
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later.
            LoggerManager.LogError(ex.ToString(), logger);
            return new List<Employee>();
        }
    }

    /**
     * gets list of employee for a given Branch
     */
    public List<Employee> getListOfEmployeeByFirstNameOrEmpIdAtBranch()
    {

        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@txtEmpID", employee.UserName);
        argumentsMap.Add("@branch", employee.Branch);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmployeeByFirstNameAutoCompleteSpecBranch, argumentsMap);

        //call getRecord method and get SqlDataReader
        DataTable dataTable = null;
        try
        {
            dataTable = storeToDb.getRecord();

            //format the Result to return to presentation layer.
            List<Employee> listOfEmployee = null;

            if (dataTable != null)
            {
                listOfEmployee = new List<Employee>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Employee emply = new Employee();
                    emply.FName = row["First_Name"].ToString();
                    emply.MName = row["Middle_Name"].ToString();
                    emply.LName = row["Last_Name"].ToString();
                    emply.EmpID = row["Emp_ID"].ToString();
                    listOfEmployee.Add(emply);
                }
            }
            return listOfEmployee;
        }
        catch (SqlException ex)
        {
            //return emptylist
            return new List<Employee>();
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return new List<Employee>();
        }
    }

    /**
     * get employee Job title and branch detail.
     */
    public Employee getEmployeeJobTitleAndBranchDetail()
    {
        //create parameter to pass
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", employee.EmpID);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmployeeJobTitleBranchAndDistrict, argumentsMap);

        //call getRecord method and get SqlDataReader
        DataTable dataTable = null;
        Employee employeeRply = null;
        try
        {
            dataTable = storeToDb.getRecord();

            //The if there was a result
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    employeeRply = new Employee();
                    employeeRply.EmpID = employee.EmpID;
                    employeeRply.UserName = row["UserId"].ToString();
                    employeeRply.FName = row["First_Name"].ToString();
                    employeeRply.MName = row["Middle_Name"].ToString();
                    employeeRply.LName = row["Last_Name"].ToString();
                    employeeRply.Sex = row["Sex"].ToString();
                    employeeRply.PrevJob = row["PrevJob"].ToString();
                    employeeRply.JobTitle = row["JobTitle"].ToString();
                    employeeRply.Branch = row["branchName"].ToString();
                    employeeRply.BranchID = row["branch"].ToString();
                    employeeRply.JobGrade = row["Grade"].ToString();
                    employeeRply.Salary = row["Salary"].ToString();
                    employeeRply.Hdate = row["HDate"].ToString();
                    employeeRply.District = row["distric_name"].ToString();

                    //We are seaarching Employee by ID, there an only be only employee
                    return employeeRply;
                }
            }
            return employeeRply;
        }
        catch (SqlException ex)
        {
            return employeeRply;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return employeeRply;
        }
    }

    public Employee detailOfEmployeToBeEvaluated(string vacancyNo, string vacancyDate)
    {
        //create parameter to pass
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", employee.EmpID);
        argumentsMap.Add("@vacancyNo", vacancyNo);
        argumentsMap.Add("@vacancyDate", vacancyDate);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmployeeToBeEvaluated, argumentsMap);

        //call getRecord method and get SqlDataReader
        DataTable dataTable = null;
        Employee employeeRply = null;
        try
        {
            dataTable = storeToDb.getRecord();

            //The if there was a result
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    employeeRply = new Employee();
                    employeeRply.EmpID = employee.EmpID;
                    employeeRply.FName = row["First_Name"].ToString();
                    employeeRply.MName = row["Middle_Name"].ToString();
                    employeeRply.LName = row["Last_Name"].ToString();
                    employeeRply.EducationalQualification = row["Educational Level"].ToString();
                    employeeRply.JobTitle = row["JobTitle"].ToString();
                    employeeRply.Branch = row["branchName"].ToString();
                    employeeRply.EmployeeType = row["applicant_type"].ToString();
                    if (row["proccessed_date"] != null && !row["proccessed_date"].ToString().Equals(""))
                    {
                        employeeRply.ProcessedDate = row["proccessed_date"].ToString();
                    }
                    if (row["checked_date"] != null && !row["checked_date"].ToString().Equals(""))
                    {
                        employeeRply.CheckedDate = row["checked_date"].ToString();
                    }

                    //We are seaarching Employee by ID, there an only be only employee
                    return employeeRply;
                }
            }
            return employeeRply;
        }
        catch (SqlException ex)
        {
            return employeeRply;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return employeeRply;
        }
    }

    //Update Employee status
    public DataTable Updateemployee()
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", employee.EmpID);
        argumentsMap.Add("@First_Name", employee.FName);
        argumentsMap.Add("@Middle_Name", employee.MName);
        argumentsMap.Add("@Last_Name", employee.LName);
        argumentsMap.Add("@Sex", employee.Sex);
        argumentsMap.Add("@Prev_Job_Tittle", employee.PrevJob);
        argumentsMap.Add("@Job_Tittle", employee.JobTitle);
        argumentsMap.Add("@Grade", employee.JobGrade);
        argumentsMap.Add("@Salary", employee.Salary);
        argumentsMap.Add("@branch", employee.Branch);
        argumentsMap.Add("@HDate", employee.Hdate);
        argumentsMap.Add("@MajorCategory", employee.MajorCategory);
        argumentsMap.Add("@district", employee.District);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdateBranchEmployeeStatus, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    //Update Applicant Result
    public DataTable UpdateApplicantResult(VacancyEvaluationForm vacancyEvaluationForm, Vacancy vacancy)
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", vacancyEvaluationForm.EmpId);
        argumentsMap.Add("@vacancy_No", vacancy.VacancyNo);
        argumentsMap.Add("@vacancy_date", vacancy.PostedDate);
        argumentsMap.Add("@education_level_mark", vacancyEvaluationForm.EducationLevelMark);
        argumentsMap.Add("@general_work_expr", vacancyEvaluationForm.GeneralWorkExpr);
        argumentsMap.Add("@specific_work_expr", vacancyEvaluationForm.SpecificWorkRxpr);
        argumentsMap.Add("@recommendation_of_mgr_line", vacancyEvaluationForm.RecommendationOfMgrLine);
        argumentsMap.Add("@interview_result_recommendation", vacancyEvaluationForm.InterviewResult);
        argumentsMap.Add("@remark", vacancyEvaluationForm.Remark);
        argumentsMap.Add("@evaluation_status", vacancyEvaluationForm.ApplicantStatus);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdateApplicantDetailform1, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public DataTable UpdateAssignedemployee()
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", employee.EmpID);
        argumentsMap.Add("@Prev_Job_Tittle", employee.PrevJob);
        argumentsMap.Add("@Job_Tittle", employee.JobTitle);
        argumentsMap.Add("@Grade", employee.JobGrade);
        argumentsMap.Add("@Salary", employee.Salary);
        argumentsMap.Add("@branch", employee.Branch);
        argumentsMap.Add("@status", employee.Promstatus);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdateListOfAssignedEmployeeStatus, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public DataTable UpdateemployeeEducationalQual(string EID, string eduLevel, string qualif)
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", EID);
        argumentsMap.Add("@EducationLevel", eduLevel);
        argumentsMap.Add("@Qualification", qualif);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdateEmpEducLevel, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public DataTable UpdateHROfficerPMSResult(string EID, string vacNo, string evaFor, string grade, string remark)
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@hr_office_EID", EID);
        argumentsMap.Add("@vacancy_No", vacNo);
        argumentsMap.Add("@evaluated_for", evaFor);
        argumentsMap.Add("@grade", grade);
        argumentsMap.Add("@remark", remark);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdateHROfficerClerkPMSResult, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }

    public DataTable UpdateAssignedemployeefromOtherDistrict()
    {
        //prepare parameters
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", employee.EmpID);
        argumentsMap.Add("@status", employee.Promstatus);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spUpdateListOfAssignedEmployeefromOtherDistrict, argumentsMap);

        //call getRecord method and get DataTable
        DataTable dataTable = storeToDb.getRecord();

        return dataTable;
    }



    /**
     * Gives the current active HR amanger. 
     */
    public static string getCurrentHRManager()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetCurrentHrManager, null);

        //call getRecord method and get SqlDataReader
        DataTable dataTable = null;
        try
        {
            dataTable = storeToDb.getRecord();

            //The if there was a result
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    return row["HrManagerEmpID"].ToString();
                }
            }
            return PageConstants.ERROR;
        }
        catch (SqlException ex)
        {
            return PageConstants.ERROR;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            return PageConstants.ERROR;
        }
    }

    /**
     * Gives the specified Employee Detail
     */
    public List<Employee> getEmployeeDetial()
    {
        //Pass Stored Procedure Name and parameter list.
        IDictionary<string, object> argumentsMap = new Dictionary<string, object>();
        argumentsMap.Add("@Emp_ID", employee.EmpID);
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetEmployeeDetail, argumentsMap);
        
        //call getRecord method and get SqlDataReader
        DataTable dataTable = null;
        try
        {
            dataTable = storeToDb.getRecord();

            //format the Result to return to presentation layer.
            List<Employee> listOfEmployee = null;

            if (dataTable != null)
            {
                listOfEmployee = new List<Employee>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Employee emply = new Employee();
                    emply.FName = row["First_Name"].ToString();
                    emply.MName = row["Middle_Name"].ToString();
                    emply.LName = row["Last_Name"].ToString();
                    emply.EmpID = row["Emp_ID"].ToString();
                    emply.Branch = row["branch"].ToString();
                    emply.BranchID = row["branchName"].ToString(); //actually we storing branch Name not the ID s
                    emply.JobTitle = row["Job_Tittle"].ToString();
                    emply.MajorCategory = row["MajorCategory"].ToString();
                    listOfEmployee.Add(emply);
                }
            }
            return listOfEmployee;
        }
        catch (SqlException ex)
        {
            //return emptylist
            return new List<Employee>();
        }

        //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            return new List<Employee>();
        }
    }

    public static Employee getLoggedOnUser(Guid userId)
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> userIdMap = new Dictionary<string, object>();
        userIdMap.Add("@UserId", userId);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spGetLoggedOnUser, userIdMap);

        //get Record. 
        DataTable dataTable = null;
        Employee employee = null;
        try
        {
            dataTable = dbOperation.getRecord();
            employee = new Employee();

            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    employee = new Employee();
                    employee.FName = row["First_Name"].ToString();
                    employee.EmpID = row["Emp_ID"].ToString();
                }
            }
            return employee;
        }
        catch (SqlException ex)
        {
            return employee;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            return employee;
        }
    }

    public static TransactionResponse handleLoggedInUserCanNotBeIdentfied()
    {
        TransactionResponse response = new TransactionResponse();
        response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
        response.setErrorCode(DBOperationErrorConstants.M_CAN_NOT_IDENTIFY_CURRENT_USER);
        response.setMessage(DBOperationErrorConstants.M_CAN_NOT_IDENTIFY_CURRENT_USER);
        return response;
    }


    /**
     * gets list of HR Officer
     */
    public static DataTable getHrOfficerList()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetAllHrOfficers, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }

    /**
     * gets list of HR Officer and also HR Clerk
     */
    public static DataTable getHrOfficerAndClerkList()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetAllHrOfficersAndHrClerk, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }

    /**
     * gets list of Previous Job title
     */
    public static DataTable getPreviousJTitle()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetPreviousJobTitle, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }

    /**
     * gets list of Current Job title
     */
    public static DataTable getCurrentJTitle()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetCurrentJobTitle, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }

    /**
     * TODO This sould NOT be here please more it to another Manager class : may be DistrictManager
     * gets list of District
     */
    public static DataTable getListOfDistrict()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetDistrictsList, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }

    /**
     * gets list of Current branch
     */
    public static DataTable getCurrentBranch()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetBranchsList, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }

    /**
     * gets list of All CBE branch (i.e. used for previoues branch most of the time)
     */
    public static DataTable getPrevBranch()
    {
        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spGetAllCBEBranchsList, null);

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
            LoggerManager.LogError(ex.ToString(), logger);
            return dataTable;
        }
    }
    /**
     * Add promoted employee from other district
    */

    public TransactionResponse storeEmployeefromOtherDistrictForPromotion(string minNumber, string districtID)
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> employeeArgumentaMap = new Dictionary<string, object>();

        employeeArgumentaMap.Add("@Minute_No", minNumber);
        employeeArgumentaMap.Add("@Emp_ID", employee.EmpID);
        employeeArgumentaMap.Add("@First_Name", employee.FName);
        employeeArgumentaMap.Add("@Middle_Name", employee.MName);
        employeeArgumentaMap.Add("@Last_Name", employee.LName);
        employeeArgumentaMap.Add("@Curr_Jtitle", employee.PrevJob);
        employeeArgumentaMap.Add("@Curr_JGrade", employee.PrevJobGrade);
        employeeArgumentaMap.Add("@Exp_JGrade", employee.JobGrade);
        employeeArgumentaMap.Add("@Salary", employee.Salary);
        employeeArgumentaMap.Add("@district", districtID);
        employeeArgumentaMap.Add("@status", "Active");

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spAddPromotedEmployeefromOtherDistrict, employeeArgumentaMap);

        TransactionResponse response = new TransactionResponse();

        //call store to DB mathod and get reponse. 
        try
        {
            storeToDb.instertNewRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_REGISTER_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_EID_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_EMP_REGISTER);
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
     * Add employee remark
    */

    public TransactionResponse storeEmployeeremark(MembershipUser currentUser)
    {
        //get detail of the logged on user.
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);
        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        //Add List of Arguments for new employee
        IDictionary<string, object> employeeRemarkMap = new Dictionary<string, object>();

        employeeRemarkMap.Add("@EmpID", employeeRemark.EmployeeID);
        employeeRemarkMap.Add("@remarkRefNo", employeeRemark.RemarkReferanceNo);
        employeeRemarkMap.Add("@dateOfRemark", employeeRemark.DateOfRemark);
        employeeRemarkMap.Add("@remarkType", employeeRemark.RemarkType);
        employeeRemarkMap.Add("@penalityPerc", employeeRemark.PenaltyPerc);
        employeeRemarkMap.Add("@managerID", employeeRemark.ManagerID);
        employeeRemarkMap.Add("@managerPosition", employeeRemark.ManagerPosition);
        employeeRemarkMap.Add("@remarkReason", employeeRemark.RemarkReason);
        employeeRemarkMap.Add("@responsiblehrOfficerID", employee.EmpID);
        employeeRemarkMap.Add("@Branch", employeeRemark.Branch);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spAddEmployeeRemark, employeeRemarkMap);

        TransactionResponse response = new TransactionResponse();

        //call store to DB mathod and get reponse. 
        try
        {
            storeToDb.instertNewRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_EMPLOYEE_REMARK_REGISTER_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_REF_NO_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_EMP_REGISTER);
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

    //delete resigned employee from database

    public TransactionResponse deleteResignedEmployee()
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            IDictionary<string, object> argumentMap = new Dictionary<string, object>();
            //task referance
            argumentMap.Add("@UserId", employee.UserName);

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spDeleteEmployeeInfor, argumentMap);
            bool isDeleteOk = dbOperation.deleteRecord();

            //put the data on Transaction response
            response.Data = isDeleteOk;

            response.setSuccess(true);

            //get delete status inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_REMOVING_INACTIVE_EMPLOYEE);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_REMOVING_INACTIVE_EMPLOYEE);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_EVIL_ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }
    }

    public TransactionResponse deleteHROfficerOrClerk(string empID, string OfficerOrClerk)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            IDictionary<string, object> argumentMap = new Dictionary<string, object>();
            //task referance
            argumentMap.Add("@empID", empID);
            argumentMap.Add("@OfficerOrClerk", OfficerOrClerk);

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spRemoveHROfficerOrClerk, argumentMap);
            bool isDeleteOk = dbOperation.deleteRecord();

            //put the data on Transaction response
            response.Data = isDeleteOk;

            response.setSuccess(true);

            //get delete status inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_REMOVING_INACTIVE_EMPLOYEE);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_REMOVING_INACTIVE_EMPLOYEE);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_EVIL_ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }
    }

    // Insert Inactive Employeee into DB
    public TransactionResponse storeInactiveEmployee()
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> employeeArgumentaMap = new Dictionary<string, object>();

        employeeArgumentaMap.Add("@EmpID", employee.EmpID);
        employeeArgumentaMap.Add("@FName", employee.FName);
        employeeArgumentaMap.Add("@MName", employee.MName);
        employeeArgumentaMap.Add("@LName", employee.LName);
        employeeArgumentaMap.Add("@Sex", employee.Sex);
        employeeArgumentaMap.Add("@JobTitle", employee.JobTitle);
        employeeArgumentaMap.Add("@branch", employee.Branch);
        employeeArgumentaMap.Add("@dateOfEmp", inactiveEmployee.DateOfEmployment);
        employeeArgumentaMap.Add("@dateOfTerm", inactiveEmployee.DateOfTermination);
        employeeArgumentaMap.Add("@majorCategory", inactiveEmployee.MajorCategory);
        employeeArgumentaMap.Add("@ReasonForleave", inactiveEmployee.ReasonForLeave);

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil storeToDb = new DBOperationsUtil(DbAccessConstants.spAddInactiveEmployee, employeeArgumentaMap);

        TransactionResponse response = new TransactionResponse();

        //call store to DB mathod and get reponse. 
        try
        {
            storeToDb.instertNewRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setSuccess(true);
            response.setMessage(DBOperationErrorConstants.M_INACTIVE_EMPLOYEE_REGISTER_OK);
        }

        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_EID_KEY_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_UNKNOWN_ERROR_EMP_REGISTER);
                response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_ERROR_AT_DB_OOPERATION);
            }
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            logException(ex);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_EVIL_ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }
        return response;
    }



    /**
     * Get Inactive employee by date interval
     */
    public TransactionResponse getInactiveEmployeeResult(string startedDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@startDate", startedDate);
            parameters.Add("@endDate", endDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetInactiveEmployeeDetail, parameters);

            DataTable getResult = dpOperation.getRecord();
            if (getResult != null && getResult.Rows.Count > 0)
            {
                response.Data = getResult;
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_REPORT_GENERATED_SUCCESS);
                response.setSuccess(true);
                return response;
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.INFO);
                response.setMessage(DBOperationErrorConstants.M_GENERATE_INACTIVE_EMPLOYEE_REPORT_EMPTY);
                response.setSuccess(false);
                return response;
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_GENERATE_INACTIVE_EMPLOYEE);
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
     * Get HR Officer/Clerk PMS report by date interval
     */
    public TransactionResponse getHROfficerPMSReportResult(string startedDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@startDate", startedDate);
            parameters.Add("@endDate", endDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetHROfficerReport, parameters);

            DataTable getResult = dpOperation.getRecord();
            if (getResult != null && getResult.Rows.Count > 0)
            {
                response.Data = getResult;
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_REPORT_GENERATED_SUCCESS);
                response.setSuccess(true);
                return response;
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.INFO);
                response.setMessage(DBOperationErrorConstants.M_GENERATE_HR_OFFICER_REPORT_EMPTY);
                response.setSuccess(false);
                return response;
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_GENERATE_HR_OFFICER_REPORT_EMPLOYEE);
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
     * Get HR Officer/Clerk PMS report by Name
     */
    public TransactionResponse getHROfficerPMSReportResultByName(string empID)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@empID", empID);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetHROfficerReportbyName, parameters);

            DataTable getResult = dpOperation.getRecord();
            if (getResult != null && getResult.Rows.Count > 0)
            {
                response.Data = getResult;
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_REPORT_GENERATED_SUCCESS);
                response.setSuccess(true);
                return response;
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.INFO);
                response.setMessage(DBOperationErrorConstants.M_GENERATE_HR_OFFICER_BY_NAME_REPORT_EMPTY);
                response.setSuccess(false);
                return response;
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_GENERATE_HR_OFFICER_REPORT_EMPLOYEE);
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
     * Get Employee Remark by date interval
     */
    public TransactionResponse getEmployeeRemarkResult(string startedDate, string endDate)
    {
        TransactionResponse response = new TransactionResponse();

        try
        {
            //Add List of Arguments for new employee
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@startDate", startedDate);
            parameters.Add("@endDate", endDate);

            DBOperationsUtil dpOperation = new DBOperationsUtil(DbAccessConstants.spGetEmployeeRemark, parameters);

            DataTable getResult = dpOperation.getRecord();
            if (getResult != null && getResult.Rows.Count > 0)
            {
                response.Data = getResult;
                response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
                response.setMessage(DBOperationErrorConstants.M_REPORT_GENERATED_SUCCESS);
                response.setSuccess(true);
                return response;
            }
            else
            {
                response.setMessageType(TransactionResponse.SeverityLevel.INFO);
                response.setMessage(DBOperationErrorConstants.M_GENERATE_EMPLOYEE_REMARK_REPORT_EMPTY);
                response.setSuccess(false);
                return response;
            }
        }
        catch (SqlException ex)
        {
            //Other SqlException is catched
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_GENERATE_INACTIVE_EMPLOYEE);
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
     * Writes log file using log4net. 
     */
    public void logException(Exception ex)
    {
        LoggerManager.LogError(ex.ToString(), logger);       
    }
}