using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3021 : BattleSkillBase
{
    private bool IsInitAddWelly;
    private bool IsSuccess;
    protected override bool CheckSkillAttackAddWelly(MomentParamModel paramModel)
    {
        if (!IsInitAddWelly)
        {
            RandomSuccess();
        }
        
        return IsSuccess;
    }

    private void RandomSuccess()
    {
        IsInitAddWelly = true;
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var hpMax = Subject.GetProperty(BattlePropertyType.MaxHp);
        IsSuccess = Util.GetRandomFloat(0, 1) <= (1 - hp / hpMax);
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        IsInitAddWelly = false;
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        IsInitAddWelly = false;
    }
    
    public override void Recycle()
    {
        base.Recycle();
        IsInitAddWelly = false;
        IsSuccess = false;
    }
}