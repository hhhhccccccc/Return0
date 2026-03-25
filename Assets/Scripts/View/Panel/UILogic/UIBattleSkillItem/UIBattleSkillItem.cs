using UnityEngine.UI;
using Zenject;

public partial class UIBattleSkillItem
{
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    public void Refresh(int skillID)
    {
        TxtName.SetText($"skillID: {skillID}");
        BindEvent((_) =>
        {
            BattleRenderManager.DispatchClickEventModel(BattleClickType.Skill, skillID);
        });
    }
}
