using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private ProjectTileSettings projectTileSettings;
    private void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * projectTileSettings.BulletSpeed);
    }
}
