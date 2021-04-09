using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Transform crosshairDot;
    [SerializeField] private CrosshairSettings crosshairSettings;

    private void Start()
    {
        crosshairSettings.CrosshairOriginalColor = crosshairDot.GetComponent<SpriteRenderer>().color;
    }

    private void FixedUpdate()
    {
        RotateCrosshair();
    }

    private void RotateCrosshair()
    {
        transform.Rotate(Vector3.forward * crosshairSettings.RotateSpeed * Time.deltaTime);
    }

    public void DetectEnemy(Ray ray, float rayDistance)
    {
        if (Physics.Raycast(ray, rayDistance, targetMask))
        {
            crosshairDot.GetComponent<SpriteRenderer>().color = Color.red;
            return;
        }
        crosshairDot.GetComponent<SpriteRenderer>().color = crosshairSettings.CrosshairOriginalColor;
    }
}
