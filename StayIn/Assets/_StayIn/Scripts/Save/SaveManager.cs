using System.Collections.Generic;
using UnityEngine;

namespace Assets._StayIn.Scripts.Save {
    public class SaveManager : MonoBehaviour{
        public static SaveManager instance;
        public static SaveManager Instance {
            get {
                if (instance == null) {
                    instance = FindAnyObjectByType<SaveManager>();
                    if (instance == null) {
                        Debug.Log("SaveManager is not in the Scene.");
                    }
                }
                return instance;
            }
        }

        private Dictionary<int, string> usedEvents = new Dictionary<int, string>();

        public Dictionary<int, string> UsedEvent => usedEvents;
    }
}
