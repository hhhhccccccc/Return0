using Zenject;

public class BattleBuffMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;

    private BattleBuffBase Model;
    public void InitMoment(BattleBuffBase model)
    {
        Model = model;
    }

    public void BattleStart()
    {
        var momentID = Model.Cfg.BattleStartMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void RoundStart()
    {
        var momentID = Model.Cfg.RoundStartMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void CalculateActionWheel()
    {
        var momentID = Model.Cfg.CalculateActionWheelMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void DoDesitionAction()
    {
        var momentID = Model.Cfg.DoDesitionMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void StartActionWheel()
    {
        var momentID = Model.Cfg.StartActionWheelMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }
    
    public void AsTargetAction(bool fromIsTeam, int skillID)
    {
        var momentID = Model.Cfg.AsTargetActionMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void ReleaseSkillAction()
    {
        var momentID = Model.Cfg.ReleaseSkillActionMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void BeforeClash()
    {
        var momentID = Model.Cfg.BeforeClashMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void UnderHit()
    {
        var momentID = Model.Cfg.UnderHitMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }
    
    public void AfterClash()
    {
        var momentID = Model.Cfg.AfterClashMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void AfterAction()
    {
        var momentID = Model.Cfg.AfterActionMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }

    public void RoundEnd()
    {
        var momentID = Model.Cfg.RoundEndMoment;
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.Spellcaster?.EntityID ?? 0;
        BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID);
    }
}
