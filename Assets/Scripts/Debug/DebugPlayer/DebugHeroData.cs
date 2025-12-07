using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class DebugHeroData
{
    public int HeroID { get; set; }
    public int SlotIndex { get; set; }
    public int Level { get; set; }
    public List<DebugSkillData> WearSkill { get; set; }
    public List<int> WearHeartMethod { get; set; }
    public List<int> WearTreasure { get; set; }
}

public class DebugSkillData
{
    public int SkillID { get; set; }
    public int VariantID { get; set; }
}