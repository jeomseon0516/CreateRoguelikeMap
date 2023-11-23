using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// .. �켱���� ť�� ���� �Ÿ��� �� ���� ���� Ż���ϴ� ������ ����
public class RoomOption : MonoBehaviour
{
    public static readonly int LEFT = 0;
    public static readonly int RIGHT = 1;
    public static readonly int TOP = 2;
    public static readonly int BOTTOM = 3;

    public RoomOption[] AroundRooms { get; set; } = new RoomOption[] { null, null, null, null };
    // .. ���� ��ȣ�� ������ ���̸� �ٲ��� ����
    public Vector2Int RoomNumber { get; set; }
}