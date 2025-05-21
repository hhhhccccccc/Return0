using System;
using UnityEngine;

public class Log
{
    private static bool IgnoreLog = false;

    public static void Debug(string msg)
    {
        if(IgnoreLog)return;
        UnityEngine.Debug.Log(msg);
    }
    
    public static void Error(string msg)
    {
        if(IgnoreLog)return;
        UnityEngine.Debug.LogError(msg);
    }
    
    public static void Error(Exception e)
    {
        if (e.Data.Contains("StackTrace"))
        {
            Error($"{e.Data["StackTrace"]}\n{e}");
            return;
        }
        string str = e.ToString();
        Error(str);
    }
}
