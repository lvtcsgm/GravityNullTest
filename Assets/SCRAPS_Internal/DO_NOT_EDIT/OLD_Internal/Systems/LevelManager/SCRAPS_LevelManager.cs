using UnityEngine;
using UnityEngine.SceneManagement;

public class SCRAPS_LevelManager : MonoBehaviour {

    private int sceneCount = 0;

    void Awake()
    {
        //get all scenes loaded in the build settings
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }

	// Use this for initialization
	void Start () {

        // We don't want this running in the editor, only when we build the project out
        if (!Application.isEditor)
        {
            for(int i = 0; i < sceneCount; i++)
            {

                if (i > 3)
                {
                    SceneManager.LoadScene(i, LoadSceneMode.Additive);
                }
            }
        }
        else //this IS THE EDITOR
        {
            //don't load additive automatically
        }
	}
}
