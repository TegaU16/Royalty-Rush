using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    [System.Serializable]
    public class MusicCategory
    {
        public string name;            // e.g., "MainMenu", "Game"
        public List<AudioClip> clips;  // Music clips for this category
    }
}
