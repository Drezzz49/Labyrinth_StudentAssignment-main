using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Student.Scripts
{
    public class BinaryHeap
    {
        private DynamicArray<(Vector2Int pos, int fCost)> heap;
        private int size;
        public BinaryHeap(int capacity)
        {
            heap = new DynamicArray<(Vector2Int pos, int fCost)>(15);
            size = 0;
        }
        public (Vector2Int pos, int fCost) Peek()
        {
            if (size == 0) throw new InvalidOperationException("Heap is empty");
            return heap[1];
        }
        public int Size()
        {
            return size;
        }
        public void Insert((Vector2Int pos, int fCost) value)
        {
            if (size == heap.Length - 1) throw new InvalidOperationException("Heap is full");
            heap[++size] = value;
            HeapifyBottomToTop(size);
        }
        private void HeapifyBottomToTop(int index)
        {
            int parent = index / 2;
            if (index <= 1) return;
            if (heap[index].fCost < heap[parent].fCost)
            {
                Swap(index, parent);
                HeapifyBottomToTop(parent);
            }
        }
        public (Vector2Int pos, int fCost) ExtractMin()
        {
            if (size == 0) throw new InvalidOperationException("Heap is empty");
            (Vector2Int, int) min = heap[1];
            heap[1] = heap[size--];
            HeapifyTopToBottom(1);
            return min;
        }
        private void HeapifyTopToBottom(int index)
        {
            int left = index * 2;
            int right = index * 2 + 1;
            int smallest = index;
            if (left <= size && heap[left].fCost < heap[smallest].fCost)
            {
                smallest = left;
            }
            if (right <= size && heap[right].fCost < heap[smallest].fCost)
            {
                smallest = right;
            }
            if (smallest != index)
            {
                Swap(index, smallest);
                HeapifyTopToBottom(smallest);
            }
        }
        private void Swap(int a, int b)
        {
            (Vector2Int, int) temp = heap[a];
            heap[a] = heap[b];
            heap[b] = temp;
        }
    }
}
