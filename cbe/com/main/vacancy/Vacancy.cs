using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Vacancy
/// </summary>
public class Vacancy
{
    public Vacancy()
    {
    }

    string vacancyNo;

    public string VacancyNo
    {
        get { return vacancyNo; }
        set { vacancyNo = value; }
    }
    string postedDate;

    public string PostedDate
    {
        get { return postedDate; }
        set { postedDate = value; }
    }
    string vancayTitle;

    public string VancayTitle
    {
        get { return vancayTitle; }
        set { vancayTitle = value; }
    }
    string branchId;

    public string BranchId
    {
        get { return branchId; }
        set { branchId = value; }
    }
    string vancyDeadline;

    public string VancyDeadline
    {
        get { return vancyDeadline; }
        set { vancyDeadline = value; }
    }
    string vacancyforJobGrade;

    public string VacancyforJobGrade
    {
        get { return vacancyforJobGrade; }
        set { vacancyforJobGrade = value; }
    }
    string vacancyOpenedFor;

    public string VacancyOpenedFor
    {
        get { return vacancyOpenedFor; }
        set { vacancyOpenedFor = value; }
    }
    string vacancyEvaluationForm;

    public string VacancyEvaluationForm
    {
        get { return vacancyEvaluationForm; }
        set { vacancyEvaluationForm = value; }
    }

    string status;

    public string Status
    {
        get { return status; }
        set { status = value; }
    }

    string generalWrkExprPercent;

    public string GeneralWrkExprPercent
    {
        get { return generalWrkExprPercent; }
        set { generalWrkExprPercent = value; }
    }

    string specificWrkExprPercent;

    public string SpecificWrkExprPercent
    {
        get { return specificWrkExprPercent; }
        set { specificWrkExprPercent = value; }
    }
    
    string yearRequiredforGeneral;

    public string YearRequiredforGeneral
    {
        get { return yearRequiredforGeneral; }
        set { yearRequiredforGeneral = value; }
    }

    string yearRequiredforSpec;

    public string YearRequiredforSpec
    {
        get { return yearRequiredforSpec; }
        set { yearRequiredforSpec = value; }
    }

    string relatedWrkExprPercent;

    public string RelatedWrkExprPercent
    {
        get { return relatedWrkExprPercent; }
        set { relatedWrkExprPercent = value; }
    }

    string managerRecPercent;

    public string ManagerRecPercent
    {
        get { return managerRecPercent; }
        set { managerRecPercent = value; }
    }

    string examinationPercent;

    public string ExaminationPercent
    {
        get { return examinationPercent; }
        set { examinationPercent = value; }
    }

    string interviewPercent;

    public string InterviewPercent
    {
        get { return interviewPercent; }
        set { interviewPercent = value; }
    }

    string applicantComplete;

    public string ApplicantComplete
    {
        get { return applicantComplete; }
        set { applicantComplete = value; }
    }

    string jobDescription;

    public string JobDescription
    {
        get { return jobDescription; }
        set { jobDescription = value; }
    }

    string jobRequirement;

    public string JobRequirement
    {
        get { return jobRequirement; }
        set { jobRequirement = value; }
    }

    string salaryAndBenefit;

    public string SalaryAndBenefit
    {
        get { return salaryAndBenefit; }
        set { salaryAndBenefit = value; }
    }

    string profileArrivedDate;

    public string ProfileArrivedDate
    {
        get { return profileArrivedDate; }
        set { profileArrivedDate = value; }
    }

    string processorStartDate;

    public string ProcessorStartDate
    {
        get { return processorStartDate; }
        set { processorStartDate = value; }
    }

    string processorEndDate;

    public string ProcessorEndDate
    {
        get { return processorEndDate; }
        set { processorEndDate = value; }
    }

    string accessorStartDate;

    public string AccessorStartDate
    {
        get { return accessorStartDate; }
        set { accessorStartDate = value; }
    }

    string accessorEndDate;

    public string AccessorEndDate
    {
        get { return accessorEndDate; }
        set { accessorEndDate = value; }
    }

    string responsibleProcessorEID;

    public string ResponsibleProcessorEID
    {
        get { return responsibleProcessorEID; }
        set { responsibleProcessorEID = value; }
    }

    string reponsibleAccessorEID;

    public string ResponsibleAccessorEID
    {
        get { return reponsibleAccessorEID; }
        set { reponsibleAccessorEID = value; }
    }
}