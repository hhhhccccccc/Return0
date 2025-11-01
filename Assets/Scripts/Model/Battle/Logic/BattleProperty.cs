using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class MinRecoverNaturalData : IModel, IRecycle
{
    private static int GlobalGuid = 0;
    public int Guid;
    public int Type;//1刚气 2玄气
    public float Value;

    public void AllocGuid()
    {
        GlobalGuid++;
        Guid = GlobalGuid;
    }

    public void Recycle()
    {
        Guid = 0;
    }
}

public class BattleProperty : IModel, IRecycle
{
    [Inject] private IPoolManager PoolManager { get; set; }
    private Dictionary<int, float> PropertyMap = new();
    private Dictionary<int, int> KeyPropertyMap = new();
    private Dictionary<int, List<BattleKey>> KeyMap = new();
    private HeroData HeroData { get; set; }

    private List<MinRecoverNaturalData> MinRecoverNaturalDataList = new();
    public MinRecoverNaturalData AddMinRecoverNaturalData(int type, float value)
    {
        var model = PoolManager.GetClass<MinRecoverNaturalData>();
        model.AllocGuid();
        model.Type = type;
        model.Value = value;
        MinRecoverNaturalDataList.Add(model);
        return model;
    }

    public void RemoveMinRecoverNaturalData(int guid)
    {
        var item = MinRecoverNaturalDataList.FirstOrDefault(d => d.Guid == guid);
        if (item != null)
        {
            MinRecoverNaturalDataList.Remove(item);
            PoolManager.RecycleClass(item);
        }
    }

    private float GetMinGangQiRecoverNatural()
    {
        if (MinRecoverNaturalDataList.Count > 0)
        {
            var max = 0.0f;
            foreach (var data in MinRecoverNaturalDataList)
            {
                if (data.Value >= max && data.Type == 1)
                {
                    max = data.Value;
                }

                return max;
            }
        }
        return 0;
    }
    
    private float GetMinXuanQiRecoverNatural()
    {
        if (MinRecoverNaturalDataList.Count > 0)
        {
            var max = 0.0f;
            foreach (var data in MinRecoverNaturalDataList)
            {
                if (data.Value >= max && data.Type == 2)
                {
                    max = data.Value;
                }

                return max;
            }
        }
        return 0;
    }
    
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

        KeyMap[(int)BattleKeyType.KeyUp] = new List<BattleKey>();
        KeyMap[(int)BattleKeyType.KeyDown] = new List<BattleKey>();
        KeyMap[(int)BattleKeyType.KeyLeft] = new List<BattleKey>();
        KeyMap[(int)BattleKeyType.KeyRight] = new List<BattleKey>();

        KeyPropertyMap[(int)BattleKeyType.KeyMax] = GameConst.Battle.KeyMax;
        KeyPropertyMap[(int)BattleKeyType.KeyMaxEx] = 0;
        KeyPropertyMap[(int)BattleKeyType.KeyRecoverNatural] = heroData.GetFightProperty_KeyRecover();

        RecoverKey(GetKeyProperty(BattleKeyType.KeyMax) + GetKeyProperty(BattleKeyType.KeyMaxEx));
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
    
    public float ChangeProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
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

            propValue = Math.Max(propValue, GetMinGangQiRecoverNatural());
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
            
