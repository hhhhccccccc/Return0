public partial class UIDebugPanel
{
    private BattleViewSelectData BattleViewSelectData { get; set; }

    public void SetDebugInfo(BattleViewSelectData info)
    {
        BattleViewSelectData = info;
        InputSelfID.text = BattleViewSelectData.SelectID.ToString();
    }
    private void OnBtnConfirm()
    {
        BattleViewSelectData.SelectID = int.Parse(InputSelfID.text);
        Close();
    }
}
