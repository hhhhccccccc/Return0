using System;
using System.Collections.Generic;
using cfg;

public class BattleProperty : IModel, IRecycle
{
    private Dictionary<int, float> PropertyMap = new();

    private Dictionary<int, int> KeyMap = new();
    private HeroData HeroData { get; set; }

    public void Init(HeroData heroData)
    {
        HeroData = heroData;
        SetProperty(BattlePropertyType.BasicMaxHp, heroData.GetFightProperty_Hp());
        SetProperty(BattlePropertyType.Hp, GetProperty(BattlePropertyType.MaxHp));
        
        SetProperty(BattlePropertyType.BasicMaxGangQi, heroData.GetFightProperty_GangQi());
        SetProperty(BattlePropertyType.GangQi, GetProperty(BattlePropertyType.MaxGangQi));
        
        SetProperty(BattlePropertyType.BasicMaxXuanQi, heroData.GetFightProperty_XuanQi());
        SetProperty(BattlePropertyType.XuanQi, GetProperty(BattlePropertyType.MaxXuanQi));
        
        SetProperty(BattlePropertyType.BasicPower, heroData.GetFightProperty_Power());
        SetProperty(BattlePropertyType.BasicTech, heroData.GetFightProperty_Tech());
        SetProperty(BattlePropertyType.BasicSpeed, heroData.GetFightProperty_Speed());
        SetProperty(BattlePropertyType.BasicClever, heroData.GetFightProperty_Clever());
        SetProperty(BattlePropertyType.BasicDefend, heroData.GetFightProperty_Defend());
        SetProperty(BattlePropertyType.BasicBreak, heroData.GetFightProperty_Break());

        SetProperty(BattlePropertyType.GangQiRecNatural, heroData.GetFightProperty_GangQiRecover());
        SetProperty(BattlePropertyType.XuanQiRecNatural, heroData.GetFightProperty_XuanQiRecover());
        
        
        SetKey(BattleKeyType.KeyUp, 0);
        SetKey(BattleKeyType.KeyDown, 0);
        SetKey(BattleKeyType.KeyLeft, 0);
        SetKey(BattleKeyType.KeyRight, 0);
        SetKey(BattleKeyType.KeyMax, GameConst.Battle.KeyMax);
        SetKey(BattleKeyType.KeyMaxEx, 0);
        SetKey(BattleKeyType.KeyRecoverNatural, heroData.GetFightProperty_KeyRecover());
        RecoverKey(GetKey(BattleKeyType.KeyMax) + GetKey(BattleKeyType.KeyMaxEx));
    }

    #region 属性相关

    public float GetGangQiRecover(float propValue)
    {
        propValue = (propValue * (1 + GetProperty(BattlePropertyType.GangQiRecPct)) +
                     GetProperty(BattlePropertyType.GangQiRecInt)) * (1 + GetProperty(BattlePropertyType.AllGangQiRecPct));
        propValue = Math.Max(propValue, 0);
        return propValue;
    }

    public float GetGangQiReduce(float propValue)
    {
        propValue = (propValue * (1 - GetProperty(BattlePropertyType.GangQiRedPct)) -
                     GetProperty(BattlePropertyType.GangQiRedInt)) * (1 - GetProperty(BattlePropertyType.AllGangQiRedPct));
        propValue = Math.Min(propValue, 0);
        return propValue;
    }

    public float GetXuanQiRecover(float propValue)
    {
        propValue = (propValue * (1 + GetProperty(BattlePropertyType.XuanQiRecPct)) +
                     GetProperty(BattlePropertyType.XuanQiRecInt)) * (1 + GetProperty(BattlePropertyType.AllXuanQiRecPct));
        propValue = Math.Max(propValue, 0);
        return propValue;
    }
    
    public float GetXuanQiReduce(float propValue)
    {
        propValue = (propValue * (1 - GetProperty(BattlePropertyType.XuanQiRedPct)) -
                     GetProperty(BattlePropertyType.XuanQiRedInt)) * (1 - GetProperty(BattlePropertyType.AllXuanQiRedPct));
        propValue = Math.Min(propValue, 0);
        return propValue;
    }
    
    public bool ChangeProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        #region 战斗资源特殊计算

        if (propType == BattlePropertyType.GangQi)
        {
            if (propValue > 0)
            {
                propValue = GetGangQiRecover(propValue);
            }
            else if (propValue < 0)
            {
                propValue = GetGangQiReduce(propValue);
            }
        }   
        
        if (propType == BattlePropertyType.XuanQi)
        {
            if (propValue > 0)
            {
                propValue = GetXuanQiRecover(propValue);
            }
            else if (propValue < 0)
            {
                propValue = GetXuanQiReduce(propValue);
            }
        }   

        #endregion
        
        if (!PropertyMap.TryAdd((int)propType, propValue))
        {
            PropertyMap[(int)propType] += propValue;
        }

