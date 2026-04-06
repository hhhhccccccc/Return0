public class BattleHeartMethod10112 : BattleHeartMethodBase
{
    private bool CanIgnore { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanIgnore = false;
    }

    public override void RoundStart()
    {
        CanIgnore = true;
    }

    public override bool IgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanIgnore)
        {
            return false;
        }
        
        if (paramModel is DamageParamModel model)
        {
            var attackSkillDamageRate = model.GetOtherFinalDamageWelly(Subject.EntityID);
            if (attackSkillDamageRate > GetConfigParamFloat(0))
            {
                CanIgnore = false;
                return true;
            }
        }

        return false;
    }

    protected override void OnHeartMethodRecycle()
    {
        CanIgnore = false;
    }
}