using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4026 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }

    //期间免疫杀式的直接伤害
    public override bool IgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (IsInAction)
        {
            return true;
        }

        return false;
    }

    //补充随机的键到达持有上限
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddRandomKeyToDefineCount(Subject, 0, ChangeKeyReason.SkillEffect);
    }
    
    //敌手获得5个键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoAddRandomKey(clashUnit, 5, ChangeKeyReason.SkillEffect);
    }
}