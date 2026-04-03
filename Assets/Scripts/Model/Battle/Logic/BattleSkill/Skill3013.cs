using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3013 : BattleSkillBase
{
    private bool CanClearBuff;
    
    //招式的刚炁消耗转为当前60%，至多60，获得1层缓速
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.6f, 60);
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 1, null, BattleMomentType.DoDesitionAction);
    }

    //施加3层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        if (CanClearBuff)
        {
            var buff = Subject.GetBuff(GameConst.Battle.ShieldBuffID);
            if (buff != null)
            {
                Subject.ClearBuff(buff.BuffID);
            }
        }
    }

    public override void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        var buff = Subject.GetBuff(GameConst.Battle.ShieldBuffID);
        if (buff != null)
        {
            dict.Add(GetSymbol, buff.LayerCount);
            CanClearBuff = true;
        }
    }
}