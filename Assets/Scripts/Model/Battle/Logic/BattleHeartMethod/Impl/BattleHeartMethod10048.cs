using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10048 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.CheckClashIsWin(Subject.EntityID))
            {
                var skill = Subject.GetSkill();
                if (skill != null)
                {
                    var target = skill.Target;
                    var removeKeyList = target.RemoveRandomKey(GetParamInt(0), ChangeKeyReason.HeartMethodEffect, ChangeKeyType.Remove);
                    if (removeKeyList != null)
                    {
                        var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.RemoveKey, target.EntityID);
                        viewModel.AddKeyList(removeKeyList);
                        EnqueueViewModel(viewModel);
                    }
                }
            }
        }
    }
}