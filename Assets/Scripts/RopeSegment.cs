using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public Dot dot;

    private void Start()
    {
        Move(dot);
    }

    public void Move(Dot dot)
    {
        var dotTransform = dot.GetComponent<RectTransform>();
        Move(dotTransform.localPosition.x, dotTransform.localPosition.y);
    }

    public void Move(float x, float y)
    {
        var dotTransform = dot.GetComponent<RectTransform>();
        var ropeTransform = this.GetComponent<RectTransform>();

        var dotCenter = dotTransform.localPosition;

        ropeTransform.localPosition = dotCenter;

        ropeTransform.transform.rotation = Quaternion.Euler(
            0,
            0,
            Mathf.Atan2(y - dotCenter.y, x - dotCenter.x) * Mathf.Rad2Deg
        );

        var distance = Vector2.Distance(dotCenter, new Vector2(x, y));
        ropeTransform.sizeDelta = new Vector2(
            distance + ropeTransform.rect.height,
            ropeTransform.rect.height
        );

        ropeTransform.pivot = new Vector2(
            (ropeTransform.rect.height / ropeTransform.rect.width) * 0.5f,
            0.5f
        );
    }

    public void Remove()
    {
        dot.UpdateState(false);

        DestroyObject(gameObject);
    }

    public void Remove(bool successed)
    {
        dot.UpdateState(false);
        dot.FlyAwayEffect(successed);

        DestroyObject(gameObject);
    }
}
