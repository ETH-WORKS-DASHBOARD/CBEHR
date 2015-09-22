using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VacancyEvaluationForm
/// </summary>
public class VacancyEvaluationForm
{
    public VacancyEvaluationForm()
    {
    }

    string empId;

    public string EmpId
    {
        get { return empId; }
        set { empId = value; }
    }

    string vacancyNo;

    public string VacancyNo
    {
        get { return vacancyNo; }
        set { vacancyNo = value; }
    }
    string vacancyDate;

    public string VacancyDate
    {
        get { return vacancyDate; }
        set { vacancyDate = value; }
    }
    string educationLevelMark;

    public string EducationLevelMark
    {
        get { return educationLevelMark; }
        set { educationLevelMark = value; }
    }
    string generalWorkExpr;

    public string GeneralWorkExpr
    {
        get { return generalWorkExpr; }
        set { generalWorkExpr = value; }
    }
    string specificWorkRxpr;

    public string SpecificWorkRxpr
    {
        get { return specificWorkRxpr; }
        set { specificWorkRxpr = value; }
    }
    string recommendationOfMgrLine;

    public string RecommendationOfMgrLine
    {
        get { return recommendationOfMgrLine; }
        set { recommendationOfMgrLine = value; }
    }
    string interviewResultRecommendation;

    public string InterviewResultRecommendation
    {
        get { return interviewResultRecommendation; }
        set { interviewResultRecommendation = value; }
    }
    string remark;

    public string Remark
    {
        get { return remark; }
        set { remark = value; }
    }

    string relatedWorExperiance;

    public string RelatedWorExperiance
    {
        get { return relatedWorExperiance; }
        set { relatedWorExperiance = value; }
    }
    string examinationResult;

    public string ExaminationResult
    {
        get { return examinationResult; }
        set { examinationResult = value; }
    }
    string interviewResult;

    public string InterviewResult
    {
        get { return interviewResult; }
        set { interviewResult = value; }
    }

    string formType;

    /**
     * use this to indicate form type.
     */
    public string FormType
    {
        get { return formType; }
        set { formType = value; }
    }

    string applicantType;
    public string ApplicantType
    {
        get { return applicantType; }
        set { applicantType = value; }
    }

    string applicantStatus;
    public string ApplicantStatus
    {
        get { return applicantStatus; }
        set { applicantStatus = value; }
    }
}