using System.Collections.Generic;
using cfg;

public class BattleTreasure10025 : BattleTreasureBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        CanTrigger = true;
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (!CanTrigger)
            {
                if (model.GetSelfSkillType(Subject.EntityID) == SkillType.SpellFormula)
                {
                    CanTrigger = true;
                }
            }
            else
            {
                if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling)
                {
                    CanTrigger = false;
                }
            }
        }
    }

    protected override void OnAddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (CanTrigger)
        {
            dict.Add(GetSymbol, Config.ParamList[0] + Config.ParamList[1] * Subject.Gr);
        }
    }

    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}
