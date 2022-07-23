using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  예시를 위한 코드
/// </summary>
public class ExampleCode : MonoBehaviour
{
    void Start()
    {
        // Dynamic 레이어 캔버스에 UIInventory이름의 프리팹을 생성시켜주고 초기화까지 시켜줍니다.
        UIManager.Instance.OpenUI<UIInventory>(UILayer.Dynamic, "UIInventory").Initialize();
    }
}