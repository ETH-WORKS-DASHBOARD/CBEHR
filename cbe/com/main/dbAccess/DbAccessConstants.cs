using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This Class holds ONLY StoredProcedure list and Constants realted to DB Access such as connection string name. 
/// Dont put any other constant in this class. 
/// </summary>
public abstract class DbAccessConstants
{
    //-------==========-------------------------
    //Stored Procedure Name
    //-------==========-------------------------

    //ADD SP's

    //SP to add new Employee
    public static string spAddNewEmployee = "ADD_NEW_EMPLOYEE";

    //SP to add new Prmotion
    public static string spAddNewPromotion = "ADD_NEW_PROMOTION";

    //SP to add new Prmotion for other district
    public static string spAddNewPromotionForOtherDistrict = "ADD_NEW_PROMOTION_FOR_OTHER_DISTRICT";

    //SP to add promoted Employee from Other district
    public static string spAddPromotedEmployeefromOtherDistrict = "ADD_PROMOTED_EMP_FROM_OTHER_DISTRICT";

    //SP to add Inactive Employee
    public static string spAddInactiveEmployee = "ADD_INACTIVE_EMPLOYEE";

    //SP to add new employee into Training room
    public static string spAddEmployeeIntoTrainingRoom = "ADD_INTO_TRAININGROOM";

    //sp to add Temenos Access(Role) change Request
    public static string spAddTemenos_Access_Req = "ADD_TEMENOS_ACCESS_CHANGE";

    //SP to add new cbe branch
    public static string spAddNewcbeBranch = "ADD_NEW_CBE_BRANCH";

    //sp to add Xpress/Intranet Access(Role) change Request
    public static string spAddXpress_Access_Req = "ADD_XPRESSINTRANET_ACCESS_CHANGE";

    //SP to add Internal applicants
    public static string spAddNewInternalApplicant = "ADD_NEW_INTERNAL_APPLICANT";

    //SP to add External applicants
    public static string spAddNewExternalApplicant = "ADD_NEW_EXTERNAL_APPLICANT";

    //SP to add evaluation using form 1
    public static string spAddNewEvaluationUsingForm1 = "ADD_EVALUATION_USING_FORM1";

    //SP to add evaluation using form 2
    public static string spAddNewEvaluationUsingForm2 = "ADD_EVALUATION_USING_FORM2";

    //sp to add new Vacancy 
    public static string spAddNewVacancy = "ADD_NEW_VACANCY";

    //sp to add new HR Officer
    public static string spAddNewOfficer = "ADD_NEW_HR_OFFICER_TO_LIST";

    //sp to add new HR Clerk
    public static string spAddNewClerk = "ADD_NEW_HR_CLERK_TO_LIST";

    //SP to add new notificaiton
    public static string spGetAddNewNotification = "ADD_NEW_NOTIFICATION";

    //SP to Insert Promotion Assignment to HR Officer
    public static string spAddPromotionAssignmentToHROfficer = "ADD_PROMOTION_ASSIGNEMT_TO_HROFFICER";

    //SP to Insert New Hr officer evalaution. 
    public static string SpAddNewHrOfficerEvaluation = "ADD_NEW_HR_OFFICER_EVALUATION";

    //SP to add Education qualification for Internal applicants
    public static string spAddEducqualificationForInternalApplicant = "ADD_EDUC_QUALIFICATION_FOR_INTERNAL_APPLICANT";

    //SP to add Employee REMARK 
    public static string spAddEmployeeRemark = "ADD_EMPLOYEE_REMARK";

    //SP to add Diploma into evaluation for readvertise
    public static string spAddDiplomaIntoEvaluation = "ADD_DIPLOMA_INTO_EVALUATION_FOR_READVERTISE_VACANCY";


    //SP to check whether employee is recently selected or not
    public static string spCheckEmployeeRecSelected = "CHECK_EMPLOYEE_IN_RECENTLY_SELECTED";


    //SP to check whether employee is in penalty or not 
    public static string spCheckEmployeePenalty = "CHECK_EMPLOYEE_IN_PENALTY";



