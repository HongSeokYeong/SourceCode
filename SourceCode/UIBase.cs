using System;
using UnityEngine;

/// <summary>
/// UI 베이스 클래스
/// </summary>
public abstract class UIBase : MonoBehaviour
{
    [System.NonSerialized]
    private Type uiType;       // UI Object 타입
    [System.NonSerialized]
    private UILayer layer;    // 출력한 UI Layer

    public Type UIType
    {
        get { return uiType; }
        set { uiType = value; }
    }
    public UILayer UILayer
    {
        get { return layer; }
        set { layer = value; }
    }

    public abstract void Initialize();

    public abstract void Release();
}
