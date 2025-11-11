using System.Collections;
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
    private BattleMomentDesItem DesItem { get; set; }
    public override void OnShow()
    {
        base.OnShow();
        SelfBf = BattleManager.SelfBf;
        ShowUI<UIBattleMomentPanel>();
        ShowUI<UIBattleSettlementPanel>();
    }

    public void Update()
    {
        var subjectID = BattleLogicStateManager.GetActionSubjectID;
        var (skillID, variantID) = Util.UnCombSkillGuid(BattleLogicStateManager.GetSelectSkillGuid);
        
        
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(subjectID);
        if (behaviour != null)
        {
            TxtSubject.SetText( $"SubjectID : {behaviour.SubjectID}");
            TxtSkillID.SetText($"SkillID : {behaviour.SkillID}");
            TxtTarget.SetText($"TargetID : {behaviour.TargetID}");
        }
        else
        {
            TxtSubject.SetText( $"SubjectID : {subjectID}");
            TxtSkillID.SetText($"SkillID : {skillID}");
            TxtTarget.SetText($"TargetID : 0");
        }
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
        BtnStart.gameObject.SetActive(model.BattleState == BattleState.PreDoDesition);
    }

    private void OnRefreshRoundView(RefreshRoundViewEventModel model)
    {
        TxtState.SetText($"Round : {BattleLogicStateManager.Round}, ActionWheel : {BattleLogicStateManager.ActionWheel}");
    }

    private void OnRefreshActionWheelView(RefreshActionWheelViewEventModel model)
    {
        TxtState.SetText($"Round : {BattleLogicStateManager.Round}, ActionWheel : {BattleLogicStateManager.ActionWheel}");
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
        var cost = model.SKillCost.Clone();
        var time = model.Time;
        var wait = new WaitForSeconds(time / cost.Count);
        StringBuilder ss = new StringBuilder($"SkillCost : ");
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
        }
    }
    
    private void OnBtnSkill1()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 1001);
    }
    
    private void OnBtnSkill2()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 1002);
    }

    private void OnBtnSkill3()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 1003);
    }
    
    private void OnBtnCancel()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Cancel);
    }

    private void OnBtnStart()
    {
        BattleLogicStateManager.PreDoDesitionEnd();
    }
}
