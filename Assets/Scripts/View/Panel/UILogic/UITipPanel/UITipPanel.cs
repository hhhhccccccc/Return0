using TMPro;
using UnityEngine;

public partial class UITipPanel
{
    // 保存当前正在执行的动画ID
    private LTDescr currentMoveTween;
    private LTDescr currentAlphaTween;
    
    public void ShowTip(string tip)
    {
        TxtTip.SetText(tip);
        PlayAnimation();
    }
    
    void PlayAnimation()
    {
        // 如果上一次动画还在播放，先停掉
        StopCurrentAnimation();
        
        // 记录初始位置和透明度
        float startY = TxtTip.rectTransform.anchoredPosition.y;
        float startAlpha = TxtTip.color.a;
        
        // 移动动画：向上200像素
        currentMoveTween = LeanTween.moveY(TxtTip.rectTransform, startY + 200, 1f)
            .setOnComplete(() => {
                // 动画完成后清空引用
                currentMoveTween = null;
                Close();
            });
        
        // 淡出动画：从当前透明度渐变到0
        currentAlphaTween = LeanTween.alpha(TxtTip.rectTransform, 0f, 1f)
            .setOnComplete(() => {
                currentAlphaTween = null;
            });
    }
    
    void StopCurrentAnimation()
    {
        // 停止移动动画
        if (currentMoveTween != null)
        {
            LeanTween.cancel(currentMoveTween.id);
            currentMoveTween = null;
        }
        
        // 停止淡出动画
        if (currentAlphaTween != null)
        {
            LeanTween.cancel(currentAlphaTween.id);
            currentAlphaTween = null;
        }
        
        // 重置文字状态到初始值（可选）
        ResetTextState();
    }
    
    void ResetTextState()
    {
        // 重置位置和透明度（恢复到动画开始前的状态）
        // 根据实际需求调整这些值
        TxtTip.rectTransform.anchoredPosition = new Vector2(
            TxtTip.rectTransform.anchoredPosition.x, 
            0  // 假设初始Y=0
        );
        
        Color color = TxtTip.color;
        color.a = 1f;
        TxtTip.color = color;
    }

    protected override void OnPanelDestroy()
    {
        StopCurrentAnimation();
    }
}
