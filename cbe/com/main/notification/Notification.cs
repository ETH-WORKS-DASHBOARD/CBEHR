using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Notification
/// </summary>
public class Notification
{
    public Notification()
    {
    }
    string receiver;

    public string Receiver
    {
        get { return receiver; }
        set { receiver = value; }
    }
    string sender;

    public string Sender
    {
        get { return sender; }
        set { sender = value; }
    }
    string taskRreference;

    public string TaskRreference
    {
        get { return taskRreference; }
        set { taskRreference = value; }
    }
    string notificationMessage;

    public string NotificationMessage
    {
        get { return notificationMessage; }
        set { notificationMessage = value; }
    }
    string notificationIsFor;

    public string NotificationIsFor
    {
        get { return notificationIsFor; }
        set { notificationIsFor = value; }
    }
    string registeredDate;

    public string RegisteredDate
    {
        get { return registeredDate; }
        set { registeredDate = value; }
    }
}