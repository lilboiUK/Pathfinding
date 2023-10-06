using System;
using System.Collections.Generic;

public class BinaryHeap<T> where T : IComparable<T>
{
    List<T> heap;
    HashSet<T> elementsInHeap;

    public BinaryHeap()
    {
        heap = new List<T>();
        elementsInHeap = new HashSet<T>();
    }
    public int Count => heap.Count;
    public void Insert(T item)
    {
        heap.Add(item);
        elementsInHeap.Add(item);

        int currentIndex = Count - 1;
        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;
            if (heap[currentIndex].CompareTo(heap[parentIndex]) >= 0)
            {
                break;
            }

            Swap(currentIndex, parentIndex);
            currentIndex = parentIndex;
        }
    }
    public bool Contains(T item)
    {
        return elementsInHeap.Contains(item);
    }
    public T ExtractMin()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Heap is empty.");
        }

        T minItem = heap[0];
        int lastIndex = Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = currentIndex * 2 + 1;
            int rightChildIndex = currentIndex * 2 + 2;
            int smallestChildIndex = currentIndex;

            if (leftChildIndex < Count && heap[leftChildIndex].CompareTo(heap[smallestChildIndex]) < 0)
            {
                smallestChildIndex = leftChildIndex;
            }

            if (rightChildIndex < Count && heap[rightChildIndex].CompareTo(heap[smallestChildIndex]) < 0)
            {
                smallestChildIndex = rightChildIndex;
            }

            if (smallestChildIndex == currentIndex)
            {
                break;
            }

            Swap(currentIndex, smallestChildIndex);
            currentIndex = smallestChildIndex;
        }

        return minItem;
    }
    private void Swap(int index1, int index2)
    {
        T temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }
}