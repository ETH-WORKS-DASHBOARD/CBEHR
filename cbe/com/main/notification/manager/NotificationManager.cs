using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Security;
using System.Data.SqlClient;
using log4net;

/// <summary>
/// Summary description for NotificationManager
/// </summary>
public class NotificationManager : IManager
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(NotificationManager));

    Notification notification;
    Promotion promotion;
    Vacancy vacancy;
    PromotionAssigment promotionAssigment;

    // use this Constractor when you work on notification only
    public NotificationManager(Notification notification)
    {
        this.notification = notification;
    }


    // use this Constractor when you work on notification of promotion
    public NotificationManager(Promotion promotion)
    {
        this.notification = new Notification();
        this.promotion = promotion;
    }

    // use this Constractor when you work on notification of Vacancy
    public NotificationManager(Vacancy vacancy)
    {
        this.notification = new Notification();
        this.vacancy = vacancy;
    }

    // use this Constractor when you work on notification of Vacancy
    public NotificationManager(PromotionAssigment promotionAssigment)
    {
        this.notification = new Notification();
        this.promotionAssigment = promotionAssigment;
    }


    public static string getNotificationCountForTheCurrentUser(string employeeId)
    {
        //Add List of Arguments for new employee
        IDictionary<string, object> employeeIdMap = new Dictionary<string, object>();
        employeeIdMap.Add("@EmpId", employeeId);
        employeeIdMap.Add("@destrictID", PageAccessManager.getDistrictID());

        //Pass Stored Procedure Name and parameter list. 
        DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spGetNotificationCountForUser, employeeIdMap);

        //get Record. 
        DataTable dataTable = dbOperation.getRecord();
        string notificationCount = null;

        if (dataTable != null)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                notificationCount = row["notificationCount"].ToString();
            }
        }
        return notificationCount;
    }

    public static TransactionResponse getAllNotificationsForCurrentEmployee(MembershipUser currentUser)
    {

        //get detail of the logged on user. 
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        TransactionResponse response = new TransactionResponse();
        try
        {
            IDictionary<string, object> employeeIdMap = new Dictionary<string, object>();
            employeeIdMap.Add("@EMP_ID", employee.EmpID);
            employeeIdMap.Add("@destrictID", PageAccessManager.getDistrictID());

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spGetAllNotificationForTheCurrentEmployee, employeeIdMap);
            DataTable dataTable = dbOperation.getRecord();
            //put the data on Transaction response
            response.Data = dataTable;
            response.setSuccess(true);
            response.setMessageType(TransactionResponse.SeverityLevel.INFO);
            response.setMessage(DBOperationErrorConstants.M_NOTIFICATION_INFO);
            //get Notifications inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_READING_NOTIFICATION);
            response.setMessage(DBOperationErrorConstants.M_ERROR_WHILE_READING_NOTIF);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }

       //CATCH ANY OTHER EXCEPTION, dont let user see any kind of unexpected error
        catch (Exception ex)
        {
            //Write this exception to file for investigation of the issue later. 
            LoggerManager.LogError(ex.ToString(), logger);
            response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_EVIL_ERROR);
            response.setMessage(DBOperationErrorConstants.M_UNKNOWN_EVIL_ERROR);
            response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
            response.setSuccess(false);
            return response;
        }
    }

    /**
     * register notification when promotion is registered. 
     */
    public TransactionResponse addNotificationForPromotion(MembershipUser currentUser)
    {
        // get the current HR managerEmployee ID.        
        this.notification.Receiver = EmployeeManager.getCurrentHRManager();
        //if ERROR is returned for HRManager, we stop the flow.     
        if (this.notification.Receiver.Equals(PageConstants.ERROR))
        {
            return handleHRManagerCanNotBeFoundError();
        }

        //get detail of the logged on user.
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);
        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        //Register Notification to be sent to the HR Manager
        this.notification.TaskRreference = promotion.MinuteNo;

        //sender is the currently logged in user. 
        this.notification.Sender = employee.EmpID;

        //Notification is for registered Promotion
        this.notification.NotificationIsFor = "1";

        //today in YYYY- MM - DD format.  
        this.notification.RegisteredDate = DateTime.Now.ToString("yyyy-MM-dd");
        TransactionResponse saveNotifResult = addNewNotification();
        if (!saveNotifResult.isSuccessful())
        {
            saveNotifResult.setMessage("  ");
        }
        return saveNotifResult;
    }

    /**
     * register notification when both processor and checker done evaluation.
     */
    public TransactionResponse addNotification(MembershipUser currentUser, string notificatioFor)
    {
        // get the current HR managerEmployee ID.        
        this.notification.Receiver = EmployeeManager.getCurrentHRManager();
        //if ERROR is returned for HRManager, we stop the flow.     
        if (this.notification.Receiver.Equals(PageConstants.ERROR))
        {
            return handleHRManagerCanNotBeFoundError();
        }

        //get detail of the logged on user.
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);
        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        //Register Notification to be sent to the HR Manager
        this.notification.TaskRreference = vacancy.VacancyNo;

        //sender is the currently logged in user. 
        this.notification.Sender = employee.EmpID;

        //Notification is for registered Promotion
        this.notification.NotificationIsFor = notificatioFor;

        //today in YYYY- MM - DD format.  
        this.notification.RegisteredDate = DateTime.Now.ToString("yyyy-MM-dd");
        TransactionResponse saveNotifResult = addNewNotification();
        if (!saveNotifResult.isSuccessful())
        {
            saveNotifResult.setMessage("  ");
        }
        return saveNotifResult;
    }

    /**
     * register notification when both processor and checker done evaluation.
     */
    public TransactionResponse addNotificationForRatingPhase2(MembershipUser currentUser, string resposibleHROfficer)
    {
        this.notification.Receiver = resposibleHROfficer;

        //get detail of the logged on user.
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);
        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        //Register Notification to be sent to the HR Manager
        this.notification.TaskRreference = vacancy.VacancyNo;

        //sender is the currently logged in user. 
        this.notification.Sender = employee.EmpID;

        //Notification is for registered Promotion
        this.notification.NotificationIsFor = "7";

        //today in YYYY- MM - DD format.  
        this.notification.RegisteredDate = DateTime.Now.ToString("yyyy-MM-dd");
        TransactionResponse saveNotifResult = addNewNotification();
        if (!saveNotifResult.isSuccessful())
        {
            saveNotifResult.setMessage("  ");
        }
        return saveNotifResult;
    }

    private TransactionResponse addNewNotification()
    {
        IDictionary<string, object> notificationParams = new Dictionary<string, object>();
        notificationParams.Add("@taskReference", notification.TaskRreference);
        notificationParams.Add("@receiverEmpId", notification.Receiver);
        notificationParams.Add("@senderEmpId", notification.Sender);
        notificationParams.Add("@notificationIsFor", notification.NotificationIsFor);
        notificationParams.Add("@registeredDate", notification.RegisteredDate);
        notificationParams.Add("@disctrictId", PageAccessManager.getDistrictID());

        TransactionResponse response = new TransactionResponse();

        //call store to DB mathod and get reponse. 
        try
        {
            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spGetAddNewNotification, notificationParams);
            dbOperation.instertNewRecord();
            response.setMessageType(TransactionResponse.SeverityLevel.SUCESS);
            response.setMessage(DBOperationErrorConstants.M_NOTIF_REGISTER_OK);
            response.setSuccess(true);
        }
        catch (SqlException ex)
        {
            //Determine if the cause was duplicate KEY.
            if (ex.ToString().Contains(DBOperationErrorConstants.PK_DUPLICATE_INDICATOR))
            {
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_DUPLICATE_NOTIF_ERROR);
                response.setErrorCode(DBOperationErrorConstants.E_DUPLICATE_KEY_ERROR);
            }
            else
            {
                //Other SqlException is catched
                response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
                response.setMessage(DBOperationErrorConstants.M_NOTIFY_REGISTERED_PROMOTION_FAILED);
                response.setErrorCode(DBOperationErrorConstants.E_PROMOTION_NOTIFICATION_FAILED);
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
     * register notification when vacancy is assigned to processor and checker. 
     */
    public TransactionResponse addNotificationForVacancyAssignement(MembershipUser currentUser, bool isReadverVacancy)
    {
        //get detail of the logged on user. 
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        //sender is the currently logged in user. 
        this.notification.Sender = employee.EmpID;

        //Register Notification to be sent to the HR Manager
        this.notification.TaskRreference = vacancy.VacancyNo;
        if (isReadverVacancy)
        {
            //Notification is for registered vacancy assigned. 
            this.notification.NotificationIsFor = "9";
        }
        else
        {
            //Notification is for registered vacancy assigned. 
            this.notification.NotificationIsFor = "4";
        }
        //today in YYYY- MM - DD format.  
        this.notification.RegisteredDate = DateTime.Now.ToString("yyyy-MM-dd");


        //add notification for the processor
        this.notification.Receiver = vacancy.ResponsibleProcessorEID;
        TransactionResponse saveNotifResult = addNewNotification();

        bool isAllNotificationSent = true;
        string notifResult = "However we encounter difficulty trying to notify ";
        if (!saveNotifResult.isSuccessful())
        {
            notifResult += " the processor";
            isAllNotificationSent = false;
        }

        //add notification for the checker

        this.notification.Receiver = vacancy.ResponsibleAccessorEID;
        saveNotifResult = addNewNotification();
        string checkerNotifFailMsg = "";


        if (!saveNotifResult.isSuccessful())
        {
            if (!isAllNotificationSent)
            {
                checkerNotifFailMsg += " and";
            }
            notifResult += checkerNotifFailMsg + " the checker";
            isAllNotificationSent = false;
        }

        //send error message only if both or either of the notification failed. 
        if (!isAllNotificationSent)
        {
            notifResult += " of the vacancy";
            saveNotifResult.setMessage(notifResult);
        }

        return saveNotifResult;
    }

    /*
     ** register notification when promotion is assigned to HR Officer.
     */
    public TransactionResponse addNotificationForPromotioAssignement(MembershipUser currentUser)
    {
        //get detail of the logged on user. 
        Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

        if (employee == null)
        {
            return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
        }

        //sender is the currently logged in user. 
        this.notification.Sender = employee.EmpID;

        //Register Notification to be sent to the HR Manager
        this.notification.TaskRreference = promotionAssigment.MinuteNo;

        //Notification is for registered promotion assigned. 
        this.notification.NotificationIsFor = "2";

        //today in YYYY- MM - DD format.  
        this.notification.RegisteredDate = DateTime.Now.ToString("yyyy-MM-dd");


        //add notification for the processor
        this.notification.Receiver = promotionAssigment.HROfficerID;
        TransactionResponse saveNotifResult = addNewNotification();

        string notifResult = "However we encounter difficulty trying to notify ";

        //send error message only if both or either of the notification failed. 
        if (!saveNotifResult.isSuccessful())
        {
            saveNotifResult.setMessage(notifResult);
        }

        return saveNotifResult;
    }

    private TransactionResponse handleHRManagerCanNotBeFoundError()
    {
        TransactionResponse transaction = new TransactionResponse();
        transaction.setMessage(DBOperationErrorConstants.M_NO_ACTIVE_HR_MANAGER);
        transaction.setMessageType(TransactionResponse.SeverityLevel.ERROR);
        transaction.setErrorCode(DBOperationErrorConstants.E_NO_ACTIVE_MANAGER);
        return transaction;
    }

    public TransactionResponse deleteNotificationForAssignedVacancy(MembershipUser currentUser)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //get detail of the logged on user. 
            Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

            if (employee == null)
            {
                return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
            }

            IDictionary<string, object> argumentMap = new Dictionary<string, object>();
            //task referance
            argumentMap.Add("@taskRef", vacancy.VacancyNo.Trim());
            argumentMap.Add("@receiverEID", employee.EmpID);
            argumentMap.Add("@destrictID", PageAccessManager.getDistrictID());

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spDeleteMailNotification, argumentMap);
            bool isDeleteOk = dbOperation.deleteRecord();

            //put the data on Transaction response
            response.Data = isDeleteOk;

            response.setSuccess(true);

            //get delete status inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_REMOVING_NOTIFICATION);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_REMOVE_NOTIFICATION);
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

    public TransactionResponse deleteNotificationForAssignedPromotion(MembershipUser currentUser)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //get detail of the logged on user. 
            Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

            if (employee == null)
            {
                return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
            }

            IDictionary<string, object> argumentMap = new Dictionary<string, object>();
            //task referance
            argumentMap.Add("@taskRef", promotionAssigment.MinuteNo);
            argumentMap.Add("@receiverEID", employee.EmpID);
            argumentMap.Add("@destrictID", PageAccessManager.getDistrictID());

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spDeleteMailNotification, argumentMap);
            bool isDeleteOk = dbOperation.deleteRecord();

            //put the data on Transaction response
            response.Data = isDeleteOk;

            response.setSuccess(true);

            //get delete status inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_REMOVING_NOTIFICATION);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_REMOVE_NOTIFICATION);
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

    public TransactionResponse deleteNotificationForRatingDonePhase1(MembershipUser currentUser)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //get detail of the logged on user. 
            Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

            if (employee == null)
            {
                return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
            }

            IDictionary<string, object> argumentMap = new Dictionary<string, object>();
            //task referance
            argumentMap.Add("@taskRef", vacancy.VacancyNo);
            argumentMap.Add("@receiverEID", employee.EmpID);
            argumentMap.Add("@destrictID", PageAccessManager.getDistrictID());

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spDeleteMailNotification, argumentMap);
            bool isDeleteOk = dbOperation.deleteRecord();

            //put the data on Transaction response
            response.Data = isDeleteOk;

            response.setSuccess(true);

            //get delete status inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_REMOVING_NOTIFICATION);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_REMOVE_NOTIFICATION);
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


    /**
     *  Used to delete notification when HR Officer post vacancy for promotion assigned to him/her
     */

    public TransactionResponse deleteNotificationForAssignedPromotionforSpecificHROfficer(MembershipUser currentUser)
    {
        TransactionResponse response = new TransactionResponse();
        try
        {
            //get detail of the logged on user. 
            Employee employee = EmployeeManager.getLoggedOnUser((Guid)currentUser.ProviderUserKey);

            if (employee == null)
            {
                return EmployeeManager.handleLoggedInUserCanNotBeIdentfied();
            }

            IDictionary<string, object> argumentMap = new Dictionary<string, object>();
            //task referance
            argumentMap.Add("@taskRef", promotion.MinuteNo);
            argumentMap.Add("@receiverEID", employee.EmpID);
            argumentMap.Add("@destrictID", PageAccessManager.getDistrictID());

            //Pass Stored Procedure Name and parameter list. 
            DBOperationsUtil dbOperation = new DBOperationsUtil(DbAccessConstants.spDeleteMailNotification, argumentMap);
            bool isDeleteOk = dbOperation.deleteRecord();

            //put the data on Transaction response
            response.Data = isDeleteOk;

            response.setSuccess(true);

            //get delete status inside the TransactionResponse. 
            return response;
        }
        catch (SqlException ex)
        {
            response.setErrorCode(DBOperationErrorConstants.E_ERROR_WHILE_REMOVING_NOTIFICATION);
            response.setMessage(DBOperationErrorConstants.M_UNABLE_TO_REMOVE_NOTIFICATION);
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

    /**
     * Writes log file using log4net. 
     */
    public void logException(Exception ex)
    {
        LoggerManager.LogError(ex.ToString(), logger);
    }
}