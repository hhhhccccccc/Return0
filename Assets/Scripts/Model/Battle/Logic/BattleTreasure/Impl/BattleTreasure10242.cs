using cfg;
using System.Linq;

//todo 表现
public class BattleTreasure10242 : BattleTreasureBase
{
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(0));
    }
}


