using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// .. 우선순위 큐로 가장 거리가 먼 방을 맵을 탈출하는 방으로 설정
public class RoomOption : MonoBehaviour
{
    public static readonly int LEFT = 0;
    public static readonly int RIGHT = 1;
    public static readonly int TOP = 2;
    public static readonly int BOTTOM = 3;

    public RoomOption[] AroundRooms { get; set; } = new RoomOption[] { null, null, null, null };
    // .. 방의 번호는 고유한 값이며 바뀌지 않음
    public Vector2Int RoomNumber { get; set; }
}