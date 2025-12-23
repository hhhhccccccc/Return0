using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4047 : BattleSkillBase
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        var buffs = Target.GetBuffList();
        foreach (var buff in buffs)
        {
            if (buff.BuffType == BuffType.Gain || buff.BuffType == BuffType.Abnormal)
            {
                var buffID = buff.BuffID;
                var layerCount = buff.LayerCount;
                var paramList = new List<float>(buff.ParamList);
                BattleBuffManager.AddBuff(Subject, buffID, Subject, layerCount, paramList, BattleMomentType.DoDesitionAction);
            }
        }
    }
} 