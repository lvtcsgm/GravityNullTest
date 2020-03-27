using UnityEngine;

public class SCRAPS_INTERNAL_Checkpoint : MonoBehaviour {

    /* SCRAPS_INTERNAL_Checkpoint.cs
     * by Derek Allen Marunowski
     * 
     * This script is for INTERNAL use only
     * Please use Checkpoint.cs instead
     * 
     * DO NOT EDIT without permission from author
     */

    [Header("Display message when checkpoint is reached?")]
    public bool displayMessage = true;

    [Header("Name of location?")]
    public string locName = "Unknown Location";

    [Header("Where does the player respawn?")]
    public Transform respawnPos;

    private ZoneSystem zs;

    private MeshRenderer compassRend;

    private void Awake()
    {
        GameObject compassPos = Instantiate(SCRAPS_ObjectManager.instance.compassMarker, transform.position, Quaternion.identity);
        compassRend = compassPos.GetComponent<MeshRenderer>();
        compassRend.material.color = Color.white;

        compassPos.GetComponent<SCRAPS_CameraFacingBillboard>().m_Camera = GameObject.Find("CompassCam").GetComponent<Camera>();
    }

    void Start()
    {
        zs = GameObject.FindObjectOfType<ZoneSystem>();
    }

    private void Update()
    {
        if (SCRAPS_ObjectManager.instance.player != null)
        {
            if (SCRAPS_ObjectManager.instance.player.GetComponent<Health>().respawnMarker != respawnPos)
            {
                compassRend.material.color = Color.white;
            }
            else
            {
                compassRend.material.color = Color.green;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            zs.NewMessage(locName);

            if (other.GetComponent<Health>().respawnMarker != respawnPos)
            {
                if (displayMessage)
                    SCRAPS_MessageSystem.instance.NewMessage("", "<b>Checkpoint Activated</b> - " + locName, SCRAPS_MessageSystem.msgType.system);

                other.GetComponent<Health>().respawnMarker = respawnPos;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            zs.NewMessage("UNKNOWN");
    }

    void OnDrawGizmosSelected()
    {
        Color nColor = Color.yellow;
        nColor.a = 0.5f;
        Gizmos.color = nColor;
        Gizmos.DrawCube(new Vector3(respawnPos.position.x, respawnPos.position.y + 1, respawnPos.position.z), new Vector3(1, 2, 1));
    }
}
