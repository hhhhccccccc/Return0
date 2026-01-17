using System.Collections.Generic;
using cfg;

public class BattleMomentViewModel : IModel
{
    public BattleSource BattleSource { get; set; }
    public int ConfigID { get; set; }
    //后面参数在逻辑层传出来
    public int EntityID { get; set; }
    public int AddSkillDamageRate { get; set; }
    public List<int> Params { get; set; } = new();
}
