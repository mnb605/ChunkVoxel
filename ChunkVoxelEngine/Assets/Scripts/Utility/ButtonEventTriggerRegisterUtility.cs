using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventTriggerRegisterUtility {
    #region EventTrigger Area
    /// <summary>
    /// 버튼에 이벤트트리거 등록
    /// </summary>
    /// <param name="btn"></param>
    public static void ButtonInit(Button btn)
    {
        EventTrigger trigger;

        if (btn.gameObject.GetComponent<EventTrigger>())
        {
            trigger = btn.gameObject.GetComponent<EventTrigger>();
        }
        else
        {
            trigger = btn.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDownCallback((PointerEventData)data, (GameObject)btn.gameObject); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnPointerUpCallback((PointerEventData)data, (GameObject)btn.gameObject); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnPointerEnterCallback((PointerEventData)data, (GameObject)btn.gameObject); });
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((data) => { OnPointerExitCallback((PointerEventData)data, (GameObject)btn.gameObject); });
            trigger.triggers.Add(entry);
        }
    }

    private static void OnPointerDownCallback(PointerEventData data, GameObject g)
    {
        g.GetComponent<Button>().targetGraphic.color = g.GetComponent<Button>().colors.pressedColor;
    }

    private static void OnPointerUpCallback(PointerEventData data, GameObject g)
    {
        g.GetComponent<Button>().targetGraphic.color = g.GetComponent<Button>().colors.normalColor;
        g.GetComponent<Button>().onClick.Invoke();
    }

    private static void OnPointerEnterCallback(PointerEventData data, GameObject g)
    {
        g.GetComponent<Button>().targetGraphic.color = g.GetComponent<Button>().colors.highlightedColor;
    }

    private static void OnPointerExitCallback(PointerEventData data, GameObject g)
    {
        g.GetComponent<Button>().targetGraphic.color = g.GetComponent<Button>().colors.normalColor;
    }
    #endregion
}
