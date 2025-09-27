using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckRandomSuccess : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var random = Util.GetRandomInt(0, 100);
        return random <= Config.ParamList[0];
    }
}