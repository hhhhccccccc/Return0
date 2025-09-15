using Zenject;

public class BattleMomentEffect_AddRandonKeyToDefineCount : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var count = Config.ParamList[1].ToInt();
            if (count == 0)
            {
                count = subject.GetKeyMax();
            }
            var has = subject.GetKeyCount();
            if (has >= count) return;
            var addCount = has - count;
            var list = Util.GetRandomKey(addCount);
            foreach (var keyType in list)
            {
                subject.ChangeKey(keyType, 1);
            }
        }
    }
}