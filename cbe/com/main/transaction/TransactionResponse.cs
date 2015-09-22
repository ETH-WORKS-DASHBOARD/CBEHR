using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class represents a response of DB transaction.
/// </summary>
public class TransactionResponse
{
    private Boolean isSuccess;

    private string message;

    private string errorCode;

    //indicate the severity level of the message. 
    public enum SeverityLevel { UNDEF, SUCESS, ERROR, WARNING, INFO };

    private SeverityLevel messageType;

    //this will carry data inside the transaction response
    //the caller should cast the data to the type it expects. 
    //for that reason Object type is used
    private Object data;

    //constructor
    public TransactionResponse()
    {
    }


    //getter and setter

    public Boolean isSuccessful()
    {
        return this.isSuccess;
    }
    public void setSuccess(Boolean isSuccess)
    {
        this.isSuccess = isSuccess;
    }

    public string getMessage()
    {
        return this.message;
    }
    public void setMessage(string message)
    {
        this.message = message;
    }

    public string getErrorCode()
    {
        return this.errorCode;
    }
    public void setErrorCode(string errorCode)
    {
        this.errorCode = errorCode;
    }

    public SeverityLevel getMessageType()
    {
        return messageType;
    }

    public void setMessageType(SeverityLevel severityLevel)
    {
        messageType = severityLevel;
    }

    public Object Data
    {
        get { return data; }
        set { data = value; }
    }
}