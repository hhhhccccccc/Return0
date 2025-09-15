
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
    
    private int ActionSubjectID;
    public int GetActionSubjectID => ActionSubjectID;
    public void SetActionSubjectID(int entityID) => ActionSubjectID = entityID;
    
    private int SelectSkillID;
    public int GetSelectSkillID => SelectSkillID;
    public void SetSelectSkillID(int skillID) => SelectSkillID = skillID;
    
    private BattleState BattleState;
    public BattleState GetBattleState => BattleState;

    public void SetBattleState(BattleState battleState)
    {
        BattleState = battleState;
        var model = PoolManager.GetClass<BattleStateChangedEventModel>();
        model.BattleState = BattleState;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    } 
    
    public int Round;
    public int ActionWheel;

    /// <summary>
    /// 当前是否是这一息决定后
    /// </summary>
    private bool AfterStartActionWheel;
    public bool GetAfterStartActionWheel => AfterStartActionWheel;
    public void SetAfterStartActionWheel(bool state) => AfterStartActionWheel = state;
    public void BattleStart()
    {
        Register<BattleClickEventModel>(OnBattleClick);
        Round = 0;
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
        MessageManager.DispatchMsg<RefreshRoundViewEventModel>(null);
        MessageManager.DispatchMsg<BattlePreCalculateUnitActionWheelEventModel>(null);
        SetBattleState(BattleState.PreDoDesition);
        SetNextAliveUnitAction();
        SetSelectSkillID(0);
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
                    LogManager.Debug($"下一个行动 : {unit.EntityID}");
                    return;
                }
            }
            
            PreDoDesitionEnd();
            LogManager.Debug($"该轮行动完毕");
        }
        else
        {
            LogManager.Debug($"没有人能行动, 结束战斗");
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
            ClickSkill(model.Param1);
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
            if (GetSelectSkillID == 0) //在选择行动的目标
            {
                if (!unitIsSelf)
                    return;
            
                SetActionSubjectID(entityID);
            }
            else if (GetSelectSkillID > 0) //选择行动的目标
            {
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(GetActionSubjectID, 
                    entityID, BattleBehaviourType.Skill, GetSelectSkillID);
                SetSelectSkillID(0);
                SetNextAliveUnitAction();
            }
        }
        else if (battleState == BattleState.ForceDoDesition)
        {
            var unit = BattleManager.GetUnit(entityID);
            var unitIsSelf = unit.IsSelf;
            if (GetSelectSkillID == 0) //在选择行动的目标
            {
                if (!unitIsSelf)
                    return;

                if (!CurrActionWheelCanDoDesitionUnit.Contains(entityID))
                    return;
                SetActionSubjectID(entityID);
            }
            else if (GetSelectSkillID > 0) //选择行动的目标
            {
                ForceDoDesition(GetActionSubjectID, entityID, BattleBehaviourType.Skill, GetSelectSkillID);
            }
        }
    }
    
    private void ClickSkill(int skillID)
    {
        SetSelectSkillID(skillID);
    }

    private void ClickCancel()
    {
        SetSelectSkillID(0);
    }

    private void ClickJump()
    {
        var battleState = GetBattleState;
        if (battleState == BattleState.PreDoDesition)
        {
            BattleLogicBehaviourManager.AddOrSetBattleBehaviour(GetActionSubjectID, 
                0, BattleBehaviourType.Jump, 0);
            SetSelectSkillID(0);
            SetNextAliveUnitAction();
        }
        else if (battleState == BattleState.ForceDoDesition)
        {
            ForceDoDesition(GetActionSubjectID, 0, BattleBehaviourType.Jump, 0);
        }
    }
    
    /// <summary>
    /// 开始一轮息的计算
    /// </summary>
    public void StartOneActionWheelCalculate()
    {
        ActionWheel++;
        MessageManager.DispatchMsg<RefreshActionWheelViewEventModel>(null);
        var canDoDesitionUnitList = BattleManager.GetCurrActionWheelCanDoDesitionUnit();
        //如果有则对列表中的人进行操作且锁定
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
        bool hasActionTimes = false;
        foreach (var unit in allAliveUnit)
        {
            if (unit.IsSelf)
                hasSelf = true;
            else
                hasOther = true;

            if (unit.ActionTimes > 0)
            {
                hasActionTimes = true;
            }
        }

        if (!hasSelf)
        {
			LogManager.Debug("我方输了");
            //我方输了
        }
        else if (!hasOther)
        {
            LogManager.Debug("敌方输了");
            //敌方输了
        }
        else if (!hasActionTimes) //没有行动次数 就是回合结束
        {
            LogManager.Debug("回合结束");
            //下一回合 调用回合结束
            MessageManager.DispatchMsg<BattleRoundEndEventModel>(null);
            //过一会调用下一回合
            MessageManager.DispatchMsg<BattleRoundStartEventModel>(null);
        }
        else //息结束
        {
            LogManager.Debug("息结束");
            //一息结束
            MessageManager.DispatchMsg<BattleOneActionWheelEndEventModel>(null);
            //一息开始
            MessageManager.DispatchMsg<BattleOneActionWheelStartEventModel>(null);
        }
    }

    public void RoundEnd()
    {
        ActionWheel = 0;
        SetSelectSkillID(0);
        SetActionSubjectID(0);
        foreach (var bf in BattleManager.BfList)
        {
            bf.RoundEnd();
        }
        
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.RoundEnd();
            }
        }

        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            unit.TryRemoveUseSkill(SkillRemoveMomentType.RoundEnd);
        }
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

    private void ForceDoDesition(int subjectID, int targetID, BattleBehaviourType behaviourType, int selectSkillID)
    {
        BattleLogicBehaviourManager.AddOrSetBattleBehaviour(subjectID, 
            targetID, behaviourType, selectSkillID);
        if (CurrActionWheelCanDoDesitionUnit.Contains(subjectID))
        {
            CurrActionWheelCanDoDesitionUnit.Remove(subjectID);
        }
        SetSelectSkillID(0);
        ForceDoDesitionUnitList.Add(subjectID);
        if (CurrActionWheelCanDoDesitionUnit.Count > 0)
        {
            var nextEntityID = CurrActionWheelCanDoDesitionUnit[0];
            SetActionSubjectID(nextEntityID);
        }
        else
        {
            //都行动完后 修改状态 设置角色当前的技能 调用这回合决定行动角色的决定行动扳机 
            var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
            setUnitSkillEventModel.SetSkillUnitList = ForceDoDesitionUnitList;
            MessageManager.DispatchMsg(setUnitSkillEventModel);
            PoolManager.RecycleClass(setUnitSkillEventModel);
            
            var triggerDoDesitionMomentEventModel = PoolManager.GetClass<BattleTriggerDoDesitionMomentEventModel>();
            triggerDoDesitionMomentEventModel.DoDesitionUnitList = ForceDoDesitionUnitList;
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
}
