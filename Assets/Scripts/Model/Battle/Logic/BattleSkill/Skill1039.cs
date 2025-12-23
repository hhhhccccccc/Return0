using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1039 : BattleSkillBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.OtherID == Subject.EntityID &&
                (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling || model.GetSelfSkillType(Subject.EntityID) == SkillType.ArtKilling))
            {
                Subject.ReduceBuffLayerCount(Config.ParamEx[0].ToInt(), Config.ParamEx[1].ToInt());
            }
        }
    }
}