using Zenject;

public partial class UIBattlePanel : Panel
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    private BattleField SelfBf;
    public override void OnShow(params object[] args)
    {
        base.OnShow(args);
        SelfBf = BattleManager.SelfBf;
    }

    public void Update()
    {
        var subjectID = BattleLogicStateManager.GetActionSubjectID;
        var skillID = BattleLogicStateManager.GetSelectSkillID;
      
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
    
    private void OnBtnSkill1()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 1);
    }
    
    private void OnBtnSkill2()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 2);
    }

    private void OnBtnSkill3()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 3);
    }
    
    private void OnBtnSkill4()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, 4);
    }

    private void OnBtnCancel()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Cancel);
    }

}
