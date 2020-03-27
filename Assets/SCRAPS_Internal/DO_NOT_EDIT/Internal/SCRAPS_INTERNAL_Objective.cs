using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class SCRAPS_INTERNAL_Objective : MonoBehaviour {

    /* SCRAPS_INTERNAL_Objective.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use SCRAPS_Objective.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    public string ID_value = "";

    public string objectiveText = "";

    public int objectiveValue = 0;
    public int objectiveMaxValue = 0;

    public bool isComplete = false;

    private SphereCollider sCol;
    private Transform player;

    private MeshRenderer objectiveRend;

    public bool isHere = false;

    private void Awake()
    {
        GameObject compassPos = Instantiate(SCRAPS_ObjectManager.instance.compassMarker, transform.position, Quaternion.identity);
        objectiveRend = compassPos.GetComponent<MeshRenderer>();
        objectiveRend.material = SCRAPS_ObjectManager.instance.objectiveIcon;

        compassPos.GetComponent<SCRAPS_CameraFacingBillboard>().m_Camera = GameObject.Find("CompassCam").GetComponent<Camera>();
    }

    private void Start()
    {
        sCol = GetComponent<SphereCollider>();

        if(sCol == null)
        {
            Debug.LogErrorFormat(gameObject, "<b>" + name + "</b> has no Sphere Collider. Attempting to correct during play-mode!", new object[0]);
            sCol = gameObject.AddComponent<SphereCollider>();
            sCol.isTrigger = true;
            sCol.center = Vector3.zero;
            sCol.radius = transform.localScale.x / 2;
            transform.localScale = Vector3.one;
        }

        ResetObjective();
    }

    private void ResetObjective()
    {
        if (transform.localScale != Vector3.one)
        {
            Debug.LogErrorFormat(gameObject, "<b>" + name + "</b> was scaled on it's transform. Attempting to correct during play-mode!", new object[0]);
            sCol.isTrigger = true;
            sCol.center = Vector3.zero;
            sCol.radius = transform.localScale.x / 2;
            transform.localScale = Vector3.one;
        }

        if(gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            Debug.LogErrorFormat(gameObject, "<b>" + name + "</b> was not set to the Ignore Raycast layer. Attempting to correct during play-mode!", new object[0]);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);

            if (dist > sCol.radius + 1)
            {
                SCRAPS_ObjectiveList.instance.RemoveObjective(ID_value);
                player = null;
                isComplete = false;
                isHere = false;
            }
            else
            {
                if (objectiveValue >= objectiveMaxValue)
                {
                    if (isComplete == false)
                    {
                        SCRAPS_ObjectiveList.instance.RemoveObjective(ID_value);
                        SCRAPS_ObjectiveList.instance.CreateObjective(ID_value, objectiveText + "\n                                                          " + objectiveValue + "/" + objectiveMaxValue);
                        SCRAPS_ObjectiveList.instance.CompleteObjective(ID_value);

                        isComplete = true;

                        objectiveRend.enabled = false;
                    }
                }
            }
        }
    }

    public void UpdateObjective(int newValue)
    {
        if (objectiveValue < objectiveMaxValue)
        {
            objectiveValue += newValue;

            if (objectiveValue > objectiveMaxValue)
                objectiveValue = objectiveMaxValue;

            SCRAPS_ObjectiveList.instance.RemoveObjective(ID_value);
            SCRAPS_ObjectiveList.instance.CreateObjective(ID_value, objectiveText + "\n                                                          " + objectiveValue + "/" + objectiveMaxValue);


            float pitchValue = ((float)objectiveValue / (float)objectiveMaxValue) + 1.5f;

            if (objectiveValue != objectiveMaxValue)
                SCRAPS_AudioManager.instance.PlaySound(SCRAPS_AudioManager.soundType.system, pitchValue);
            else
            {
                SCRAPS_AudioManager.instance.PlaySound(SCRAPS_AudioManager.soundType.positive, 1.2f);
                SCRAPS_MessageSystem.instance.NewMessage("Player", "You've completed: <b><color=green>" + objectiveText + "</color></b>", SCRAPS_MessageSystem.msgType.emote);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (player == null)
        {
            if (other.tag == "Player")
            {
                isHere = true;
                player = other.transform;
                SCRAPS_ObjectiveList.instance.CreateObjective(ID_value, objectiveText + "\n                                                          " + objectiveValue + "/" + objectiveMaxValue);
            }
        }
    }
}
