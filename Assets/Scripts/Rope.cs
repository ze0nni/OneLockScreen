using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Rope : MonoBehaviour
{
    enum RopeState {
        // Wait for user interact
        Ready,
        // Will rejected for next update
        Rejected,
        // Will resolved for next update
        Resolved,
        // Wait for Resolve() or Reject()
        Completed
    }

    public GameObject RopePrefab;

    private RopeState state = RopeState.Ready;
    private Stack<RopeSegment> segments = new Stack<RopeSegment>();

    readonly private Subject<int[]> passwordSubject = new Subject<int[]>();
    public IObservable<int[]> password { get => passwordSubject; }
    readonly private Subject<int[]> currentPasswordSubject = new Subject<int[]>();
    public IObservable<int[]> currentPassword { get => currentPasswordSubject; }

    private RectTransform rect;

    private void Start()
    {
        this.rect = GetComponent<RectTransform>();

        UpdatePassword();
    }

    private void Update()
    {
        if (this.state == RopeState.Ready && Input.GetMouseButtonUp(0))
        {
            this.state = RopeState.Completed;

            UpdatePassword();
            
            return;
        }
    
        if (this.state == RopeState.Rejected) {
            DoReject();
            this.state = RopeState.Ready;
        }

        if (this.state == RopeState.Resolved)
        {
            DoResole();
            this.state = RopeState.Ready;
        }

        if (this.state == RopeState.Ready && segments.Count > 0)
        {
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
    }

    internal void OnDotInteract(Dot dot)
    {
        if (this.state != RopeState.Ready) {
            return;
        }
        if (!Input.GetMouseButton(0)) {
            return;
        }

        dot.UpdateState(true);

        PushSegment(dot);
    }

    private RopeSegment PushSegment(Dot dot) {
        if (segments.Count != 0) {
            var topSegment = segments.Peek();
            topSegment.Move(dot);
        }
        var ropeSegmentGo = Instantiate(RopePrefab, this.transform);

        var ropeSegment = ropeSegmentGo.GetComponent<RopeSegment>();
        ropeSegment.dot = dot;

        segments.Push(ropeSegment);

        UpdateCurrentPassword();

        return ropeSegment;
    }

    private void ClearSegments() {
        while (segments.Count != 0) {
            segments.Pop().Remove();
        }
        UpdatePassword();
    }

    public void Reject() {
        // Its wrong to call passwordSubject.OnNext here
        if (this.state == RopeState.Completed) {
            this.state = RopeState.Rejected;
        }
    }

    // Call only from update
    private void DoReject() {
        while (segments.Count != 0)
        {
            segments.Pop().Remove(false);
        }
        UpdatePassword();
    }

    public void Resolve()
    {
        // Its wrong to call passwordSubject.OnNext here
        if (this.state == RopeState.Completed)
        {
            this.state = RopeState.Resolved;
        }
    }

    // Call only from update
    private void DoResole()
    {
        while (segments.Count != 0)
        {
            segments.Pop().Remove(true);
        }
        UpdatePassword();
    }

    //

    void UpdatePassword() {
        passwordSubject.OnNext(
            segments.Select(s => s.dot.DotIndex).Reverse().ToArray()
        );
        UpdateCurrentPassword();
    }

    void UpdateCurrentPassword() {
        currentPasswordSubject.OnNext(
            segments.Select(s => s.dot.DotIndex).Reverse().ToArray()
        );
    }
}
