public class BattleHeartMethod10065 : BattleHeartMethodBase
{
    private int MinChangeValue => GetConfigParamInt(0);
    private bool CanTrigger { get; set; }
    public override void DoDesitionAction(bool isPreDesition)
    {
        var skill = Subject.GetSkill();
        if (skill != null && isPreDesition)
        {
            CanTrigger = true;
            var preCalculate = Subject.PreChangeActionWheel;
            if (preCalculate < MinChangeValue)
            {
                var delta = MinChangeValue - preCalculate;
                DoChangeActionWheel(Subject, delta);
            }
        }
    }

    public override void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        if (changeActionWheel < MinChangeValue)
        {
            changeActionWheel = MinChangeValue;
        }
    }

    public override void ClearTempData()
    {
        CanTrigger = false;
    }
}