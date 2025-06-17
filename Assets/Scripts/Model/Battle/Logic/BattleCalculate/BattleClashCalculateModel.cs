public class BattleClashCalculateModel : IModel
{
    public BattleClashType ClashType;
    public BattleClashActionModel SubjectActionModel;
    public BattleClashActionModel TargetActionModel;
}

public class BattleClashActionModel
{
    public int SubjectID;
    public int TargetID;
    public int SkillID;
}
