using cfg;

public class BattleMomentEffect_AddIngoreBeCountList : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList != null)
        {
            var keyType = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                target.AddIgnoreBeCounterKey((BattleKeyType)keyType);
            }
        }
    }
}