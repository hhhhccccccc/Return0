using System;
using System.Collections.Generic;
using Zenject;

public abstract class Panel : View
{
    private Action CloseCallBack;
    
    public virtual void OnShow()
    {
    }
    
    public virtual void OnHide()
    {
        if (CloseCallBack != null)
        {
            CloseCallBack.Invoke();
            CloseCallBack = null;
        }
    }

    protected void Close()
    {
        OnClose();
        UIManager.CloseUI(gameObject.name);
    }

    protected virtual void OnClose(){}
}