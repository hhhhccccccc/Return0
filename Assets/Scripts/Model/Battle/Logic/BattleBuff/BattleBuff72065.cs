using System.Collections.Generic;
using cfg;

//封锁目标两个键直到回合结束
public class BattleBuff72065 : BattleBuffBase
{
    private List<int> LockedKeyGuidList = new();
    protected override void OnBuffStart()
    {
        base.OnBuffStart();
        var count = Config.ParamEx[0].ToInt();
        var lockedKeyList = Subject.LockRandomKey(count);
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
        foreach (var guid in LockedKeyGuidList)
        {
            Subject.UnlockKey(guid);
        }
        base.OnBuffRemove();
    }
    protected override void OnBuffRecycle()
    {
        LockedKeyGuidList.Clear();
    }
}
