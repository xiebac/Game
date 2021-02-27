using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressablesManager : MonoBehaviour
{
    public async void LoadSingleSceneAsync(object key)
    {
        var loadHandle = Addressables.LoadSceneAsync(key,LoadSceneMode.Single);
        await loadHandle.Task;

    }
}
