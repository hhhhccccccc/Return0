using cfg;
using Zenject;

public class BattleMomentEffect_AddRandonKeyToDefineCount : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var count = Config.ParamList[1].ToInt();
                if (count == 0)
                {
                    count = target.GetKeyPropertyMax();
                }
                var has = target.GetAllKeyCount();
                if (has >= count) return;
                var addCount = has - count;
                var list = Util.GetRandomKey(addCount);
                Subject.ChangeKeyList(list, true, ChangeKeyReason.SkillEffect);
            }
        }
    }
}