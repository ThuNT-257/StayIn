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
    }
}
