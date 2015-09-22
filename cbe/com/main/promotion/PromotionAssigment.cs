using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PromotionAssigment
/// </summary>
public class PromotionAssigment
{
	public PromotionAssigment()
	{
	}
    //Promotion promotion;

    //Promotion PromotionValues
    //{
    //    get { return promotion; }
    //    set { promotion = value; }
    //}
    string hrOfficerID;

    public string HROfficerID
    {
        get { return hrOfficerID; }
        set { hrOfficerID = value; }
    }
    string deadLine;

    public string DeadLine
    {
        get { return deadLine; }
        set { deadLine = value; }
    }
    string remark;

    public string Remark
    {
        get { return remark; }
        set { remark = value; }
    }
    string minuteNo;

    public string MinuteNo
    {
        get { return minuteNo; }
        set { minuteNo = value; }
    }
}