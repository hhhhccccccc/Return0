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
            if (model.HitID == Subject.EntityID &&
                (model.AttackSkillType == SkillType.PowerKilling || model.AttackSkillType == SkillType.ArtKilling))
            {
                Subject.ReduceBuffLayerCount(Config.ParamEx[0].ToInt(), Config.ParamEx[1].ToInt());
            }
        }
    }
}