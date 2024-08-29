using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseUnit : MonoBehaviour
{
    [Header("Base Unit")]
    [SerializeField] string unitName;
    [SerializeField] string unitEnemy;


    [SerializeField] float unitHealth; 
    [SerializeField] float unitSpeed; 
    [SerializeField] float unitMaxSpeed; //max speed that the unit velocity cant go over
    [SerializeField] float unitDmg;
    [SerializeField] float unitDmgRange;
    [SerializeField] float unitVision;
    [SerializeField] float unitMaxSlopeAngle;
    [SerializeField] float unitCD;
    [SerializeField] string[] unitAnimNames;
    [SerializeField] string unitDamageType;//tells if unit does slash dmg, explosive dmg, projectile dmg
    [SerializeField] GameObject target; //current enemy the entity is trying to attack
    [SerializeField] LayerMask collisionObject;


    [Header("Unit Componenets")]
    [SerializeField] Animator unitAnim;
    [SerializeField] GameObject unityBody;
    [SerializeField] GameObject ragDollBody;
    [SerializeField] GameObject trackerObj;
    [SerializeField] GameObject weapon;
    [SerializeField] Transform deathSpawn;
    GameObject currentTracker;

    public Rigidbody rb;
    Vector3 distanceFromTarget;
    string damageTaken;

    bool inBattle;
    bool hasTarget;
    bool inCharge;
    bool inAttack;
    bool spawnRagdoll;
    bool canDie;
    bool avoidObtacle;
    bool canAttack;
    bool avoidObstacle;
    bool spawnTrackerObj;
    bool onSlope;
    private RaycastHit slopeHit;
    private Vector3 slopeMoveDir;
    

    private void Start()
    {
        canDie = true;
        spawnRagdoll = false;
        spawnTrackerObj = true;
        canAttack = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        CheckIfOnSlope();
        if(unitAnim != null)
            CheckStates();
        
        if (canDie == true)
            UnitDeath();

        //checks if the battle has started
        if (inBattle == true)
        {
            Invoke("CheckIfOnPathToObject", 1);

            if(avoidObstacle != true)
                FindTarget();
        }

        
        if (inAttack == true)
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

        if (currentTracker == null)
        {
            target = null;
            avoidObstacle = false;
            spawnTrackerObj = true;
        }
        
        if (hasTarget == true && distanceFromTarget.magnitude <= unitDmgRange)
        {
            if (avoidObstacle != true)
            {
                inAttack = true;
                unitAnim.SetBool(unitAnimNames[1], inAttack);
            }
            else
            {
                target = null;
                avoidObstacle = false;
                spawnTrackerObj = true;
                Destroy(currentTracker); //deletes current tracker object once unit gets close
            }
        }
        else
        {
            inAttack = false;
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
            slopeMoveDir = directionToTarget;

            //checks if velocity is over maxSpeed
            if (Mathf.Abs(rb.velocity.magnitude) >= unitMaxSpeed)
                return;

            //checks if the unit is on a slope
            if (onSlope != true)
                rb.AddForce(directionToTarget * unitSpeed, ForceMode.Force);
            else
                rb.AddForce(GetSlopeMoveDir() * unitSpeed, ForceMode.Force);


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
                collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 600f, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag != unitEnemy)
            CheckIfOnPathToObject();
    }
    

    //checks if the entity is on the path of hitting an object
    void CheckIfOnPathToObject()
    {
        Ray unitVisionRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * unitVision, Color.magenta);
        
        //Checks if entity hits an object that has collider
        if (Physics.Raycast(unitVisionRay, out RaycastHit hit, unitVision,collisionObject ))
        {

            print("Hitting some objects");
            if (spawnTrackerObj == true)
            {
                if (hit.collider.gameObject.tag != unitEnemy)
                {
                    var randDir = Random.Range(0, 2);
                    print(randDir);
                    Ray directionToGo;
                    //picks a random direction for the entity to go
                    if (randDir == 0)
                    {
                        directionToGo = new Ray(transform.position, transform.right);
                        currentTracker = Instantiate(trackerObj, directionToGo.GetPoint(3f), Quaternion.identity);
                    }
                    else if (randDir == 1)
                    {
                        directionToGo = new Ray(transform.position, -transform.right);
                        currentTracker = Instantiate(trackerObj, directionToGo.GetPoint(3f), Quaternion.identity);
                    }
                    target = currentTracker;
                    avoidObstacle = true;
                    spawnTrackerObj = false;
                }
            }
        }
    }

    void CheckIfOnSlope()
    {
        Ray slopeCheckRay = new Ray(transform.position, -transform.up);
        Debug.DrawRay(transform.position, -transform.up * 3f);

        if (Physics.Raycast(slopeCheckRay, out slopeHit, collisionObject))
        {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle < unitMaxSlopeAngle && angle != 0)
            {
                print("On slope!!!");
                onSlope = true;
            }
            else
            {
                onSlope = false;
            }
        }
    }

    Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(slopeMoveDir, slopeHit.normal);
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

    public void SetInBattle(bool isInBattle)
    {
        inBattle = isInBattle;
    }
}