    //GET SP's

    //SP to get Employee Detail with ( LIKE @First_name + '%' OR LIKE @Emp_Id + %
    public static string spGetEmployeeByFirstNameAutoComplete = "GET_EMPLOYEE_BY_FNAME_OR_ID_AUTO";

    //SP to get Employee Detail with ( LIKE @First_name + '%' OR LIKE @Emp_Id + % and branch
    public static string spGetEmployeeByFirstNameAutoCompleteSpecBranch = "GET_EMPLOYEE_BY_FNAME_OR_ID_AUTO_SPEC_BRANCH";

    //SP to get EMployee BRANCE DETAIL using userid
    public static string spGetEmployeeBranchDetail = "GET_EMPLOYEE_BRANCH_DETAIL";

    //SP to get Employee branch , PreviousJob, job title, district. using Employee ID  
    public static string spGetEmployeeJobTitleBranchAndDistrict = "GET_EMPLOYEE_JOBTITLE_BRANCE_DISTRICT";

    //SP to get Employee to be evaluated for Processor/checker. using Employee ID  
    public static string spGetEmployeeToBeEvaluated = "GET_DETAIL_OF_EMPLOYE_TO_BE_EVALUATED";

    //SP to get Prmotion or assigned list
    public static string spGetPromotionAssigned = "GET_PROMOTED_ASSIGNED_EMPLOYEE";

    //SP to get Employee detail using EMP_ID
    public static string spGetEmployeeDetail = "GET_EMPLOYEE_DETAIL";

    //SP to get Employee detail using EMP_ID and BranchID
    public static string spGetEmployeeDetailAtBranch = "GET_SPEC_BRAN_EMPLOYEE_DETAIL";

    //sp to get Branch Employee current status
    public static string spGetBranchEmployeeStatus = "GET_BRANCH_EMPLOYEE_STATUS";

    //SP to get promoted employee for the specified Post and also for specified Status
    public static string spGetPromotedEmpBasedOnPostAndStatus = "GET_PROMOTED_EMPLOYEE_BASED_ON_POST_STATUS";

    //SP to get all promoted employee
    public static string spGetAllPromotedEmp = "GET_ALL_PROMOTED_EMPLOYEE";

    //sp to get All Branch Employee current status 
    public static string spGetAllBranchEmployeeStatus = "GET_ALL_BRANCH_EMPLOYEE_STATUS";

    //sp to get list of Assigned Employee 
    public static string spGetListOfAssignedEmployeeStatus = "GET_LIST_OF_ASSIGNED_EMPLOYEE";

    //sp to get list of Assigned Employee  from other district
    public static string spGetListOfAssignedEmployeefromOtherDistrict = "GET_ASSIGNED_EMPLOYEE_FROM_OTHER_DISTRICT";

    //sp to get All CBE branchs list
    public static string spGetAllCBEBranchsList = "GET_ALLCBE_BRANCHS_LIST";

    //sp to get branch list
    public static string spGetBranchsList = "GET_BRANCHS_LIST";

    //sp to get district list
    public static string spGetDistrictsList = "GET_DISTRICT_LIST";

    //sp to get previous job title
    public static string spGetPreviousJobTitle = "GET_PREVIOUS_JOB_TITLE";

    //sp to get current job title
    public static string spGetCurrentJobTitle = "GET_CURRENT_JOB_TITLE";

    //sp to get Temenos Access(Role) change Request List 
    public static string spGetTemenos_Access_Req = "GET_TEMENOS_ACCESS_REQ_RESULT";

    //sp to get list of promotion to assigned into HR Officer
    public static string spGetPromotionAssignedToHROfficer = "GET_LIST_OF_PROMOTION_ASSIGNED_TO_HR_OFFICER";

    //SP to get list of Active vacancies  
    public static string spAllActiveVacancy = "GET_ALL_ACTIVE_VACANY";

