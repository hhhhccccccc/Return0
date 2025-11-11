using System.Collections.Generic;
using cfg;

public interface IBattlePropertyChanged
{
    /// <summary>
    /// 获取威力改变
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetAddWellyRate(int skillGuid);
    /// <summary>
    /// 获取威力改变效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetAddWellyEffect(int skillGuid);
    /// <summary>
    /// 尝试设置基础威力
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetBaseWellyRate(int skillGuid, ref float value);
    /// <summary>
    /// 尝试设置威力增长
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetAddWellyRate(int skillGuid, ref float value);
}
