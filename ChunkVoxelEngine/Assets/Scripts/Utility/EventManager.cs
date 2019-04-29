using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

/// <summary>
/// 단순 이벤트 전달 - 파라미터가 오브젝트 배열 형태이므로 이벤트 타입을 지정할 때 파라미터 명시 필요
/// </summary>
public class EventManager : GameObjectSingleton<EventManager>
{
    
    public enum EVENTTYPE
    {
        ClickModeChange,
        PointEnterToUI,
        PointExitToUI,
        PointUpToUI,
        PointDownToUI,
    }


    public delegate void EventDelegate(EVENTTYPE type, params object[] pars);


    public static void RegisterListener(EVENTTYPE type, EventDelegate listener)
    {
        if (_inst.eventDelegates.ContainsKey(type))
        {
            if (Array.Exists(_inst.eventDelegates[type].GetInvocationList(), p => p.Equals(listener))) // == listener))
            {
                Debug.LogError("Already registered listener : " + listener.Method.ReflectedType.Name + "." + listener.Method.Name);
                return;
            }

            _inst.eventDelegates[type] += listener;
        }
        else
            _inst.eventDelegates[type] = listener;
    }

    public static void UnregisterListener(EVENTTYPE type, EventDelegate listener)
    {
        EventDelegate evt_listener;

        if (_inst == null)
            return;

        if (_inst.eventDelegates.ContainsKey(type))
        {
            evt_listener = _inst.eventDelegates[type];
            evt_listener -= listener;
            if (evt_listener == null)
                _inst.eventDelegates.Remove(type);
            else
                _inst.eventDelegates[type] = evt_listener;
        }
    }

    public static void UnregisterListener(EventDelegate listener)
    {
        if (_inst == null)
            return;

        EVENTTYPE[] array = new EVENTTYPE[_inst.eventDelegates.Count];
        _inst.eventDelegates.Keys.CopyTo(array, 0);

        for (int i = 0; i < array.Length; i++)
        {
            _inst.eventDelegates[array[i]] -= listener;
        }
    }

    
    public static void InvokeEvent(EVENTTYPE type, params object[] pars)
    {
        if (_inst.eventDelegates.ContainsKey(type))
        {
            if (_inst.eventDelegates[type] != null)
            {
                // 리스너를 넣었다가 빼면 키는 존재하지만 null이 됨.
                _inst.eventDelegates[type](type, pars);
            }
        }
    }

    /////////////////////////////////////////////////////////////////
    // private
    private Dictionary<EVENTTYPE, EventDelegate> eventDelegates;

    protected override void OnAttached()
    {
        this.eventDelegates = new Dictionary<EVENTTYPE, EventDelegate>(new EventComparer());
    }

    private class EventComparer : IEqualityComparer<EVENTTYPE>
    {
        public bool Equals(EVENTTYPE t1, EVENTTYPE t2)
        {
            return t1 == t2;
        }

        public int GetHashCode(EVENTTYPE t)
        {
            return t.GetHashCode();
        }
    }
}

