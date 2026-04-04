using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4046 : BattleSkillBase
{
    //下一次行动中恢复刚炁时获得等量玄炁，恢复玄炁时获得等量刚炁（玉摄念状态）
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, 30331, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
} 