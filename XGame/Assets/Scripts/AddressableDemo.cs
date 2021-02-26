using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableDemo : MonoBehaviour
{
    public Transform parent;
    public List<object> _updateKeys;
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
    public async void UpdateCatalog()
    {

        //开始连接服务器检查更新
        var handle = Addressables.CheckForCatalogUpdates(false);
        await handle.Task;
        Debug.Log("check catalog status " + handle.Status);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> catalogs = handle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                foreach (var catalog in catalogs)
                {
                    Debug.Log("catalog  " + catalog);
                }
                Debug.Log("download catalog start ");
                var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;
                foreach (var item in updateHandle.Result)
                {
                    Debug.Log("catalog result " + item.LocatorId);
                    foreach (var key in item.Keys)
                    {
                        Debug.Log("catalog key " + key);
                    }
                    _updateKeys.AddRange(item.Keys);
                }
                Debug.Log("download catalog finish " + updateHandle.Status);
            }
            else
            {
                Debug.Log("dont need update catalogs");
            }
        }
        Addressables.Release(handle);
    }
    public IEnumerator DownAssetImpl()
    {
        var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
        yield return downloadsize;
        Debug.Log("start download size :" + downloadsize.Result);

        if (downloadsize.Result > 0)
        {
            var download = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
            yield return download;
            //await download.Task;
            Debug.Log("download result type " + download.Result.GetType());
            foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
            {
                var ab = item.GetAssetBundle();
                Debug.Log("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    Debug.Log("asset name " + name);
                }
            }
            Addressables.Release(download);
        }
        Addressables.Release(downloadsize);
    }
    public void ClearCache()
    { 
    }
    public void DownLoad()
    {
        StartCoroutine(DownAssetImpl());
    }
    // Start is called before the first frame update
    void Start()
    {
        Addressables.InitializeAsync();
        TestAsync();
    }
    public int i = 0;
    public async void TestAsync()
    {
        for (int j = 0; j < 10; j++) 
        {
            Debug.Log("5555   " + j);
            await Task.Delay(3000);
            Debug.Log("3333");
        }
        Debug.Log("444");
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("666  " + i);
    }
}
