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
                // ����Ƽ ���� ��ġ�Ǿ� �ִ��� ã�´�.
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
    /// AssetManager �ʱ�ȭ
    /// </summary> 
    private void Initialize()
    {
        
    }

    /// <summary>
    /// ������ �����ϰ� AsyncOperationHandle�� ��ȯ���ִ� �Լ�
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
    /// ������ ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="gameObject"></param>
    public void ReleaseInstance(GameObject gameObject)
    {
        Addressables.ReleaseInstance(gameObject);
    }
}
