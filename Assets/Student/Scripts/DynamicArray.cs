using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;

namespace Assets.Student.Scripts
{
    public class DynamicArray<Item>
    {
        private Item[] a; // array of items
        private int N; // number of elements on stack
        public DynamicArray(int initSize) 
        { 
            a = new Item[initSize];
        }
        
        public bool isEmpty() { return N == 0; }

        private void resize(int max)
        {
            Item[] temp = new Item[max];
            for (int i = 0; i < N; i++)
                temp[i] = a[i];
            a = temp;
        }

        public void push(Item item)
        {
            if (N == a.Count()) resize(2 * a.Count());
            a[N++] = item;
        }
        
        public Item pop()
        {
            Item item = a[--N];
            if (N > 0 && N == a.Count() / 4) resize(a.Count() / 2);
            return item;
        }

        public void Add(Item item)
        {
            if (N == a.Count()) resize(2 * a.Count());
            a[N++] = item;
        }

        public Item RemoveLast()
        {
            Item item = a[--N];
            if (N > 0 && N == a.Count() / 4) resize(a.Count() / 2);
            return item;
        }
    }
}
