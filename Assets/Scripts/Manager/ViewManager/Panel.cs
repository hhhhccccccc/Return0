using System;
using System.Collections.Generic;
using Zenject;

public abstract class Panel : View
{
    private Action CloseCallBack;
    public virtual PanelLayerType PanelLayerType => PanelLayerType.Normal;
    
    public virtual void Esc()
    {
        Close();
    }
    
    public virtual void OnShow()
    {
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        RegisterEvent();
        OnPanelCreate();
    }

    protected virtual void OnPanelCreate() { }

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

    protected override void OnDestroy()
    {
        UnRegisterEvent();
        ReleaseItemChilds();
        OnViewDestroy();
    }

    protected virtual void OnViewDestroy()
    {
    
    }

}