using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// list constatns related to Vacancy here
/// </summary>
public abstract class VacancyConstants
{
    public static string VACANCY_NOT_ACTIVE_YET = "0";
    public static string VACANCY_ACTIVE = "1";
    public static string VACANCY_APPLICANT_REGISTERED = "2";
    public static string VACANCY_ASSIGNED_TO_HR_OFFICERS = "3";
    public static string VACANCY_PROCESSING_DONE = "4";
    public static string VACANCY_CHECKING_DONE = "5";
    public static string VACANCY_EVALUTION1_DONE = "6";   
    public static string VACANCY_NEED_INTERVIEW_ONLY = "7";
    public static string VACANCY_NEED_EXAM_ONLY = "8";
    public static string VACANCY_NEED_EXAM_AND_INTERVIEW = "9";     
    public static string VACANCY_EVALUTION2_DONE = "10";
    public static string VACANCY_READVERTISED = "11";
    public static string VACANCY_CANELLED = "12";

    public static string APPLICANT_PASSED_TO_PHASE2 = "1";
    public static string APPLICANT_PASSED_FINAL = "2"; 
}