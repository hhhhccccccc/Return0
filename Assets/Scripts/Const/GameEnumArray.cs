public enum PanelLayerType
{
    Background,
    Midground,
    Foreground,
    Top,
    Pop,
}

public enum JobPriority
{
    Low = 1,
    Mid = 2,
    High = 3
}

public enum BattleKey
{
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4
}

public enum BattleObjType
{
    Role = 0
}

public enum BattleState
{
    PreDoDesition = 1,//预先行动
    ForceDoDesition = 2,//强制预先行动
    ActionWheelMomentCalculate = 3,//行动息扳机计算
    ActionWheelLogicCalculate = 4,//行动息逻辑计算
}

public enum InputType
{
    Keyboard = 0,
    Mouse = 1,
}

public enum BattleBehaviourType
{
    Jump = 0,
    Skill = 1
}

public enum BattleClickType
{
    Entity = 0,
    Skill = 1,
    Cancel = 2
}

/// <summary>
/// 战斗封装的一个交锋数据
/// </summary>
public enum BattleClashType
{
    SingleAction = 1,//单方面行动
    SingleClash = 2,//单向交锋
    DoubleClash = 3,//双向交锋
}

public enum SkillType
{
    None = 0,
    MartialArts = 1, //武杀式
    KillingStyle = 2, //术杀式
    TechniqueImperialStyle = 3, //技御式
    SpellFormula = 4, //法咒式
}