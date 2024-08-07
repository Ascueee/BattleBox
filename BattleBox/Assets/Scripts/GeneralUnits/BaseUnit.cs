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
    [SerializeField] GameObject target;
    [SerializeField] float atkAnimationsVariations;

    [Header("Unit Componenets")]
    [SerializeField] Animator unitAnim;
    [SerializeField] GameObject unityBody;
    [SerializeField] GameObject ragDollBody;

    Rigidbody rb;
    Vector3 distanceFromTarget;
    bool hasTarget;
    bool inCharge;
    bool inAttack;
    bool spawnRagdoll;
    bool canDie;

    private void Start()
    {
        canDie = true;
        spawnRagdoll = false;
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
        UnitAttack();

        print(inCharge);
    }

    private void FixedUpdate()
    {
        if (inCharge == true)
        {
            UnitCharge();
        }
    }

    void CheckStates()
    {
        if(target != null)
        {
            hasTarget = true;
            distanceFromTarget = transform.position - target.transform.position;
            
        }
        else
        {
            hasTarget = false;
        }

        //checks if the target is farther then unit range
        if(hasTarget == true && distanceFromTarget.magnitude >= unitDmgRange)
        {
            inCharge = true;
            unitAnim.SetBool("inCharge", inCharge);
        }
        else
        {
            inCharge = false;
            unitAnim.SetBool("inCharge", inCharge);
        }

        if (hasTarget == true && distanceFromTarget.magnitude <= unitDmgRange)
        {
            inAttack = true;
            unitAnim.SetBool("inAttack", inAttack);
        }
        else
        {
            print("target not in range");
            inAttack = false;
            unitAnim.SetBool("inAttack", inAttack);
        }
    }


    void UnitDeath()
    {
        if (unitHealth <= 0)
        {
            canDie = false;
            Destroy(unityBody);
            var deadBody = Instantiate(ragDollBody, transform.position, Quaternion.identity);
        }
    }


    
    //run towards the target
    void UnitCharge()
    {
        RotateToTarget();
        //gets direction to target
        var directionToTarget = target.transform.position - transform.position;
        directionToTarget = directionToTarget.normalized;//normalizes it

        //checks if velocity is over maxSpeed
        if(Mathf.Abs(rb.velocity.magnitude) >= unitMaxSpeed)
        {
            return;
        }

        //adds force in direction to move unit
        rb.AddForce(directionToTarget * unitSpeed, ForceMode.Force);
    }


    //spawns atk object for entity
    void UnitAttack()
    {
        
    }


    //rotates the unit to face the target
    void RotateToTarget()
    {
        Vector3 lookAtRotation = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(lookAtRotation);

        //transform.LookAt(p);
    }

    //Finds the closest enemy to the unit
    public void FindTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(unitEnemy);

        //itterates through each target 
        for (int i = 0; i < targets.Length; i++)
        {

            if (i == 0)
            {
                target = targets[i];
            }
            //gets the distances
            var targetDistanceCheck = transform.position - targets[i].transform.position;
            var targetTwoDistanceCheck = transform.position - target.transform.position;

            if (targetDistanceCheck.magnitude <= targetTwoDistanceCheck.magnitude)
            {
                target = targets[i];
            }
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
}
