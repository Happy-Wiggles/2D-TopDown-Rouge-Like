using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform Target;
    public float SmoothSpeed;
    public Vector3 Offset;

    void LateUpdate()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 desiredPos = Target.position + Offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, SmoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }
}
