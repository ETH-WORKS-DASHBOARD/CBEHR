using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * This class hold Error code and Error Message constants
 */
public abstract class DBOperationErrorConstants
{
    //------=========-------------------------------------
    //Messages, all messages start with M_
    //------=========-------------------------------------

    public static string CHECK_AGAIN_TRY = "Please check and try again";

    public static string CONTACT_ADMIN = "please contact your system administrator";

    public static string M_DUPLICATE_EID_KEY_ERROR = "Duplicate employee record is detected, " + CHECK_AGAIN_TRY;

    public static string M_DUPLICATE_REF_NO_KEY_ERROR = "Duplicate reference number is detected, " + CHECK_AGAIN_TRY;

    public static string M_DUPLICATE_APPLICANT_KEY_ERROR = "Duplicate appicant record is detected, " + CHECK_AGAIN_TRY;

    public static string M_DUPLICATE_HR_EVALUATION_KEY_ERROR = "Duplicate HR officer evaluation record is detected, " + CHECK_AGAIN_TRY;

    public static string M_DUPLICATE_VACANCY_KEY_ERROR = "Duplicate vacancy record is detected, " + CHECK_AGAIN_TRY;

    public static string M_DUPLICATE_PROMOTION_KEY_ERROR = "Duplicate promotion record is detected, " + CHECK_AGAIN_TRY;

    public static string M_DUPLICATE_APP_FORM_KEY_ERROR = "This applicant evaluation is already registered, Please go to modification page if you want to change the rating";

    public static string M_DUPLICATE_NOTIF_ERROR = "Duplicate notification is registered for this user";

    public static string M_EMPLOYEE_REGISTER_OK = "Employee registered successfully";

    public static string M_EMPLOYEE_REMARK_REGISTER_OK = "Employee Remark registered successfully";

    public static string M_INACTIVE_EMPLOYEE_REGISTER_OK = "Employee added into inactive employee successfully";

    public static string M_EMPLOYEE_REGISTERED_FOR_VACANCY_OK = "Employee registered for vacancy successfully";

    public static string M_HR_OFFICER_VALUATION_OK = "HR officer evaluation registered successfully";

    public static string M_EMPLOYEE_EVALUATION_REGISTERED_OK = "Employee evaluation registered successfully";

    public static string M_PROMOTION_REGISTER_OK = "Promotion registered successfully";

    public static string M_NOTIF_REGISTER_OK = "Notification registered successfully";

    public static string M_VACANCY_REGISTER_OK = "Vacancy registered successfully";

    public static string M_VACANCY_UPDATE_OK = "Vacancy updated successfully";

    public static string M_UNKNOWN_ERROR_EMP_REGISTER = "Unknown error occurred while registering employee, " + CHECK_AGAIN_TRY;

    public static string M_UNKNOWN_ERROR_APPLICANT_REGISTER = "Unknown error occurred while registering applicant, " + CONTACT_ADMIN;

    public static string M_UNKNOWN_ERROR_HR_OFFICER_REGISTER = "Unknown error occurred while registering HR officer evaluation, " + CONTACT_ADMIN;

    public static string M_UNKNOWN_ERROR_APP_RATING_REGISTER = "Unknown error occurred while registering applicant evaluation result, " + CHECK_AGAIN_TRY;

    public static string M_UNKNOWN_ERROR_VACANCY_REGISTER = "Unknown error occurred while registering Vacancy, " + CHECK_AGAIN_TRY;

    public static string M_UNKNOWN_ERROR_UPDATE_VACANCY = "Unknown error occurred while updating Vacancy, " + CHECK_AGAIN_TRY;

    public static string M_UNKNOWN_ERROR_UPDATE_APP_EVA_STATUS = "Unknown error occurred while assing vacancy applicants, " + CONTACT_ADMIN;

    public static string M_UNKNOWN_ERROR_GENERATING_REPORT = "Unknown error occurred while generating one or more report, " + CONTACT_ADMIN;

    public static string M_UNKNOWN_ERROR_REGISTERING_NEW_BRANCH = "Unknown error occurred while registering branch, " + CONTACT_ADMIN;

    public static string M_UNKNOWN_ERROR_VERIFY_COMPLETE_VANCANCY = "Unknown error occurred while checking all evaluations, " + CHECK_AGAIN_TRY;

    public static string M_TRANSACTION_SUCCESS = "Your operation is successful";

    public static string M_VACANCY_ASSIGNED_SUCCESS = " Vacancy successfully assigned.";

    public static string M_APPLICANTS_PASSED_SUCCESS  = " Applicants passed sucessfully ";

    public static string M_RATING_UPDATE_SUCCESS = "Rating updated sucessfully.";

    public static string M_NOTIFICATION_INFO = " Please note that this notification(s) will be removed only after you finish the task assign to you";

