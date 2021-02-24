using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class AddressableDemo : MonoBehaviour
{
    public Transform parent;
    public void LocalStatic()
    {
        Addressables.InstantiateAsync("Assets/Prefabs/localstatic.prefab",parent);
    }
    public void LocalNonStatic()
    {
        Addressables.InstantiateAsync("Assets/Prefabs/localnonstatic.prefab", parent);
    }
    public void RemoteStatic()
    {
        Addressables.InstantiateAsync("Assets/Prefabs/remotestatic.prefab", parent);
    }
    public void RemoteNonStatic()
    {
        Addressables.InstantiateAsync("Assets/Prefabs/remotenonstatic.prefab", parent);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
