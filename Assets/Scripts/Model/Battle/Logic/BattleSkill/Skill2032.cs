using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill2032 : BattleSkillBase
{
    private float RandomWelly;

    public override void Init(int skillID, BattleUnit subject, BattleUnit target, bool needResourceCost = true, bool isRepeat = false)
    {
        base.Init(skillID, subject, target, needResourceCost, isRepeat);
        RandomWelly = Util.GetRandomFloat(Config.SkillAddWellyRate[0], Config.SkillAddWellyRate[1]);
    }

    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        var hasKey = Subject.GetAllKeyTypeList();
        var removeKeyList = hasKey.Distinct();
        var removeCount = Config.ParamEx[0].ToInt();
        foreach (var removeKeyType in removeKeyList)
        {
            Subject.ChangeKey((BattleKeyType)removeKeyType, -1);
            removeCount--;
            if (removeCount <= 0)
            {
                return;
            }
        }
    }

    protected override float SkillAddWellyRate()
    {
        return RandomWelly;
    }
}