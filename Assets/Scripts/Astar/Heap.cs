using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap
{
    private Node[] heap;
    private int heapSize = 0;

    public Heap(int arrSize)
    {
        heap = new Node[arrSize];
    }

    public void Insert(Node node)
    {
        heap[heapSize] = node;
        SortUp(heapSize);
        heapSize++;
    }

    public Node GetMin()
    {
        heapSize--;
        swap(0, heapSize);
        SortDown(0);
        return heap[heapSize];
    }

    public bool Contains(Node node)
    {
        for (int i = 0; i < heapSize; i++)
        {
            if (heap[i] == node) return true;
        }
        return false;
    }

    public int Size
    {
        get
        {
            return heapSize;
        }
    }

    private void SortUp(int idx)
    {
        while (true)
        {
            int parent = (idx - 1) / 2;
            if (heap[idx].fCost < heap[parent].fCost)
            {
                swap(idx, parent);
            }
            else break;

            idx = parent;
        }
    }

    private void SortDown(int idx)
    {
        while (true)
        {
            int leftChild = idx * 2 + 1;
            int rightChild = idx * 2 + 2;
            int swapIdx = 0;

            if (leftChild < heapSize)
            {
                swapIdx = leftChild;

                if (rightChild < heapSize && heap[rightChild].fCost < heap[swapIdx].fCost)
                {
                    swapIdx = rightChild;
                }

                if (heap[swapIdx].fCost < heap[idx].fCost)
                {
                    swap(swapIdx, idx);
                }
                else return;
            }
            else return;

            idx = swapIdx;
        }
    }

    private void swap(int a, int b)
    {
        Node temp = heap[a];
        heap[a] = heap[b];
        heap[b] = temp;
        return;
    }
}
