using UnityEngine;
using UnityEngine.UI;

public class ClickModeUI : MonoBehaviour {
    [SerializeField]
    private Button addBlockButton;
    [SerializeField]
    private Button replaceBlockButton;
    [SerializeField]
    private Text currentModeText;

    private void Start()
    {
        InitializingUI();
    }

    private void OnDestroy()
    {
        RemoveButtonEvent();
    }

    public void InitializingUI()
    {
        addBlockButton.onClick.AddListener(ChangeModeToAddblock);
        replaceBlockButton.onClick.AddListener(ChangeModeToReplaceblock);
        ButtonEventTriggerRegisterUtility.ButtonInit(addBlockButton);
        ButtonEventTriggerRegisterUtility.ButtonInit(replaceBlockButton);

        EventManager.RegisterListener(EventManager.EVENTTYPE.PointEnterToUI, UIEventListener);
    }

    public void RemoveButtonEvent()
    {
        addBlockButton.onClick.RemoveListener(ChangeModeToAddblock);
        replaceBlockButton.onClick.RemoveListener(ChangeModeToReplaceblock);
        EventManager.UnregisterListener(EventManager.EVENTTYPE.PointEnterToUI, UIEventListener);
    }

    private void ChangeModeToAddblock()
    {
        EventManager.InvokeEvent(EventManager.EVENTTYPE.ClickModeChange, InputMode.AddBlock);
        currentModeText.text = "AddBlock";
    }

    private void ChangeModeToReplaceblock()
    {
        EventManager.InvokeEvent(EventManager.EVENTTYPE.ClickModeChange, InputMode.ReplaceBlock);
        currentModeText.text = "ReplaceBlock";
    }

    private void UIEventListener(EventManager.EVENTTYPE type, object[] pars)
    {
        switch (type)
        {
            case EventManager.EVENTTYPE.PointEnterToUI:
                GameObject target = (GameObject)pars[0];

                if(CompareGOtoButton(target, addBlockButton))
                {
                    addBlockButton.GetComponent<Image>().color = addBlockButton.colors.highlightedColor;
                }
                else if (CompareGOtoButton(target, replaceBlockButton))
                {
                    replaceBlockButton.GetComponent<Image>().color = replaceBlockButton.colors.highlightedColor;
                }
                break;
        }
    }

    private bool CompareGOtoButton(GameObject target, Button uiButton)
    {
        bool isEqual = (target == uiButton.gameObject) ? true : false;

        return isEqual;
    }
}
