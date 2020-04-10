using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocketScreeRopeComponent : MonoBehaviour
{
    #region Inner classes 


    class Segment
    {
        readonly public LocketScreenDotComponent dot;
        readonly public GameObject rope;

        public Segment(LocketScreenDotComponent dot, GameObject rope)
        {
            this.dot = dot;
            this.rope = rope;

            Move(0, 0);
        }

        public void Move(LocketScreenDotComponent dot)
        {
            var dotTransform = dot.GetComponent<RectTransform>();
            Move(dotTransform.position.x, dotTransform.position.y);
        }

        public void Move(float x, float y) {
            var dotTransform = dot.GetComponent<RectTransform>();
            var ropeTransform = rope.GetComponent<RectTransform>();

            var dotCenter = dotTransform.position;

            ropeTransform.position = dotCenter;

            ropeTransform.transform.rotation = Quaternion.Euler(
                0,
                0,
                Mathf.Atan2(y - dotCenter.y, x- dotCenter.x) * Mathf.Rad2Deg
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

        public void Remove() {
            dot.UpdateState(false);
            DestroyObject(rope);
        }
    }

#endregion

    public GameObject RopePrefab;

    private Stack<Segment> segments = new Stack<Segment>();

    private void Update()
    {
        if (segments.Count == 0) {
            return;
        }

        if (!Input.GetMouseButton(0)) {
            ClearSegments();

            return;
        }

        var segment = segments.Peek();

        //Looks like canvas and window coordinates always match
        segment.Move(
            Input.mousePosition.x,
            Input.mousePosition.y
        );
    }

    internal void DotEventHandler(LocketScreenDotComponent dot)
    {
        if (!Input.GetMouseButton(0)) {
            return;
        }

        dot.UpdateState(true);

        PushSegment(dot);
    }

    private Segment PushSegment(LocketScreenDotComponent dot) {
        if (segments.Count != 0) {
            var topSegment = segments.Peek();
            topSegment.Move(dot);
        }
        var dotTransform = dot.GetComponent<RectTransform>();

        var ropeGo = Instantiate(RopePrefab);
        ropeGo.transform.SetParent(this.transform);

        var ropeTransform = ropeGo.GetComponent<RectTransform>();
        ropeTransform.position = dotTransform.position;

        var segment = new Segment(dot, ropeGo);
        segments.Push(segment);

        return segment;
    }

    private void ClearSegments() {
        while (segments.Count != 0) {
            segments.Pop().Remove();
        }
    }
}