    public static string PK_DUPLICATE_INDICATOR = "Violation of PRIMARY KEY";

    public static string M_DUPLICATE_PROMOTION_ASSIGNEMENT = "Promotion already assigned to another HR Officer.";

    public static string M_UNKNOWN_EVIL_ERROR = "The System ecounter an expected error, " + CONTACT_ADMIN;

    public static string M_NO_ACTIVE_HR_MANAGER = "Either there is no HR Manager registered or more than 1 HR manager registered, " + CHECK_AGAIN_TRY;

    public static string M_CAN_NOT_IDENTIFY_CURRENT_USER = "We can not identify the currently logged in user, " + CONTACT_ADMIN;

    public static string M_CAN_NOT_AUTHENTICATE_THE_CURRENT_USER = "We have difficult authenticating the current user, this promotion is not notified to the HR Manager";

    public static string M_UNAUTHORIZED_USER = "The current user is unauthorised user. Please login to the system or contact your system administrator";

    public static string M_NOTIFY_REGISTERED_PROMOTION_FAILED = "We have a problem trying to notify this promotion to the HR Manager, please note the promotion Minute No. and inform your HR Manager";

    public static string M_ERROR_WHILE_READING_NOTIF = "There is  error while Reading notification " + CONTACT_ADMIN + " if the problem persists";

    public static string M_EMPLOYEE_ALREAD_IEXISTS = "Employee with this ID already exists";

    public static string M_UNABLE_TO_REMOVE_NOTIFICATION = "We are unable to remove your task from mail box " + CONTACT_ADMIN;

    public static string M_NO_EMPLOYEE_REGISTERED_FOR_PROCESSOR = "Please make sure that applicant is registered for this vacancy but not checked by the checker yet";

    public static string M_NO_EMPLOYEE_REGISTERED_FOR_PROCESSOR_SECOND_PHASE = "Please make sure that applicant is registered for this vacancy & pass for the second phase evaluation";

    public static string M_NO_EMPLOYEE_REGISTERED_FOR_CHECKER = "Please make sure that applicant is registered for this vacancy and also evaluated by the processor";

    public static string M_EMPLOYEE_ALREADY_PROCESSED_BY_YOU = "This applicant is already processor by you, You may edit and update the evaluation again";

    public static string M_EMPLOYEE_ALREADY_CHECKED_BY_YOU = "This applicant is already checked by you, You may edit the evaluation here";

    public static string M_EMPLOYEE_ALREADY_CHECKED = "This applicant rating is already reviewd by the checker. Processing is not possible";

    public static string M_EMPLOYEE_NOT_PROCESSED_YET = "This applicant is not evaluated by processor yet, please try again later";

    public static string M_VACANCY_NOT_IN_ASSIGNED_STATE = "Either evaluation of this vacancy is completed or it is not assigned to you";

    public static string M_VACANCY_COMPLETION_WARNING = "<u><b>Are you Sure you want to Complete this vacancy evaluation ? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, the evaluation in your side will be marked as completed! " +
                               "<br />" +
                               "and you will not be able to do any rating on this vacancy any more " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_VACANCY_REVERSE_WARNING = "<u><b>Are you Sure you want to Reverse this vacancy status ? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, the status of this vacancy is changed into applicant registration not completed! " +
                               "<br />" +
                               "and you should have to complete the vacancy again as soon as you have registered all applicant " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_VACANCY_PROFILE_ARRIVED_WARNING = "<u><b>Are you Sure you want to Register profile arrived date for the selected vacancy? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, the applicant profile is considered as arrived! " +
                               "<br />" +
                               "and you will not be able undo this operation, so please make sure the profile is arrived before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_VACANCY_RE_ADVERTISE = "<u><b>Are you Sure you want to Re-advertise the selected vacancy? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, the status of the vacancy changed into Re-Advertise! " +
                               "<br />" +
                               "and you will not be able undo this operation, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_VACANCY_CANCELLED = "<u><b>Are you Sure you want to CANCEL vacancy? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, the status of the vacancy is changed into CANCELLED! " +
                               "<br />" +
                               "and you will not be able undo this operation, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_VACANCY_COMPLETE_APPLICANT_REG = "<u><b>Are you Sure you want to complete registration of applicant for the selected vacancy? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, you can't register applicant for the selected vacancy! " +
                               "<br />" +
                               "and you will not be able undo this operations, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_DEACTIVATED_EMPLOYEE = "<u><b>Are you Sure you want to deactive the employee? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, you will never get information related to this employee! " +
                               "<br />" +
                               "The selected employee is marked as inactive. " +
                               "<br />" +
                               "and you will not be able undo this operations, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_EMPLOYEE_REMARK = "<u><b>Are you Sure you want to register remark for the above employee? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, the employee will be affected positively or negatively based on the remark! " +
                               "<br />" +
                               "If the remark is 'Bad Remark' the employee can't be applied for vacancy atleast for 6 month or 1 year based on the penality. " +
                               "<br />" +
                               "and you will not be able undo this operations, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_PROMOTION_NOT_REQUIRED_VACANCY = "<u><b>Are you Sure you DONT want to post vacancy for the selected Promotion? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, you can't assign promotion to HR Officer to post vacancy! " +
                               "<br />" +
                               "and you will not be able undo this operations, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_POST_VACANCY_TO_OUTSIDE = "<u><b>Are you Sure you want to Post vacancy? </b></u>" +
                                "<br />" +
                                "<br />" +
                               "<span style='color:Yellow'><b>WARNING ::::</b></span> If you confirm, vacancy is displayed on vacancy page. Everyone that will visit the page can see it," +
                               "<br />" +
                               "and you will not be able undo this operations, so please make sure before you confirm it. " +
                               "<br />" +
                               "<br />" +
                               "If you are not sure yet,  <u>You can cancel</u> this operation and do it later." +
                               "<br />" +
                               "<br />";
    public static string M_VACANCY_COMPLETION_NOT_POSSIBLE = "You can NOT complete evaluation of this vacancy. There exists not evaluated applicant(s) of this vacancy.";

