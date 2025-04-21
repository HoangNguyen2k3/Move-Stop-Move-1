using UnityEngine;

public class KeepScreenSize : MonoBehaviour
{

    public Camera mainCamera;
    public Transform target; // enemy
    public Vector3 offset = Vector3.up;
    public float scaleAtDefaultDistance = 1f;
    public float defaultDistance = 5f;
    public Transform beginPos;
    private Vector3 initialTargetScale;
    public float maxYOffset = 1f;


    private void Awake() {
        beginPos = transform;
        initialTargetScale = Vector3.one;
    }
    void Start() {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void LateUpdate() {
        if (target == null) return;

        Vector3 worldPos = target.position + offset;

        Vector3 camToObj = worldPos - mainCamera.transform.position;
        float distanceOnViewAxis = Vector3.Dot(camToObj, mainCamera.transform.forward);

        float scale = scaleAtDefaultDistance * (distanceOnViewAxis / defaultDistance);
        float yOffset = Mathf.Lerp(0f, maxYOffset, scale);
        transform.position = target.position + offset + new Vector3(0f, yOffset + 0.4f, 0f);
        //transform.position = target.position + offset;

        transform.localScale = Vector3.one * scale;
        if (target.localScale != initialTargetScale) {
            float scaleFactor = target.localScale.x / initialTargetScale.x;
            transform.localScale /= scaleFactor;
        }

    }
}
