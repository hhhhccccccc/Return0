using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill2057 : BattleSkillBase
{
    //对目标造成160%技的伤害，扣除其15%全部属性并增加等量的属性，消耗其20刚炁20玄炁并增加等量的炁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var pct = 0.15f;
        var speed = Target.GetProperty(BattlePropertyType.Speed);
        var breaked = Target.GetProperty(BattlePropertyType.Break);
        var defend = Target.GetProperty(BattlePropertyType.Defend);
        var power = Target.GetProperty(BattlePropertyType.Power);
        var tech = Target.GetProperty(BattlePropertyType.Tech);
        var hpMax = Target.GetProperty(BattlePropertyType.MaxHp);
        var clever = Target.GetProperty(BattlePropertyType.Clever);
        
        DoChangeProperty(Subject, BattlePropertyType.SpeedInt, speed * pct, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.BreakInt, breaked * pct, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.DefendInt, defend * pct, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.PowerInt, power * pct, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.TechInt, tech * pct, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.MaxHpInt, hpMax * pct, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.CleverInt, clever * pct, BattleSource.Skill);
        
        DoChangeProperty(Target, BattlePropertyType.SpeedInt, -speed * pct, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.BreakInt, -breaked * pct, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.DefendInt, -defend * pct, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.PowerInt, -power * pct, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.TechInt, -tech * pct, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.MaxHpInt, -hpMax * pct, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.CleverInt, -clever * pct, BattleSource.Skill);
        
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 20, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 20, BattleSource.Skill);
        
        DoChangeProperty(Target, BattlePropertyType.GangQi, -20, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.XuanQi, -20, BattleSource.Skill);
    }

    //清除其全部毒瘴状态并为全部角色施加1层毒瘴状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoClearBuff(Target, GameConst.Battle.BuffDuZhang);
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            DoAddBuff(unit, GameConst.Battle.BuffDuZhang, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }
} 