    //SP to get list of Active vacancies by date interval
    public static string spAllActiveVacancyByDateInterval = "GET_ALL_ACTIVE_VACANY_BY_DATE_INTERVAL";

    //SP to get list of Active vacancies  with applicant registeration NULL
    public static string spAllActiveVacancyWithApplRegFinishedNull = "GET_ALL_ACTIVE_VACANY_WITH_APP_REG_FINISHED_NULL_OR_READV";

    //SP to get list of Active vacancies with applicant registeration NOT NULL
    public static string spAllActiveVacancyWithProfileDateNotNull = "GET_ALL_ACTIVE_VACANY_WITH_APP_REG_FINISHED_NOT_NULL";

    //SP to get list of Active vacancies with profile date NULL
    public static string spAllActiveVacancyWithProfileDateNull = "GET_ALL_ACTIVE_VACANY_WITH_PROFILE_DATE_NULL";

    //SP to get Vacancy list to be assigned to HR-officer 
    //along with number of applicants in each vacancy. 
    public static string spVacancyListToAssignToHrOfficerWithApplicantsNo = "GET_VACANCY_TO_ASSIGN_WITH_APPLICANTS_NUM";

    //SP to get list of vacancy that is not assigned to HR Officer to be cancelled
    public static string spGetListOfVacancyNotAssigned2HROfficerTobeCancelled = "GET_VACANCY_TO_BE_CANCELLED_BEFORE_ASSIGNED";

    //SP to get all employee processors can rate/evaluate. 
    public static string spGetListOfEployeeForProcessor = "GET_LIST_EMPLOYEE_FOR_PROCESSOR";

    //SP to get all employee processors can rate/evaluate for the second phase. 
    public static string spGetListOfEployeeForProcessorSecondPhase = "GET_EMPLOYEE_LIST_PASSED_TO_SECOND_PHASE";

    //SP to get all employee a checker can check. 
    public static string spGetListOfEployeeForChecker = "GET_LIST_EMPLOYEE_FOR_CHECKER";

    //SP to get not announced promotion  
    public static string spGetNotAnnouncedPromotion = "GET_NOT_ANNOUNCED_PROMOTION_MINUTE_NUMB";

    //SP to get the logged on user detail.  
    public static string spGetLoggedOnUser = "GET_LOGGEN_ON_USER";

    //SP to get notification count for the logged on user. 
    public static string spGetNotificationCountForUser = "GET_NOTIFICATION_COUNT_FOR_USER";

    //sp to get list of HR officers
    public static string spGetAllHrOfficers = "GET_LIST_OF_HR_OFFICER";

    //sp to get list of HR officers and HR clerk
    public static string spGetAllHrOfficersAndHrClerk = "GET_LIST_OF_HR_OFFICER_AND_CLERK";

    //sp to get list of vacancy assigned to a processor
    public static string spGetVacancyListForProcessor = "GET_LIST_OF_VACANCY_ASSIGNED_TO_PROCESSOR";

    //sp to get list of vacancy assigned to a checker
    public static string spGetVacancyListForChecker = "GET_LIST_OF_VACANCY_ASSIGNED_TO_CHECKER";

    //sp to get list of vacancy assigned to a processor for the 2nd phase
    public static string spGetVacancyListForProcessor2ndPhase = "GET_LIST_OF_VACANCY_ASSIGNED_TO_PROCESSOR_2ND_PHASE";

    //SP to get all notification assigned to the current user. 
    public static string spGetAllNotificationForTheCurrentEmployee = "GET_NOTIFICATION_LIST_FOR_EMPLOYEE";

    //sp to get the current active HR manager. 
    public static string spGetCurrentHrManager = "GET_CURRENT_HR_MANAGER";

    //sp to get not checked (after being processed) Employee list
    public static string spGetNotCheckedEmployee = "GET_LIST_OF_NOT_CHECKED_EMPLOYEE";

    //sp to get not processed/evaluated Employee list
    public static string spGetNotProcessedEmployee = "GET_LIST_OF_NOT_PROCESSED_EMPLOYEE";

