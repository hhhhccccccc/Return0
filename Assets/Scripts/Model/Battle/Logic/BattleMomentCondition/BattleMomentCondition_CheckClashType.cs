using cfg;

public class BattleMomentCondition_CheckClashType : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        if (ParamModel is DamageParamModel model)
        {
            var param = Config.ParamList[0].ToInt();
            if (param == 1 && model.BattleClashType == BattleClashType.SingleAction)
            {
                return true;
            }
            if (param == 2 && model.BattleClashType == BattleClashType.SingleClash)
            {
                return true;
            }
            if (param == 3 && model.BattleClashType == BattleClashType.DoubleClash)
            {
                return true;
            }
            if (param == 4 && (model.BattleClashType == BattleClashType.SingleClash || model.BattleClashType == BattleClashType.DoubleClash))
            {
                return true;
            }
        }

        return false;
    }
}