using System;
using UnityEngine;

public class LogManager : ManagerBase, ILogManager
{
    private bool IgnoreLog = false;

    public void D(string msg)
    {
        if(IgnoreLog)return;
        UnityEngine.Debug.Log(msg);
    }
    
    public void E(string msg)
    {
        if(IgnoreLog)return;
        UnityEngine.Debug.LogError(msg);
    }
    
    public void E(Exception e)
    {
        if (e.Data.Contains("StackTrace"))
        {
            E($"{e.Data["StackTrace"]}\n{e}");
            return;
        }
        string str = e.ToString();
        E(str);
    }
}