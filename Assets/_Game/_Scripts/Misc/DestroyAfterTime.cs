using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float time_active;
    [SerializeField] private float time_remain;
    private float time_real;
    private bool active_collider = false;
    private Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();
    }
    private void Update()
    {
        time_real += Time.deltaTime;
        if (time_real > time_remain && !active_collider)
        {
            col.enabled = false;
            active_collider = true;
        }
        if (time_real > time_active)
        {
            Destroy(gameObject);
        }
    }
}
