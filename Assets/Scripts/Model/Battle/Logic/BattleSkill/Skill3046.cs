using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill3046 : BattleSkillBase
{
    //todo 行动期间不受异常状态的影响
    
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }

    //消耗2个→键和1个←键
    protected override void OnSelfActionWheelStart()
    {
        var removeKeyList = new List<BattleKeyType>
        {
            BattleKeyType.KeyRight,
            BattleKeyType.KeyRight,
            BattleKeyType.KeyLeft,
        };
        DoChangeKeyList(Subject, removeKeyList, false, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }

    //获得4层刚屏
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGangPing, Subject, 4, null, BattleMomentType.AfterAction);
    }
}