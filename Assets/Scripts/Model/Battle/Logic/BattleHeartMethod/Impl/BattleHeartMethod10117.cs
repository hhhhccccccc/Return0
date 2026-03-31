using cfg;

//todo 表现
public class BattleHeartMethod10117 : BattleHeartMethodBase
{
   public override bool CheckDontBeCounter(MomentParamModel paramModel)
   {
      var skill = Subject.GetSkill();
      if (skill == null)
      {
         return false;
      }

      if (skill.GetSKillType != SkillType.PowerKilling)
      {
         return false;
      }
      
      if (paramModel is DamageParamModel model)
      {
         if (model.BattleClashType == BattleClashType.SingleClash ||
             model.BattleClashType == BattleClashType.DoubleClash)
         {
            var otherTruthDamage = model.GetOtherAttackTruthDamageValue(Subject.EntityID);
            var other = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
            var damageRate = Subject.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
            var (selfTruthDamage, v2, v3, v4) = Subject.GetSkillDamageValue(other, DamageType.Direct, BattleSource.Skill, damageRate, model);
            if (selfTruthDamage > otherTruthDamage)
            {
               return true;
            }
         }
         
      }

      return false;
   }
}