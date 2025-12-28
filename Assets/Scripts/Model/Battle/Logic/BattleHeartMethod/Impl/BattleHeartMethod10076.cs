using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10076 : BattleHeartMethodBase
{
    private SkillType SkillType { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        SkillType = SkillType.PowerKilling;
    }

    public override void RoundStart()
    {
        base.RoundStart();
        if (SkillType == SkillType.PowerKilling)
        {
            SkillType = SkillType.ArtKilling;
        }
        else if (SkillType == SkillType.ArtKilling)
        {
            SkillType = SkillType.PowerKilling;
        }
    }
    
    public override bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var otherSKillType = model.GetOtherSkillType(Subject.EntityID);
            if (otherSKillType == SkillType)
            {
                return false;
            }

            return true;
        }

        return true;
    }

    protected override void OnRecycle()
    {
        SkillType = SkillType.None;
    }
}