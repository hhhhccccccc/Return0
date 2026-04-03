using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill2058 : BattleSkillBase
{
    private bool CanAddWelly { get; set; }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CanAddWelly)
        {
            return 0.2f;
        }

        return 0;
    }

    public override void SelfActionWheelStart()
    {
        if (CheckBuffCompare(Subject, GameConst.Battle.BuffDuZhang, Target, GameConst.Battle.BuffDuXiangZuo, DataRelation.DaYuDengYu))
        {
            CanAddWelly = true;
        }
    }

    protected override void OnSkillRecycle()
    {
        CanAddWelly = false;
    }
} 