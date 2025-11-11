using System;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect3076002 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var addBuffID = Config.ParamList[1].ToInt();
            var checkBuffID =  Config.ParamList[2].ToInt();
            foreach (var target in targetList)
            {
                if (Subject.ActionWheel < target.ActionWheel)
                {
                    var delta = target.ActionWheel - Subject.ActionWheel;
                    var checkBuff = target.GetBuff(checkBuffID);
                    var buffCount = checkBuff?.LayerCount ?? 0;
                    if (buffCount <= 0)//没buff添加一半
                    {
                        BattleBuffManager.AddBuff(target, addBuffID, Subject, (int)(Math.Ceiling(delta / 2.0f)), null, MomentType);
                    }
                    else
                    {
                        BattleBuffManager.AddBuff(target, addBuffID, Subject, delta, null, MomentType);
                    }
                }
            }
        }
    }
}