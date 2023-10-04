using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace Cola_prioridad
{
    public partial class Form1 : Form
    {
        Cola cola;
        public Form1()
        {
            InitializeComponent();
            cola = new Cola();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }


        //este metodo muestra todos los nodos de la cola en el listbox preservando la cola original
        //Dado que para mostrar cada nodo se necesita desencolar y extraer el nodo del frente pero si hiciera solo eso
        // la cola orignal quedaria vacia por lo tanto con auxCola logramos mantener la cola original

        private void Mostrar(Cola pCola, ListBox pListbox)
        {
            pListbox.Items.Clear();
            Cola auxCola = new Cola();
            Nodo auxNodo = pCola.Desencolar();
            while (auxNodo != null)  //el primer while desencola cada nodo de la cola original (pCola) y lo muestra en el listbox
            {  //al final del while la cola original estará vacía y auxCola tendrá los nodos ordenados
                pListbox.Items.Add($"{auxNodo.Id} - {auxNodo.PRIORIDAD}");
                auxCola.Encolar(auxNodo.Id, auxNodo.PRIORIDAD);
                auxNodo = pCola.Desencolar();

            }
            auxNodo = auxCola.Desencolar();
            while (auxNodo != null)
            { //el segundo while desencola cada nodo de auxCola y lo vuelve a encolar en la cola original pCola
               //al final de este bucle auxCola estará vacia y pCola tendra todos sus nodos de vuelta en el orden original
                
                pCola.Encolar(auxNodo.Id, auxNodo.PRIORIDAD);
                auxNodo = auxCola.Desencolar();
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string idInput = Interaction.InputBox("Ingrese id del nodo");
            int prioridad;

            while (true)
            {
                string prioridadInput = Interaction.InputBox("Ingrese prioridad");
                bool isNumber = int.TryParse(prioridadInput, out prioridad);
                //intenta convertir la cadena prioridadInput en entero,
                //si tiene exito devuelve true y asigna el valor convertido a prioridad gracias al parametro out si falla, (por ejemplo si el usuario ingresa letras en lugar de un numero) devuelve false

                //pregunta si isNumber es falso
                if (!isNumber || prioridad < 1 || prioridad > 3)
                {
                    MessageBox.Show("Prioridad INVALIDA");

                }
                else
                {
                    break;
                }


            }
            //encola el nuevo nodo y muestra la cola actualizada
            cola.Encolar(idInput, prioridad);
            Mostrar(cola, listBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Nodo auxNodo = cola.Desencolar();
            if (auxNodo == null) MessageBox.Show("No hay nodos en la cola !!!");
            else MessageBox.Show($"El nodo desencolado es: {auxNodo.Id}");
            Mostrar(cola, listBox1);

        }

        public class Cola
        {
            Nodo NCP;
            public Cola() { NCP = new Nodo(); NCP.Siguiente = null; }

            public void Encolar(string pId, int pPRIORIDAD)
            {
                if (NCP.Siguiente == null)
                {
                    NCP.Siguiente = new Nodo(pId, pPRIORIDAD);
                }
                else
                { 
                    //me trae el nodo despues del cual deberia insertar el nuevo nodo para mantener orden de prioridad y llegada
                    //es decur si quiero añadir un nodo con prioridad 3, la funcion trae el ultimo nodo con prioridad 3. SI no encuentra ningun nodo con prioridad 3,
                    //va a traer el ultimo nodo con prioridad 2, y si tampoco lo encuentra, trae el ultimo nodo con prioridad 1
                    

                    Nodo auxNodo = UltimoNodoPRIORIDAD(pPRIORIDAD);
                    Nodo auxNodoSiguiente = auxNodo.Siguiente;
                    auxNodo.Siguiente = new Nodo(pId, pPRIORIDAD);
                    auxNodo.Siguiente.Siguiente = auxNodoSiguiente;
                }
            }

            public Nodo Desencolar()
            {
                if (NCP.Siguiente == null)
                {
                    return NCP.Siguiente;
                }
                else
                {
                    Nodo auxNodo = NCP.Siguiente;
                    NCP.Siguiente = auxNodo.Siguiente;
                    auxNodo.Siguiente = null;
                    return auxNodo;
                }
            }


//el proposito de esta funcion es encontrar el ultimo nodo en la cola q tenga prioridad igual o mayor a pPRIORIDAD 

            private Nodo UltimoNodoPRIORIDAD(int pPRIORIDAD)
            {
                return UltimoNodoRecursiva(NCP, pPRIORIDAD); //empiezo por el 1er nodo
            }


            
   //esta funcion encuentra el ultimo nodo en la cola con prioridad igual o mayor a la que se desea encolar
   //para asegurarnos que se mantenga el orden de llegada para nodos con la misma prioridad
            private Nodo UltimoNodoRecursiva(Nodo pNodo, int pPRIORIDAD)
            {  //o sea encuentra la posicion correcta en la lista para encolar el nuevo nodo
                //manteniendo los nodos ordenados segun su prioridad, y para nodos con la misma prioridad, en el orden que fueron añadidos
                Nodo auxNodo;



  //*identifica el caso que no hay mas nodos despues del siguiente nodo, si pNodo.siguiente es el ultimo nodo, entonces
  //pNodo.sig.sig será null. En este caso según la prioridad devuelvo pNodo o pNodo.Sig
 


                if (pNodo.Siguiente.Siguiente == null) //Caso no hay nada a la derecha
                {
                    if (pNodo.Siguiente.PRIORIDAD > pPRIORIDAD) auxNodo = pNodo;  //si tengo >1 miro el nodo de la derecha
                    else auxNodo = pNodo.Siguiente;
                }

//*caso que hay mas nodos despues del siguiente nodo. Si la prioridad del siguiente es mayo rque pPRIORIDAD, entonces se devuelve pNodo.
//de lo contrario la funcion se llama a sí misma recursivamente con el siguiente nodo.

//verifico la prioridad del nodo siguiente y se decide si retornar el nodo 

                else
                {
                    if (pNodo.Siguiente.PRIORIDAD > pPRIORIDAD) auxNodo = pNodo; //Si tengo >1 miro el nodo de la derecha
                    else auxNodo = UltimoNodoRecursiva(pNodo.Siguiente, pPRIORIDAD);
                }
                return auxNodo;
            }
        }

        public class Nodo
        {
            public Nodo(string pId = "", int pPRIORIDAD = 1, Nodo pSiguiente = null)
            {
                Id = pId;
                PRIORIDAD = pPRIORIDAD;
                Siguiente = pSiguiente;
            }
            public string Id { get; set; }
            public int PRIORIDAD { get; set; }
            public Nodo Siguiente { get; set; }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Nodo auxNodo = cola.Desencolar();
            if (auxNodo == null) MessageBox.Show("No hay nodos en la cola !!!");
            else MessageBox.Show($"El nodo desencolado es: {auxNodo.Id}");
            Mostrar(cola, listBox1);
        }
    }
}