    public static string M_VACANCY_COMPLETED_SUCCESS = "Vacancy Completed Successfully";

    public static string M_VACANCY_CANCELLED_SUCCESS = "Vacancy Cancelled Successfully";

    public static string M_VACANCY_READVERTISED_SUCCESS = "Vacancy changed to readvertised state Successfully";

    public static string M_DIPLOMA_APPLICANT_SUCCESS = "Diploma Applicant Included to evaluation Successfully";

    public static string M_VACANCY_REVERSED_SUCCESS = "Vacancy Reversed Successfully";

    public static string M_VACANCY_COMPLETING_FAIL = "We encounter a difficulty trying to complete this vacancy. " + CONTACT_ADMIN;

    public static string M_VACANCY_REVERSE_FAIL = "We encounter a difficulty trying to reverse this vacancy. " + CONTACT_ADMIN;

    public static string M_VACANCY_READVERTISED_FAIL = "We encounter a difficulty trying to readvertise this vacancy. " + CONTACT_ADMIN;

    public static string M_VACANCY_CANCELLATION_FAIL = "We encounter a difficulty trying to cancel this vacancy. " + CONTACT_ADMIN;

    public static string M_VACANCY_CHANGED_TO_ASSIGNED_FROM_READVERTISE = "Re-advertised vacancy applicant registration is completed. The vacancy can be Processed and also checked now.";

    public static string M_VACANCY_CHANGED_TO_ASSIGNED_FROM_READVERTISE_FAIL = "We encounter a difficulty trying to change vacancy status from readvertised to assigned to HR Officer.";

    public static string M_VACANCY_READVERTISE_FAIL = "We encounter a difficulty trying to readvertise this vacancy. " + CONTACT_ADMIN;

    public static string M_INCLUDE_DIPLOMA_FAIL = "We encounter a difficulty trying to Include diploma Applicant to evaluation . " + CONTACT_ADMIN;

    public static string M_VACANCY_STAUS_CHECK_FAIL = "We encounter a difficulty trying verify status of this vacancy";

    public static string M_VACANCY_NOT_PROCESSED_YET = "You can not complete this vacancy as processing is not finished yet";

    public static string M_PROFILE_DATE_REGISTERED = "Profile arrived date is registered successfully!";

    public static string M_APPLICANT_REGISTERED_COMPLETE = "Applicant registration is completed successfully";

    public static string M_PROFILE_DATE_NOT_REGISTERED = "Error encountered while trying to register profile arrived date. Please contact your system administrator if the issue persists.";

    public static string M_APPLICANT_REGISTERED_NOT_COMPLETE = "Error encountered while trying to complete this applicant registration . Please contact your system administrator if the issue persists.";

    public static string M_APPLICANT_COMPLETE_NOT_REGISTERED = "Applicant registeration is not completed yet. please complete applicant registration first.";

    public static string M_APPLICANT_COMPLETE_REGISTERED_ALREADY = "The selected Vacancy is already completed.";

    public static string M_PROFILE_ARRIVED_DATE_REGISTERED_ALREADY = "The selected Vacancy Profile date is already registered.";

    public static string M_UNABLE_TO_GET_VACANCY_EVALUATION_RESULT = "We encounter an error while trying to get Vacancy evaluation result " + CONTACT_ADMIN;

