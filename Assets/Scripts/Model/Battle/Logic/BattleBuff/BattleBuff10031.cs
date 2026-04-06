using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10031 : BattleBuffBase
{
    /// <summary>
    /// 受到攻击后自身未存在行动则消耗一层反击状态对其使用武杀式：反击
    /// </summary>
    /// <param name="paramModel"></param>
    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        var other = GetOtherUnit(paramModel);
        //受到行动后这一息没有行动过 且当前没有正在释放的技能 且是杀式
        if (CheckSkillIsKillingStyle(other, true) && !Subject.ActionWheelIsAction && Subject.GetSkill() == null)
        {
            if (Subject.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(GameConst.Battle.SkillCounterattack, 0), other))
            {
                var list = new List<int>();
                DoAddActionTimes(Subject, 1);
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(Subject.EntityID,
                    other.EntityID, BattleBehaviourType.Skill, GameConst.Battle.SkillCounterattack, 0);
                list.Add(Subject.EntityID);
                
                var setUnitSkillEventModel = PM.GetClass<BattleSetUnitSkillEventModel>();
                setUnitSkillEventModel.SetSkillUnitList = list;
                MessageManager.DispatchMsg(setUnitSkillEventModel);
                PM.RecycleClass(setUnitSkillEventModel);

                DoSetActionWheelToNow(Subject);
                DoReduceBuffLayerCount(Subject, BuffID, 1);
            }
        }
    }
}
