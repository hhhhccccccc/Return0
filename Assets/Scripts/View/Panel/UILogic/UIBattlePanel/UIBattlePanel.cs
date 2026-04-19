using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

public partial class UIBattlePanel
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    private BattleField SelfBf { get; set; }
    private BattleField OtherBf { get; set; }
    private BattleMomentDesItem DesItem { get; set; }
    private int SubjectID { get; set; }
    private List<UIBattleSkillItem> SkillItemList = new();
    private List<UIBattleHeadItem> SelfTopHeadList = new();
    private List<UIBattleHeadItem> OtherTopHeadList = new();
    private UIBattleTeamInfoItem SelfTeamInfo { get; set; }
    private UIBattleTeamInfoItem OtherTeamInfo { get; set; }
    protected override void OnCreate()
    {
        SelfBf = BattleManager.SelfBf;
        OtherBf = BattleManager.OtherBf;
        InitTopItemInfo();
        InitMiddleTeamInfo();
    }

    private void InitMiddleTeamInfo()
    {
        if (SelfTeamInfo == null)
        {
            SelfTeamInfo = CreateUIComponentByType<UIBattleTeamInfoItem>(TfMiddleLeftInfoNode);
        }

        SelfTeamInfo.SetBf(SelfBf);
        if (OtherTeamInfo == null)
        {
            OtherTeamInfo = CreateUIComponentByType<UIBattleTeamInfoItem>(TfMiddleRightInfoNode);
        }

        OtherTeamInfo.SetBf(OtherBf);
    }

    private void InitTopItemInfo()
    {
        var selfUnits = SelfBf.GetBattleUnitDict().Values.ToList();
        CreateUIComponents(SelfTopHeadList, selfUnits.Count, TfTopLeftHeadNode);
        for (int i = 0; i < SelfTopHeadList.Count; i++)
        {
            SelfTopHeadList[i].Init(selfUnits[i]);
        }
        
        var otherUnits = OtherBf.GetBattleUnitDict().Values.ToList();
        CreateUIComponents(OtherTopHeadList, otherUnits.Count, TfTopRightHeadNode);
        for (int i = 0; i < OtherTopHeadList.Count; i++)
        {
            OtherTopHeadList[i].Init(otherUnits[i]);
        }
    }

    public void SetTopActive(bool active)
    {
        GoTopContent.SetActive(active);
        if (!active)
        {
            GoMiddleContent.SetActive(active);
        }
    }
    
    public override void OnShow()
    {
        //ShowUI<UIBattleMomentPanel>();
        //ShowUI<UIBattleSettlementPanel>();
    }
    
    public void Update()
    {
        
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        Register<RefreshBattleRenderEventModel>(OnRefreshBattleRender);
        Register<RefreshActionWheelViewEventModel>(OnRefreshActionWheelView);
        Register<RefreshRoundViewEventModel>(OnRefreshRoundView);
        Register<BattleStateChangedEventModel>(OnBattleStateChanged);
        Register<ShowSkillKeyRenderEventModel>(OnShowSkillKeyRender);
    }

    private void OnBattleStateChanged(BattleStateChangedEventModel model)
    {
        //BtnStart.gameObject.SetActive(model.BattleState == BattleState.PreDoDesition);
    }

    private void OnRefreshRoundView(RefreshRoundViewEventModel model)
    {
        //TxtState.SetText($"回合 : {BattleLogicStateManager.Round}, 当前息 : {BattleLogicStateManager.ActionWheel}");
    }

    private void OnRefreshActionWheelView(RefreshActionWheelViewEventModel model)
    {
        //TxtState.SetText($"回合 : {BattleLogicStateManager.Round}, 当前息 : {BattleLogicStateManager.ActionWheel}");
    }

    private void OnRefreshBattleRender(RefreshBattleRenderEventModel model)
    {
        if (model.RefreshUIBattle)
        {
            RefreshSkill();
        }
    }

    private void RefreshSkill()
    {
        
    }

    private void OnShowSkillKeyRender(ShowSkillKeyRenderEventModel model)
    {
        /*var cost = model.SKillCost.Clone();
        var time = model.Time;
        var wait = new WaitForSeconds(time / cost.Count);
        StringBuilder ss = new StringBuilder($"技能消耗 : ");
        StartCoroutine(ShowSkillCost());
        IEnumerator ShowSkillCost()
        {
            TxtSkillCost.gameObject.SetActive(true);
            foreach (var t in cost)
            {
                ss.Append(t.ToString());
                TxtSkillCost.SetText(ss);
                yield return wait;
            }
            TxtSkillCost.gameObject.SetActive(false);
        }*/
    }
    
    private void OnBtnCancel()
    {
        //BattleRenderManager.DispatchClickEventModel(BattleClickType.Cancel);
    }

    private void OnBtnStart()
    {
        //BattleLogicStateManager.PreDoDesitionEnd();
    }

    private void OnBtnLook()
    {
        if (GoMiddleContent.gameObject.activeSelf)
        {
            GoMiddleContent.gameObject.SetActive(false);
        }
        else if (!GoMiddleContent.gameObject.activeSelf)
        {
            GoMiddleContent.gameObject.SetActive(true);
        }
    }

    private void OnBtnStop()
    {
        
    }
}
