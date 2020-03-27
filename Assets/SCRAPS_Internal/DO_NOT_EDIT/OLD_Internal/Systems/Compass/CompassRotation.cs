using UnityEngine;

public class CompassRotation : MonoBehaviour {

    private Transform follow;

	// Update is called once per frame
	void Update () {

        if (follow == null)
        {
            GameObject player = SCRAPS_ObjectManager.instance.player;
            
            if(player != null)
            {
                follow = player.transform;
            }
        }  
        else
        {
            Vector3 newRot = follow.rotation.eulerAngles;
            newRot.x = 0;
            newRot.z = 0;

            transform.rotation = Quaternion.Euler(newRot);
        }
	}
}
