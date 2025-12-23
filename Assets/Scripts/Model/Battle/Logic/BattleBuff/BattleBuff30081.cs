using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30081 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var skillID = Subject.GetSkillID();
            if (skillID == GameConst.Battle.SkillFuXiaoJian)
            {
                var targetID = model.SelfID == Subject.EntityID ? model.OtherID : model.SelfID;
                var target = BattleManager.GetUnit(targetID);
                BattleBuffManager.AddBuff(target, Config.ParamEx[0].ToInt(), Subject, Config.ParamEx[1].ToInt());
            }
        }
    }
}
