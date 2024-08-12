using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [Header("Base Unit")]
    [SerializeField] string unitName;
    [SerializeField] string unitEnemy;


    [SerializeField] float unitHealth;
    [SerializeField] float unitSpeed;
    [SerializeField] float unitMaxSpeed;
    [SerializeField] float unitDmg;
    [SerializeField] float unitDmgRange;
    [SerializeField] float unitCD;
    [SerializeField] string[] unitAnimNames;
    [SerializeField] string unitDamageType;//tells if unit does slash dmg, explosive dmg, projectile dmg
    [SerializeField] GameObject target;
    [SerializeField] GameObject currentAtkObj;


    [Header("Unit Componenets")]
    [SerializeField] Animator unitAnim;
    [SerializeField] GameObject unityBody;
    [SerializeField] GameObject ragDollBody;
    [SerializeField] GameObject weapon;
    [SerializeField] Transform deathSpawn;

    public Rigidbody rb;
    Vector3 distanceFromTarget;
    string damageTaken;

    bool hasTarget;
    bool inCharge;
    bool inAttack;
    bool spawnRagdoll;
    bool canDie;
    bool canAttack;

    private void Start()
    {
        canDie = true;
        spawnRagdoll = false;
        canAttack = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(unitAnim != null)
            CheckStates();
        if (canDie == true)
            UnitDeath();

        FindTarget();
        if(inAttack == true)
            UnitAttack();
    }

    private void FixedUpdate()
    {
        if (inCharge == true)
            UnitCharge();
    }

   public void CheckStates()
    {
        if(target != null)
        {
            hasTarget = true;
            distanceFromTarget = transform.position - target.transform.position;
            
        }
        else
            hasTarget = false;

        //checks if the target is farther then unit range
        if(hasTarget == true && distanceFromTarget.magnitude >= unitDmgRange)
        {
            inCharge = true;
            unitAnim.SetBool(unitAnimNames[0], inCharge);
        }
        else
        {
            inCharge = false;
            unitAnim.SetBool(unitAnimNames[0], inCharge);
        }

        if (hasTarget == true && distanceFromTarget.magnitude <= unitDmgRange)
        {
            inAttack = true;
            unitAnim.SetBool(unitAnimNames[1], inAttack);
        }
        else
        {
            inAttack = false;
            Destroy(currentAtkObj);
            unitAnim.SetBool(unitAnimNames[1], inAttack);
        }
    }


    public void UnitDeath()
    {
        if (unitHealth <= 0)
        {
            canDie = false;
            Destroy(unityBody);
            var deadBody = Instantiate(ragDollBody, deathSpawn.position, Quaternion.identity);
            if(damageTaken == "Slash")
            {
                deadBody.GetComponent<Gore>().SetDmgType(damageTaken);
            }
            Destroy(gameObject);
        }
    }


    
    //run towards the target
    public void UnitCharge()
    {
        RotateToTarget();
        //gets direction to target
        if (target != null)
        {
            var directionToTarget = target.transform.position - transform.position;
            directionToTarget = directionToTarget.normalized;//normalizes it


            //checks if velocity is over maxSpeed
            if (Mathf.Abs(rb.velocity.magnitude) >= unitMaxSpeed)
                return;


            //adds force in direction to move unit
            rb.AddForce(directionToTarget * unitSpeed, ForceMode.Force);
        }
    }


    //spawns atk object for entity
    public void UnitAttack()
    {
        //TODO PERFORM DAMAGE ON TARGET
        if(canAttack == true)
        {
            target.GetComponentInParent<BaseUnit>().SetUnitHealth(unitDmg);
            target.GetComponentInParent<BaseUnit>().SetTypeOfDmg(unitDamageType);
            canAttack = false;
            Invoke("ResetAttack", unitCD);
            
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    //rotates the unit to face the target
    void RotateToTarget()
    {
        if(target != null)
        {
            Vector3 lookAtRotation = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(lookAtRotation);
        }
    }

    //Finds the closest enemy to the unit
    public void FindTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(unitEnemy);

        //itterates through each target 
        for (int i = 0; i < targets.Length; i++)
        {
            //for the first itteration set the target to first deployed
            if (i == 0)
                target = targets[i];

            //gets the distances
            var targetDistanceCheck = transform.position - targets[i].transform.position;
            var targetTwoDistanceCheck = transform.position - target.transform.position;

            if (targetDistanceCheck.magnitude <= targetTwoDistanceCheck.magnitude)
                target = targets[i];

        }
    }

    //COLLISION METHODS
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ragdoll")
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
                collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 300f, ForceMode.Force);
        }
    }

    //GETTER METHODS
    public string GetUnitName()
    {
        return unitName;
    }

    public string GetUnitEnemy()
    {
        return unitEnemy;
    }

    public float GetUnitHealth()
    {
        return unitHealth;
    }

    public float GetUnitSpeed()
    {
        return unitSpeed;
    }

    public float GetUnitDmg()
    {
        return unitDmg;
    }

    //SETTER METHODS

    public void SetUnitHealth(float dmgTaken)
    {
        unitHealth -= dmgTaken;
    }

    public void SetTypeOfDmg(string typeOfDmg)
    {
        damageTaken = typeOfDmg;
    }
}
