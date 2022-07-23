using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// UI 출력 Layer
/// </summary>
public enum UILayer
{
    None,
    Static,
    Dynamic,
    Popup,
}

/// <summary>
/// UI 객체를 관리하는 클래스
/// - 각 Layer에 UI를 출력 및 제거
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 유니티 씬에 배치되어 있는지 찾는다.
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("UIManager");
                    instance = gameObject.AddComponent<UIManager>();
                }
                instance.Initialize();
            }

            return instance;
        }
    }
    #endregion
    // 레이어 오브젝트(Clone 생성할 위치)
    public Dictionary<UILayer, GameObject> layerParents;

    // 화면에 출력된 UI 리스트
    private Dictionary<UILayer, List<UIBase>> openedUIs;

    private string uiStaticTag = "UIStatic";
    private string uiDynamicTag = "UIDynamic";
    private string uiPopupTag = "UIPopup";

    /// <summary>
    /// UIManager 초기화
    /// </summary>
    public void Initialize()
    {
        InitOpenedUIs();
        InitLayerParents();
    }

    /// <summary>
    /// 레이어를 key값으로 사용하는 부모 오브젝트 딕셔너리를 생성
    /// </summary>
    private void InitLayerParents()
    {
        GameObject _obj = null;
        layerParents = new Dictionary<UILayer, GameObject>();

        // Layer의 부모 GameObject를 등록
        foreach (UILayer uiLayerValue in Enum.GetValues(typeof(UILayer)))
        {
            switch (uiLayerValue)
            {
                case UILayer.Static:
                    _obj = GameObject.FindGameObjectWithTag(uiStaticTag);
                    break;
                case UILayer.Dynamic:
                    _obj = GameObject.FindGameObjectWithTag(uiDynamicTag);
                    break;
                case UILayer.Popup:
                    _obj = GameObject.FindGameObjectWithTag(uiPopupTag);
                    break;
            }
            layerParents.Add(uiLayerValue, _obj);
        }
    }

    /// <summary>
    /// 현재 오픈되어 있는 UI들을 관리 할 딕셔너리를 생성
    /// </summary>
    private void InitOpenedUIs()
    {
        openedUIs = new Dictionary<UILayer, List<UIBase>>
        {
            // Layer별 UI를 관리할 리스트 생성
            { UILayer.Static, new List<UIBase>() },
            { UILayer.Dynamic, new List<UIBase>() },
            { UILayer.Popup, new List<UIBase>() },
        };
    }

    /// <summary>
    /// 주어진 Layer에 UI를 생성.
    /// 단, 이미 오픈된 UI는 최상위에 출력 시킨다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="layer"></param>
    /// <returns></returns>
    public T OpenUI<T>(UILayer layer, string prefabName = null) where T : UIBase
    {
        Type uiType = typeof(T);

        List<UIBase> uiList = null;

        if (openedUIs == null)
        {
            InitOpenedUIs();
        }

        if (openedUIs.TryGetValue(layer, out uiList))
        {
            UIBase openedUI = Get<T>(layer);
            if (openedUI != null)
            {
                openedUI.gameObject.SetActive(true);
                openedUI.transform.SetAsLastSibling();
                uiList.LastIndexOf(openedUI);
                return (T)openedUI;
            }
            else
            {
                var instantiateHandle = AssetManager.Instance.AssetInstantiate(prefabName);

                instantiateHandle.WaitForCompletion();

                T uiObject = instantiateHandle.Result.GetComponent<T>();
                uiList.Add(uiObject);

                // UIBase의 속성 값을 변경
                uiObject.UILayer = layer;
                uiObject.UIType = uiType;

                // 해당 layer의 부모를 찾아 자식으로 설정
                GameObject parentObject = GetUIRoot(layer);
                if (parentObject != null)
                {
                    uiObject.gameObject.SetActive(true);
                    uiObject.gameObject.transform.SetParent(parentObject.transform);
                    uiObject.gameObject.transform.localScale = Vector3.one;
                    uiObject.gameObject.transform.localPosition = Vector3.zero;
                    uiObject.transform.SetAsLastSibling();
                }

                return uiObject;
            }
        }
        return null;
    }

    /// <summary>
    /// UI의 릴리즈 함수를 호출해서 정리 후 비활성화 시킨다.
    /// </summary>
    /// <param name="ui"></param>
    public void CloseUI(UIBase ui)
    {
        ui?.Release();
        ui?.gameObject.SetActive(false);
    }

    /// <summary>
    /// 해당하는 타입의 객체를 찾고 릴리즈 함수 호출및 정리 후 비활성화 시킨다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void CloseUI<T>() where T : UIBase
    {
        UIBase ui = Get<T>();

        CloseUI(ui);
    }

    /// <summary>
    /// UILayer에서 해당 타입의 UI를 찾고 릴리즈 함수 호출 및 정리 후 비활성화 시킨다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="layer"></param>
    public void CloseUI<T>(UILayer layer) where T : UIBase
    {
        UIBase ui = Get<T>(layer);
        if (ui != null)
        {
            CloseUI(ui);
        }
    }

    /// <summary>
    /// UILayer의 모든 UI정리 및 비활성화.
    /// </summary>
    /// <param name="layer"></param>
    public void CloseAll(UILayer layer)
    {
        if (openedUIs != null)
        {
            List<UIBase> uiList = null;
            if (openedUIs.TryGetValue(layer, out uiList))
            {
                foreach (var ui in uiList)
                {
                    CloseUI(ui);
                }
            }
        }
    }

    /// <summary>
    /// 해당 UI가 존재하는지 여부 확인
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Contains<T>() where T : UIBase
    {
        return (Get<T>() != null);
    }

    /// <summary>
    /// 주어진 UILayer에 해당 UI가 존재하는지 여부 확인
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="layer"></param>
    /// <returns></returns>
    public bool Contains<T>(UILayer layer) where T : UIBase
    {
        return (Get<T>(layer) != null);
    }

    /// <summary>
    /// 주어진 UILayer에서 해당타입의 UIBase를 리턴한다.
    /// </summary>
    /// <typeparam name="T">UIBase</typeparam>
    /// <param name="layer"></param>
    /// <returns></returns>
    public T Get<T>(UILayer layer) where T : UIBase
    {
        List<UIBase> uiList = null;
        if (openedUIs.TryGetValue(layer, out uiList))
        {
            Type uiType = typeof(T);
            foreach (var ui in uiList)
            {
                if (ui.UIType == uiType)
                {
                    return ui as T;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 전체 UILayer에서 주어진 UI 타입을 찾는다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>() where T : UIBase
    {
        Type uiType = typeof(T);
        foreach (var list in openedUIs)
        {
            foreach (var ui in list.Value)
            {
                if (ui.UIType == uiType)
                {
                    return ui as T;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// UI가 생성될 부모 오브젝트를 찾는다.
    /// </summary>
    /// <param name="layer">layer</param>
    /// <returns></returns>
    public GameObject GetUIRoot(UILayer layer)
    {
        GameObject parent = null;
        layerParents.TryGetValue(layer, out parent);

        return parent;
    }
}