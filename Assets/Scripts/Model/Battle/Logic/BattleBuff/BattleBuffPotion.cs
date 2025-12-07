using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuffPotion : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        base.OnBuffStart();
        Subject.AddPotion(BuffID);
    }

    protected override void OnBuffRemove()
    {
        Subject.TryRemovePotion(BuffID);
        base.OnBuffRemove();
    }
}
