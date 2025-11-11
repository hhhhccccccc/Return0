using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill2033 : BattleSkillBase
{
    private float RandomWelly;

    public override void Init(int skillID, BattleUnit subject, BattleUnit target, bool needResourceCost = true, bool isRepeat = false)
    {
        base.Init(skillID, subject, target, needResourceCost, isRepeat);
        RandomWelly = Util.GetRandomFloat(Config.SkillAttackAddWelly[0], Config.SkillAttackAddWelly[1]);
    }

    public override void AfterSelfActionWheelStart()
    {
        base.AfterSelfActionWheelStart();
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
        Subject.ChangeKey((BattleKeyType)removeTwoKeyType, -2);
        if (removeOneList.Contains(removeTwoKeyType))
        {
            removeOneList.Remove(removeTwoKeyType);
        }

        if (removeOneList.Count > 0)
        {
            var removeOneKeyType = Util.GetRandom(removeOneList);
            Subject.ChangeKey((BattleKeyType)removeOneKeyType, -1);
        }
    }
    
    protected override float SkillAddWellyRate()
    {
        return RandomWelly;
    }
}