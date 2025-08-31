
using UnityEngine;

public class UnitMoveTween : TweenBase
{
    public UnitMoveTween(GameObject go) : base(go)
    {
        
    }

    public void Play(Vector3 pos, float time)
    {
        if (LTDescr != null)
        {
            LTDescr.pause();
            LTDescr = null;
        }
    
        LTDescr = LeanTween.move(Go, pos, 0.5f)
            .setOnComplete(() =>
            {
                LTDescr = null; 
            });
    }
}
