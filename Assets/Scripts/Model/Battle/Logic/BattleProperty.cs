using System;
using System.Collections.Generic;
using System.Linq;
using cfg;

public class BattleProperty : IModel
{
    private Dictionary<int, float> PropertyMap = new();

    private Dictionary<int, int> KeyMap = new();

    public void Init(Character character)
    {
        SetProperty(BattlePropertyType.BasicMaxHp, character.Hp);
        SetProperty(BattlePropertyType.Hp, GetProperty(BattlePropertyType.MaxHp));
        
        SetProperty(BattlePropertyType.BasicMaxGangQi, character.GangQi);
        SetProperty(BattlePropertyType.GangQi, GetProperty(BattlePropertyType.MaxGangQi));
        
        SetProperty(BattlePropertyType.BasicMaxXuanQi, character.XuanQi);
        SetProperty(BattlePropertyType.XuanQi, GetProperty(BattlePropertyType.MaxXuanQi));
        
        SetProperty(BattlePropertyType.BasicSpeed, character.Speed);
        SetProperty(BattlePropertyType.BasicPower, character.Power);
        SetProperty(BattlePropertyType.BasicDefend, character.Defend);
        SetProperty(BattlePropertyType.BasicTech, character.Tech);
        SetProperty(BattlePropertyType.BasicBreak, character.Break);
        SetProperty(BattlePropertyType.BasicClever, character.Clever);

        InitKey();
    }

    private void InitKey()
    {
        SetKey(BattleKeyType.KeyUp, 0);
        SetKey(BattleKeyType.KeyDown, 0);
        SetKey(BattleKeyType.KeyLeft, 0);
        SetKey(BattleKeyType.KeyRight, 0);
        SetKey(BattleKeyType.KeyMax, GameConst.Battle.KeyMax);
        SetKey(BattleKeyType.KeyMaxEx, 0);
        
        SetKey(BattleKeyType.KeyUp, 10);
    }

    #region 属性相关

      public bool ChangeProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        #region 战斗资源特殊计算

        if (propType == BattlePropertyType.GangQi)
        {
            if (propValue > 0)
            {
                propValue = (propValue * (1 + GetProperty(BattlePropertyType.GangQiRecPct)) +
                             GetProperty(BattlePropertyType.GangQiRecInt)) * (1 + GetProperty(BattlePropertyType.AllGangQiRecPct));
                propValue = Math.Max(propValue, 0);
            }
            else if (propValue < 0)
            {
                propValue = (propValue * (1 - GetProperty(BattlePropertyType.GangQiRedPct)) -
                             GetProperty(BattlePropertyType.GangQiRedInt)) * (1 - GetProperty(BattlePropertyType.AllGangQiRedPct));
                propValue = Math.Min(propValue, 0);
            }
        }   
        
        if (propType == BattlePropertyType.XuanQi)
        {
            if (propValue > 0)
            {
                propValue = (propValue * (1 + GetProperty(BattlePropertyType.XuanQiRecPct)) +
                             GetProperty(BattlePropertyType.XuanQiRecInt)) * (1 + GetProperty(BattlePropertyType.AllXuanQiRecPct));
                propValue = Math.Max(propValue, 0);
            }
            else if (propValue < 0)
            {
                propValue = (propValue * (1 - GetProperty(BattlePropertyType.XuanQiRedPct)) -
                             GetProperty(BattlePropertyType.XuanQiRedInt)) * (1 - GetProperty(BattlePropertyType.AllXuanQiRedPct));
                propValue = Math.Min(propValue, 0);
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
            if (KeyMap[(int)propType] < count)
                return false;
            KeyMap[(int)propType] -= count;
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

    private int GetKeyMax()
    {
        return GetKey(BattleKeyType.KeyMax) + GetKey(BattleKeyType.KeyMaxEx);
    }

    #endregion
}
