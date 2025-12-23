using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4055 : BattleSkillBase
{
    private List<BattleUnit> TempUnitList = new();
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        TempUnitList.Clear();
        TempUnitList.AddRange(BattleManager.GetAllAliveUnit());
        if (TempUnitList.Contains(Subject))
        {
            TempUnitList.Remove(Subject);
        }
        if (TempUnitList.Count > 0)
        {
            var value = Config.ParamEx[0] + Subject.Gr * Config.ParamEx[0];
            var addValue = 0.0f;
            foreach (var target in TempUnitList)
            {
                target.ChangeProperty(BattlePropertyType.MaxHpInt, -value, BattleSource.Skill);
                addValue += value;
            }

            Subject.ChangeProperty(BattlePropertyType.MaxHpInt, addValue, BattleSource.Skill);
        }
    }
}