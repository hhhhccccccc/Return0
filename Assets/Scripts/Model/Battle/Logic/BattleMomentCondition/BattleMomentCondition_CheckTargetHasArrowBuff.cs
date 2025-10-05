using System.Linq;
using Zenject;

public class BattleMomentCondition_CheckTargetHasArrowBuff : BattleMomentCondition
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        { 
            var state = Config.ParamList[2].ToInt() == 1;
            var type = Config.ParamList[1].ToInt();
            switch (type)
            {
                case 1:
                    if (state && BattleBuffManager.CheckTargetHasUpFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }

                    if (!state && BattleBuffManager.CheckTargetHasUpFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }
                    break;
                case 2:
                    if (state && BattleBuffManager.CheckTargetHasDownFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }

                    if (!state && BattleBuffManager.CheckTargetHasDownFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }
                    break;
                case 3:
                    if (state && BattleBuffManager.CheckTargetHasLeftFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }

                    if (!state && BattleBuffManager.CheckTargetHasLeftFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }
                    break;
                case 4:
                    if (state && BattleBuffManager.CheckTargetHasRightFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }

                    if (!state && BattleBuffManager.CheckTargetHasRightFirstSkillBuff(target.EntityID))
                    {
                        return true;
                    }
                    break;
            }
        }

        return false;
    }
}