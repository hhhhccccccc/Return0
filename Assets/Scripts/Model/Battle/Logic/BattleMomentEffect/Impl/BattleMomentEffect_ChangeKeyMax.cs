using cfg;
using Zenject;

public class BattleMomentEffect_ChangeKeyMax : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var count = Config.ParamList[1].ToInt();
            subject.ChangeKey(BattleKeyType.KeyMaxEx, count);
        }
    }
}