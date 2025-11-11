using cfg;
using Zenject;

public class BattleMomentCondition_CheckBuffMechanism : BattleMomentCondition
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0].ToInt());
        if (target != null)
        {
            var relation = Config.ParamList[2].ToInt() == 1;
            if (relation)
            {
                return target.HasBuffMechanism((BuffMechanism)Config.ParamList[1].ToInt());
            }
            
            return !target.HasBuffMechanism((BuffMechanism)Config.ParamList[1].ToInt());
        }

        return false;
    }
}