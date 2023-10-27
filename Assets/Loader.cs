using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Loader : MonoBehaviour
{
    public string addressableKey;
    public Texture townhallEnvironment;
    public bool readyToUse;

    public static Loader instance;

    

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
    }

    async void Start()
    {
        AsyncOperationHandle<Texture> handle = Addressables.LoadAssetAsync<Texture>(addressableKey);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Background Data Success to Download : " + handle.Result);
            Texture data = handle.Result;
            Debug.Log("Texture : " + data);
            townhallEnvironment = data;
            readyToUse = true;
            
        }
        else
        {
            // Ada kesalahan dalam memuat sumber daya
            Debug.Log("Background Data Failed to Download : " + addressableKey);
            readyToUse = false;
        }

        Addressables.Release(handle);
    }
}