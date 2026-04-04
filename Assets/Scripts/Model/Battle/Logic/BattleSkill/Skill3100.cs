using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3100 : BattleSkillBase
{
    //每带有一个异常状态威力减少10的百分比

    public override float GetWellyRateEx(int skillGuid)
    {
        return Subject.GetRandomBuffByType(BuffType.Abnormal).Count * 0.1f;
    }
}