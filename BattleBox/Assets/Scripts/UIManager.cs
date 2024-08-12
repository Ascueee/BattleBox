using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image blueOrderBackground;
    [SerializeField] Image brawlerIcon;
    [SerializeField] Image knightIcon;
    [SerializeField] TextMeshProUGUI blueOrderMenuText;
    [SerializeField] float lerpTime;
    [SerializeField] float backgroundAlpha;

    [Header("Blue Order Prefabs")]
    [SerializeField] GameObject blueBrawler;
    [SerializeField] GameObject blueKnight;

    [Header("Yellow Order Prefabs")]
    [SerializeField] GameObject yellowBrawler;
    [SerializeField] GameObject yellowKnight;

    [Header("UIManager Componenets")]
    [SerializeField] Cursor cursor;
    bool inBlueMenu;
    bool exitBlueMenu;
    bool blueSelect;

    float blueState = 0;


    
    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        UnitSelectInput();

        TurnOnOffBlueOrderMenu();

    }

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.B) )
        {
            if(blueState == 0)
            {
                inBlueMenu = true;
                blueSelect = true;
                blueState++;
            }
            else
            {
                exitBlueMenu = true;
                blueSelect = false;
                blueState = 0;
            }
        }
    }

    void TurnOnOffBlueOrderMenu()
    {
        LeanTween.init();

        if(inBlueMenu == true)
        {
            BlueMenuOn();
        }

        if(exitBlueMenu == true)
        {
            BlueMenuOff();
        }
    }


    void BlueMenuOn()
    {
        LeanTween.value(blueOrderBackground.gameObject, blueOrderBackground.color.a, backgroundAlpha, lerpTime).setOnUpdate(ChangeBlueBackground);
    }

    void BlueMenuOff()
    {
        LeanTween.value(blueOrderBackground.gameObject, blueOrderBackground.color.a, 0f, lerpTime).setOnUpdate(ChangeBlueBackground);
    }

    void ChangeBlueBackground(float a)
    {
        var alphaChange = new Vector4(blueOrderBackground.color.r, blueOrderBackground.color.g, blueOrderBackground.color.b, a);
        blueOrderBackground.color = alphaChange;

        if(inBlueMenu == true)
        {
            if (blueOrderBackground.color.a == backgroundAlpha)
            {
                brawlerIcon.gameObject.SetActive(true);
                knightIcon.gameObject.SetActive(true);
                blueOrderMenuText.gameObject.SetActive(true);
                inBlueMenu = false;
            }
        }
        else if(exitBlueMenu == true)
        {
            if (blueOrderBackground.color.a == 0f)
            {
                brawlerIcon.gameObject.SetActive(false);
                knightIcon.gameObject.SetActive(false);
                blueOrderMenuText.gameObject.SetActive(false);
                exitBlueMenu = false;
            }
        }

    }


    void SetCursorEntity(GameObject entity)
    {
        if (cursor != null)
            cursor.SetEntitySpawner(entity);
    }
    void UnitSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(blueSelect == true)
            {
                SetCursorEntity(blueBrawler);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(blueSelect == true)
            {
                SetCursorEntity(blueKnight);
            }

        }
            
    }

    public void SetCursorObj(Cursor currentCursor)
    {
        cursor = currentCursor;
    }
}
