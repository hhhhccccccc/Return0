using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill2033 : BattleSkillBase
{
    private float RandomWelly;

    public override void Init(int skillID, BattleUnit subject, BattleUnit target, bool needResourceCost = true, bool isRepeat = false)
    {
        base.Init(skillID, subject, target, needResourceCost, isRepeat);
        RandomWelly = Util.GetRandomFloat(0, 1.4f);
    }

    protected override void OnSelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        var hasKey = Subject.GetAllKeyTypeList();
        var upCount = hasKey.Count(key => key == (int)BattleKeyType.KeyUp);
        var downCount = hasKey.Count(key => key == (int)BattleKeyType.KeyDown);
        var leftCount = hasKey.Count(key => key == (int)BattleKeyType.KeyLeft);
        var rightCount = hasKey.Count(key => key == (int)BattleKeyType.KeyRight);
        var removeTwoList = new List<int>();
        
        if (upCount >= 2)
        {
            removeTwoList.Add(1);
        }
        if (downCount >= 2)
        {
            removeTwoList.Add(2);
        }
        if (leftCount >= 2)
        {
            removeTwoList.Add(3);
        }
        if (rightCount >= 2)
        {
            removeTwoList.Add(4);
        }
        
        var removeOneList = new List<int>();
        if (upCount >= 1)
        {
            removeTwoList.Add(1);
        }
        if (downCount >= 1)
        {
            removeTwoList.Add(2);
        }
        if (leftCount >= 1)
        {
            removeTwoList.Add(3);
        }
        if (rightCount >= 1)
        {
            removeTwoList.Add(4);
        }

        var removeTwoKeyType = Util.GetRandom(removeTwoList);
        var list = new List<BattleKeyType>
        {
            (BattleKeyType)removeTwoKeyType,
            (BattleKeyType)removeTwoKeyType
        };
        if (removeOneList.Contains(removeTwoKeyType))
        {
            removeOneList.Remove(removeTwoKeyType);
        }
        if (removeOneList.Count > 0)
        {
            var removeOneKeyType = Util.GetRandom(removeOneList);
            list.Add((BattleKeyType)removeOneKeyType);
            DoChangeKeyList(Subject, list, false, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
        }
    }

    //todo 威力增加0~140的百分比
    public override float GetWellyRateEx(int skillGuid)
    {
        return RandomWelly;
    }
}