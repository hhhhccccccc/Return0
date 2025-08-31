using cfg;

public class BattleMomentEffect_AddIngoreBeCountList : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var keyType = Config.ParamList[1].ToInt();
            target.AddIgnoreBeCounterKey((BattleKeyType)keyType);
        }
    }
}