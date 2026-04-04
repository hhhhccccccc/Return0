using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4055 : BattleSkillBase
{
    //扣除其余在场角色体上限160+GR*32，自身增加等量的体上限
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var value = Config.ParamEx[0] + Subject.Gr * Config.ParamEx[0];
        var addValue = 0.0f;
        foreach (var target in BattleManager.GetAllAliveUnit())
        {
            if (target != Subject)
            {
                DoChangeProperty(target, BattlePropertyType.MaxHpInt, -value, BattleSource.Skill);
                addValue += value;
            }
        }
        DoChangeProperty(Subject, BattlePropertyType.MaxHpInt, addValue, BattleSource.Skill);
    }

    //获得1次行动次数，玄炁+5
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 5, BattleSource.Skill);
    }
}