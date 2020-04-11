﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DotComponent
    : MonoBehaviour
    , IPointerEnterHandler
    , IPointerDownHandler
{
    public Sprite CommonDot;
    public Sprite ActiveDot;
    public Sprite RejectedDot;

    public int DotIndex;
    public bool State { get; private set; }

    public UnityAction<DotComponent> OnInteract;

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
}