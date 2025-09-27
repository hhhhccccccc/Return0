using cfg;

public class BattleMomentEffect_IgnoreNotHasReserveEnergyBuff : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null && target.IsAlive())
        {
            var state = Config.ParamList[2].ToInt();
            switch (Config.ParamList[1].ToInt())
            {
                case 1:
                    target.AddIgnoreTargetNotHasUpBuff(state == 1 ? 1 : -1);
                    break;
                case 2:
                    target.AddIgnoreTargetNotHasDownBuff(state == 1 ? 1 : -1);
                    break;
                case 3:
                    target.AddIgnoreTargetNotHasLeftBuff(state == 1 ? 1 : -1);
                    break;
                case 4:
                    target.AddIgnoreTargetNotHasRightBuff(state == 1 ? 1 : -1);
                    break;
            }
        }
    }
}