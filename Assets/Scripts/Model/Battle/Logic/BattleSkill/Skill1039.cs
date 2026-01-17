using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1039 : BattleSkillBase
{
    private bool CanTrigger { get; set; }
    public override void Init(int skillGuid, BattleUnit subject, BattleUnit target, bool needCostResource = true, bool isRepeat = false)
    {
        base.Init(skillGuid, subject, target, needCostResource, isRepeat);
        CanTrigger = false;
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfSkillUseSuccess(Subject.EntityID))
            {
                CanTrigger = true;
            }
        }
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (!CanTrigger)
        {
            return;
        }
        if (paramModel is DamageParamModel model)
        {
            var otherSkillType = model.GetOtherSkillType(Subject.EntityID);
            if (otherSkillType == SkillType.PowerKilling || otherSkillType == SkillType.ArtKilling)
            {
                Subject.ReduceBuffLayerCount(Config.ParamEx[0].ToInt(), Config.ParamEx[1].ToInt());
            }
        }
    }

    public override void Recycle()
    {
        CanTrigger = false;
        base.Recycle();
    }
}