    //SP to get  District setting
    public static string spGetDistrictSetting = "GET_DESTRICT_SETTING_VALUE";

    //SP to get  already Evaluated employee
    public static string spGetAlreadyEvaluatedEmployee = "GET_ALREADY_EVALUATED_EMPLOYEE";

    //SP to get  already Evaluated employee for second phase
    public static string spGetAlreadyEvaluatedEmployeeForSecondPhase = "GET_ALREADY_EVALUATED_EMPLOYEE_FOR_SECOND_PHASE";

    //SP to get the current status of Vacancy 
    public static string spGetCurrentStatusOfVacancy = "GET_CURRENT_STATUS_OF_VACANCY";

    //SP to get Hr Officer processor rating report
    public static string spGetHROfficerProcessorRatingReport = "GET_HR_OFFICER_PROCESSOR_RATING_REPORT";

    //SP to get Hr Officer Checker rating report
    public static string spGetHROfficerCheckerRatingReport = "GET_HR_OFFICER_CHECKER_RATING_REPORT";

    //SP to get all Hr Officer processor rating report
    public static string spGetAllHROfficerProcessorRatingReport = "GET_ALL_HR_OFFICER_PROCESSING_RATING_REPORT";

    //SP to get All Hr Officer Checker rating report
    public static string spAllGetHROfficerCheckerRatingReport = "GET_ALL_HR_OFFICER_CHECKER_RATING_REPORT";

    //SP to get applicant detial for profile request
    public static string spGetApplicantDetialForProfileRequest = "GET_APPLICANT_DETIAL_FOR_PROFILE_REQUEST";

    //SP to get applicant detial
    public static string spGetApplicantDetial = "GET_APPLICANT_DETIAL";

    //SP to get vacancy which is applicant registeration complete
    public static string spGetApplicantRegCompVacancy = "GET_APPLICANT_REGISTRATION_COMPLETE_VACANCY";

    //SP to get status of the current vacancy. 
    public static string spCurrentVacancyStatus = "GET_VACANCY_STATUS";

    //SP to get whether applicant registration complete or not.
    public static string spIsApplicantRegisterComplete = "IS_APPLICANT_REGISRATION_COMPLETE_REGISTERED";

    //SP to get whether applicant registration complete or not.
    public static string spIsProfileDateRegistered = "IS_PROFILE_ARRIVED_DATE_REGISTERED";

    //SP to get phase 1 Vacancy evaluation result. 
    public static string spGetApplicantPhas1Evaluation = "GET_APPLICANT_PHASE1_EVALUTION";

    //SP to get recently selected, lateral and penality employee
    public static string spGetRecSelectedLateralPenality = "GET_REC_SEL_LATERAL_PENALITY_EMPLOYEE";

    //SP to get phase 2 Vacancy evaluation result.
    public static string spGetApplicantPhas2Evaluation = "GET_APPLICANT_PHASE2_EVALUTION";

    //SP to get count of applicant evluated by checker grouped by checking date
    public static string spGetApplicantCountChecker = "GET_APPLICANT_COUNT_EVALUATED_BY_CHECKER";

    //SP to get count of applicant evluated by processor grouped by processing date
    public static string spGetApplicantCountProcessor = "GET_APPLICANT_COUNT_EVALUATED_BY_PROCESSOR";

    //SP to get Hr officers detail for a given vacancy. 
    public static string spHROfficerDetailOfAVacancy = "GET_HR_OFFICERS_DETAIL_OF_A_VACANCY";

    //SP to get Promotion detail: Minute No + HrOfficer name if this vacany is opened for it.
    public static string spPromotionDetailThisVacancyOpenedFor = "GET_PROMOTION_DETAIL_FOR_WHICH_VACANCY_OPENED";

    //SP to get Vacancy detial spGetVacancyDetailToBeEditted
    public static string spGetVacancyDetail = "GET_VACANCY_DETAIL";

    //SP to get Vacancy detial by date interval 
    public static string spGetVacancyByDateInterval = "GET_VACANCY_DETIAL_BY_DATE_INTERVAL";

