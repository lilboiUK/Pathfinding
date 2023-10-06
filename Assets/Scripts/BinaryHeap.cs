using System;
using System.Collections.Generic;

public class NodeBinaryHeap
{
    List<Node> heap;
    HashSet<Node> elementsInHeap;

    public NodeBinaryHeap()
    {
        heap = new List<Node>();
        elementsInHeap = new HashSet<Node>();
    }
    public void Insert(Node item)
    {
        heap.Add(item);
        elementsInHeap.Add(item);

        int currentIndex = heap.Count - 1;
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
    public bool Contains(Node item)
    {
        return elementsInHeap.Contains(item);
    }
    public Node ExtractMin()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("Heap is empty.");
        }

        Node minItem = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = currentIndex * 2 + 1;
            int rightChildIndex = currentIndex * 2 + 2;
            int smallestChildIndex = currentIndex;

            if (leftChildIndex < heap.Count && heap[leftChildIndex].CompareTo(heap[smallestChildIndex]) < 0)
            {
                smallestChildIndex = leftChildIndex;
            }

            if (rightChildIndex < heap.Count && heap[rightChildIndex].CompareTo(heap[smallestChildIndex]) < 0)
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
        Node temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }
}