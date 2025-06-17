using Zenject;

public abstract class BattleTreasureMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;

    private BattleTreasureBase Model;

    public void InitMoment(BattleTreasureBase model)
    {
        Model = model;
    }

    public void BattleStart()
    {
        var momentID = Model.Cfg.BattleStartMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void RoundStart()
    {
        var momentID = Model.Cfg.RoundStartMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void CalculateActionWheel()
    {
        var momentID = Model.Cfg.CalculateActionWheelMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }

    public void DoDesitionAction()
    {
        var momentID = Model.Cfg.DoDesitionMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
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
        var momentID = Model.Cfg.RoundEndMoment;
        var subjectID = Model.Subject.EntityID;
        BattleMomentManager.TriggerMoment(momentID, subjectID);
    }
}