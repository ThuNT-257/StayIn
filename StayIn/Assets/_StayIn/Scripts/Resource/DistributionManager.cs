using UnityEngine;

public class DistributionManager : MonoBehaviour
{
    private static DistributionManager instance;

    public static DistributionManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<DistributionManager>();
                if(instance == null) {
                    Debug.Log("DistributionManager is not in the Scene.");
                }
            }
            return instance;
        }
    }


}
