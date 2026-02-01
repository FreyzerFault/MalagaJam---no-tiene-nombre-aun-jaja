using System;

namespace Dialogue
{
    [Serializable]
    public struct Dialogue 
    {
        public enum Character { Macaco, Faisan, Perro, Momotaro, Ogro, Unknown }
        public enum Mood { Default = 0, Enfadado = 1, Asustado = 2, Feliz = 3, Molesto = 4, }
            
        public Character character;
        public Mood mood;
        public string text;
    }
}