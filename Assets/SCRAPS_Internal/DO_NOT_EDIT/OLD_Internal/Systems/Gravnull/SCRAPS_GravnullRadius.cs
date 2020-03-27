using UnityEngine;

public class SCRAPS_GravnullRadius : MonoBehaviour {

    public Transform target;
    public SphereCollider sCol;
    
    void Start()
    {
        sCol = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>() && other.gameObject.layer == LayerMask.NameToLayer("Gravnull"))
        {
            target = other.transform;
        }
    }
}
