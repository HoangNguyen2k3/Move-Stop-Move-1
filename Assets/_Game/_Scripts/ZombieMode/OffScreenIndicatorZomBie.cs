using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicatorZombie : MonoBehaviour
{
    public Transform target;
    public GameObject zombie;
    public Camera mainCamera;
    public float edgeOffset = 50f;
    public Image arrow;
    public GameObject child;
    public RectTransform canvasRect;
    public RectTransform arrowRect;

    private void Awake() {
        if (mainCamera == null)
            mainCamera = Camera.main;

        /*        if (arrow != null) {
                    //arrowRect = arrow.GetComponent<RectTransform>();
                    canvas = arrow.GetComponentInParent<Canvas>();
                    //canvasRect = canvas.GetComponent<RectTransform>();
                }*/
        InitializeArrow();
    }

    private void InitializeArrow() {
        if (arrowRect != null) {
            arrow.enabled = false;
            arrowRect.anchoredPosition = new Vector2(-1000, -1000);
        }

        if (child != null) {
            child.SetActive(false);
        }
    }
    private void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }
        if (zombie.activeSelf == false) {
            child.SetActive(false);
            return;
        }

        if (ZombieGameController.Instance.currentInLobbyZombie) {
            if (child.activeSelf)
                child.SetActive(false);
            return;
        }
        else {
            if (!child.activeSelf)
                child.SetActive(true);
        }

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        bool isOffScreen = viewportPos.z < 0 || viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;

        if (isOffScreen) {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

            if (screenPos.z < 0) {
                screenPos *= -1;
            }

            screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

            Vector3 dir = (target.position - mainCamera.transform.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            arrow.GetComponent<RectTransform>().position = screenPos;
            arrow.enabled = true;
        }
        else {
            arrow.enabled = false;
        }
    }
}
