using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DotFlyAwayEffect : MonoBehaviour
{
    public float LifeTime = 0.5f;

    private Image image;
    private float timeLeft;

    private void Start()
    {
        this.image = GetComponent<Image>();
        this.timeLeft = this.LifeTime;
    }

    void Update()
    { 
        this.timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            DestroyObject(gameObject);
            return;
        }

        var rect = GetComponent<RectTransform>();
        var ratio = 1f - (timeLeft / LifeTime);

        rect.localScale = new Vector2(
            1f + ratio,
            1f + ratio
        );

        var color = image.color;
        color.a = 1 - ratio;
        image.color = color;
    }
}
