using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using log4net;

/// <summary>
/// Summary description for ErrorLogManager
/// </summary>
public class LoggerManager
{

    public static void LogError(String ex, ILog logger)
    {
        //set log file anme
        log4net.GlobalContext.Properties["LogName"] = getfilePathPrefix() + "_ServerError.log";

        //configure log4net
        log4net.Config.XmlConfigurator.Configure();

        logger.Error(ex);
    }

    public static void LogInfo(String infoMessage, ILog logger)
    {
        //set log file anme
        log4net.GlobalContext.Properties["LogName"] = getfilePathPrefix() + "_ApplicationLog.log";

        //configure log4net
        log4net.Config.XmlConfigurator.Configure();

        logger.Info(infoMessage);
    }

    public static void LogDebug(String debugMessage, ILog logger)
    {

        //set log file anme
        log4net.GlobalContext.Properties["LogName"] = getfilePathPrefix() + "_DebugLog.log";

        //configure log4net
        log4net.Config.XmlConfigurator.Configure();

        logger.Debug(debugMessage);
    }

    public static void upDateWithGenericErrorMessage(TransactionResponse response)
    {
        //Display generic message. 
        response.setMessageType(TransactionResponse.SeverityLevel.ERROR);
        response.setMessage(DBOperationErrorConstants.M_SYSTEM_ENCOUNTERED_UNKNOWN_ERROR);
        response.setErrorCode(DBOperationErrorConstants.E_UNKNOWN_EVIL_ERROR);
        response.setSuccess(false);
    }

    private static string getfilePathPrefix()
    {
        return PageAccessManager.getSessionData(PageConstants.LOGPATH_session) + DateTime.Now.ToString("M_d_yyyy");
    }
}