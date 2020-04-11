using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dot
    : MonoBehaviour
    , IPointerEnterHandler
    , IPointerDownHandler
{
    public Sprite CommonDot;
    public Sprite ActiveDot;
    public GameObject ResolvedDotPrefab;
    public GameObject RejectedDotPrefab;

    public int DotIndex;
    public bool State { get; private set; }

    public UnityAction<Dot> OnInteract;

    private Image image;

    void Start() {
        this.image = GetComponent<Image>();

        UpdateState(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!State)
        {
            OnInteract?.Invoke(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var transform = GetComponent<RectTransform>();

        if (!State)
        {
            OnInteract?.Invoke(this);
        }
    }

    public void UpdateState(bool value) {
        this.image.sprite = value
            ? this.ActiveDot
            : this.CommonDot
            ;

        this.State = value;
    }

    public void FlyAwayEffect(bool successed) {
        var rejectetDot = Instantiate(
            successed ?  ResolvedDotPrefab : RejectedDotPrefab, 
            transform.parent);

        var dotTransform = this.GetComponent<RectTransform>();
        var rejectetDotTransform = rejectetDot.GetComponent <RectTransform> ();

        rejectetDotTransform.localPosition = dotTransform.localPosition;
    }
}