using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill3065 : BattleSkillBase
{
    //todo 根据目标的伤口状态层数造成3~8次伤口效果
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var buffID = Config.ParamEx[0].ToInt();
            var delta = Config.ParamEx[1].ToInt();
            var buff = target.GetBuff(buffID);
            if (buff != null)
            {
                buff.TriggerBuffMomentByCountIgnoreLayerCount(buff.LayerCount + delta, model);
            }
        }
    }
} 