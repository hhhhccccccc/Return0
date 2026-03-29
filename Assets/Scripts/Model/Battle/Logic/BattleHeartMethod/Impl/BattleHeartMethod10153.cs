//todo 表现
public class BattleHeartMethod10153 : BattleHeartMethodBase
{
    private int SkillCount { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        SkillCount = 0;
    }

    public override void SkillEnd(BattleSkillBase skill)
    {
        base.SkillEnd(skill);
        if (skill != null)
        {
            if (skill.IsRepeat)
            {
                SkillCount++;
                if (SkillCount == GetParamInt(0))
                {
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, GetParamInt(1));
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, GetParamInt(2));
                }
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        SkillCount = 0;
    }
}