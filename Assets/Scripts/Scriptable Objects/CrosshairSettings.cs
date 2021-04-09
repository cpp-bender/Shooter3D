using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter / Crosshair Settings")]
public class CrosshairSettings : ScriptableObject
{
    [SerializeField] private float rotateSpeed;
    private Color crosshairOriginalColor;

    public Color CrosshairOriginalColor { get => crosshairOriginalColor; set => crosshairOriginalColor = value; }
    public float RotateSpeed { get => rotateSpeed; }
}
