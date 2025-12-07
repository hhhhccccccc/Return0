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
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    private Dictionary<int, float> PropertyMap = new();
    private Dictionary<int, int> KeyPropertyMap = new();
    private Dictionary<int, List<BattleKey>> KeyMap = new();
    private HeroData HeroData { get; set; }

    private List<MinRecoverNaturalData> MinRecoverNaturalDataList = new();
    
    private BattleUnit Unit { get; set; }
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
    
    public void Init(HeroData heroData, BattleUnit unit)
    {
        HeroData = heroData;
        Unit = unit;
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

        var tryConvertType = ChangePropertyChangeModelBeEffect(propType, propValue);
        
        if (!PropertyMap.TryAdd((int)tryConvertType, propValue))
        {
            PropertyMap[(int)tryConvertType] += propValue;
        }

        TryAdjustLimit();
        return propValue;
    }

    public BattlePropertyType ChangePropertyChangeModelBeEffect(BattlePropertyType propType, float propValue)
    {
        var hasMethod10060 = Unit.CheckHasMethod(GameConst.Battle.HeartMethod10060);
        if (hasMethod10060 && propValue > 0)
        {
            if (propType == BattlePropertyType.DefendInt)
            {
                return BattlePropertyType.PowerInt;
            }
            
            if (propType == BattlePropertyType.DefendPct)
            {
                return BattlePropertyType.PowerPct;
            }
            
            if (propType == BattlePropertyType.AllDefendPct)
            {
                return BattlePropertyType.AllPowerPct;
            }
            
            if (propType == BattlePropertyType.DefendAddPct)
            {
                return BattlePropertyType.PowerAddPct;
            }
            
            if (propType == BattlePropertyType.BreakInt)
            {
                return BattlePropertyType.TechInt;
            }
            
            if (propType == BattlePropertyType.BreakPct)
            {
                return BattlePropertyType.TechPct;
            }
            
            if (propType == BattlePropertyType.AllBreakPct)
            {
                return BattlePropertyType.AllTechPct;
            }
            
            if (propType == BattlePropertyType.BreakAddPct)
            {
                return BattlePropertyType.TechAddPct;
            }
        }
        
        return propType;
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

    public void TryAdjustLimit()
    {
        #region 上限判断

        var maxHp = GetProperty(BattlePropertyType.MaxHp);
        var hp = GetProperty(BattlePropertyType.Hp);
        if (hp >= maxHp)
        {
            PropertyMap[(int)BattlePropertyType.Hp] = maxHp;
        }
        
        var maxGangQi = GetProperty(BattlePropertyType.MaxGangQi);
        var gangQi = GetProperty(BattlePropertyType.GangQi);
        if (gangQi >= maxGangQi)
        {
            PropertyMap[(int)BattlePropertyType.GangQi] = maxGangQi;
        }
        
        var maxXuanQi = GetProperty(BattlePropertyType.MaxXuanQi);
        var xuanQi = GetProperty(BattlePropertyType.XuanQi);
        if (xuanQi >= maxXuanQi)
        {
            PropertyMap[(int)BattlePropertyType.XuanQi] = maxXuanQi;
        }
        
        #endregion
    }

    public bool SetProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        PropertyMap[(int)propType] = propValue;
        return true;
    }
    
    public float GetProperty(BattlePropertyType propType, GetPropertySourceModel model = null)
    {
        var p = propType switch
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
            BattlePropertyType.Power => (GetProperty(BattlePropertyType.BasicPower) 
                                         *  (1 + GetProperty(BattlePropertyType.PowerPct) * GetProperty(BattlePropertyType.PowerAddPct)) 
                                            + GetProperty(BattlePropertyType.PowerInt) * GetProperty(BattlePropertyType.PowerAddPct)) 
                                        * (1 + GetProperty(BattlePropertyType.AllPowerPct)),
            BattlePropertyType.Tech => (GetProperty(BattlePropertyType.BasicTech)
                                        * (1 + GetProperty(BattlePropertyType.TechPct) * GetProperty(BattlePropertyType.TechAddPct)) 
                                           + GetProperty(BattlePropertyType.TechInt) * GetProperty(BattlePropertyType.TechAddPct)) 
                                       * (1 + GetProperty(BattlePropertyType.AllTechPct)),
            BattlePropertyType.Speed => (GetProperty(BattlePropertyType.BasicSpeed) 
                                         * (1 + GetProperty(BattlePropertyType.SpeedPct) * GetProperty(BattlePropertyType.SpeedAddPct)) 
                                            + GetProperty(BattlePropertyType.SpeedInt) * GetProperty(BattlePropertyType.SpeedAddPct)) 
                                        * (1 + GetProperty(BattlePropertyType.AllSpeedPct)),
            BattlePropertyType.Clever => (GetProperty(BattlePropertyType.BasicClever) 
                                          * (1 + GetProperty(BattlePropertyType.CleverPct) * GetProperty(BattlePropertyType.CleverAddPct)) 
                                             + GetProperty(BattlePropertyType.CleverInt) * GetProperty(BattlePropertyType.CleverAddPct)) 
                                         * (1 + GetProperty(BattlePropertyType.AllCleverPct)),
            BattlePropertyType.Defend => (GetProperty(BattlePropertyType.BasicDefend) 
                                          * (1 + GetProperty(BattlePropertyType.DefendPct) * GetProperty(BattlePropertyType.DefendAddPct)) 
                                             + GetProperty(BattlePropertyType.DefendInt) * GetProperty(BattlePropertyType.DefendAddPct)) 
                                         * (1 + GetProperty(BattlePropertyType.AllDefendPct)),
            BattlePropertyType.Break => (GetProperty(BattlePropertyType.BasicBreak)
                                         * (1 + GetProperty(BattlePropertyType.BreakPct) * GetProperty(BattlePropertyType.BreakAddPct)) 
                                            + GetProperty(BattlePropertyType.BreakInt) * GetProperty(BattlePropertyType.BreakAddPct)) 
                                        * (1 + GetProperty(BattlePropertyType.AllBreakPct)),
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

        var skillAdd = 0.0f;
        var skill = Unit.GetSkill();
        if (skill != null)
        {
            skillAdd = skill.GetProperty(propType);
        }
       
        var changeModelAdd = Unit.GetBattlePropertyChanged().Sum(changeModel => GetPropertyChangeModelBeEffect(changeModel, propType, model));
        return p + changeModelAdd + skillAdd;
    }
    
    private float GetPropertyChangeModelBeEffect(IGetBattlePropertyChanged changeModel, BattlePropertyType pType, GetPropertySourceModel model = null)
    {
        var hasMethod10060 = Unit.CheckHasMethod(GameConst.Battle.HeartMethod10060);
        if (hasMethod10060)
        {
            #region 心法10060影响
            
            //防变成力
            if (pType == BattlePropertyType.PowerInt)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.PowerInt);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.DefendInt);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.DefendInt)
            {
                var property = changeModel.GetProperty(BattlePropertyType.DefendInt);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.PowerPct)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.PowerPct);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.DefendPct);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.DefendPct)
            {
                var property =  changeModel.GetProperty(BattlePropertyType.DefendPct);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.AllPowerPct)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.AllPowerPct);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.AllDefendPct);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.AllDefendPct)
            {
                var property =  changeModel.GetProperty(BattlePropertyType.AllDefendPct);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.PowerAddPct)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.PowerAddPct);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.DefendAddPct);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.DefendAddPct)
            {
                var property =  changeModel.GetProperty(BattlePropertyType.DefendAddPct);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            //破变成技
            if (pType == BattlePropertyType.TechInt)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.TechInt);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.BreakInt);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.BreakInt)
            {
                var property = changeModel.GetProperty(BattlePropertyType.BreakInt);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.TechPct)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.TechPct);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.BreakPct);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.BreakPct)
            {
                var property =  changeModel.GetProperty(BattlePropertyType.BreakPct);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.AllTechPct)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.AllTechPct);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.AllBreakPct);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.AllBreakPct)
            {
                var property =  changeModel.GetProperty(BattlePropertyType.AllBreakPct);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.TechAddPct)
            {
                var propertyA = changeModel.GetProperty(BattlePropertyType.TechAddPct);
                var propertyB =  changeModel.GetProperty(BattlePropertyType.BreakAddPct);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.BreakAddPct)
            {
                var property =  changeModel.GetProperty(BattlePropertyType.BreakAddPct);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }

            #endregion
           
        }

        return changeModel.GetProperty(pType, model);
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

    public int GetKeyCount(BattleKeyType keyType, bool isLocked = false)
    {
        if (KeyMap.TryGetValue((int)keyType, out var list))
        {
            if (!isLocked)
            {
                return list.Count;
            }

            return list.Count(data => data.Locked);
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

    public int GetKeyProperty(BattleKeyType keyType) => KeyPropertyMap.GetValueOrDefault((int)keyType, 0);

    public int ChangeKeyProperty(BattleKeyType keyType, int value, ChangeKeyReason reason)
    {
        KeyPropertyMap[(int)keyType] += value;
        return value;
    }

    private List<BattleKey> TempRemoveKeyList = new();
    
    public List<BattleKey> ChangeKey(BattleKeyType keyType, int count, ChangeKeyReason reason)
    {
        //添加键
        var list = new List<BattleKey>();
        if (count > 0)
        {
            var now = GetAllKeyCount();
            var max = GetKeyPropertyMax();
            if (now >= max)
            {
                return list;
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
                    keyData.Pollution = false;
                    addList.Add(keyData);
                    list.Add(keyData);
                }
                return list;
            }

            return list;
        }

        if (count < 0)
        {
            //技能输入的时候代替键
            TempRemoveKeyList.Clear();
            if (reason == ChangeKeyReason.SkillCost)
            {
                var replaceTypeList = new List<int>
                {
                    (int)keyType
                };
                
                foreach (var changeModel in Unit.GetBattlePropertyChanged())
                {
                    changeModel.KeyReplace(replaceTypeList, keyType);
                }

                foreach (var costType in replaceTypeList)
                {
                    TempRemoveKeyList.AddRange(KeyMap[costType]);
                }
            }
            else
            {
                TempRemoveKeyList.AddRange(KeyMap[(int)keyType]);
            }

            
            if (KeyMap.TryGetValue((int)keyType, out var removeList) && removeList.Count > 0)
            {
                //移除优先级高的键
                List<BattleKey> cloneData;
                if (reason == ChangeKeyReason.SkillCost)
                {
                    cloneData = removeList.OrderByDescending(data => data.GetPriority()).ToList();
                }
                else
                { 
                    cloneData = removeList.OrderByDescending(data => Guid.NewGuid()).ToList();
                }
                var removeCount = Math.Abs(count);
                while (cloneData.Any() && removeCount > 0)
                {
                    var randomRemoveData = cloneData[0];
                    cloneData.RemoveAt(0);
                    removeList.Remove(randomRemoveData);
                    list.Add(randomRemoveData);
                    BattleLogicStateManager.AddWaitRecycleKeyData(randomRemoveData);
                    removeCount--;
                }

                return list;
            }
        }
        
        return list;
    }

    public BattleKey RemoveKeyByGuid(int guid)
    {
        var keyData = GetAllKeyDataList().FirstOrDefault(data => data.KeyGuid == guid);
        if (keyData != null)
        {
            var keyType = keyData.KeyType;
            var list = KeyMap[(int)keyType];
            list.Remove(keyData);
            return keyData;
        }

        return null;
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
        return GetKeyProperty(BattleKeyType.KeyMax)
               + GetKeyProperty(BattleKeyType.KeyMaxEx)
               + Unit.GetBattlePropertyChanged().Sum(changeModel => changeModel.GetKeyMaxEx());
    }
    
    /// <summary>
    /// 锁住键
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<BattleKey> LockRandomKey(int count)
    {
        var keyDataList = GetAllKeyDataList().Where(data => !data.Locked).ToList();
        if (keyDataList.Count <= 0)
        {
            return null;
        }

        var dataCount = keyDataList.Count;
        var randomKeyDataList = Util.GetRandomNoSame(keyDataList, Util.GetSameChanceList(dataCount), dataCount);
        randomKeyDataList.ForEach(data => data.Locked = true);
        return randomKeyDataList;
    }
    /// <summary>
    /// 解锁键
    /// </summary>
    /// <param name="guid"></param>
    public BattleKey UnlockKey(int guid)
    {
        var keyDataList = GetAllKeyDataList();
        var keyData = keyDataList.FirstOrDefault(data => data.KeyGuid == guid);
        if (keyData != null)
        {
            keyData.Locked = false;
        }
        return keyData;
    }
    /// <summary>
    /// 污染键
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<BattleKey> PollutionRandomKey(int count)
    {
        var keyDataList = GetAllKeyDataList().Where(data => !data.Pollution).ToList();
        if (keyDataList.Count <= 0)
        {
            return null;
        }
        var dataCount = keyDataList.Count;
        var randomKeyDataList = Util.GetRandomNoSame(keyDataList, Util.GetSameChanceList(dataCount), dataCount);
        randomKeyDataList.ForEach(data => data.Pollution = true);
        return randomKeyDataList;
    }
    /// <summary>
    /// 解除污染键
    /// </summary>
    /// <param name="guid"></param>
    public BattleKey UnPollutionKey(int guid)
    {
        var keyDataList = GetAllKeyDataList();
        var keyData = keyDataList.FirstOrDefault(data => data.KeyGuid == guid);
        if (keyData != null)
        {
            keyData.Pollution = false;
        }
        return keyData;
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

    public List<BattleKey> CheckKeyLimit()
    {
        var keyMaxCount = GetKeyPropertyMax();
        var currCount = GetAllKeyCount();
        var delta = currCount - keyMaxCount;
        var result = new List<BattleKey>();
        if (delta > 0)
        {
            var list = Util.GetRandomNoSame(GetAllKeyDataList(), Util.GetSameChanceList(delta), delta);
            foreach (var keyData in list)
            {
                var removeKeyData = RemoveKeyByGuid(keyData.KeyGuid);
                if (removeKeyData != null)
                {
                    result.Add(removeKeyData);
                }
            }
        }

        return result;
    }
}
