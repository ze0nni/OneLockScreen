using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegmentComponent : MonoBehaviour
{
    public DotComponent dot;

    private void Start()
    {
        var dotTransform = dot.GetComponent<RectTransform>();

        Move(dotTransform.position.x, dotTransform.position.y);
    }

    public void Move(DotComponent dot)
    {
        var dotTransform = dot.GetComponent<RectTransform>();
        Move(dotTransform.position.x, dotTransform.position.y);
    }

    public void Move(float x, float y)
    {
        var dotTransform = dot.GetComponent<RectTransform>();
        var ropeTransform = this.GetComponent<RectTransform>();

        var dotCenter = dotTransform.position;

        ropeTransform.position = dotCenter;

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
}
