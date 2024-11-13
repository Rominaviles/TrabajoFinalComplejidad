
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace tpfinal
{

    public class Estrategia
    {
        //Retorna un texto con las hojas de las Heaps utilizadas en los métodos anteriores construidas a partir de los datos de entrada.
        public String Consulta1(List<Proceso> datos)
        {
            var (minHeap, maxHeap) = CrearHeaps(datos);

            string hojasMin = ObtenerHojas(minHeap);
            string hojasMax = ObtenerHojas(maxHeap);

            string resultado = "Las hojas del MinHeap por tiempo CPU son las siguientes:\n";
            resultado += hojasMin + "\n";
            resultado += "Las hojas del MaxHeap por prioridad son las siguientes:\n";
            resultado += hojasMax;

            return resultado;
        }

        //Retorna un texto con las alturas de las Heaps utilizadas en los métodos anteriores construidas a partir de los datos de entrada.
        public String Consulta2(List<Proceso> datos)
        {
            List<Proceso> MinHeap = new List<Proceso>();
            List<Proceso> MaxHeap = new List<Proceso>();

            ShortesJobFirst(datos, MinHeap);
            PreemptivePriority(datos, MaxHeap);

            int alturaMinHeap = (int)Math.Floor(Math.Log2(MinHeap.Count));
            int alturaMaxHeap = (int)Math.Floor(Math.Log2(MaxHeap.Count));

            return $"La altura del MinHeap es: {alturaMinHeap}\nLa altura del MaxHeap es: {alturaMaxHeap}";
        }


        //Retorna un texto que contiene los datos de las Heaps utilizadas en los métodos anteriores, explicitando en el texto resultado los niveles en los que se encuentran ubicados cada uno de los datos.
        public String Consulta3(List<Proceso> datos)
        {
            var (minHeap, maxHeap) = CrearHeaps(datos);

            // Obtener los niveles de ambos Heaps
            var nivelesMin = porNiveles(minHeap);
            var nivelesMax = porNiveles(maxHeap);

            string resultado = "Los niveles del MinHeap por tiempo CPU:\n" + FormatearNiveles(nivelesMin) + "\n";
            resultado += "Los niveles del MaxHeap por prioridad:\n" + FormatearNiveles(nivelesMax);

            return resultado;
        }


        //Retorna en la variable collected los procesos ordenados del de menor tiempo de uso de la CPU al de mayor de la lista datos utilizando una MinHeap como estructura de datos soporte.
        public void ShortesJobFirst(List<Proceso> datos, List<Proceso> collected)
        {
            var minHeap = new Heap<Proceso>((t1, t2) => t1.tiempo.CompareTo(t2.tiempo));
            foreach (var proceso in datos)
            {
                minHeap.Insert(proceso);
            }

            while (!minHeap.IsEmpty())
            {
                collected.Add(minHeap.Delete());
            }
        }

        //Retorna en la variable collected los procesos ordenados del de mayor prioridad al de menor prioridad de la lista datos utilizando una MaxHeap como estructura de datos soporte.
        public void PreemptivePriority(List<Proceso> datos, List<Proceso> collected)
        {
            var maxHeap = new Heap<Proceso>((p1, p2) => p2.prioridad.CompareTo(p1.prioridad));
            foreach (var proceso in datos)
            {
                maxHeap.Insert(proceso);
            }

            while (!maxHeap.IsEmpty())
            {
                collected.Add(maxHeap.Delete());
            }
        }

        //Metodo auxiliares
        // Retorna una tupla de heaps a partir de una lista de procesos, ordenando uno según el tiempo de ejecución y el otro según la prioridad.
        private (Heap<Proceso> minHeap, Heap<Proceso> maxHeap) CrearHeaps(List<Proceso> datos)
        {
            List<Proceso> MinHeap = new List<Proceso>();
            List<Proceso> MaxHeap = new List<Proceso>();

            ShortesJobFirst(datos, MinHeap);
            PreemptivePriority(datos, MaxHeap);

            Heap<Proceso> minHeap = new Heap<Proceso>((p1, p2) => p1.tiempo.CompareTo(p2.tiempo));
            foreach (var proceso in MinHeap)
            {
                minHeap.Insert(proceso);
            }

            Heap<Proceso> maxHeap = new Heap<Proceso>((p1, p2) => p2.prioridad.CompareTo(p1.prioridad));
            foreach (var proceso in MaxHeap)
            {
                maxHeap.Insert(proceso);
            }

            return (minHeap, maxHeap);
        }

        //Calcula el índice de inicio de las hojas en un heap, recopila los nombres de todos los nodos hoja desde ese índice hasta el final del heap y los devuelve.
        private string ObtenerHojas(Heap<Proceso> heap)
        {
            int primerHoja = heap.Count / 2;

            List<string> hojas = new List<string>();

            for (int i = primerHoja; i < heap.Count; i++)
            {
                hojas.Add(heap.GetHeap()[i].nombre);
            }

            return string.Join(", ", hojas);
        }

        //Recorrido por niveles
        private List<List<Proceso>> porNiveles(Heap<Proceso> heap)
        {
            List<List<Proceso>> niveles = new List<List<Proceso>>();
            Cola<(Proceso nodo, int indice)> cola = new Cola<(Proceso nodo, int indice)>();

            cola.encolar((heap.GetHeap()[0], 0));

            while (cola.cantidadElementos() > 0)
            {
                int nivelActualCantidad = cola.cantidadElementos();
                List<Proceso> nivelActual = new List<Proceso>();

                for (int i = 0; i < nivelActualCantidad; i++)
                {
                    // Desencolar el nodo actual y su índice
                    var (nodoActual, indice) = cola.desencolar();
                    nivelActual.Add(nodoActual);  // Agregar el nodo actual al nivel actual

                    // Calcular los índices de los hijos en base al índice actual en el Heap
                    int hijoIzquierdoIndx = 2 * indice + 1;
                    int hijoDerechoIndx = 2 * indice + 2;

                    // Encolar los hijos si existen en el Heap
                    if (hijoIzquierdoIndx < heap.Count)
                    {
                        cola.encolar((heap.GetHeap()[hijoIzquierdoIndx], hijoIzquierdoIndx));
                    }
                    if (hijoDerechoIndx < heap.Count)
                    {
                        cola.encolar((heap.GetHeap()[hijoDerechoIndx], hijoDerechoIndx));
                    }
                }

                niveles.Add(nivelActual);
            }

            return niveles;
        }

        //Devuelve la representación final de todos los niveles formateados como una cadena de texto.
        private string FormatearNiveles(List<List<Proceso>> niveles)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < niveles.Count; i++)
            {
                sb.AppendLine($"Nivel {i}: {string.Join(", ", niveles[i].Select(p => p.nombre))}");
            }

            return sb.ToString();
        }

    }

}
