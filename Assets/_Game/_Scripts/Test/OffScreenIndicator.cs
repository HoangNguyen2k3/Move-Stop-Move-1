using NUnit.Framework.Constraints;
using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
    public float edgeOffset = 50f;
    public GameObject indicatorObject;
    public GameObject enemy;
    public float bannerHeight = 90f;

    private void Start() {
        if (Banner.instance) {
            if (Banner.instance.isSuccess == true) {
                bannerHeight = 160f;
            }
            else {
                bannerHeight = 0f;
            }
        }
    }
    /*    public void CheckOffScreen() {
            if (target == null) {
                Destroy(gameObject);
                return;
            }
            if (enemy.activeSelf == false) {
                indicatorObject.SetActive(false);
                return;
            }
            if (GameStateManager.Instance.CheckFalseIndicator()) {
                return;
            }
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
            bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

            if (screenPos.z < 0) {
                screenPos.x = Screen.width - screenPos.x;
                screenPos.y = Screen.height - screenPos.y;
            }

            indicatorObject.SetActive(isOffScreen);

            if (isOffScreen) {
                screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
                screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

                Vector3 targetDirection = target.position - Camera.main.transform.position;
                targetDirection.z = 0;

                indicatorObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
            }

            indicatorObject.GetComponent<RectTransform>().position = screenPos;
        }*/
    public void CheckOffScreen() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }
        if (!enemy.activeSelf) {
            indicatorObject.SetActive(false);
            return;
        }
        if (GameStateManager.Instance.CheckFalseIndicator()) {
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        bool isBehindCamera = screenPos.z < 0;

        if (isBehindCamera) {
            screenPos *= -1; // Flip l?i n?u sau camera
        }

        bool isOffScreen =
            screenPos.x < 0 || screenPos.x > Screen.width ||
            screenPos.y < 0 || screenPos.y > Screen.height || isBehindCamera;

        indicatorObject.SetActive(isOffScreen);

        if (isOffScreen) {
            screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset + bannerHeight, Screen.height - edgeOffset);

            Vector3 targetDirection = (target.position - mainCamera.transform.position);
            targetDirection.z = 0;
            indicatorObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
        }

        indicatorObject.GetComponent<RectTransform>().position = screenPos;
    }

}
