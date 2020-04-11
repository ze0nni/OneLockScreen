using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Rope))]
public class CurrentPasswordText : MonoBehaviour
{
    public Text text;

    private IDisposable subscribe;

    void OnEnable() {
        var rope = GetComponent<Rope>();

        this.subscribe = rope.currentPassword.Subscribe(p =>
        {
            text.text = string.Join("-", p);
        });
    }

    void OnDisable()
    {
        subscribe.Dispose();
    }
}
