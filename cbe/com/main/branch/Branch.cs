using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BranchDetial
/// </summary>
public class Branch
{
	public Branch()
	{
	}

    public string branchName;
    public string BranchName
    {
        get { return branchName; }
        set { branchName = value; }
    }

    public string district;
    public string District
    {
        get { return district; }
        set { district = value; }
    }
}