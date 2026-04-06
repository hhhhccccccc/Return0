using System.Collections.Generic;
using cfg;

public class BattleHeartMethod10110 : BattleHeartMethodBase
{
    public bool CanTrigger { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void AfterChangeKey(List<BattleKey> changeKeyList, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (Subject.GetAllKeyCount() <= 0)
        {
            var max = Subject.GetKeyPropertyMax();
            var addKeyList = Subject.AddRandomKey(max, ChangeKeyReason.HeartMethodEffect);
            if (addKeyList is { Count: > 0 })
            {
                var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey);
                viewModel.AddKeyList(addKeyList);
                EnqueueViewModel(viewModel);
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}