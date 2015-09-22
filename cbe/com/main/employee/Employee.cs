using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class represents Employee Data
/// </summary>
public class Employee
{
    public Employee()
    {
    }

    private string userName;

    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }

    private Guid userId;

    public Guid UserID
    {
        get { return userId; }
        set { userId = value; }
    }

    string empID;

    public string EmpID
    {
        get { return empID; }
        set { empID = value; }
    }
    string fName;

    public string FName
    {
        get { return fName; }
        set { fName = value; }
    }
    string mName;

    public string MName
    {
        get { return mName; }
        set { mName = value; }
    }
    string lName;

    public string LName
    {
        get { return lName; }
        set { lName = value; }
    }
    string sex;

    public string Sex
    {
        get { return sex; }
        set { sex = value; }
    }

    private string salary;

    public string Salary
    {
        get { return salary; }
        set { salary = value; }
    }

    string hdate;

    public string Hdate
    {
        get { return hdate; }
        set { hdate = value; }
    }

    string prevJob;

    public string PrevJob
    {
        get { return prevJob; }
        set { prevJob = value; }
    }
    string jobTitle;

    public string JobTitle
    {
        get { return jobTitle; }
        set { jobTitle = value; }
    }
    string prevJobGrade;

    public string PrevJobGrade
    {
        get { return prevJobGrade; }
        set { prevJobGrade = value; }
    }
    string jobGrade;

    public string JobGrade
    {
        get { return jobGrade; }
        set { jobGrade = value; }
    }

    string branch;

    public string Branch
    {
        get { return branch; }
        set { branch = value; }
    }

    string branchID;

    public string BranchID
    {
        get { return branchID; }
        set { branchID = value; }
    }

    string district;

    public string District
    {
        get { return district; }
        set { district = value; }
    }

    //String representation of some fields:
    string sprevJob;

    public string SPrevJob
    {
        get { return sprevJob; }
        set { sprevJob = value; }
    }
    string sjobTitle;

    public string SJobTitle
    {
        get { return sjobTitle; }
        set { sjobTitle = value; }
    }
    string sjobGrade;

    public string SJobGrade
    {
        get { return sjobGrade; }
        set { sjobGrade = value; }
    }

    string sbranch;

    public string SBranch
    {
        get { return sbranch; }
        set { sbranch = value; }
    }

    string sdistrict;

    public string SDistrict
    {
        get { return sdistrict; }
        set { sdistrict = value; }
    }

    string status;

    public string Promstatus
    {
        get { return status; }
        set { status = value; }
    }

    string processedDate;

    public string ProcessedDate
    {
        get { return processedDate; }
        set { processedDate = value; }
    }

    string checkedDate;

    public string CheckedDate
    {
        get { return checkedDate; }
        set { checkedDate = value; }
    }

    //employeecan be internal OrderedParallelQuery external employee
    string employeeType;

    public string EmployeeType
    {
        get { return employeeType; }
        set { employeeType = value; }
    }

    string educationalQualification;

    public string EducationalQualification
    {
        get { return educationalQualification; }
        set { educationalQualification = value; }
    }

    string majorCategory;

    public string MajorCategory
    {
        get { return majorCategory; }
        set { majorCategory = value; }
    }
}