    //SP to get Vacancy detial by date interval 
    public static string spGetVacancyToGenerateReportByDateInterval = "GET_VACANCY_TO_GENERATE_REPORT_BY_DATE_INTERVAL";

    //SP to get Final Phase Vacancy detial by date interval 
    public static string spGetFinalPhaseVacancyByDateInterval = "GET_FINAL_PHASE_VACANCY_BY_DATE_INTERVAL";

    //SP to get Vacancy detial to be editted 
    public static string spGetVacancyDetailToBeEditted = "GET_VACANCY_DETIAL_TO_BE_EDITTED";

    //SP to get Vacancy detial to be editted 
    public static string spGetVacancyDetailToAnnounce = "GET_VACANCY_DETIAL_TO_ANNOUNCE";

    //SP to get Vacancy detial which is not assigned to HR Officer
    public static string spGetVacancyDetailNotAssignedToBeEditted = "GET_NOT_ASSIGNED_VACANCY_DETIAL_TO_EDIT";

    //SP to get Inactive Employee 
    public static string spGetInactiveEmployeeDetail = "GET_INACTIVE_EMPLOYEE";

    //SP to get Employee Remark
    public static string spGetEmployeeRemark = "GET_EMPLOYEE_REMARK";

    //SP to get List of vacancy detail that is not announced 
    public static string spGetListOfNotAnnouncedVacancy = "GET_LIST_OF_VACANCY_NOT_ANNOUNCED";

    //SP to get vacancy detail for HR Officer to announce
    public static string spGetNotAnnouncedVacancyForHROfficerPost = "GET_NOT_ANNOUNCED_VACANCY_FOR_HR_OFFICER";

    //SP to get all applicants not evaluated for a given vacancy. 
    public static string spGetNotEvaluatedApplicants = "GET_NOT_EVALUATED_APPLICANT";

    //SP to get all applicants not evaluated for a given vacancy on second phase. 
    public static string spGetNotEvaluatedApplicantsforSecondPhase = "GET_NOT_EVALUATED_APPLICANT_FOR_SECOND_PHASE";

    //SP to get all managerial job position
    public static string spGetAllManagerialJobposition = "GET_MANAGERIAL_JOBTITLE";

    //SP to get HR Officer PMS report by date interval
    public static string spGetHROfficerReport = "GET_HR_OFFICER_PMS_REPORT";

    //SP to get HR Officer PMS report by date Name 
    public static string spGetHROfficerReportbyName = "GET_HR_OFFICER_PMS_REPORT_BY_NAME";

    //SP to get Employee at managerial position at spcified position
    public static string spGetEmpAtManagerialPosition = "GET_EMPLOYEE_AT_MANAGERIAL_POSITION";

    //SP to get of HR Officer
    public static string spGetHROfficer = "GET_CURRENT_HR_OFFICER";

    //SP to get of HR Clerk
    public static string spGetHRClerk = "GET_CURRENT_HR_CLERK";

    //SP to get applicant result after evaluation
    public static string spGetApplicantResultAfterEvaluation = "GET_APPLICANT_RESULT_AFTER_EVALUATION";

    //SP to get applicant passed to second phase
    public static string spGetApplicantPassedToSecPhase = "GET_APPLICANT_PASSED_TO_SEC_PHASE";


    //UPDATE SP's

    //sp to Update branch Employee Status
    public static string spUpdateBranchEmployeeStatus = "UPDATE_BRANCH_EMPLOYEE_STATUS";

    //sp to update list of Assigned Employee 
    public static string spUpdateListOfAssignedEmployeeStatus = "UPDATE_LIST_OF_ASSIGNED_EMPLOYEE";

    //sp to update list of Assigned Employee from other district
    public static string spUpdateListOfAssignedEmployeefromOtherDistrict = "UPDATE_LIST_OF_ASSIGNED_EMPLOYEE_FROM_OTHER_DISTRICT";

