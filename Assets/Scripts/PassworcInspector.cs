using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using MoonSharp.Interpreter;

public class PassworcInspector : MonoBehaviour
{
    private static readonly string ResolverFuncBody = @"
        return function(reject, resolve)
            
            local password = { 8, 5, 2, 1, 0, 3, 6, 7 }

            return function(value)
                if #value > #password then
                    reject()
                    return
                end

                for i=1,#value do
                    if value[i] != password[i] then
                        reject()
                        return
                    end
                end

                if #value == #password then
                    resolve()
                else 
                    reject()
                end
            end
        end
    ";

    public Rope rope;

    private Script script;
    private DynValue ResolverFunc;

    private IDisposable subscribe;

    void OnEnable() {
        this.script = new Script();
        this.ResolverFunc = script.Call(script.DoString(ResolverFuncBody),
            (Action)rope.Reject,
            (Action)rope.Resolve
        );


        this.subscribe = rope.password.Subscribe(PasswordHandler);
    }
    
    void OnDisable() {
        subscribe.Dispose();
    }

    void PasswordHandler(int[] password) {
        script.Call(ResolverFunc, password);
    }
}
