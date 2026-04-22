using System;
using UnityEngine;
using UnityEngine.UI;

public class EventItem<T> : Item
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

    public void BindEvent(Action<T> action)
    {
        Action = action;
        Debug("sss");
        if (gameObject.GetComponent<CanvasRenderer>() == null)
        {
            gameObject.AddComponent<CanvasRenderer>();
        }
        
        if (gameObject.GetComponent<GraphV2>() == null)
        {
            gameObject.AddComponent<GraphV2>();
        }
        
        if (gameObject.GetComponent<UIButton>() == null)
        {
            gameObject.AddComponent<UIButton>();
        }

        if (Button == null)
        {
            Button = gameObject.GetComponent<UIButton>();
            Button.transition = Selectable.Transition.None;
            Button.onClick.AddListener(ClickEvent);
        }
    }
}
