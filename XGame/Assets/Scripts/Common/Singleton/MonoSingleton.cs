using UnityEngine;
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T mInstance = null;
    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(T)) as T;
                if (mInstance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    mInstance = go.AddComponent<T>();
                    GameObject parent = GameObject.Find("root");
                    if (parent == null)
                    {
                        parent = new GameObject("root");
                        DontDestroyOnLoad(parent);
                    }
                    if (parent != null)
                    {
                        go.transform.parent = parent.transform;
                    }
                }
            }

            return mInstance;
        }
    }
    public void Startup()
    {

    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as T;
        }
        DontDestroyOnLoad(gameObject);
        Init();
    }

    protected virtual void Init()
    {

    }

    public void DestroySelf()
    {
        Dispose();
        mInstance = null;
        Destroy(gameObject);
    }

    public virtual void Dispose()
    {

    }

}