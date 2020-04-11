﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(RopeComponent))]
public class LockScreenComponent : MonoBehaviour
{
    public GameObject DotPrefab;
    public float DotsDistance = 120f;

    readonly private int size = 3;

    private RopeComponent rope;

    void Start() {
        this.rope = GetComponent<RopeComponent>();

        var dotsOrigin = new Vector2(
            -((size - 1) * DotsDistance) / 2f,
            -((size - 1) * DotsDistance) / 2f
        );

        var dotIndex = 0;
        for (var x = 0; x < size; x ++) {
            for (var y = 0; y < size; y++) {
                var dotGo = Instantiate(DotPrefab);
                dotGo.transform.SetParent(this.transform, false);

                var dot = dotGo.GetComponent<DotComponent>();
                dot.DotIndex = dotIndex;
                dot.OnInteract += this.rope.OnDotInteract;

                var transform = dotGo.GetComponent<RectTransform>();
                transform.localPosition = new Vector2(x, y) * DotsDistance + dotsOrigin;

                dotIndex++;
            }
        }
    }
}
