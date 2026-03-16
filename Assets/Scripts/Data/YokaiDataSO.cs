using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    public enum YokaiType { Tengu, Banshee, Ushi, Chochin, Wanjudo, Kappa }
    
    [CreateAssetMenu(fileName = "YokaiData", menuName = "Momotaro/Characters/Yokai")]
    public class YokaiDataSO : ScriptableObject
    {
        [Serializable]
        public class YokaiData
        {
            public YokaiType yokaiType;
            public bool collected;
            public Sprite sprite;
        }
        
        [SerializedDictionary("Yokai", "Data")]
        public SerializedDictionary<YokaiType, YokaiData> yokaiDictionary;
        
        public event Action<YokaiData> OnCollected;

        private void Awake()
        {
            foreach (YokaiType type in Enum.GetValues(typeof(YokaiType)).Cast<YokaiType>().ToArray()) 
                yokaiDictionary.Add(type, new YokaiData { yokaiType = type, collected = false });
        }

        public YokaiData GetData(YokaiType type) => yokaiDictionary[type];
        public bool IsCollected(YokaiType type) => yokaiDictionary[type].collected;
        public Sprite GetSprite(YokaiType type) => yokaiDictionary[type].sprite;
        
        public void Collect(YokaiType type)
        {
            if (yokaiDictionary.TryGetValue(type, out YokaiData data))
            {
                data.collected = true;
                OnCollected?.Invoke(data);
            }
        }
        
        [Button]
        public void Reset()
        {
            foreach (YokaiData yokaiData in yokaiDictionary.Values) 
                yokaiData.collected = false;
        }
    }
}
