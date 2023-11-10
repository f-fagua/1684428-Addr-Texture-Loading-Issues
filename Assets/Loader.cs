using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.Threading.Tasks;
using System;
using System.IO;

public class Loader : MonoBehaviour
{
    public static Loader instance;

    public string addressableKey;
    public string addressableKey2;

    public Texture townhallEnvironment;
    public Sprite iconUI;

    public bool readyToUse;

    [SerializeField] private AssetLabelReference labelReference;
    [SerializeField] private AsyncOperationHandle downloadHandle;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Addressables.InitializeAsync();
        DownloadAssetsAsync(labelReference);
    }

    private async void DownloadAssetsAsync(AssetLabelReference key)
    {
        downloadHandle = Addressables.DownloadDependenciesAsync(key);

        try
        {
            while (!downloadHandle.IsDone)
            {
                Debug.Log("Asset Downloading");
                await Task.Yield();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Asset error: " + ex.Message);
        }

        Debug.Log("Asset Done");
        StartCoroutine(LoadDataAsync());
    }

    IEnumerator LoadDataAsync()
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync(addressableKey);
        yield return locationHandle;

        Debug.Log("Asset: " + locationHandle.Result[0]);

        // ==================
        AsyncOperationHandle<Texture> handle = Addressables.LoadAssetAsync<Texture>(locationHandle.Result[0]);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Background Data Success to Download : " + handle.Result);
            Texture data = handle.Result;
            townhallEnvironment = data;
            readyToUse = true;
        }
        else
        {
            Debug.Log("Background Data Failed to Download : " + handle.Result);
            Debug.Log("Background Data Failed to Download : " + handle.Status);
            readyToUse = false;
        }

        //Addressables.Release(handle);

        // ==================

        //AsyncOperationHandle<Sprite> handle2 = Addressables.LoadAssetAsync<Sprite>(handle.Result[1]);
        //await handle2.Task;

        //if (handle2.Status == AsyncOperationStatus.Succeeded)
        //{
        //    Debug.Log("Background Data Success to Download : " + handle2.Result);

        //    Sprite data = handle2.Result;
        //    iconUI = data;
        //    readyToUse = true;
        //}
        //else
        //{
        //    // Ada kesalahan dalam memuat sumber daya
        //    Debug.Log("Background Data Failed to Download : " + handle2.Result);
        //    Debug.Log("Background Data Failed to Download : " + handle2.Status);

        //    readyToUse = false;
        //}

        //Addressables.Release(handle2);
        // ==================s
    }
}