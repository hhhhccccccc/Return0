﻿using System;

public abstract class Panel : View
{
    public SingleUIConfig UIInfo;
    private Action CloseCallBack;
    public virtual void OnShow(params object[] args)
    {
    }
    
    public virtual void OnShowWithOpenCb(Action callBack, params object[] args)
    {
        callBack?.Invoke();
    }

    public virtual void OnShowWithCloseCb(Action callBack, params object[] args)
    {
        CloseCallBack = callBack;
    }
    
    public virtual void OnShowWithDoubleCb(Action openCallBack, Action closeCallBack, params object[] args)
    {
        openCallBack?.Invoke();
        CloseCallBack = closeCallBack;
    }
    
    public virtual void OnHide()
    {
        if (CloseCallBack != null)
        {
            CloseCallBack.Invoke();
            CloseCallBack = null;
        }
    }
}