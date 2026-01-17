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
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                BattleBuffManager.AddBuff(target, Config.ParamEx[0].ToInt(), Subject, Config.ParamEx[1].ToInt());
            }
        }
    }
}
