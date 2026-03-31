using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill2032 : BattleSkillBase
{
    private float RandomWelly;

    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (RandomWelly == 0)
        {
            RandomWelly = Util.GetRandomFloat(0, 0.5f);
        }
    }

    public override void SelfActionWheelStart()
    {
        var hasKey = Subject.GetAllKeyTypeList().Clone();
        var removeKeyList = hasKey.Distinct().ToList();

        while (removeKeyList.Count > 3)
        {
            removeKeyList.RemoveAt(0);
        }

        DoChangeKeyList(Subject, removeKeyList.Select(o => (BattleKeyType)o).ToList(), false, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        return RandomWelly;
    }

    protected override void OnSkillRecycle()
    {
        RandomWelly = 0;
    }
}