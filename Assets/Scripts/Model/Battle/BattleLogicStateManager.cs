
using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleLogicStateManager : SingleModel
{
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private DateSys DateSys { get; set; }
    [Inject] private WeatherSys WeatherSys { get; set; }
    
    private int ActionSubjectID;
    public int GetActionSubjectID => ActionSubjectID;
    public void SetActionSubjectID(int entityID) => ActionSubjectID = entityID;
    
    private int SelectSkillGuid;
    public int GetSelectSkillGuid => SelectSkillGuid;
    public void SetSelectSkillGuid(int skillGuid) => SelectSkillGuid = skillGuid;
    
    private BattleState BattleState;
    public BattleState GetBattleState => BattleState;

    /// <summary>
    /// 本回合行动过的角色
    /// </summary>
    private HashSet<int> RoundAlreadyActionUnitList = new();
    public void TryAddRoundAlreadyActionUnit(int entityID)
    {
        RoundAlreadyActionUnitList.Add(entityID);
    }

    public List<int> RoundUsedSkillGuid = new();
    public void AddRoundUsedSkillGuid(int skillGuid) => RoundUsedSkillGuid.Add(skillGuid);
    public bool UnitIsRoundAlreadyAction(int entityID) => RoundAlreadyActionUnitList.Contains(entityID);
    
    public void SetBattleState(BattleState battleState)
    {
        BattleState = battleState;
        var model = PoolManager.GetClass<BattleStateChangedEventModel>();
        model.BattleState = BattleState;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }
    
    public int Round { get; set; }
    public int ActionWheel { get; set; }

    /// <summary>
    /// 当前是否是这一息决定后
    /// </summary>
    private bool AfterStartActionWheel;
    public bool GetAfterStartActionWheel => AfterStartActionWheel;
    public void SetAfterStartActionWheel(bool state) => AfterStartActionWheel = state;
    public void BattleStart()
    {
        Register<BattleClickEventModel>(OnBattleClick);
        ChangeChrono(DateSys.ChronoType, BattleChronoContinueType.Round, 999999);
        ChangeWeather(WeatherSys.GetCurrZoneWeatherData().WeatherType, BattleWeatherContinueType.Round, 999999);
        Round = 0;
        LogManager.D("[战斗开始]");
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.BattleStart();
            }
        }
        MessageManager.DispatchMsg<BattleRoundStartEventModel>(null);
    }
    
    public void RoundStart()
    {
        SetBattleState(BattleState.RoundStart);
        Round++;
        ActionWheel = 0;
        foreach (var bf in BattleManager.BfList)
        {
            bf.RoundStart();
        }
        
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.RoundStart();
            }
        }

        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            unit.TryRemoveUseSkill(SkillRemoveMomentType.NextRoundStart);           
        }
        MessageManager.DispatchMsg<RefreshRoundViewEventModel>(null);
        MessageManager.DispatchMsg<BattlePreCalculateUnitActionWheelEventModel>(null);
        SetBattleState(BattleState.PreDoDesition);
        SetNextAliveUnitAction();
        SetSelectSkillGuid(0);
    }

    private void SetNextAliveUnitAction()
    {
        var aliveUnit = BattleManager.GetSelfBfAliveUnit();
        if (aliveUnit.Count > 0)
        {
            var behaviourList = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue();
            foreach (var unit in aliveUnit)
            {
                if (behaviourList.All(behaviour => behaviour.SubjectID != unit.EntityID))
                {
                    SetActionSubjectID(unit.EntityID);
                    RefreshBattleRender();
                    LogManager.D($"下一个行动 : {unit.EntityID}");
                    return;
                }
            }
            
            PreDoDesitionEnd();
            LogManager.D($"该轮行动完毕");
        }
        else
        {
            LogManager.D($"没有人能行动, 结束战斗");
        }
    }

    public void PreDoDesitionEnd()
    {
        MessageManager.DispatchMsg<BattlePreDoDesitionEndEventModel>(null);
    }

    private void OnBattleClick(BattleClickEventModel model)
    {
        if (model.ClickType == BattleClickType.Entity)
        {
            ClickUnit(model.Param1);
        }
        else if (model.ClickType == BattleClickType.Skill)
        {
            ClickSkill(model.Param1, model.Param2);
        }
        else if (model.ClickType == BattleClickType.Cancel)
        {
            ClickCancel();
        }

        RefreshBattleRender();
    }
    
    private void RefreshBattleRender(bool selfBf = true, bool otherBf = true, bool uiBattle = true)
    {
        var model = PoolManager.GetClass<RefreshBattleRenderEventModel>();
        model.BattleState = GetBattleState;
        model.RefreshSelfBf = selfBf;
        model.RefreshOtherBf = otherBf;
        model.RefreshUIBattle = uiBattle;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }

    private void ClickUnit(int entityID)
    {
        var battleState = GetBattleState;
        if (battleState == BattleState.PreDoDesition)
        {
            var unit = BattleManager.GetUnit(entityID);
            var unitIsSelf = unit.IsSelf;
            if (GetSelectSkillGuid == 0) //在选择行动的目标
            {
                if (!unitIsSelf)
                    return;
            
                SetActionSubjectID(entityID);
            }
            else if (GetSelectSkillGuid > 0) //选择行动的目标
            {
                var (skillID, variantID) = Util.UnCombSkillGuid(GetSelectSkillGuid);
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(GetActionSubjectID, 
                    entityID, BattleBehaviourType.Skill, skillID, variantID);
                SetSelectSkillGuid(0);
                SetNextAliveUnitAction();
            }
        }
        else if (battleState == BattleState.ForceDoDesition)
        {
            var unit = BattleManager.GetUnit(entityID);
            var unitIsSelf = unit.IsSelf;
            if (GetSelectSkillGuid == 0) //在选择行动的目标
            {
                if (!unitIsSelf)
                    return;

                if (!CurrActionWheelCanDoDesitionUnit.Contains(entityID))
                    return;
                SetActionSubjectID(entityID);
            }
            else if (GetSelectSkillGuid > 0) //选择行动的目标
            {
                ForceDoDesition(GetActionSubjectID, entityID, BattleBehaviourType.Skill, GetSelectSkillGuid);
            }
        }
    }
    
    private void ClickSkill(int skillID, int variantID)
    {
        var skillGuid = Util.CombSkillGuid(skillID, variantID);
        SetSelectSkillGuid(skillGuid);
    }

    private void ClickCancel()
    {
        SetSelectSkillGuid(0);
    }

    private void ClickJump()
    {
        var battleState = GetBattleState;
        if (battleState == BattleState.PreDoDesition)
        {
            BattleLogicBehaviourManager.AddOrSetBattleBehaviour(GetActionSubjectID, 
                0, BattleBehaviourType.Jump, 0, 0);
            SetSelectSkillGuid(0);
            SetNextAliveUnitAction();
        }
        else if (battleState == BattleState.ForceDoDesition)
        {
            ForceDoDesition(GetActionSubjectID, 0, BattleBehaviourType.Jump, 0);
        }
    }
    
    /// <summary>
    /// 有行动但是
    /// </summary>
    private List<int> TempTodoRepeatUseSkillFailList = new();
    
    /// <summary>
    /// 需要重复行动的人
    /// </summary>
    private List<int> TempTodoRepeatUseSkillSuccessList = new();
 
    /// <summary>
    /// 开始一轮息的计算
    /// </summary>
    public void StartOneActionWheelCalculate()
    {
        ActionWheel++;
        MessageManager.DispatchMsg<RefreshActionWheelViewEventModel>(null);
        
        //做需要有重复行动成功的人
        TempTodoRepeatUseSkillSuccessList.Clear();
        //做需要有重复行动失败的人
        TempTodoRepeatUseSkillFailList.Clear();
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            var repeatData = unit.RepeatUseSkillData;
            if (repeatData != null)
            {
                var target = BattleManager.GetUnit(repeatData.TargetID);
                if (unit.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(repeatData.SkillID, repeatData.VariantID), target))
                {
                    unit.AddActionTimes(1);
                    BattleLogicBehaviourManager.AddOrSetBattleBehaviour(unit.EntityID,
                        repeatData.TargetID, BattleBehaviourType.Skill, repeatData.SkillID, repeatData.VariantID, false, true);
                    TempTodoRepeatUseSkillSuccessList.Add(unit.EntityID);
                }
                else if (repeatData.IfLostChangeToOther)
                {
                    var enemies = BattleManager.GetAllOpponentUnit(unit.EntityID, true);
                    var randomEnemy = Util.GetRandom(enemies);
                    if (unit.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(repeatData.SkillID, repeatData.VariantID), randomEnemy))
                    {
                        unit.AddActionTimes(1);
                        BattleLogicBehaviourManager.AddOrSetBattleBehaviour(unit.EntityID,
                            randomEnemy.EntityID, BattleBehaviourType.Skill, repeatData.SkillID, repeatData.VariantID, false, true);
                        TempTodoRepeatUseSkillSuccessList.Add(unit.EntityID);
                    }
                    else
                    {
                        TempTodoRepeatUseSkillFailList.Add(unit.EntityID);
                    }
                }
                else
                {
                    TempTodoRepeatUseSkillFailList.Add(unit.EntityID);
                }
            }
        }
        //移除重复行动失败的人的重复数据
        foreach (var entityID in TempTodoRepeatUseSkillFailList)
        {
            var unit = BattleManager.GetUnit(entityID);
            unit.RemoveRepeatUseSkill();
        }
        
        var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
        setUnitSkillEventModel.SetSkillUnitList = TempTodoRepeatUseSkillSuccessList;
        MessageManager.DispatchMsg(setUnitSkillEventModel);
        PoolManager.RecycleClass(setUnitSkillEventModel);
        TempTodoRepeatUseSkillSuccessList.Clear();
        
        //如果有则对列表中的人进行操作且锁定
        var canDoDesitionUnitList = BattleManager.GetCurrActionWheelCanDoDesitionUnit();
        foreach (var entityID in canDoDesitionUnitList)
        {
            if (TempTodoRepeatUseSkillFailList.Contains(entityID))
            {
                canDoDesitionUnitList.Remove(entityID);
            }
        }
        //对即将要操作的人进行决定行动前的扳机
        foreach (var entityID in canDoDesitionUnitList)
        {
            foreach (var moment in BattleManager.GetUnit(entityID).GetBattleMoment())
            {
                moment.BeforeDoDesitionAction();
            }
        }
        if (canDoDesitionUnitList.Count > 0)
        {
            StartActionWheelDoDesition(canDoDesitionUnitList);
        }
        else//没有就开始计算扳机
        {
            StartOneActionWheelLogicCalculate();
        }
    }

    public void TryEnd()
    {
        var allAliveUnit = BattleManager.GetAllAliveUnit();
        bool hasSelf = false;
        bool hasOther = false;
        bool hasNextAction = false;
        foreach (var unit in allAliveUnit)
        {
            if (unit.IsSelf)
                hasSelf = true;
            else
                hasOther = true;

            //如果后面有行动息大于回合行动息 或者 重复数据不为空
            if (unit.ActionWheel > ActionWheel || unit.RepeatUseSkillData != null)
            {
                hasNextAction = true;
            }
        }

        if (!hasSelf)
        {
			LogManager.D("我方输了");
            //我方输了
        }
        else if (!hasOther)
        {
            LogManager.D("敌方输了");
            //敌方输了
        }
        else if (!hasNextAction) 
        {
            LogManager.D("回合结束");
            //下一回合 调用回合结束
            MessageManager.DispatchMsg<BattleRoundEndEventModel>(null);
            //过一会调用下一回合
            MessageManager.DispatchMsg<BattleRoundStartEventModel>(null);
        }
        else //息结束
        {
            LogManager.D("息结束");
            //一息结束
            MessageManager.DispatchMsg<BattleOneActionWheelEndEventModel>(null);
            //一息开始
            MessageManager.DispatchMsg<BattleOneActionWheelStartEventModel>(null);
        }
    }

    private void RecycleRoundEndData()
    {
        foreach (var data in WaitRecycleKeyDataList)
        {
            PoolManager.RecycleClass(data);
        }

        WaitRecycleKeyDataList.Clear();
    }
    
    public void RoundEnd()
    {
        RecycleRoundEndData();
        
        ActionWheel = 0;
        SetSelectSkillGuid(0);
        SetActionSubjectID(0);
        //调用回合结束扳机
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.RoundEnd();
            }
        }
        
        foreach (var bf in BattleManager.BfList)
        {
            bf.RoundEnd();
        }
        
        //移除回合结束的技能
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            unit.TryRemoveUseSkill(SkillRemoveMomentType.RoundEnd);
        }
        
        ReduceChorono(BattleChronoContinueType.Round);
        ReduceWeather(BattleWeatherContinueType.Round);
        
        RoundAlreadyActionUnitList.Clear();
        RoundUsedSkillGuid.Clear();
        RoundUnitDieList.Clear();
    }
    
    /// <summary>
    /// 开始当前息的角色强制行动
    /// </summary>
    private List<int> CurrActionWheelCanDoDesitionUnit = new();
    /// <summary>
    /// 记录这一息哪些角色被强制行动了
    /// </summary>
    private List<int> ForceDoDesitionUnitList = new();
    private void StartActionWheelDoDesition(List<int> canDoDesitionUnitList)
    {
        SetBattleState(BattleState.ForceDoDesition);
        CurrActionWheelCanDoDesitionUnit = canDoDesitionUnitList;
        ForceDoDesitionUnitList.Clear();
        RefreshBattleRender();
        StartForceDoDesition();
    }
    
    /// <summary>
    /// 开始强制行动
    /// </summary>
    private void StartForceDoDesition()
    {
        var firstUnitEntityID = CurrActionWheelCanDoDesitionUnit[0];
        SetActionSubjectID(firstUnitEntityID);
        RefreshBattleRender();
        //负责UI变化
    }

    private void ForceDoDesition(int subjectID, int targetID, BattleBehaviourType behaviourType, int selectSkillGuid)
    {
        var (skillID, variantID) = Util.UnCombSkillGuid(selectSkillGuid);
        BattleLogicBehaviourManager.AddOrSetBattleBehaviour(subjectID, 
            targetID, behaviourType, skillID, variantID);
        if (CurrActionWheelCanDoDesitionUnit.Contains(subjectID))
        {
            CurrActionWheelCanDoDesitionUnit.Remove(subjectID);
        }
        SetSelectSkillGuid(0);
        ForceDoDesitionUnitList.Add(subjectID);
        if (CurrActionWheelCanDoDesitionUnit.Count > 0)
        {
            var nextEntityID = CurrActionWheelCanDoDesitionUnit[0];
            SetActionSubjectID(nextEntityID);
        }
        else
        {
            //都行动完后 修改状态 设置角色当前的技能 
            var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
            setUnitSkillEventModel.SetSkillUnitList = ForceDoDesitionUnitList;
            MessageManager.DispatchMsg(setUnitSkillEventModel);
            PoolManager.RecycleClass(setUnitSkillEventModel);
            
            //调用这回合决定行动角色的决定行动扳机 
            var triggerDoDesitionMomentEventModel = PoolManager.GetClass<BattleTriggerDoDesitionMomentEventModel>();
            triggerDoDesitionMomentEventModel.DoDesitionUnitList = ForceDoDesitionUnitList;
            triggerDoDesitionMomentEventModel.IsPreDesition = false;
            MessageManager.DispatchMsg(triggerDoDesitionMomentEventModel);
            PoolManager.RecycleClass(triggerDoDesitionMomentEventModel);
            
            SetActionSubjectID(0);
            StartOneActionWheelLogicCalculate();
        }
        RefreshBattleRender();
    }
    
    /// <summary>
    /// 执行这一轮息的所有单位的逻辑计算
    /// </summary>
    private void StartOneActionWheelLogicCalculate()
    {
        var model = PoolManager.GetClass<BattleOneActionWheelLogicCalculateEventModel>();
        model.ActionWheelUnit = BattleManager.GetCurrActionWheelUnit();
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }

    private Action<int> AddUnitToNowLogicCalculate;

    public void RegisterAddUnitToNowLogicCalculate(Action<int> cb)
    {
        AddUnitToNowLogicCalculate = cb;
    }

    public void CallAddUnitToNowLogicCalculate(int entityID)
    {
        AddUnitToNowLogicCalculate?.Invoke(entityID);
    }

    public ChronoType BattleChronoType { get; set; }
    public List<BattleChronoData> BattleChronoDatas = new();
    public WeatherType BattleWeatherType { get; set; }
    public List<BattleWeatherData> BattleWeatherDatas = new();
    public void ChangeChrono(ChronoType chronoType, BattleChronoContinueType continueType, int times)
    {
        var data = PoolManager.GetClass<BattleChronoData>();
        data.ChronoType = chronoType;
        data.ContinueType = continueType;
        data.Times = times;
        BattleChronoDatas.Add(data);
        SetChrono();
    }

    private void SetChrono()
    {
        if (BattleChronoDatas.Count > 0)
        {
            var chronoData = BattleChronoDatas.Last();
            BattleChronoType = chronoData.ChronoType;
        }
    }

    /// <summary>
    /// 时辰的改变
    /// </summary>
    /// <param name="type"></param>
    public void ReduceChorono(BattleChronoContinueType type)
    {
        for (int i = BattleChronoDatas.Count - 1; i >= 0; i--)
        {
            var chronoData = BattleChronoDatas[i];
            if (type == BattleChronoContinueType.Round)
            {
                if (chronoData.ContinueType == BattleChronoContinueType.Round)
                {
                    chronoData.Times--;
                    if (chronoData.Times <= 0)
                    {
                        BattleChronoDatas.RemoveAt(i);
                    }
                    PoolManager.RecycleClass(chronoData);
                }
                else if (chronoData.ContinueType == BattleChronoContinueType.ActionWheel)
                {
                    BattleChronoDatas.RemoveAt(i);
                    PoolManager.RecycleClass(chronoData);
                }
            }
            else if (type == BattleChronoContinueType.ActionWheel)
            {
                if (chronoData.ContinueType == BattleChronoContinueType.ActionWheel)
                {
                    chronoData.Times--;
                    if (chronoData.Times <= 0)
                    {
                        BattleChronoDatas.RemoveAt(i);
                    }
                    PoolManager.RecycleClass(chronoData);
                }
            }
        }
        SetChrono();
    }
    
    public void ChangeWeather(WeatherType weatherType, BattleWeatherContinueType continueType, int times)
    {
        var data = PoolManager.GetClass<BattleWeatherData>();
        data.WeatherType = weatherType;
        data.ContinueType = continueType;
        data.Times = times;
        BattleWeatherDatas.Add(data);
        SetWeather();
    }

    private void SetWeather()
    {
        if (BattleWeatherDatas.Count > 0)
        {
            var weatherData = BattleWeatherDatas.Last();
            BattleWeatherType = weatherData.WeatherType;
        }
    }

    public void ReduceWeather(BattleWeatherContinueType type)
    {
        //天气的改变
        for (int i = BattleWeatherDatas.Count - 1; i >= 0; i--)
        {
            var weatherData = BattleWeatherDatas[i];
            if (type == BattleWeatherContinueType.Round)
            {
                if (weatherData.ContinueType == BattleWeatherContinueType.Round)
                {
                    weatherData.Times--;
                    if (weatherData.Times <= 0)
                    {
                        BattleWeatherDatas.RemoveAt(i);
                    }
                    PoolManager.RecycleClass(weatherData);
                }
                else if (weatherData.ContinueType == BattleWeatherContinueType.ActionWheel)
                {
                    BattleWeatherDatas.RemoveAt(i);
                    PoolManager.RecycleClass(weatherData);
                }
            }
            else if (type == BattleWeatherContinueType.ActionWheel)
            {
                if (weatherData.ContinueType == BattleWeatherContinueType.ActionWheel)
                {
                    weatherData.Times--;
                    if (weatherData.Times <= 0)
                    {
                        BattleWeatherDatas.RemoveAt(i);
                    }
                    PoolManager.RecycleClass(weatherData);
                }
            }
            
        }
        SetWeather();
    }

    public override void Clear()
    {
        ActionSubjectID = 0;
        SelectSkillGuid = 0;
        BattleState = BattleState.None;
        RoundAlreadyActionUnitList.Clear();
        RoundUsedSkillGuid.Clear();
        Round = 0;
        ActionWheel = 0;
        AfterStartActionWheel = false;
        CurrActionWheelCanDoDesitionUnit.Clear();
        ForceDoDesitionUnitList.Clear();
        AddUnitToNowLogicCalculate = null;
        BattleChronoType = ChronoType.None;
        BattleChronoDatas.Clear();
        BattleWeatherType = WeatherType.None;
        BattleWeatherDatas.Clear();
        base.Clear();
    }

    public void ActionWheelEnd()
    {
        ReduceChorono(BattleChronoContinueType.ActionWheel);
        ReduceWeather(BattleWeatherContinueType.ActionWheel);
        SetAfterStartActionWheel(false);
    }

    /// <summary>
    /// 等待回收的键
    /// </summary>
    private List<BattleKey> WaitRecycleKeyDataList = new();
    public void AddWaitRecycleKeyData(BattleKey keyData) => WaitRecycleKeyDataList.Add(keyData);

    private List<int> RoundUnitDieList = new();
    public void AddRoundUnitDieList(int entityID) => RoundUnitDieList.Add(entityID);
    public bool HasRoundUnitDie() => RoundUnitDieList.Any();
}
