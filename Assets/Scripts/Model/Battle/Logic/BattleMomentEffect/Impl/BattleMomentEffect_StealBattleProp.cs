using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_StealBattleProp : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var times = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                for (var i = 1; i <= times; i++)
                {
                    var propID = target.GetRandomProp();
                    if (propID != 0)
                    {
                        target.ReduceProp(propID, 1);
                        Subject.Bf.AddProp(propID, 1);
                    }
                }
            }
        }
    }
}