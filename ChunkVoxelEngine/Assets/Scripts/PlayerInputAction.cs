using UnityEngine;

public enum InputMode
{
    AddBlock = 0,
    ReplaceBlock = 1,
}

public class PlayerInputAction : MonoBehaviour {
    [SerializeField]
    private Transform PlayerHeadTrack;

    private InputMode curMode = InputMode.AddBlock;

    public InputMode GetCurrentMode
    {
        get { return curMode; }
    }

    private void Start()
    {
        EventManager.RegisterListener(EventManager.EVENTTYPE.ClickModeChange, EventListener);
    }

    private void OnDestroy()
    {
        EventManager.UnregisterListener(EventManager.EVENTTYPE.ClickModeChange, EventListener);
    }

    private void EventListener(EventManager.EVENTTYPE type, object[] pars)
    {
        switch (type)
        {
            case EventManager.EVENTTYPE.ClickModeChange:
                curMode = (InputMode)pars[0];
                break;
        }
    }

    private void Update()
    {
        //Debug.Log(Input.GetAxis("Mouse Y"));
        if (Input.GetAxis("Mouse Y") > 0.5f)
        {
            PlayerMoveFoward();
        }
        else if (Input.GetAxis("Mouse Y") < -0.5f)
        {
            PlayerMoveBackward();
        }
    }

    private void PlayerMoveFoward()
    {
        Vector3 direction = PlayerHeadTrack.forward.normalized;
        MoveFoward(direction);
    }

    private void PlayerMoveBackward()
    {
        Vector3 direction = -PlayerHeadTrack.forward.normalized;
        MoveFoward(direction);
    }

    void MoveFoward(Vector3 moveDirection)
    {
        //Vector3 direction = PlayerHeadTrack.forward.normalized;
        transform.Translate(moveDirection);
    }
}
