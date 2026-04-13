using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3021 : BattleSkillBase
{
    //todo 每损失1%体减少该招式1刚炁消耗直到下次释放
    
    private float AddWelly { get; set; }
    protected override void OnSelfActionWheelStart()
    {
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var hpMax = Subject.GetProperty(BattlePropertyType.MaxHp);
        AddWelly = Util.GetRandomFloat(0, 1) <= (1 - hp / hpMax) ? 0.5f : 0;
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        return AddWelly;
    }

    protected override void OnSkillRecycle()
    {
        AddWelly = 0;
    }
    
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        AddWelly = 0;
    }
}