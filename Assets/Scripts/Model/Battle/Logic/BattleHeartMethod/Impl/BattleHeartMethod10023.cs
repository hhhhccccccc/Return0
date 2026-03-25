using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10023 : BattleHeartMethodBase
{
    private float SkillWelly => GetParamFloat(0);
    public override float GetSkillWelly(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) == SkillType.ArtKilling)
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
            return SkillWelly;
        }
        
        return 0;
    }
}