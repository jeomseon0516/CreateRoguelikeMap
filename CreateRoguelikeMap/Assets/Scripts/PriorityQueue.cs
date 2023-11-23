using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * �켱���� ť�� ����ϴ� ���� AStar�� ���¸���Ʈ�� �� ��忡�� �ڽ�Ʈ�� ���� ���� ��带 ã�ƾ��ϱ� ����
 * min heap���� ������ �켱���� ť�� ����Ѵ�. �迭�̳� ��ũ�� ����Ʈ�� ��� ������ �Ҷ� �ſ� ū ����� �� �� �ִ�.
 * ����Ʈ�� ��� ���� �պκп� �ڽ�Ʈ�� ���� ��带 ���� ��� �ڿ� �ִ� ��ҵ��� ��� ��ĭ �� �ڷ� �о��־�� �ؼ�
 * ȿ���� �ſ� ����������. ��ũ�� ����Ʈ�� ��� ������ �Ҷ� �־��� ��� ��� �����͸� ��ȸ�� �� �����Ƿ� ȿ���� �����ʴ�.
 * �켱���� ť�� ��� Ʈ���� ������ �̷���� �ֱ� ������ insert �Ҷ� ���̰� ����������� ����Ƚ���� 1ȸ �þ�Ƿ� �ſ� ȿ�����̴�.
 * �� Ʈ���� ������ �����Ѵ�. �ڽ� ����� ��ġ�� �׻� Ȯ�����̱� ������ �迭�� ������ �����ϴ�. index ���� ����
*/
public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> _heap = new List<T>();
    public int Count { get => _heap.Count; }

    public void Clear()
    {
        _heap.Clear();
    }

    public void Push(T t)
    {
        _heap.Add(t);

        int nowIndex = _heap.Count - 1;

        // ���� �� ��忡 ���� ���� �� ���������� �ڽ��� ��ġ�� ã�ư���.
        while (nowIndex > 0)
        {
            int parentIndex = (nowIndex - 1) / 2;

            if (_heap[nowIndex].CompareTo(_heap[parentIndex]) < 0) // Ư�� ���ǿ� �����ϴ� ��� �ڽ��� ��ġ�� �ȴ�.
                break;

            T temp = _heap[nowIndex];
            _heap[nowIndex] = _heap[parentIndex];
            _heap[parentIndex] = temp;

            nowIndex = parentIndex;
        }
    }
    public T Pop()
    {
        // �׻� ��Ʈ ���� Ư�� ���ǿ� ���� ���ĵ� ���̹Ƿ� ��Ʈ ��带 ��ȯ ���ش�.
        T ret = _heap[0];

        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex--);

        int nowIndex = 0;

        while (true)
        {
            int leftIndex = nowIndex * 2 + 1;
            int rightIndex = nowIndex * 2 + 2;

            int nextIndex = nowIndex;

            if (leftIndex <= lastIndex && _heap[nextIndex].CompareTo(_heap[leftIndex]) < 0)
                nextIndex = leftIndex;
            if (rightIndex <= lastIndex && _heap[nextIndex].CompareTo(_heap[rightIndex]) < 0)
                nextIndex = rightIndex;

            if (nowIndex == nextIndex)
                break;

            T temp = _heap[nowIndex];
            _heap[nowIndex] = _heap[nextIndex];
            _heap[nextIndex] = temp;

            nowIndex = nextIndex;
        }

        return ret;
    }

    public T this[int index]
    {
        get => _heap[index];
    }
}
