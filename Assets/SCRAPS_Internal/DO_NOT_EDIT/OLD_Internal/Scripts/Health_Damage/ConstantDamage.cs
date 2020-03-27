using UnityEngine;

public class ConstantDamage : Damage
{
    public float damageInterval = 1;
    private float currentInterval = 0;
    private GameObject myTarget = null;
    private bool targetHere = false;

    void Update()
    {
        if (targetHere)
        {
            if (myTarget.GetComponent<Health>().currentHealth > 0)
            {
                currentInterval += Time.deltaTime;
                if (currentInterval >= damageInterval)
                {
                    myTarget.GetComponent<Health>().ApplyDamage(baseDamage);
                    currentInterval = 0;
                }
            }
        }
    }

    void OnTriggerEnter(Collider entity)
    {
        if(entity.GetComponent<Health>() != null)
        {
            myTarget = entity.gameObject;
            targetHere = true;
            //print(gameObject.name + " targeted " + myTarget.name + "!");
        }
    }

    void OnTriggerExit(Collider entity)
    {
        if (entity.gameObject == myTarget)
        {
            targetHere = false;
            myTarget = null;
            //print(gameObject.name + " lost its target.");
        }
    }
}

