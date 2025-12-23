using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill2057 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var pct = Config.ParamEx[1];
            var changeGangQi = Config.ParamEx[2];
            var changeXuanQi = Config.ParamEx[3];
            target.ChangeProperty(BattlePropertyType.GangQi, -changeGangQi);
            target.ChangeProperty(BattlePropertyType.XuanQi, -changeXuanQi);
            var speed = target.GetProperty(BattlePropertyType.Speed);
            var breaked = target.GetProperty(BattlePropertyType.Break);
            var defend = target.GetProperty(BattlePropertyType.Defend);
            var power = target.GetProperty(BattlePropertyType.Power);
            var tech = target.GetProperty(BattlePropertyType.Tech);
            var hpMax = target.GetProperty(BattlePropertyType.MaxHp);
            var clever = target.GetProperty(BattlePropertyType.Clever);
            Subject.ChangeProperty(BattlePropertyType.SpeedInt, speed * pct);
            Subject.ChangeProperty(BattlePropertyType.BreakInt, breaked * pct);
            Subject.ChangeProperty(BattlePropertyType.DefendInt, defend * pct);
            Subject.ChangeProperty(BattlePropertyType.PowerInt, power * pct);
            Subject.ChangeProperty(BattlePropertyType.TechInt, tech * pct);
            Subject.ChangeProperty(BattlePropertyType.MaxHpInt, hpMax * pct);
            Subject.ChangeProperty(BattlePropertyType.CleverInt, clever * pct);
            target.ChangeProperty(BattlePropertyType.GangQi, changeGangQi);
            target.ChangeProperty(BattlePropertyType.XuanQi, changeXuanQi);
        }
    }
} 