using UnityEngine;
using UnityEngine.SceneManagement;

public class SCRAPS_Menu_FadeFunctions : MonoBehaviour {

    public void StartFadeOut()
    {
        GetComponent<Animator>().SetTrigger("changemenu");
    }

    public void QuitGame()
    {
        if (!Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        else
        {
            Application.Quit();
        }
    }

	public void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }
}
