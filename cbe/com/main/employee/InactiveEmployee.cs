using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InactiveEmployee
/// </summary>
public class InactiveEmployee
{
	public InactiveEmployee()
	{
	}
    
    string dateOfEmployment;

    public string DateOfEmployment
    {
        get { return dateOfEmployment; }
        set { dateOfEmployment = value; }
    }
    string dateOfTermination;

    public string DateOfTermination
    {
        get { return dateOfTermination; }
        set { dateOfTermination = value; }
    }
    string majorCategory;

    public string MajorCategory
    {
        get { return majorCategory; }
        set { majorCategory = value; }
    }
    string reasonForLeave;

    public string ReasonForLeave
    {
        get { return reasonForLeave; }
        set { reasonForLeave = value; }
    }
}