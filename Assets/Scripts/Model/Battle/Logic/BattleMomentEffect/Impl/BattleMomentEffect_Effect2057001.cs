using cfg;
using Zenject;

//速破防力技体巧
public class BattleMomentEffect_Effect2057001 :BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var pct = Config.ParamList[1];
            var changeGangQi = Config.ParamList[2];
            var changeXuanQi = Config.ParamList[3];
            foreach (var target in targetList)
            {
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
}