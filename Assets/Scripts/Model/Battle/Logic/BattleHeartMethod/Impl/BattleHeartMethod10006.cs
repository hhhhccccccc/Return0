using System;
using System.Collections.Generic;
using cfg;

public class BattleHeartMethod10006 : BattleHeartMethodBase
{
    private Dictionary<int, int> TimesDict = new();
    private int MaxCount => GetConfigParamInt(2);
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        var skills = subject.TakeSkillDataManager.GetTakeSkillData();
        foreach (var data in skills)
        {
            TimesDict[data.SkillID] = MaxCount;
        }
    }
    
    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (model == null)
        {
            return 0;
        }

        if (model.SourceType != GetPropertySourceType.ReceiveSkillDamage)
        {
            return 0;
        }

        var skillID = model.TypeID;
        if (TimesDict.TryGetValue(skillID, out var times))
        {
            if (propertyType == BattlePropertyType.BreakPct)
            {
                return GetConfigParamFloat(0) * times;
            }
        
            if (propertyType == BattlePropertyType.DefendPct)
            {
                return GetConfigParamFloat(1) * times;
            }
        }
        return 0;
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model && (model.BattleClashType == BattleClashType.SingleAction || model.BattleClashType == BattleClashType.DoubleClash))
        {
            var otherSkillID = model.GetOtherSkillID(Subject.EntityID);
            var skillType = BattleUtil.GetSkillTypeBySkillID(otherSkillID);
            if (skillType == SkillType.PowerKilling || skillType == SkillType.ArtKilling)
            {
                if (TimesDict.ContainsKey(otherSkillID))
                {
                    TimesDict[otherSkillID]++;
                    TimesDict[otherSkillID] = Math.Min(TimesDict[otherSkillID], MaxCount);
                }
                else
                {
                    TimesDict[otherSkillID] = 1;
                }
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        TimesDict.Clear();
    }
}