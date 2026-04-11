using System.Collections;
using System.Collections.Generic;
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
    private int SubjectID { get; set; }
    private List<UIBattleSkillItem> SkillItemList = new();
    public override void OnShow()
    {
        base.OnShow();
        SelfBf = BattleManager.SelfBf;
        ShowUI<UIBattleMomentPanel>();
        ShowUI<UIBattleSettlementPanel>();
    }

    public void Update()
    {
        if (SubjectID != BattleLogicStateManager.GetActionSubjectID)
        {
            SubjectID = BattleLogicStateManager.GetActionSubjectID;
            var (skillID, variantID) = Util.UnCombSkillGuid(BattleLogicStateManager.GetSelectSkillGuid);
            var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(SubjectID);
            if (behaviour != null)
            {
                TxtSubject.SetText( $"行动人 : {behaviour.SubjectID}");
                TxtSkillID.SetText($"技能ID : {behaviour.SkillID}");
                TxtTarget.SetText($"目标 : {behaviour.TargetID}");
            }
            else
            {
                TxtSubject.SetText( $"行动人 : {SubjectID}");
                TxtSkillID.SetText($"技能ID : {skillID}");
                TxtTarget.SetText($"目标 : 0");
            }

            var unit = BattleManager.GetUnit(SubjectID);
            var skills = unit.TakeSkillDataManager.GetTakeSkillData();
            CreateUIComponents(SkillItemList, skills.Count, TfRightMenu);
            for (int i = 0; i < skills.Count; i++)
            {
                SkillItemList[i].Refresh(skills[i].SkillID);
            }
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
        TxtState.SetText($"回合 : {BattleLogicStateManager.Round}, 当前息 : {BattleLogicStateManager.ActionWheel}");
    }

    private void OnRefreshActionWheelView(RefreshActionWheelViewEventModel model)
    {
        TxtState.SetText($"回合 : {BattleLogicStateManager.Round}, 当前息 : {BattleLogicStateManager.ActionWheel}");
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
        }
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
