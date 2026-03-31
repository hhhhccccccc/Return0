using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveKey : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private ConfigHelper ConfigHelper { get; set; }

    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var list = new List<BattleKeyType>();
            for (int i = 1; i < Config.ParamList.Count; i++)
            {
                list.Add((BattleKeyType)Config.ParamList[i].ToInt());
            }
            foreach (var target in targetList)
            {
                target.ChangeKeyList(list, false, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
            }
        }
    }
}