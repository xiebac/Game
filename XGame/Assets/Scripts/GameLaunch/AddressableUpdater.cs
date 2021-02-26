using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableUpdater : MonoBehaviour
{
    public Text status_Text;
    public Slider update_Slider;
    float totalTime = 0f;
    bool needUpdateRes = false;
    // Start is called before the first frame update
    void Start()
    {
        totalTime = 0f;
        status_Text.text = "正在检测资源更新...";
    }
    public void StartCheckUpdate()
    {
        UpdateCatalog();
    }
    public async void UpdateCatalog()
    {
        var start = DateTime.Now;
        var initHandle = Addressables.InitializeAsync();
        await initHandle.Task;
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        await checkHandle.Task;
        Debug.Log(string.Format("CheckIfNeededUpdate use {0}ms", (DateTime.Now - start).Milliseconds));
        Debug.Log($"catalog count: {checkHandle.Result.Count} === check status: {checkHandle.Status}");
        if (checkHandle.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> catalogs = checkHandle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                needUpdateRes = true;
                status_Text.text = "正在更新资源...";
                update_Slider.normalizedValue = 0f;
                update_Slider.gameObject.SetActive(true);
                start = DateTime.Now;
                AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;
                var locators = updateHandle.Result;
                Debug.Log($"locator count: {locators.Count}");
                foreach (var v in locators)
                {
                    var sizeHandle = Addressables.GetDownloadSizeAsync(v.Keys);
                    await sizeHandle.Task;
                    long size = sizeHandle.Result;
                    Debug.Log($"download size:{size}");
                    if (size > 0)
                    {
                        //UINoticeTip.Instance.ShowOneButtonTip("更新提示", $"本次更新大小：{size}", "确定", null);
                        //yield return UINoticeTip.Instance.WaitForResponse();
                        var downloadHandle = Addressables.DownloadDependenciesAsync(v.Keys, Addressables.MergeMode.Union);
                        while (!downloadHandle.IsDone)
                        {
                            float percentage = downloadHandle.PercentComplete;
                            Debug.Log($"download pregress: {percentage}");
                            update_Slider.normalizedValue = percentage;
                        }
                        await downloadHandle.Task;
                        Addressables.Release(downloadHandle);
                    }
                }

                Debug.Log(string.Format("UpdateFinish use {0}ms", (DateTime.Now - start).Milliseconds));
                //yield return UpdateFinish();
                Addressables.Release(updateHandle);
            }
            Addressables.Release(checkHandle);
        }
    }
    IEnumerator checkUpdate()
    {
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
