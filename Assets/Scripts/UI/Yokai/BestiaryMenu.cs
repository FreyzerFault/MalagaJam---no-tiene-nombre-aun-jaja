using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Yokai
{
    public class BestiaryMenu : MonoBehaviour
    {
        public YokaiDataSO yokaiData;
        
        private Dictionary<YokaiType, YokaiCardUI> yokaiCardsDictionary;

        private void Awake()
        {
            YokaiCardUI[] cards = GetComponentsInChildren<YokaiCardUI>();
            YokaiType[] types = Enum.GetValues(typeof(YokaiType)).Cast<YokaiType>().ToArray();
            if (cards.Length != types.Length)
                Debug.LogError($"No hay el mismo número de YokayCards ({cards.Length}) que de Yokais ({types.Length})", this);

            yokaiCardsDictionary = new Dictionary<YokaiType, YokaiCardUI>();
            for (int i = 0; i < types.Length; i++)
            {
                YokaiType type = types[i];
                YokaiCardUI cardUI = cards[i];
                yokaiCardsDictionary.Add(type, cardUI);
            }
        }

        private void UpdateYokai(YokaiType type)
        {
            if (yokaiCardsDictionary.TryGetValue(type, out YokaiCardUI cardUI)) 
                cardUI.UpdateData(yokaiData.GetData(type));
        }
    }
}
