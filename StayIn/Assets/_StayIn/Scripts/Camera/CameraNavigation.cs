using System.Collections;
using UnityEngine;

//Use for room navigation
public class CameraNavigation : MonoBehaviour
{
    [SerializeField]
    private Transform[] roomAnchors;
    [SerializeField]
    private FadeManager fadeManager;

    [SerializeField]
    private GameObject btnLeft;
    [SerializeField]
    private GameObject btnRight;

    private int currentRoomIndex = 1;
    private bool isFading = false;

    private void Start() {
        if (roomAnchors.Length > 0 && fadeManager != null) {
            Vector3 pos = Camera.main.transform.position;
            pos.x = roomAnchors[currentRoomIndex].position.x;
            Camera.main.transform.position = pos;

            fadeManager.blackScreenGroup.alpha = 0;
            isFading = false;

            UpdateButtonVisibility();
        } else {
            Debug.LogError("Need Anchors!");
        }
    }

    public void LeftRoom() {
        if(currentRoomIndex > 0 && !isFading) {
            currentRoomIndex--;
            StartCoroutine(StartFadeAndMoveCamera());
        }
    }

    public void RightRoom() {
        if(currentRoomIndex < roomAnchors.Length - 1  && !isFading) {
            currentRoomIndex++;
            StartCoroutine(StartFadeAndMoveCamera());
        }
    }

    private IEnumerator StartFadeAndMoveCamera() {
        isFading = true;

        btnLeft.SetActive(false);
        btnRight.SetActive(false);

        yield return StartCoroutine(fadeManager.FadeIn());

        Vector3 newPos = Camera.main.transform.position;
        newPos.x = roomAnchors[currentRoomIndex].position.x;
        Camera.main.transform.position = newPos;

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(fadeManager.FadeOut());

        UpdateButtonVisibility();
        isFading = false;
    }

    private void UpdateButtonVisibility() {
        btnLeft.SetActive(currentRoomIndex > 0);
        btnRight.SetActive(currentRoomIndex < roomAnchors.Length - 1);
    }
}
