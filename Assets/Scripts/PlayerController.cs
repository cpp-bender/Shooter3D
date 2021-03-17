using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Rigidbody body;
    [SerializeField] private PlayerMovementData playerData;
    private Vector3 moveVelocity;

    [Header("Mouse Look Settings")]
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        moveVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        body.MovePosition(body.position + moveVelocity * playerData.MoveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (ground.Raycast(ray, out rayDistance))
        {
            Vector3 rayPoint = ray.GetPoint(rayDistance);
            Vector3 lookPoint = new Vector3(rayPoint.x, transform.position.y, rayPoint.z);
            transform.LookAt(lookPoint);
        }
    }
}