        #region MyReg上限判断ion

        if (propType == BattlePropertyType.Hp)
        {
            var maxHp = GetProperty(BattlePropertyType.MaxHp);
            if (PropertyMap[(int)BattlePropertyType.Hp] > maxHp)
            {
                PropertyMap[(int)BattlePropertyType.Hp] = maxHp;
            }
            else if (PropertyMap[(int)BattlePropertyType.Hp] < 0)
            {
                PropertyMap[(int)BattlePropertyType.Hp] = 0;
            }
        }    
        
        if (propType == BattlePropertyType.GangQi)
        {
            var maxGangQi = GetProperty(BattlePropertyType.MaxGangQi);
            if (PropertyMap[(int)BattlePropertyType.GangQi] > maxGangQi)
            {
                PropertyMap[(int)BattlePropertyType.GangQi] = maxGangQi;
            }
            else if (PropertyMap[(int)BattlePropertyType.GangQi] < 0)
            {
                PropertyMap[(int)BattlePropertyType.GangQi] = 0;
            }
        }   
        
        if (propType == BattlePropertyType.XuanQi)
        {
            var maxXuanQi = GetProperty(BattlePropertyType.MaxXuanQi);
            if (PropertyMap[(int)BattlePropertyType.XuanQi] > maxXuanQi)
            {
                PropertyMap[(int)BattlePropertyType.XuanQi] = maxXuanQi;
            }
            else if (PropertyMap[(int)BattlePropertyType.XuanQi] < 0)
            {
                PropertyMap[(int)BattlePropertyType.XuanQi] = 0;
            }
        }  

        #endregion
        
