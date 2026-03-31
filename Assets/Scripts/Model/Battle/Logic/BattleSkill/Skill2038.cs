using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2038 : BattleSkillBase
{
    //招式的玄炁消耗转为当前100%
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 1f, 0);
    }

    public override float GetWellyIncrease(int skillGuid)
    {
        return 1;
    }
    
    //todo 追加50%技的伤害
    
    //todo （若产生过交锋，未成功且本回合未被其他友方使用过招式则在本回合下次行动时可使用隐藏招式：沉嵩镇渊）
}