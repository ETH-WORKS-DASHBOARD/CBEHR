using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Holds info about the Vacancy Applicant
/// </summary>
public class VacancyApplicant
{
	public VacancyApplicant()
	{
	}

    string empId;

    public string EmpId
    {
        get { return empId; }
        set { empId = value; }
    }

    string vacancyDate;

    public string VacancyDate
    {
        get { return vacancyDate; }
        set { vacancyDate = value; }
    }

    string vacancyNo;

    public string VacancyNo
    {
        get { return vacancyNo; }
        set { vacancyNo = value; }
    }

    string fName;
    public string FirstName
    {
        get { return fName; }
        set { fName = value; }
    }

    string mName;
    public string MiddleName
    {
        get { return mName; }
        set { mName = value; }
    }

    string lName;
    public string LastName
    {
        get { return lName; }
        set { lName = value; }
    }

    string curBranch;
    public string CurrentBranch
    {
        get { return curBranch; }
        set { curBranch = value; }
    }

    string jobTitle;
    public string JobTitle
    {
        get { return jobTitle; }
        set { jobTitle = value; }
    }

    string curJGrade;
    public string CurrentJGrade
    {
        get { return curJGrade; }
        set { curJGrade = value; }
    }

    string eduLevel;
    public string EducLevel
    {
        get { return eduLevel; }
        set { eduLevel = value; }
    }

    string qualif;
    public string Qualification
    {
        get { return qualif; }
        set { qualif = value; }
    }
}