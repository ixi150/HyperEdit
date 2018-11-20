using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothTime = 1;
    [SerializeField] float rbVelocityMultiplier = 1;

    float z;
    Vector3 velocity;
    new Camera camera;

    private void Awake()
    {
        z = transform.position.z;
        camera = GetComponent<Camera>();
        camera.opaqueSortMode = UnityEngine.Rendering.OpaqueSortMode.NoDistanceSort;
        camera.transparencySortMode = TransparencySortMode.Orthographic;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        var targetPos = target.position;
        var targetRb = target.GetComponent<Rigidbody2D>();
        if (targetRb)
            targetPos += (Vector3)targetRb.velocity * rbVelocityMultiplier;

        var pos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        pos.z = z;
        transform.position = pos;
    }
}
