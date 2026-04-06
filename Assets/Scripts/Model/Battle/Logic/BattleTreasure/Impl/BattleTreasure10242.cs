using cfg;
using System.Linq;

public class BattleTreasure10242 : BattleTreasureBase
{
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(0), null, BattleMomentType.AfterAction);
    }
}


