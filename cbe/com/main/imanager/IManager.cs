using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IManager
/// </summary>
public interface IManager
{
    //All amanagers should provide a way to catch exception
    void logException(Exception ex);
}