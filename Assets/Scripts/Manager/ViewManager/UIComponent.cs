using cfg;
using UnityEngine;

public class UIComponent : View
{
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    
    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
