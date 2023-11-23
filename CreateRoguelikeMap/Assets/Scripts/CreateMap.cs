using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public enum DIRECTION
    {
        LEFT = 0,
        RIGHT,
        TOP,
        BOTTOM
    }

    private const float INTERVAL = 0.25f;
    private float SIZE = 1f;

    private List<RoomOption> _openList = new List<RoomOption>();
    private List<List<RoomOption>> _rooms = new List<List<RoomOption>>();
    private RoomOption _rootRoom = null;
    private Vector2Int _keepMapSize;
    private Vector2Int _mapSize;
    private int _randCount;
    public List<List<RoomOption>> Rooms
    {
        get => _rooms;
        set
        {
            _rooms = value;
        }
    }
    public Vector2Int MapSize
    {
        get => _mapSize;
        set
        {
            _mapSize = value;
        }
    }
    public int RandCount
    {
        get => _randCount;
        set
        {
            _randCount = value;
        }
    }
    public Vector2Int KeepMapSize => _keepMapSize;
    // .. 내부 요소들을 계속 new로 생성해서 가비지 콜렉터에서 계속 수집이 일어나지만 일단은 이대로 작업
    public void UpdateCube()
    {
        if (_keepMapSize == _mapSize) return;

        destroyCube();
        _keepMapSize = _mapSize;

        for (int y = 0; y < _keepMapSize.y; ++y)
        {
            List<RoomOption> rooms = new List<RoomOption>();

            float interval = SIZE + INTERVAL;

            for (int x = 0; x < _keepMapSize.x; ++x)
            {
                RoomOption room = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<RoomOption>();

                room.RoomNumber = new Vector2Int(x, y);
                room.transform.position = new Vector3(x * interval, y * interval, 0f);
                rooms.Add(room);
            }

            _rooms.Add(rooms);
        }
    }
    public void CreateRandomMap()
    {
        foreach (List<RoomOption> rooms in _rooms)
        {
            foreach (RoomOption room in rooms)
            {
                room.gameObject.SetActive(false);

                foreach (MyGizmo door in room.gameObject.GetComponents<MyGizmo>())
                    DestroyImmediate(door);
            }
        }

        _rootRoom = _rooms[_mapSize.y / 2][_mapSize.x / 2];

        changeMaterialColor(_rootRoom.gameObject, Color.blue);

        Vector2Int currentRoomNumber = _rootRoom.RoomNumber;

        Vector2Int leftPoint = new Vector2Int(currentRoomNumber.x - 1, currentRoomNumber.y);
        Vector2Int rightPoint = new Vector2Int(currentRoomNumber.x + 1, currentRoomNumber.y);
        Vector2Int bottomPoint = new Vector2Int(currentRoomNumber.x, currentRoomNumber.y - 1);
        Vector2Int topPoint = new Vector2Int(currentRoomNumber.x, currentRoomNumber.y + 1);

        _rootRoom.gameObject.SetActive(true);

        if (!checkEdge(leftPoint))
            _rootRoom.AroundRooms[RoomOption.LEFT] = _rooms[leftPoint.y][leftPoint.x];
        if (!checkEdge(rightPoint))
            _rootRoom.AroundRooms[RoomOption.RIGHT] = _rooms[rightPoint.y][rightPoint.x];
        if (!checkEdge(bottomPoint))
            _rootRoom.AroundRooms[RoomOption.BOTTOM] = _rooms[bottomPoint.y][bottomPoint.x];
        if (!checkEdge(topPoint))
            _rootRoom.AroundRooms[RoomOption.TOP] = _rooms[topPoint.y][topPoint.x];

        int count = 1;

        for (int i = 0; i < _rootRoom.AroundRooms.Length; ++i)
        {
            if (ReferenceEquals(_rootRoom.AroundRooms[i], null) || !_rootRoom.AroundRooms[i]) continue;

            _rootRoom.AroundRooms[i].gameObject.SetActive(true);
            _openList.Add(_rootRoom.AroundRooms[i]);
            ++count;

            if (count >= _randCount)
                break;
        }

        while (count < _randCount)
        {
            int index = Random.Range(0, _openList.Count);
            RoomOption currentRoom = currentRoom = _openList[index];
            _openList.RemoveAt(index);

            if (createRoomToPoint(new Vector2Int(currentRoom.RoomNumber.x - 1, currentRoom.RoomNumber.y)))
                ++count;
            if (count >= _randCount)
                break;

            if (createRoomToPoint(new Vector2Int(currentRoom.RoomNumber.x + 1, currentRoom.RoomNumber.y)))
                ++count;
            if (count >= _randCount)
                break;

            if (createRoomToPoint(new Vector2Int(currentRoom.RoomNumber.x, currentRoom.RoomNumber.y + 1)))
                ++count;
            if (count >= _randCount)
                break;

            if (createRoomToPoint(new Vector2Int(currentRoom.RoomNumber.x, currentRoom.RoomNumber.y - 1)))
                ++count;
            if (count >= _randCount)
                break;
        }

        foreach (List<RoomOption> rooms in _rooms)
        {
            foreach (RoomOption room in rooms)
            {
                Vector2Int left   = new Vector2Int(room.RoomNumber.x - 1, room.RoomNumber.y);
                Vector2Int right  = new Vector2Int(room.RoomNumber.x + 1, room.RoomNumber.y);
                Vector2Int bottom = new Vector2Int(room.RoomNumber.x, room.RoomNumber.y - 1);
                Vector2Int top    = new Vector2Int(room.RoomNumber.x, room.RoomNumber.y + 1);

                checkAroundRoom(room, RoomOption.LEFT,   left,   new Vector3(-0.25f,  0f, -1f));
                checkAroundRoom(room, RoomOption.RIGHT,  right,  new Vector3(0.25f,   0f, -1f));
                checkAroundRoom(room, RoomOption.BOTTOM, bottom, new Vector3(0f,  -0.25f, -1f));
                checkAroundRoom(room, RoomOption.TOP,    top,    new Vector3(0f,   0.25f, -1f));
            }
        }

        _openList.Clear();
    }
    private void checkAroundRoom(RoomOption pivotRoom, int aroundRoom, Vector2Int point, Vector3 doorPosition)
    {
        if (checkEdge(point) || !_rooms[point.y][point.x].gameObject.activeSelf) return;

        pivotRoom.AroundRooms[aroundRoom] = _rooms[point.y][point.x];
        MyGizmo door = pivotRoom.gameObject.AddComponent<MyGizmo>();
        door.Pivot = pivotRoom.transform.position + doorPosition;
        door.GizmoColor = Color.green;
        door.Radius = 0.075f;
    }
    private bool createRoomToPoint(Vector2Int point)
    {
        if (checkEdge(point)) return false;

        RoomOption currentRoom = _rooms[point.y][point.x];

        // .. 생성할 위치의 큐브가 비활성화 상태일때만 true
        if (!currentRoom.gameObject.activeSelf)
        {
            currentRoom.gameObject.SetActive(true);
            _openList.Add(currentRoom);
            return true;
        }

        return false;
    }
    // .. 가장자리라면 맵 생성이 불가능 하기 때문에
    private bool checkEdge(Vector2Int point)
    {
        return point.x < 0 || point.x > _mapSize.x - 1 || point.y < 0 || point.y > _mapSize.y - 1;
    }
    private void changeMaterialColor(GameObject obj, Color color)
    {
        if (!obj.TryGetComponent(out MeshRenderer meshRenderer)) return;

        Material instanceMaterial = new Material(meshRenderer.sharedMaterial);
        instanceMaterial.color = Color.blue;
        meshRenderer.material = instanceMaterial;
    }
    private void destroyCube()
    {
        foreach (List<RoomOption> cubes in _rooms)
        {
            foreach (RoomOption cube in cubes)
            {
                if (ReferenceEquals(cube, null) || !cube) continue;
                DestroyImmediate(cube.gameObject);
            }
            cubes.Clear();
        }
        _rooms.Clear();
    }
}