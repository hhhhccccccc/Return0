using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect2008002 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        { 
            var buffID = Config.ParamList[1].ToInt();
            var propertyID = Config.ParamList[2].ToInt();
            var pct = Config.ParamList[3];
            foreach (var target in targetList)
            {
                var propertyValue = target.GetProperty((BattlePropertyType)propertyID);
                propertyValue *= pct;
                BattleBuffManager.AddBuff(Subject, buffID, Subject, 1, new List<float> { propertyValue }, MomentType);
                BattleBuffManager.AddBuff(target, buffID, Subject, 1, new List<float> { -propertyValue }, MomentType);
            }
        }
    }
}