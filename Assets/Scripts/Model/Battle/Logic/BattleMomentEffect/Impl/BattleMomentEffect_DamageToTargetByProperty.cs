using cfg;
using Zenject;

public class BattleMomentEffect_DamageToTargetByProperty : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subjectList = GetUnitByParamID(Config.ParamList[0]);
        var targetList = GetUnitByParamID(Config.ParamList[1]);
        if (subjectList.Count > 0 && targetList.Count > 0)
        {
            var subject = subjectList[0];
            var propertyID = Config.ParamList[2].ToInt();
            var property = subject.GetProperty((BattlePropertyType)propertyID);
            var damage = property * Config.ParamList[3];
            var damageType = (DamageType)(Config.ParamList[4].ToInt());
            var source = (BattleSource)(Config.ParamList[5].ToInt());
            foreach (var target in targetList)
            {
                target.ReduceHp(damage, damageType, subject.EntityID);
            }
        }
    }
}