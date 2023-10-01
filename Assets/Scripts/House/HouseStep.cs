using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace House
{
    [Serializable]
    public class HouseStep
    {
        public AudioClip VoiceOver;
        public List<GameObject> GameObjectsToActivate;
        public Color TintColor = Color.white;
        public string DialogText;
    }
}