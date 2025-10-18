using cfg;

public class BattleMomentEffect_TriggerBuffTimes : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var buffID = Config.ParamList[1].ToInt();
            var minCount = Config.ParamList[2].ToInt();
            var maxCount = Config.ParamList[3].ToInt();
            var count = Util.GetRandomInt(minCount, maxCount + 1);
            foreach (var target in targetList)
            {
                var buff = target.GetBuff(buffID);
                if (buff != null)
                {
                    buff.TriggerBuffMomentByCount(count, ParamModel);
                }
            }
        }
    }
}