            propValue = Math.Max(propValue, GetMinXuanQiRecoverNatural());
        }   

        #endregion
        
        if (!PropertyMap.TryAdd((int)propType, propValue))
        {
            PropertyMap[(int)propType] += propValue;
        }

        /*#region 上限判断

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

        #endregion*/

        TryAdjustLimit();
        return propValue;
    }
    
    public bool ChangeProperty_Abs(BattlePropertyType propType, float propValue, BattleSource source)
    {
        if (!PropertyMap.TryAdd((int)propType, propValue))
        {
            PropertyMap[(int)propType] += propValue;
        }
        TryAdjustLimit();
        return true;
    }

    private void TryAdjustLimit()
    {
        var hp = GetProperty(BattlePropertyType.Hp);
        var hpMax = GetProperty(BattlePropertyType.MaxHp);
        if (hp > hpMax)
        {
            SetProperty(BattlePropertyType.Hp, hpMax);
        }
        
        var gangQi = GetProperty(BattlePropertyType.GangQi);
        var gangQiMax = GetProperty(BattlePropertyType.MaxGangQi);
        if (gangQi > gangQiMax)
        {
            SetProperty(BattlePropertyType.GangQi, gangQiMax);
        }
        
        var xuanQi = GetProperty(BattlePropertyType.XuanQi);
        var xuanQiMax = GetProperty(BattlePropertyType.MaxXuanQi);
        if (xuanQi > xuanQiMax)
        {
            SetProperty(BattlePropertyType.XuanQi, xuanQiMax);
        }
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

    public int GetKeyCount(BattleKeyType keyType, bool ignoreLocked = true)
    {
        if (KeyMap.TryGetValue((int)keyType, out var list))
        {
            if (!ignoreLocked)
            {
                return list.Count;
            }

            return list.Count(data => !data.Locked);
        }

        return 0;
    }
    
    private List<int> TempAllKeyTypeList = new();

    public List<int> GetAllKeyTypeList()
    {
        TempAllKeyTypeList.Clear();
        foreach (var keyType in Util.KeyList)
        {
            for (int i = 1; i <= GetKeyCount(keyType);i++)
            {
                TempAllKeyTypeList.Add((int)keyType);
            }
        }

        return TempAllKeyTypeList;
    }

    private List<BattleKey> TempAllKeyDataList = new();
    
    public List<BattleKey> GetAllKeyDataList()
    {
        TempAllKeyDataList.Clear();
        TempAllKeyDataList.AddRange(KeyMap[(int)BattleKeyType.KeyUp]);
        TempAllKeyDataList.AddRange(KeyMap[(int)BattleKeyType.KeyDown]);
        TempAllKeyDataList.AddRange(KeyMap[(int)BattleKeyType.KeyLeft]);
        TempAllKeyDataList.AddRange(KeyMap[(int)BattleKeyType.KeyRight]);
        return TempAllKeyDataList;
    }

    private int GetKeyProperty(BattleKeyType keyType)
    {
        return KeyPropertyMap.GetValueOrDefault((int)keyType, 0);
    }

    private void ChangeKeyProperty(BattleKeyType keyType, int value)
    {
        KeyPropertyMap[(int)keyType] += value;
    }
    
    public bool ChangeKey(BattleKeyType keyType, int count)
    {
        if (keyType == BattleKeyType.KeyMax || keyType == BattleKeyType.KeyMaxEx || keyType == BattleKeyType.KeyRecoverNatural)
        {
            ChangeKeyProperty(keyType, count);
            return true;
        }
        
        //添加键
        if (count > 0)
        {
            var now = GetAllKeyCount();
            var max = GetKeyPropertyMax();
            if (now >= max)
            {
                return false;
            }

            var addCount = Math.Min(max - now, count);
            if (addCount >= 1 && KeyMap.TryGetValue((int)keyType, out var addList))
            {
                for (int i = 1; i <= addCount; i++)
                {
                    var keyData = PoolManager.GetClass<BattleKey>();
                    keyData.AllocGuid();
                    keyData.KeyType = keyType;
                    keyData.Locked = false;
                    addList.Add(keyData);
                }
                return true;
            }

            return false;
        }

        if (count < 0)
        {
            //移除键
            var keyCount = GetKeyCount(keyType);
            if (keyCount <= 0)
            {
                return false;
            }

            if (KeyMap.TryGetValue((int)keyType, out var removeList) && removeList.Count > 0)
            {
                var removeCount = Math.Abs(count);
                while (removeList.Any() && removeCount > 0)
                {
                    var randomRemoveData = Util.GetRandom(removeList);
                    removeList.Remove(randomRemoveData);
                    PoolManager.RecycleClass(randomRemoveData);
                    removeCount--;
                }

                return true;
            }
        }
        
        return false;
    }

    public void RemoveAllKey()
    {
        KeyMap[(int)BattleKeyType.KeyUp].Clear();
        KeyMap[(int)BattleKeyType.KeyDown].Clear();
        KeyMap[(int)BattleKeyType.KeyLeft].Clear();
        KeyMap[(int)BattleKeyType.KeyRight].Clear();
    }
    
    public int GetAllKeyCount()
    {
        return GetKeyCount(BattleKeyType.KeyUp)
               + GetKeyCount(BattleKeyType.KeyDown)
               + GetKeyCount(BattleKeyType.KeyLeft)
               + GetKeyCount(BattleKeyType.KeyRight);
    }

    public int GetKeyPropertyMax()
    {
        return GetKeyProperty(BattleKeyType.KeyMax) + GetKeyProperty(BattleKeyType.KeyMaxEx);
    }

    public void RecoverKey(int count)
    {
        var getKey = Util.GetRandomKey(count);
        foreach (var key in getKey)
        {
            ChangeKey(key, 1);
        }
    }

    public void RemoveRandomKey(int count)
    {
        var allKey = GetAllKeyTypeList().Clone();
        var removeList = Util.GetRandomNoSame(allKey, Util.GetSameChanceList(allKey.Count), count);
        foreach (var removeKeyType in removeList)
        {
            ChangeKey((BattleKeyType)removeKeyType, -1);
        }
    }
    
    public void RecoverKeyNatural() => RecoverKey(GetKeyProperty(BattleKeyType.KeyRecoverNatural));

    public int LockRandomKey()
    {
        var keyDataList = GetAllKeyDataList().Where(data => !data.Locked).ToList();
        if (keyDataList.Count <= 0)
        {
            return 0;
        }
        var randomKeyData = Util.GetRandom(keyDataList);
        randomKeyData.Locked = true;
        return randomKeyData.KeyGuid;
    }

    public void UnlockKey(int guid)
    {
        var keyDataList = GetAllKeyDataList();
        var keyData = keyDataList.FirstOrDefault(data => data.KeyGuid == guid);
        if (keyData != null)
        {
            keyData.Locked = false;
        }
    }
    
    #endregion

    public void Recycle()
    {
        foreach (var model in MinRecoverNaturalDataList)
        {
            PoolManager.RecycleClass(model);
        }
        MinRecoverNaturalDataList.Clear();
        PropertyMap.Clear();
        KeyMap.Clear();
        HeroData = null;
    }
}
