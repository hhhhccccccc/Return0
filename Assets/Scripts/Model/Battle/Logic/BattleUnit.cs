using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : IModel
{
    public BattleField Bf;
    
    public static int GlobalEntityID = 0;

    public int SlotIndex;
    
    public int EntityID;
    
    protected BattleObjType ObjType;

    private BattleProperty Property;

    /// <summary>
    /// 携带的buff
    /// </summary>
    private List<BattleBuffBase> Buffs = new List<BattleBuffBase>();

    /// <summary>
    /// 携带的心法
    /// </summary>
    private List<BattleHeartMethodBase> HeartMethods = new List<BattleHeartMethodBase>();
    
    /// <summary>
    /// 携带的宝器
    /// </summary>
    private List<BattleTreasureBase> Treasures = new List<BattleTreasureBase>();
    public virtual void Init(BattleField bf, Character character)
    {
        Bf = bf;
        ResetEntityID();
        Property = new BattleProperty();
        //Property.Init(character);
    }

    private void ResetEntityID()
    {
        GlobalEntityID += 1;
        EntityID = GlobalEntityID;
    }

    protected bool ChangeProperty(string propName, int propValue)
    {
        return Property.ChangeProperty(propName, propValue);
    }

    protected bool SetProperty(string propName, int propValue)
    {
        return Property.SetProperty(propName, propValue);
    }

    protected int GetProperty(string propName)
    {
        return Property.GetProperty(propName);
    }
}
