using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ControllerLaserPoint : MonoBehaviour {
    [SerializeField]
    [Range(1, 20)]
    private float MaxDistance;
    [SerializeField]
    private Transform pointer;

    private float distance;
    private RaycastHit hit;
    private bool pointerHoverToCollider;
    
    bool isHover = false;
    LineRenderer lineRenderer;

    public bool GetIsPointerHover
    {
        get { return pointerHoverToCollider; }
    }

    public RaycastHit GetRaycast
    {
        get { return hit; }
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        pointerHoverToCollider = pointerRayCast();

        if (Input.GetMouseButtonDown(0) && CheckUIInteraction())
        {
            
            hit.transform.GetComponent<Button>().onClick.Invoke();
        }
    }

    bool pointerRayCast()
    {
        Vector3 pointerPos;
        isHover = Physics.Raycast(transform.position, transform.up, out hit, MaxDistance);
        if (isHover)
        {
            distance = Mathf.Clamp(Vector3.Distance(transform.position, hit.point), 0, MaxDistance);
            Vector3 direction = (hit.point - transform.position).normalized;
            pointerPos = hit.point - direction;
        }
        else
        {
            distance = MaxDistance;
            Vector3 direction = transform.up;
            pointerPos = transform.position + direction * MaxDistance;
        }
        SetLinePosition(pointerPos);
       
        if (CheckUIInteraction())
        {
            EventManager.InvokeEvent(EventManager.EVENTTYPE.PointEnterToUI, hit.transform.gameObject);
            return false;
        }
        return true;
    }

    bool CheckUIInteraction()
    {
        return (hit.transform != null && hit.transform.tag == "UI");
    }

    void SetLinePosition(Vector3 position)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, position);
        SetPointerPosition(position);
    }

    void SetPointerPosition(Vector3 position)
    {
        pointer.transform.position = position;
        SetPointerScale();
    }

    void SetPointerScale()
    {
        float minimumDistance = 1.0f;
        float maximumDistance = 20.0f;

        float minimumDistanceScale = 0.1f;
        float maximumDistanceScale = 1.0f;

        float norm = (distance - minimumDistance) / (maximumDistance - minimumDistance);
        norm = Mathf.Clamp01(norm);

        Vector3 minScale = Vector3.one * maximumDistanceScale;
        Vector3 maxScale = Vector3.one * minimumDistanceScale;

        pointer.transform.localScale = Vector3.Lerp(maxScale, minScale, norm);
    }
}
