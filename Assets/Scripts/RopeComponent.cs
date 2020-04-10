using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeComponent : MonoBehaviour
{
    public GameObject RopePrefab;

    private Stack<RopeSegmentComponent> segments = new Stack<RopeSegmentComponent>();

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

    internal void OnDotInteract(DotComponent dot)
    {
        if (!Input.GetMouseButton(0)) {
            return;
        }

        dot.UpdateState(true);

        PushSegment(dot);
    }

    private RopeSegmentComponent PushSegment(DotComponent dot) {
        if (segments.Count != 0) {
            var topSegment = segments.Peek();
            topSegment.Move(dot);
        }
        var ropeSegmentGo = Instantiate(RopePrefab, this.transform);

        var ropeSegment = ropeSegmentGo.GetComponent<RopeSegmentComponent>();
        ropeSegment.dot = dot;

        segments.Push(ropeSegment);

        return ropeSegment;
    }

    private void ClearSegments() {
        while (segments.Count != 0) {
            segments.Pop().Remove();
        }
    }
}
