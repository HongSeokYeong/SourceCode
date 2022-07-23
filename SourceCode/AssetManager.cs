using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : MonoBehaviour
{
    #region Singleton
    private static AssetManager _instance = null;
    public static AssetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 유니티 씬에 배치되어 있는지 찾는다.
                _instance = FindObjectOfType<AssetManager>();
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("AssetManager");
                    _instance = gameObject.AddComponent<AssetManager>();
                }
                _instance.Initialize();
            }

            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// AssetManager 초기화
    /// </summary> 
    private void Initialize()
    {
        
    }

    /// <summary>
    /// 에셋을 생성하고 AsyncOperationHandle을 반환해주는 함수
    /// </summary>
    /// <param name="key"></param>
    /// <param name="parent"></param>
    /// <param name="instantiateInWorldSpace"></param>
    /// <returns></returns>
    public AsyncOperationHandle<GameObject> AssetInstantiate(object key, Transform parent = null, bool instantiateInWorldSpace = false)
    {
        var instantiateHandle = Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, true);
        
        return instantiateHandle;
    }

    /// <summary>
    /// 생성된 에셋을 해제하는 함수
    /// </summary>
    /// <param name="gameObject"></param>
    public void ReleaseInstance(GameObject gameObject)
    {
        Addressables.ReleaseInstance(gameObject);
    }
}
