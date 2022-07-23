using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 UI 클래스
/// </summary>
public class UIInventory : UIBase
{
    // UI생성시 호출될 초기화 함수
    public override void Initialize()
    {
        // UIInventory 프리팹의 자식에 있는 자식 UI들의 초기화를 시켜줍니다.
        // ex) 이미지 변경, 버튼 생성 등등
    }

    // UI삭제시 호출될 해제 함수
    public override void Release()
    {
    }
}