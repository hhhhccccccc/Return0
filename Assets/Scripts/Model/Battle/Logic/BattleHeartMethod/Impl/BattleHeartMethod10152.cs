using cfg;

//todo 表现
public class BattleHeartMethod10152 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = false;
    }

    public override void RoundStart()
    {
        base.RoundStart();
        if (CanTrigger)
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, GetConfigParamInt(0));
            CanTrigger = false;
        }
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var skillType = model.GetOtherSkillType(Subject.EntityID);
            if (skillType == SkillType.PowerKilling)
            {
                CanTrigger = true;
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}