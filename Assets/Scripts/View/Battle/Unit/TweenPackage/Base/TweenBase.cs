using UnityEngine;

public abstract class TweenBase
{
    protected LTDescr LTDescr;

    protected GameObject Go;

    public TweenBase(GameObject go)
    {
        Go = go;
    }
}
