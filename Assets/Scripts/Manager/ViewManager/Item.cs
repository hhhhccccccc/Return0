using cfg;
using UnityEngine;

public class Item : View
{
    protected override void OnAwake()
    {
        base.OnAwake();
        OnItemCreate();
    }
    
    protected virtual void OnItemCreate(){}

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    
    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }

    public void Release()
    {
        UnRegisterEvent();
        ReleaseItemChilds();
        OnRelease();
    }

    protected virtual void OnRelease() { }
}
