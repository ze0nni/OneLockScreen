using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocketScreenDotComponent : MonoBehaviour
{
    public Sprite CommonDot;
    public Sprite ActiveDot;
    public Sprite RejectedDot;
}
    public bool State { get; private set; }

    private Image image;

    void Start() {
        this.image = GetComponent<Image>();

        UpdateState(false);
    }

    public void UpdateState(bool value) {
        this.image.sprite = value
            ? this.ActiveDot
            : this.CommonDot
            ;

        this.State = value;
    }
}