using cfg;

public class BattleHeartMethod10118 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        InTrigger = false;
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var skillType = model.GetOtherSkillType(Subject.EntityID);
            if (skillType == SkillType.PowerKilling)
            {
                InTrigger = true;
            }
        }
    }

    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.DefendPct && InTrigger)
        {
            return GetParamFloat(0);
        }

        return 0;
    }

    public override void RemoveBeforeNextAction()
    {
        InTrigger = false;
    }

    protected override void OnRecycle()
    {
        InTrigger = false;
    }
}