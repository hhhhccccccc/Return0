
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleRenderManager : View
{
    #region 封装事件

    public void DispatchClickEventModel(BattleClickType clickType, int param1 = 0, int param2 = 0, int param3 = 0, int param4 = 0)
    {
        var model = PoolManager.GetClass<BattleClickEventModel>();
        model.ClickType = clickType;
        model.Param1 = param1;
        model.Param2 = param2;
        model.Param3 = param3;
        model.Param4 = param4;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }

    public void DelayCall(Action action, float delay)
    {
        StartCoroutine(DelayCall_Co(action, delay));
    }

    private IEnumerator DelayCall_Co(Action action, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
    
        action?.Invoke();
    }
    
    #endregion
}
