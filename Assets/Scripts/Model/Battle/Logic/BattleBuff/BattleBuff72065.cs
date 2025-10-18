using System.Collections.Generic;
using cfg;

//封锁目标两个键直到回合结束
public class BattleBuff72065 : BattleBuffBase
{
    private List<int> LockedKeyGuidList = new();
    protected override void OnStart()
    {
        base.OnStart();
        var count = Config.ParamEx[0].ToInt();
        for (int i = 1; i <= count; i++)
        {
            var lockedGuid = Subject.LockRandomKey();
            if (lockedGuid > 0)
            {
                LockedKeyGuidList.Add(lockedGuid);
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

    public override void Recycle()
    {
        LockedKeyGuidList.Clear();
        base.Recycle();
    }
}
