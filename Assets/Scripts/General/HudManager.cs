using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private GameObject gameOverText;

    private void FixedUpdate()
    {
        if (playerSettings.IsPlayerDead)
        {
           StartCoroutine(OnPlayerDeath());
        }
    }


    public IEnumerator OnPlayerDeath()
    {
        Image image = backgroundImage.GetComponent<Image>();
        Color tempColor = image.color;
        while (tempColor.a <= 1)
        {
            tempColor.a += .01f;
            image.color = tempColor;
            yield return null;
        }
        gameOverText.SetActive(true);
    }
}
