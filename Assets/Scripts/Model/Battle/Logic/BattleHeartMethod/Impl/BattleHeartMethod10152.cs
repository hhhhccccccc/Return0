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
        if (CanTrigger)
        {
            DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, GetConfigParamInt(0), null, BattleMomentType.RoundStart);
            CanTrigger = false;
        }
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
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