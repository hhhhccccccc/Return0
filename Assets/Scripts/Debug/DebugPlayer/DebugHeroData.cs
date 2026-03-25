using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[Serializable]
public class DebugHeroData
{
    [LabelText("英雄ID")]
    public int HeroID = 1;
    public int SlotIndex;
    public int Level = 10;
    public List<DebugSkillData> WearSkill;
    public List<int> WearHeartMethod;
    public List<int> WearTreasure;
}

[Serializable]
public class DebugSkillData
{
    public int SkillID;
    public int VariantID;
}