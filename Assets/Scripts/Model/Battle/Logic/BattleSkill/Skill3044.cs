using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill3044 : BattleSkillBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    private int WinTargetID { get; set; }
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.AttackClashWin)
            {
                WinTargetID = model.AttackID;
            }
            else if (model.HitClashWin)
            {
                WinTargetID = model.HitID;
            }
        }
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        if (WinTargetID > 0)
        {
            var target = BattleManager.GetUnit(WinTargetID);
            if (target != null)
            {
                target.ReturnSkillResourceCost(returnKey: true);
            }

            WinTargetID = 0;
        }
    }

    public override void Recycle()
    {
        WinTargetID = 0;
        base.Recycle();
    }
}