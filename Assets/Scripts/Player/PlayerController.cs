using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Entity
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private PlayerSettings playerData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GunController gunController;

    private Vector3 moveVelocity;

    private void OnEnable()
    {
        playerData.IsPlayerDead = false;
    }

    private void Awake()
    {
        health = playerData.StartingHealth;
        this.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        playerData.IsPlayerDead = true;
        Destroy(gameObject);
    }

    private void Update()
    {
        Rotate();
        Shoot();
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
            Debug.DrawLine(ray.origin, rayPoint, Color.red);
            Vector3 lookPoint = new Vector3(rayPoint.x, transform.position.y, rayPoint.z);
            transform.LookAt(lookPoint);
        }
    }

    private void Shoot()
    {
        gunController.Shoot();
    }
}
