using Zenject;

public class BattleSkillMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;
    
    private BattleSkillBase Model;

    protected void InitMoment(BattleSkillBase model)
    {
        Model = model;
    }

    public void BattleStart()
    {
        
    }

    public void RoundStart()
    {
       
    }

    public void CalculateActionWheel()
    {
        
    }

    public void DoDesitionAction()
    {
        var momentID = Model.Cfg.DoDesitionMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);;
    }

    public void StartActionWheel()
    {
        var momentID = Model.Cfg.StartActionWheelMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }
    
    public void AsTargetAction(bool fromIsTeam, int skillID)
    {
        var momentID = Model.Cfg.AsTargetActionMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void ReleaseSkillAction()
    {
        var momentID = Model.Cfg.ReleaseSkillActionMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void BeforeClash()
    {
        var momentID = Model.Cfg.BeforeClashMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void UnderHit()
    {
        var momentID = Model.Cfg.UnderHitMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }
    
    public void AfterClash()
    {
        var momentID = Model.Cfg.AfterClashMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }
    
    public void AfterAction()
    {
        var momentID = Model.Cfg.AfterActionMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void RoundEnd()
    {
       
    }
}