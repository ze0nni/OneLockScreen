using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class RopeComponent : MonoBehaviour
{
    enum RopeState {
        Ready,
        Rejected,
        Completed
    }

    public GameObject RopePrefab;

    private Stack<RopeSegmentComponent> segments = new Stack<RopeSegmentComponent>();
    private RopeState state = RopeState.Ready;
    // Stop interact until GetMouseButtonUp
    private bool stopForMouseInteract = false;

    readonly private Subject<int[]> passwordSubject = new Subject<int[]>();
    public IObservable<int[]> password { get => passwordSubject; }

    private RectTransform rect;

    private void Start()
    {
        this.rect = GetComponent<RectTransform>();

        UpdatePassword();
    }

    private void Update()
    {
        if (stopForMouseInteract && Input.GetMouseButtonUp(0)) {
            stopForMouseInteract = false;
        }

        if (this.state == RopeState.Rejected) {
            DoReject();
            this.state = RopeState.Ready;
        }

        if (segments.Count == 0) {
            return;
        }

        if (!Input.GetMouseButton(0)) {
            ClearSegments();

            return;
        }

        var segment = segments.Peek();
        
        //TIP: Pass null for 'camera'. May be problem in future
        var mousePosition = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            new Vector2(Input.mousePosition.x, Input.mousePosition.y),
            null,
            out mousePosition
        );

        segment.Move(
            mousePosition.x,
            mousePosition.y
        );
    }

    internal void OnDotInteract(DotComponent dot)
    {
        if (this.stopForMouseInteract || this.state != RopeState.Ready) {
            return;
        }

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

        UpdatePassword();

        return ropeSegment;
    }

    private void ClearSegments() {
        while (segments.Count != 0) {
            segments.Pop().Remove();
        }
        UpdatePassword();
    }

    public void Reject() {
        // Int wrong to call passwordSubject.OnNext here
        if (this.state == RopeState.Ready) {
            this.state = RopeState.Rejected;
        }
        stopForMouseInteract = true;
    }

    // Call only from update
    private void DoReject() {
        while (segments.Count != 0)
        {
            segments.Pop().Reject();
        }
        UpdatePassword();
    }

    //

    void UpdatePassword() {
        passwordSubject.OnNext(
            segments.Select(s => s.dot.DotIndex).Reverse().ToArray()
        );
    }
}
