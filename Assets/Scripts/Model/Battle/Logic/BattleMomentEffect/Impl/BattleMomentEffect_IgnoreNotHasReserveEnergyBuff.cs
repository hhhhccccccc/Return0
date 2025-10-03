using cfg;

public class BattleMomentEffect_IgnoreNotHasReserveEnergyBuff : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var state = Config.ParamList[2].ToInt();
            foreach (var target in targetList)
            {
                if (target.IsAlive())
                {
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
    }
}