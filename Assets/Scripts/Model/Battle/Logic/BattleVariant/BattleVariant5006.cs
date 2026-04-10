using System;
using cfg;

public class BattleVariant5006 : BattleVariantBase
{
    //行动期间防 额外增加60+GR*6
    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (Skill.IsInAction)
        {
            if (propertyType == BattlePropertyType.DefendInt)
            {
                return 60 + Subject.Gr * 6;
            }
        }

        return 0;
    }

    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
