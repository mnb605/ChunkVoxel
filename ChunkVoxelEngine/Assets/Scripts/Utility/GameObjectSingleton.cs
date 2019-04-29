using UnityEngine;
using System.Collections;

public class GameObjectSingleton<T> : MonoBehaviour where T : GameObjectSingleton<T>
{
    public static bool Loaded
    {
        get
        {
            return __inst != null && __inst.Valid;
        }
    }

    public static T GetInstance()
    {
        return __inst;
    }


    public static T Instance
    {
        get { return __inst; }
    }

    protected static T _inst
    {
        get
        {
            return __inst;
        }
    }

    /////////////////////////////////////////////////////////////////
    // instance member

    protected virtual bool Valid
    {
        get
        {
            return __inst != null;
        }
    }

    protected virtual void Awake()
    {
        if (__inst != null)
        {
            Debug.Log(typeof(T).Name + " is already attached");
            return;
        }

        __inst = this as T;
        this.OnAttached();
    }

    protected virtual void OnDestroy()
    {
        if (object.ReferenceEquals(this, __inst))
        {
            __inst = null;
            this.OnDetached();
        }
    }

    protected virtual void OnAttached()
    {

    }

    protected virtual void OnDetached()
    {

    }

    public static T Create(string strObjectName)
    {
        if (Loaded)
        {
            return __inst;
        }

        GameObject newObject = new GameObject(strObjectName);
        T newComponent = newObject.AddComponent<T>();
        if (null == newComponent)
        {

        }

        return newComponent;
    }

    /////////////////////////////////////////////////////////////////
    // private
    private static T __inst = null;
}

