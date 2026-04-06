using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10026 : BattleHeartMethodBase
{
    private float SkillWelly => GetConfigParamFloat(1);
    private bool CanTrigger { get; set; }
    private bool IsIgnoreAdd { get; set; }
    private List<int> SkillTypeList = new();
    
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = false;
        SkillTypeList.Clear();
    }

    public override void SkillEnd(BattleSkillBase skillBase)
    {
        base.SkillEnd(skillBase);
        if (IsIgnoreAdd)
        {
            IsIgnoreAdd = false;
            return;
        }
        var skillType = skillBase.GetSKillType;
        if (!CanTrigger)
        {
            if (!SkillTypeList.Contains((int)skillType))
            {
                SkillTypeList.Add((int)skillType);
                if (SkillTypeList.Count >= GetConfigParamInt(0))
                {
                    CanTrigger = true;
                }
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var skillType = BattleUtil.GetSkillTypeBySkillID(model.GetSelfSkillID(Subject.EntityID));
            if (CanTrigger && (skillType == SkillType.PowerKilling || skillType == SkillType.ArtKilling))
            {
                CanTrigger = false;
                IsIgnoreAdd = true;
                SkillTypeList.Clear();
            }
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        var skillType = BattleUtil.GetSkillTypeBySkillID(s);
        if (CanTrigger && (skillType == SkillType.PowerKilling || skillType == SkillType.ArtKilling))
        {
            return SkillWelly;
        }

        return 0;
    }
    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
        SkillTypeList.Clear();
    }
}