    //SP to update Vacancy processor and checker info.
    public static string spUpdateVacancyEvaluators = "UPDATE_VACANCY_WITH_EVALUATORS";

    //SP to update applicant raing on form 1
    public static string spUpdateApplicantRatingForm1 = "UPDATE_APPLICANT_RATING_FORM1";

    //SP to update applicant raing on form 2
    public static string spUpdateApplicantRatingForm2 = "UPDATE_APPLICANT_RATING_FORM2";

    //SP to update applicant rating phase 2 on form 1
    public static string spUpdateApplicantRatingphase2Form1 = "UPDATE_APPLICANT_RATING_PHASE_TWO_ON_FORM1";

    //SP to update applicant rating phase 2 on form 2
    public static string spUpdateApplicantRatingphase2Form2 = "UPDATE_APPLICANT_RATING_PHASE_TWO_ON_FORM2";

    //SP to update Vacancy status. 
    public static string spUpdateVacancyStatus = "UPDATE_VACANCY_STATUS";

    //SP to update Vacancy status. 
    public static string spUpdateVacancyStatusAfterRating = "UPDATE_VACANCY_STATUS_AFTER_RATING";

    //SP to update profile arrived date. 
    public static string spUpdateProfileArrivedDate = "UPDATE_PROFILE_ARRIVED_DATE";

    //SP to update applicant registration finished date 
    public static string spUpdateApplicantRegistrationFinishedDate = "UPDATE_APPLICANT_REGISTRATION_FINISHED_DATE";

    //SP to update list of applicants evliation status
    public static string spUpdateAppicantsStatus = "UPDATE_APPLICANTS_EVALUATION_STATUS";

    //SP to update promition table vacancy Number, Vacancy date & Promotion status 
    public static string spUpdatePromotionStatus = "UPDATE_PROMOTION_STATUS";

    //SP to get all promoted employee
    public static string spUpdatePromotedEmp = "UPDATE_PROMOTION";

    //SP to get all promoted employee
    public static string spUpdateEmpEducLevel = "UPDATE_EMPLOYEE_EDUCATIONAL_LEVEL";

    //SP to update vacancy detail that is assigned to HR Officer
    public static string spUpdateAssignedVacancyDetail = "UPDATE_ASSIGNED_VACANCY_DETAIL";

    //SP to update vacancy detail that is assigned to HR Officer 
    public static string spUpdateNotAnnouncedVacancyToAnnounced = "UPDATE_NOT_ANNOUNCED_VACANCY_TO_ANNOUNCE";

    //SP to update vacancy detail that is NOT assigned to HR Officer
    public static string spUpdateNotAssignedVacancyDetail = "UPDATE_NOT_ASSIGNED_VACANCY_DETAIL";

    //SP to update HR Officer/Clerk PMS Result
    public static string spUpdateHROfficerClerkPMSResult = "UPDATE_HR_OFFICER_PMS_RESULT";

    //SP to update HR Officer/Clerk PMS Result
    public static string spUpdateApplicantDetailform1 = "UPDATE_APPLICANT_RESULT_FORM1";

    //SP to readvertise vacancy from profile arrived date. 
    public static string spReadvertiseVacFromProfileArr = "READVERTISE_VAC_FROM_PROFILE_ARRIVED";

    //DELETE SP's

    //SP to remove notification from mail box when action is taken on that task 
    public static string spDeleteMailNotification = "DELETE_MAIL_NOTIFICATION";

    //SP to delete Employee information 
    public static string spDeleteEmployeeInfor = "DELETE_USER_INFORMATION";

    //SP to delete Employee information  REMOVE_OFFICER_OR_CLERK_FROM_LIST
    public static string spDeleteApplicant = "DELETE_EMPLOYEE_FROM_VACANCY";

    //SP to delete Employee information  
    public static string spRemoveHROfficerOrClerk = "REMOVE_OFFICER_OR_CLERK_FROM_LIST";


    //------=========-------------------------------------
    //Connection String property name
    //------=========-------------------------------------
    public static string connStringTagName = "HRTrainingConnectionString";

}