        return true;
    }

    public bool SetProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        PropertyMap[(int)propType] = propValue;
        return true;
    }

    public float GetProperty(BattlePropertyType propType)
    {
        return propType switch
        {
            BattlePropertyType.MaxHp => (GetProperty(BattlePropertyType.BasicMaxHp) *
                                            (1 + GetProperty(BattlePropertyType.MaxHpPct)) +
                                            GetProperty(BattlePropertyType.MaxHpInt)) *
                                        (1 + GetProperty(BattlePropertyType.AllMaxHpPct)),
            /*BattlePropertyType.Hp => (GetProperty(BattlePropertyType.BasicHp) *
                                         (1 + GetProperty(BattlePropertyType.HpPct)) +
                                         GetProperty(BattlePropertyType.HpInt)) *
                                     (1 + GetProperty(BattlePropertyType.AllHpPct)),*/
            BattlePropertyType.MaxGangQi => (GetProperty(BattlePropertyType.BasicMaxGangQi) *
                                                (1 + GetProperty(BattlePropertyType.MaxGangQiPct)) +
                                                GetProperty(BattlePropertyType.MaxGangQiInt)) *
                                            (1 + GetProperty(BattlePropertyType.AllMaxGangQiPct)),
            /*BattlePropertyType.GangQi => (GetProperty(BattlePropertyType.BasicGangQi) *
                                             (1 + GetProperty(BattlePropertyType.GangQiPct)) +
                                             GetProperty(BattlePropertyType.GangQiInt)) *
                                         (1 + GetProperty(BattlePropertyType.AllGangQiPct)),*/
            BattlePropertyType.MaxXuanQi => (GetProperty(BattlePropertyType.BasicMaxXuanQi) *
                                                (1 + GetProperty(BattlePropertyType.MaxXuanQiPct)) +
                                                GetProperty(BattlePropertyType.MaxXuanQiInt)) *
                                            (1 + GetProperty(BattlePropertyType.AllMaxXuanQiPct)),
            /*BattlePropertyType.XuanQi => (GetProperty(BattlePropertyType.BasicXuanQi) *
                                             (1 + GetProperty(BattlePropertyType.XuanQiPct)) +
                                             GetProperty(BattlePropertyType.XuanQiInt)) *
                                         (1 + GetProperty(BattlePropertyType.AllXuanQiPct)),*/
            BattlePropertyType.Power => (GetProperty(BattlePropertyType.BasicPower) *
                                            (1 + GetProperty(BattlePropertyType.PowerPct)) +
                                            GetProperty(BattlePropertyType.PowerInt)) *
                                        (1 + GetProperty(BattlePropertyType.AllPowerPct)),
            BattlePropertyType.Tech => (GetProperty(BattlePropertyType.BasicTech) *
                                           (1 + GetProperty(BattlePropertyType.TechPct)) +
                                           GetProperty(BattlePropertyType.TechInt)) *
                                       (1 + GetProperty(BattlePropertyType.AllTechPct)),
            BattlePropertyType.Speed => (GetProperty(BattlePropertyType.BasicSpeed) *
                                            (1 + GetProperty(BattlePropertyType.SpeedPct)) +
                                            GetProperty(BattlePropertyType.SpeedInt)) *
                                        (1 + GetProperty(BattlePropertyType.AllSpeedPct)),
            BattlePropertyType.Clever => (GetProperty(BattlePropertyType.BasicClever) *
                                             (1 + GetProperty(BattlePropertyType.CleverPct)) +
                                             GetProperty(BattlePropertyType.CleverInt)) *
                                         (1 + GetProperty(BattlePropertyType.AllCleverPct)),
            BattlePropertyType.Defend => (GetProperty(BattlePropertyType.BasicDefend) *
                                             (1 + GetProperty(BattlePropertyType.DefendPct)) +
                                             GetProperty(BattlePropertyType.DefendInt)) *
                                         (1 + GetProperty(BattlePropertyType.AllDefendPct)),
            BattlePropertyType.Break => (GetProperty(BattlePropertyType.BasicBreak) *
                                            (1 + GetProperty(BattlePropertyType.BreakPct)) +
                                            GetProperty(BattlePropertyType.BreakInt)) *
                                        (1 + GetProperty(BattlePropertyType.AllBreakPct)),
            /*BattlePropertyType.GangQiRec => (GetProperty(BattlePropertyType.BasicGangQiRec) *
                                                (1 + GetProperty(BattlePropertyType.GangQiRecPct)) +
                                                GetProperty(BattlePropertyType.GangQiRecInt)) *
                                            (1 + GetProperty(BattlePropertyType.AllGangQiRecPct)),
            BattlePropertyType.XuanQiRec => (GetProperty(BattlePropertyType.BasicXuanQiRec) *
                                                (1 + GetProperty(BattlePropertyType.XuanQiRecPct)) +
                                                GetProperty(BattlePropertyType.XuanQiRecInt)) *
                                            (1 + GetProperty(BattlePropertyType.AllXuanQiRecPct)),*/
            _ => PropertyMap.GetValueOrDefault((int)propType, 0)
        };
    }

    public float GetPropertyPct(BattlePropertyType propType)
    {
        return propType switch
        {
            BattlePropertyType.Hp => GetProperty(BattlePropertyType.Hp) / GetProperty(BattlePropertyType.MaxHp),
            BattlePropertyType.GangQi => GetProperty(BattlePropertyType.GangQi) / GetProperty(BattlePropertyType.MaxGangQi),
            BattlePropertyType.XuanQi => GetProperty(BattlePropertyType.XuanQi) / GetProperty(BattlePropertyType.MaxXuanQi),
            _ => 0
        };
    }

    #endregion

    #region 键相关

    public int GetKey(BattleKeyType keyType)
    {
        return KeyMap.GetValueOrDefault((int)keyType, 0);    
    }
    
    private List<int> TempKeyList = new();

    public List<int> GetKeyList()
    {
        TempKeyList.Clear();
        foreach (var keyType in Util.KeyList)
        {
            for (int i = 1; i <= GetKey(keyType);i++)
            {
                TempKeyList.Add(GetKey(keyType));
            }
        }

        return TempKeyList;
    }
    
    public void SetKey(BattleKeyType keyType, int value)
    {
        KeyMap[(int)keyType] = value;
    }
    
    public bool ChangeKey(BattleKeyType propType, int count)
    {
        if (propType == BattleKeyType.KeyMax)
        {
            KeyMap[(int)propType] += count;
            return true;
        }
        
        if (count > 0)
        {
            var now = GetKeyCount();
            var max = GetKeyMax();
            if (now >= max)
            {
                return false;
            }

            var addCount = Math.Min(max - now, count);
            KeyMap[(int)propType] += addCount;
            return true;
        }
        else
        {
            if (KeyMap[(int)propType] < -count)
                return false;
            KeyMap[(int)propType] += count;
            return true;
        }
    }

    public int GetKeyCount()
    {
        return GetKey(BattleKeyType.KeyUp)
               + GetKey(BattleKeyType.KeyDown)
               + GetKey(BattleKeyType.KeyLeft)
               + GetKey(BattleKeyType.KeyRight);
    }

    public int GetKeyMax()
    {
        return GetKey(BattleKeyType.KeyMax) + GetKey(BattleKeyType.KeyMaxEx);
    }

    public void RecoverKey(int count)
    {
        var getKey = Util.GetRandomKey(count);
        foreach (var key in getKey)
        {
            ChangeKey(key, 1);
        }
    }

    public void RecoverRandomKey(int count)
    {
        var allKey = GetKeyList().Clone();
        var removeList = Util.GetRandomNoSame(allKey, Util.GetSameChanceList(allKey.Count), count);
        foreach (var removeKeyType in removeList)
        {
            ChangeKey((BattleKeyType)removeKeyType, -1);
        }
    }
    
    public void RecoverKeyNatural() => RecoverKey(GetKey(BattleKeyType.KeyRecoverNatural));

    #endregion

    public void Recycle()
    {
        PropertyMap.Clear();
        KeyMap.Clear();
        HeroData = null;
    }
}
