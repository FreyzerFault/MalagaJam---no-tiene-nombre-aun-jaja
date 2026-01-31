using UnityEngine;
using System.Collections.Generic;
using System;


[CreateAssetMenu(fileName = "DialogueList", menuName = "Matsuri/Dialogue")]
public class DialogueList : ScriptableObject
{
    [Serializable]
    public struct Dialogue {
       public string Imagen;
        public string Frase;
    }
    public List<Dialogue> ListaDialogos = new List<Dialogue>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Forma de añadir posicion
    
        ListaDialogos.Add(new Dialogue() {
            Imagen = "Perrete",
            Frase = "Buenas",
        });
        
        
    }

    // Update is called once per frame
  
}