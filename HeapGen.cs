using System;
using System.Collections.Generic;

namespace tpfinal
{
    public class Heap<T> //Clase Heap generica
    {
        //Atributos del Heap
        private List<T> heap = new List<T>();
        private Comparison<T> comparador;

        // Constructor que recibe un comparador para definir el Heap
        public Heap(Comparison<T> comparador)
        {
            this.comparador = comparador;
        }

        // Método Insert
        public void Insert(T item)
        {
            heap.Add(item);
            HeapifyUp(heap.Count - 1); //"-1" para ajustar el indice)
        }

        // Método Delete
        public T? Delete() 
        {
            if (heap.Count == 0) return default;

            T aux = heap[0]; 
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return aux;
        }

        // Método filtrado hacia arriba
        private void HeapifyUp(int index)
        {
            int padreIndex = (index - 1) / 2;
            if (padreIndex >= 0 && comparador(heap[index], heap[padreIndex]) < 0) //Determinacion de Maxheap/Minheap
            {
                Swap(index, padreIndex);
                HeapifyUp(padreIndex);
            }
        }

        // Filtrado hacia abajo
        private void HeapifyDown(int index)
        {
            int izquierdo = 2 * index + 1;
            int derecho = 2 * index + 2;
            int target = index;

            if (izquierdo < heap.Count && comparador(heap[izquierdo], heap[target]) < 0)
                target = izquierdo;
            if (derecho < heap.Count && comparador(heap[derecho], heap[target]) < 0)
                target = derecho;

            if (target != index) //Si el index es diferente se debe mantener la propiedad del heap
            {
                Swap(index, target);
                HeapifyDown(target);
            }
        }

        private void Swap(int index1, int index2)
        {
            T temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }

        //Metodos con expresiones lambda

        public bool IsEmpty() => heap.Count == 0;

        // Devuelve la cantidad de elementos en el heap
        public int Count => heap.Count;

        // Devuelve la lista interna del heap (para posibles consultas)
        public List<T> GetHeap() => heap;
    }

}