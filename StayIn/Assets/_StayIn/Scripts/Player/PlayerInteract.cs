using UnityEditor.Rendering;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    private float interactRange = 0.5f;
    [SerializeField]
    private LayerMask itemLayer;
    [SerializeField]
    private Vector2 scanOffset = new Vector2(0, 0.5f);

    [Header("References")]
    [SerializeField]
    private InputReader reader;

    private ItemObject currentTarget;

    private void OnEnable() {
        reader.InteractEvent += HandlePickup;
    }

    private void Update() {
        ScanForOutline();
    }

    private void OnDisable() {
        reader.InteractEvent -= HandlePickup;
    }

    private void ScanForOutline() {
        Vector2 scanOrigin = (Vector2)transform.position + scanOffset;
        Collider2D hit = Physics2D.OverlapCircle(scanOrigin, interactRange, itemLayer);

        if (hit != null) {
            Vector2 targetPos = hit.transform.position;
            Vector2 direction = targetPos - scanOrigin;
            float distance = Vector2.Distance(scanOrigin, targetPos);

            int obstacleMask = LayerMask.GetMask("Obstacles");
            RaycastHit2D obstacleHit = Physics2D.Raycast(scanOrigin, direction, distance, obstacleMask);

            if (obstacleHit.collider != null) {
                if (obstacleHit.collider.gameObject == gameObject || obstacleHit.collider == hit) {
                    Debug.DrawRay(scanOrigin, direction.normalized * distance, Color.green);
                    UpdateTarget(hit.GetComponent<ItemObject>());
                } else {
                    Debug.DrawRay(scanOrigin, direction.normalized * distance, Color.red);
                    UpdateTarget(null);
                }
            } else {
                Debug.DrawRay(scanOrigin, direction.normalized * distance, Color.green);
                UpdateTarget(hit.GetComponent<ItemObject>());
            }
        } else {
            UpdateTarget(null);
        }
    }

    private void UpdateTarget(ItemObject newTarget) {
        if (currentTarget != newTarget) {
            if (currentTarget != null) currentTarget.SetHighlight(false);
            currentTarget = newTarget;
            if (currentTarget != null) currentTarget.SetHighlight(true);
        }
    }

    private void HandlePickup() {
        if (currentTarget != null) {
            // currentTarget.Collect(); 

            Debug.Log("Pickup: " + currentTarget.name);

            Destroy(currentTarget.gameObject);
            currentTarget = null;
        }
    }

    private void OnDrawGizmosSelected() {
        Vector2 scanOrigin = (Vector2)transform.position + scanOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(scanOrigin, interactRange);

        Gizmos.color = (currentTarget != null) ? new Color(0, 1, 0, 0.2f) : new Color(0, 0, 1, 0.1f);
        Gizmos.DrawSphere(scanOrigin, interactRange);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, scanOrigin);
    }
}
