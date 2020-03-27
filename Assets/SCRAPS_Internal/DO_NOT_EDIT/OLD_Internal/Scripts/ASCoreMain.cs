using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASCoreMain : MonoBehaviour {

    [Header("UI SYSTEM")]
    public GameObject gradingCanvas;
    public Text timeEstimateTxt;
    //private SetupVolumeTimeEstimator volEst;
    private GameObject mapProjector;
    //private bool toggleMap = false;

    [Header("GOD CAMERA SYSTEM")]
    public bool toggleCam = false;
    public GameObject playerAvatar;
    public GameObject gravnullSys;
    public GameObject playerCamera;
    public GameObject godCamera;
    private bool isPlayer = true;

    public Checkpoint[] myPoints;

    private bool releaseCursor = false;

    void Awake()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //GenerateASCoreConsole();
        gradingCanvas.SetActive(false);
    }

    public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    // Update is called once per frame
    void Update () {

        //Show or Hide the Console!
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            ASCoreConsole();
        }
        
        GodCamera();

        if (gradingCanvas.activeSelf == true)
        {
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            releaseCursor = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                releaseCursor = !releaseCursor;
            }

            if (releaseCursor == true)
            {
                Time.timeScale = 1.0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1.0f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void ASCoreConsole()
    {
        myPoints = GameObject.FindObjectsOfType<Checkpoint>();

        if (playerAvatar == null)
            playerAvatar = GameObject.FindWithTag("Player");

        if (gravnullSys == null)
            gravnullSys = GameObject.Find("GravnullSystem");

        if (playerCamera == null)
            playerCamera = GameObject.Find("ASCoreCamera");

        if (mapProjector == null)
            mapProjector = GameObject.Find("ASCoreMapProjector");

        gradingCanvas.SetActive(!gradingCanvas.activeSelf);
    }

    void GodCamera()
    {
        if (toggleCam)
        {
            //assume player starts first
            if (isPlayer)
            {
                godCamera.transform.position = playerCamera.transform.position;
                godCamera.transform.rotation = playerCamera.transform.rotation;

                godCamera.SetActive(true);

                playerCamera.SetActive(false);
                playerAvatar.SetActive(false);
                gravnullSys.SetActive(false);

                isPlayer = false;
            }
            else
            {
                playerAvatar.transform.position = godCamera.transform.position;
                playerCamera.transform.position = godCamera.transform.position;

                playerAvatar.transform.rotation = godCamera.transform.rotation;
                playerCamera.transform.rotation = godCamera.transform.rotation;

                playerCamera.SetActive(true);
                playerAvatar.SetActive(true);
                gravnullSys.SetActive(true);

                godCamera.SetActive(false);

                isPlayer = true;
            }

            toggleCam = false;
        }
    }

    public void ToggleCam()
    {
        toggleCam = true;
        ASCoreConsole();
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