    public static string M_UNABLE_TO_GET_RECENTLY_SELECTED_RESULT = "We encounter an error while trying to get recently selected " + CONTACT_ADMIN;

    public static string M_UNABLE_TO_GET_LATERAL_APPLICANT_RESULT = "We encounter an error while trying to get recently selected " + CONTACT_ADMIN;
    
    public static string M_NO_VACANCY_EVALUTION_RESULT = "Please check if applicants are evaluated for this vacancy and also if the vacancy is still active, if the issue persists " + CONTACT_ADMIN;

    public static string M_VACANCY_EVALUTION_RESULT_GENERATED_SUCCESS = "Vacancy evaluation result generated successfully ";

    public static string M_NO_VACANCY_FOUND_FOR_THIS_PROCESS = "No vacancy found for this process ";

    public static string M_NO_VACANCY_FOUND_FOR_DATE_INTERVAL = "There is no vacancy posted bewteen the selected date interval";

    public static string M_APPLICANT_RANK_NOT_GENERATED_YET = "Please make sure that you have generated applicants ranking report before this operation";

    public static string M_SYSTEM_ENCOUNTERED_UNKNOWN_ERROR = "The system encountered unknown error";

    public static string M_UNABLE_TO_REMOVING_INACTIVE_EMPLOYEE = "We encounter an error while trying to remove inactive employee " + CONTACT_ADMIN;

    public static string M_REPORT_GENERATED_SUCCESS = "Report generated successfully!";

    public static string M_GENERATE_EMPLOYEE_REMARK_REPORT_EMPTY = "There is no employee remark between the selected date interval";

    public static string M_GENERATE_INACTIVE_EMPLOYEE_REPORT_EMPTY = "There is no inactive employee between the selected date interval";

    public static string M_GENERATE_HR_OFFICER_REPORT_EMPTY = "There is no report between the selected date interval";

    public static string M_GENERATE_HR_OFFICER_BY_NAME_REPORT_EMPTY = "There is no report for the selected HR Officer/Clerk.";

    public static string M_UNABLE_TO_GENERATE_INACTIVE_EMPLOYEE = "We encounter an error while trying to get inactive employee " + CONTACT_ADMIN;

    public static string M_UNABLE_TO_GENERATE_HR_OFFICER_REPORT_EMPLOYEE = "We encounter an error while trying to get PMS report for HR Officer/Clerk " + CONTACT_ADMIN;

    public static string M_HR_OFFICER_REGISTER_OK = "Successfully Added to the list";

    public static string M_UNABLE_TO_APPLICANT_RESULT = "We encounter an error while trying to get applicant result " + CONTACT_ADMIN;

    //------=========-------------------------------------
    //Error Codesm all Error codes start with E_
    //------=========-------------------------------------

    //Error codes 100 - 199 are related to general DB access error
    public static string E_DUPLICATE_KEY_ERROR = "(105)";

    public static string E_UNKNOWN_ERROR_AT_DB_OOPERATION = "(107)";

    //Error code 200 - 299 is realted to authenticating and authorising the current user
    public static string E_CAN_NOT_IDENTIFY_CURRENT_USER = "(203)";
    public static string E_UNAUTHORIZED_USER = "(205)";


    //Error code 300 - 399 is related to HR employees (HR Managerm HR officer, ... )
    public static string E_NO_ACTIVE_MANAGER = "(301)";

    //Error code 400 -499 is related to Notification operations
    public static string E_PROMOTION_ASSIGNEMETN_FAILED = "(401)";
    public static string E_PROMOTION_NOTIFICATION_FAILED = "(402)";
    public static string E_ERROR_WHILE_READING_NOTIFICATION = "(405)";
    public static string E_ERROR_WHILE_REMOVING_NOTIFICATION = "(409)";
    public static string E_ERROR_WHILE_REMOVING_INACTIVE_EMPLOYEE = "(410)";

    //Error code 500 - 599 is realted to vacancy operations
    public static string E_VACANCY_COMPLETION_NOT_POSSIBLE = "(501)";
    public static string E_VACANCY_COMPLETION_FAILED = "(502)";
    public static string E_PROFILE_ARRIVED_DATE_UPDATE_FAILED = "(503)";
    public static string E_NO_EVALUATION_RESULT = "(504)";
    public static string E_VACANCY_CANCELLATION_FAILED = "(505)";
    public static string E_VACANCY_REVERSE_FAILED = "(506)";
    public static string E_VACANCY_READVERTISE_FAILED = "(507)";
    public static string E_VACANCY_READVERTISE_TO_REASSIGNED_FAILED = "(508)";
    public static string E_INCLUDE_DIPLOMA_APPLICANT_FAILED = "(509)";

    //UNKOWN Critical exception. 
    public static string E_UNKNOWN_EVIL_ERROR = "(1000)";
}

