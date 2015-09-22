using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Promotion
/// </summary>
public class Promotion
{
    public Promotion()
    {
    }
    string empID;

    public string EmpID
    {
        get { return empID; }
        set { empID = value; }
    }
    string prevBranch;

    public string PrevBranch
    {
        get { return prevBranch; }
        set { prevBranch = value; }
    }
    string branch;

    public string Branch
    {
        get { return branch; }
        set { branch = value; }
    }
    string minuteNo;

    public string MinuteNo
    {
        get { return minuteNo; }
        set { minuteNo = value; }
    }
    string post;

    public string Post
    {
        get { return post; }
        set { post = value; }
    }
    string status;

    public string Status
    {
        get { return status; }
        set { status = value; }
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
    string isAnnounced;

    public string IsAnnounced
    {
        get { return isAnnounced; }
        set { isAnnounced = value; }
    }
    string promotionDate;

    public string PromotionDate
    {
        get { return promotionDate; }
        set { promotionDate = value; }
    }
}