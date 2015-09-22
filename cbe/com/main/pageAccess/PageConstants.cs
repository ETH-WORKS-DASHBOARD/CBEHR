using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Holds Constants related to pages. 
/// </summary>
public abstract class PageConstants
{
    public static string AT = "@";

    public static string ERROR = "ERROR";

    public static string GREATER_GREATER_THAN = ">>";

    public static string Y = "Y";

    //url parameter tag to indicare if we are on Notification list page
    public static string NOTIF_PAGE_TAG = "FNOTIF";

    public static string PROCESSOR = "PROCESSOR";

    public static string CHECKER = "CHECKER";

    public static string REPORT_PHASE1 = "1";

    public static string REPORT_PHASE2 = "2";


    // ------------ District setting

    //get district name
    public static string DISTRCT_NAME = "DISTRCIT_NAME";

    //get log Path
    public static string LOG_PATH = "LOG_PATH";

    // -------- Session data TAG. 
    public static string LOGPATH_session = "LOGPATH";

    // ....... Property file TAG
    public static string DISTRICT_ID = "DISTRICT_ID";

    //get absolute path of property file
    private static string PROPERTY_FILES_DIRECTORY = "Properties";
    private static string DISTRICT_SETTING_FILE = "destrictProperties.txt";
    public static string DISTRICT_PROPERTY_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PROPERTY_FILES_DIRECTORY, DISTRICT_SETTING_FILE);
}

