using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ���ø� ���� �ڵ�
/// </summary>
public class ExampleCode : MonoBehaviour
{
    void Start()
    {
        // Dynamic ���̾� ĵ������ UIInventory�̸��� �������� ���������ְ� �ʱ�ȭ���� �����ݴϴ�.
        UIManager.Instance.OpenUI<UIInventory>(UILayer.Dynamic, "UIInventory").Initialize();
    }
}