using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10007 : BattleHeartMethodBase
{
    private Dictionary<int, int> TimesDict = new();
    private int MaxCount => GetParamInt(2);
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        var skills = subject.TakeSkillDataManager.GetTakeSkillData();
        foreach (var data in skills)
        {
            TimesDict[data.SkillID] = MaxCount;
        }
    }
    
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (model == null)
        {
            return 0;
        }

        if (model.SourceType != GetPropertySourceType.ReceiveSkillDamage)
        {
            return 0;
        }

        var skillID = model.ID;
        if (TimesDict.TryGetValue(skillID, out var times))
        {
            if (propertyType == BattlePropertyType.BreakPct)
            {
                return GetParamFloat(0) * times;
            }
        
            if (propertyType == BattlePropertyType.DefendPct)
            {
                return GetParamFloat(1) * times;
            }
        }
        return 0;
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel { AttackUseSuccess: true } model)
        {
            var targetSkillID = model.AttackID == Subject.EntityID ? model.HitSkillID : model.AttackSkillID;
            var config = ConfigManager.GetBattleSkillConfig(targetSkillID);
            if (config.IsNeedTarget == 0)
            {
                return;
            }
            var skillType = BattleUtil.GetSkillTypeBySkillID(targetSkillID);
            if (skillType == SkillType.PowerKilling || skillType == SkillType.ArtKilling)
            {
                if (TimesDict.ContainsKey(targetSkillID))
                {
                    TimesDict[targetSkillID]++;
                    TimesDict[targetSkillID] = Math.Min(TimesDict[targetSkillID], MaxCount);
                }
                else
                {
                    TimesDict[targetSkillID] = 1;
                }
            }
        }
        else if (paramModel is DamageParamModel model2 && (model2.BattleClashType == BattleClashType.SingleAction || model2.BattleClashType == BattleClashType.DoubleClash))
        {
            var targetSkillID = model2.AttackID == Subject.EntityID ? model2.HitSkillID : model2.AttackSkillID;
            var skillType = BattleUtil.GetSkillTypeBySkillID(targetSkillID);
            if (skillType == SkillType.PowerKilling || skillType == SkillType.ArtKilling)
            {
                if (TimesDict.ContainsKey(targetSkillID))
                {
                    TimesDict[targetSkillID]++;
                    TimesDict[targetSkillID] = Math.Min(TimesDict[targetSkillID], MaxCount);
                }
                else
                {
                    TimesDict[targetSkillID] = 1;
                }
            }
        }
    }

    public override void Recycle()
    {
        TimesDict.Clear();
        base.Recycle();
    }
}