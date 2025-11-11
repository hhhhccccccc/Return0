using cfg;
using Zenject;

public class BattleMomentEffect_ChangeKeyMax : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var count = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                target.ChangeKeyProperty(BattleKeyType.KeyMaxEx, count);
            }
        }
    }
}