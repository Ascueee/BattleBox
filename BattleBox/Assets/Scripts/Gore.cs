using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gore : MonoBehaviour
{
    [Header("Gore System Vars")]
    [SerializeField] Vector3 scale;
    [SerializeField] GameObject fleshWoundObj;
    [SerializeField] GameObject bloodParticleEffect;

    [Header("Body Parts")]
    [SerializeField] Transform unitHead;
    [SerializeField] Transform unitRightArm;
    [SerializeField] Transform unitLeftArm;
    [SerializeField] Transform unitRightLeg;
    [SerializeField] Transform unitLeftLeg;

    [Header("Body Parts Objects")]
    [SerializeField] GameObject unitHeadObj;
    [SerializeField] GameObject unitArmObj;
    [SerializeField] GameObject unitLeg;
    string dmgType;
    bool cutOffLimb = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cutOffLimb == true)
            SlashDmgGore();
    }


    //does slash damage to ragdoll
    void SlashDmgGore()
    {
        //checks dmg type
        if(dmgType == "Slash")
        {
            print("GORE HAPPENING AHHH");
            //checks if the ragdoll can instantiate the limb
            if(cutOffLimb == true)
            {
                //randomly pick out a limb
                int randLimbHit = Random.RandomRange(0, 5);
                print("LimbGone is: " + randLimbHit);
                //instantiate gore effects onto ragdoll
                if (randLimbHit == 0)
                {
                    unitHead.transform.localScale = scale;
                    var fleshWound = Instantiate(fleshWoundObj, unitHead.transform.position, Quaternion.identity);
                    var bloodVfx = Instantiate(bloodParticleEffect, unitHead.transform.position, Quaternion.identity);
                    var headObj = Instantiate(unitHeadObj, unitHead.transform.position, Quaternion.identity);
                    fleshWound.GetComponent<FleshWound>().SetFleshWoundLocation(unitHead.transform);
                    bloodVfx.GetComponent<FleshWound>().SetFleshWoundLocation(unitHead.transform);
                }
                else if (randLimbHit == 1)
                {
                    unitRightArm.transform.localScale = scale;
                    var fleshWound = Instantiate(fleshWoundObj, unitRightArm.transform.position, Quaternion.identity);
                    var bloodVfx = Instantiate(bloodParticleEffect, unitRightArm.transform.position, Quaternion.identity);
                    var rightArmObj = Instantiate(unitArmObj, unitRightArm.transform.position, Quaternion.identity);
                    fleshWound.GetComponent<FleshWound>().SetFleshWoundLocation(unitRightArm.transform);
                    bloodVfx.GetComponent<FleshWound>().SetFleshWoundLocation(unitRightArm.transform);
                }
                else if (randLimbHit == 2)
                {
                    unitLeftArm.transform.localScale = scale;
                    var fleshWound = Instantiate(fleshWoundObj, unitLeftArm.transform.position, Quaternion.identity);
                    var bloodVfx = Instantiate(bloodParticleEffect, unitLeftArm.transform.position, Quaternion.identity);
                    var leftArmObj = Instantiate(unitArmObj, unitLeftArm.transform.position, Quaternion.identity);
                    fleshWound.GetComponent<FleshWound>().SetFleshWoundLocation(unitLeftArm);
                    bloodVfx.GetComponent<FleshWound>().SetFleshWoundLocation(unitLeftArm);
                }   
                else if (randLimbHit == 3)
                {
                    unitRightLeg.transform.localScale = scale;
                    var fleshWound = Instantiate(fleshWoundObj, unitRightLeg.transform.position, Quaternion.identity);
                    var bloodVfx = Instantiate(bloodParticleEffect, unitRightLeg.transform.position, Quaternion.identity);
                    var rightLegObj = Instantiate(unitLeg, unitRightLeg.transform.position, Quaternion.identity);
                    fleshWound.GetComponent<FleshWound>().SetFleshWoundLocation(unitRightLeg);
                    bloodVfx.GetComponent<FleshWound>().SetFleshWoundLocation(unitRightLeg);
                }
                else
                {
                    unitLeftLeg.transform.localScale = scale;
                    var fleshWound = Instantiate(fleshWoundObj, unitLeftLeg.transform.position, Quaternion.identity);
                    var bloodVfx = Instantiate(bloodParticleEffect, unitLeftLeg.transform.position, Quaternion.identity);
                    var leftLegObj = Instantiate(unitLeg, unitLeftLeg.transform.position, Quaternion.identity);
                    fleshWound.GetComponent<FleshWound>().SetFleshWoundLocation(unitLeftLeg);
                    bloodVfx.GetComponent<FleshWound>().SetFleshWoundLocation(unitLeftLeg);
                }
                    

                cutOffLimb = false;
            }
        }
    }

    public void SetDmgType(string dmgTaken)
    {
        dmgType = dmgTaken;
    }




}
