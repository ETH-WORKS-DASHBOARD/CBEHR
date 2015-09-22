using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmployeeRemark
/// </summary>
public class EmployeeRemark
{
	public EmployeeRemark()
	{
	}

    string employeeID;

    public string EmployeeID
    {
        get { return employeeID; }
        set { employeeID = value; }
    }
    string remarkReferanceNo;

    public string RemarkReferanceNo
    {
        get { return remarkReferanceNo; }
        set { remarkReferanceNo = value; }
    }
    string dateOfRemark;

    public string DateOfRemark
    {
        get { return dateOfRemark; }
        set { dateOfRemark = value; }
    }
    string remarkType;

    public string RemarkType
    {
        get { return remarkType; }
        set { remarkType = value; }
    }
    string penaltyPerc;

    public string PenaltyPerc
    {
        get { return penaltyPerc; }
        set { penaltyPerc = value; }
    }
    string managerID;

    public string ManagerID
    {
        get { return managerID; }
        set { managerID = value; }
    }
    string managerPosition;


    public string ManagerPosition
    {
        get { return managerPosition; }
        set { managerPosition = value; }
    }
    string remarkReason;

    public string RemarkReason
    {
        get { return remarkReason; }
        set { remarkReason = value; }
    }
    string responsiblehrOfficerID;

    public string ResponsiblehrOfficerID
    {
        get { return responsiblehrOfficerID; }
        set { responsiblehrOfficerID = value; }
    }
    string branch;

    public string Branch
    {
        get { return branch; }
        set { branch = value; }
    }
}