using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playScreen;

    public void StartGame()
    {
        StartCoroutine(FadeOutPlayScreen());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeOutPlayScreen()
    {
        yield return new WaitForSeconds(.25f);
        Image playScreenImage = playScreen.GetComponent<Image>();
        Color tempColor = playScreenImage.color;
        tempColor.a = 1f;
        while (tempColor.a >= 0)
        {
            tempColor.a -= .1f;
            playScreenImage.color = tempColor;
            yield return null;
        }
        SceneManager.LoadScene(1);
        playScreen.SetActive(false);
    }
}