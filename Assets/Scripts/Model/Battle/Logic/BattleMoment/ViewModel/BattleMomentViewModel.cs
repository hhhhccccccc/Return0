using System.Collections.Generic;
using cfg;

public class BattleMomentViewModel : IModel, IRecycle
{
    public BattleSource BattleSource { get; set; }
    public MomentViewType ViewType { get; set; }
    public int ConfigID { get; set; }
    //后面参数在逻辑层传出来
    public int EntityID { get; set; }
    public List<float> FloatParam { get; set; } = new();
    public List<BattleKey> KeyParam { get; set; } = new();

    public void AddParam(float param)
    {
        FloatParam.Add(param);
    }

    public void AddKey(BattleKey key)
    {
        KeyParam.Add(key);
    }

    public void AddKeyList(List<BattleKey> keyList)
    {
        KeyParam.AddRange(keyList);
    }
    
    public void Recycle()
    {
        BattleSource = BattleSource.None;
        ConfigID = 0;
        EntityID = 0;
        FloatParam.Clear();
        KeyParam.Clear();
    }
}

public enum MomentViewType
{
    None = 0,
    StoreKey = 1, //存储键
    ConvertStoreKey = 2, //转化存储键
    AddHeartMethod = 3, //提供心法
    AddWelly = 4, //增加招式威力
    AddRate = 5, //增加招式倍率
    AddDamageInt = 6, //增加招式整数伤害
    AddKey = 7, //添加Key 目标，键
    ChangeHp = 8, //改变血
    IgnoreSkillDirectDamage = 9, //抵免伤害
    Treasure10187 = 10, //回合结束时全场角色消耗玄炁累计超过500则清空计数并5次对随机敌手造成47伤害
    Treasure10196 = 11, //三个回合内只会触发一次，若目标体高于99%则不进入冷却
    AddActionTimes = 12, //行动次数
    HeartMethod10012 = 13, //击破敌人获得1次行动次数并且玄炁+25、刚炁+25
    ChangeGangQi = 14, //改变刚气
    ChangeXuanQi = 15, //改变玄气
    ChangeActionWheel = 16, //改变息   表现：数量 当前息 当前息溢值
    RemoveKey = 17, //减少键 目标，键
    HeartMethod10065 = 18, //预先行动阶段决定的行动至少会加快1息
    HeartMethod10072 = 19, //每息首个行动的敌手根据其行动的类型及其最后一个键给予对应的留劲状态，在敌手每回合首次获得留劲状态时在本回合获得1次行动次数   表现：传挂心法的人的ID
    HeartMethod10073 = 20, //只会在回合其中一息受到直接伤害，每回合变化
    HeartMethod10074 = 21, //只会受到敌手在单个回合连续第4次行动所造成的伤害
    HeartMethod10076 = 22, //不会受到武杀式/术杀式的直接伤害，每回合变化
    BattleHeartMethod10113 = 23, //每息刚炁+3，玄炁+3，获得1个随机的键。
    SetBreak = 24, //直接进入击破状态
    HeartMethod10135 = 25, //每获得1层毒瘴状态后随机获得1个键并恢复10刚炁10玄炁
}