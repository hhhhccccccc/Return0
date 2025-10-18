using System.Linq;
using Zenject;

public class BattleMomentCondition_CheckTargetHasAvatarBuff : BattleMomentCondition
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        { 
            var state = Config.ParamList[1].ToInt() == 1;
            if (state && BattleBuffManager.CheckTargetHasAvatarBuff(target.EntityID))
            {
                return true;
            }

            if (!state && !BattleBuffManager.CheckTargetHasAvatarBuff(target.EntityID))
            {
                return true;
            }
        }

        return false;
    }
}