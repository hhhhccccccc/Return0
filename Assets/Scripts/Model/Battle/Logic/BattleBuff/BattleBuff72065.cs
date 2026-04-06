using System.Collections.Generic;
using cfg;

//todo 封锁目标两个键直到回合结束
public class BattleBuff72065 : BattleBuffBase
{
    private List<int> LockedKeyGuidList = new();
    protected override void OnBuffStart()
    {
        var count = GetConfigParamInt(0);
        var lockedKeyList = DoLockRandomKey(Subject, count);
        if (lockedKeyList != null)
        {
            foreach (var keyData in lockedKeyList)
            {
                LockedKeyGuidList.Add(keyData.KeyGuid);
            }
        }
    }

    protected override void OnBuffRemove()
    {
        DoUnlockKey(Subject, LockedKeyGuidList);
    }
    
    protected override void OnBuffRecycle()
    {
        LockedKeyGuidList.Clear();
    }
}
