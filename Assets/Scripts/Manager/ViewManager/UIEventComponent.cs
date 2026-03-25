using System;
using UnityEngine;
using UnityEngine.UI;

public class UIEventComponent<T> : UIComponent
{
    private T Component { get; set; }
    private UIButton Button { get; set; }
    private Action<T> Action { get; set; }
    private void ClickEvent()
    {
        Action?.Invoke(Component);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Component = gameObject.GetComponent<T>();
    }

    protected void BindEvent(Action<T> action)
    {
        Action = action;
        
        if (gameObject.GetComponent<CanvasRenderer>() == null)
        {
            gameObject.AddComponent<CanvasRenderer>();
        }
        
        if (gameObject.GetComponent<Graphic>() == null)
        {
            gameObject.AddComponent<Graphic>();
        }
        
        if (gameObject.GetComponent<UIButton>() == null)
        {
            gameObject.AddComponent<UIButton>();
        }

        if (Button == null)
        {
            Button = gameObject.GetComponent<UIButton>();
            Button.onClick.AddListener(ClickEvent);
        }
    }
}
