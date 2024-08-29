using System;
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
    [SerializeField] TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI modeText;
    [SerializeField] private TextMeshProUGUI controlsText;
    [SerializeField] float lerpTime;
    [SerializeField] float backgroundAlpha;

    [Header("Blue Order Prefabs")]
    [SerializeField] GameObject blueBrawler;
    [SerializeField] GameObject blueKnight;

    [Header("Yellow Order Prefabs")]
    [SerializeField] GameObject yellowBrawler;
    [SerializeField] GameObject yellowKnight;

    [Header("UIManager Componenets")] [SerializeField]
    private Cursor cursor;
    bool inBlueMenu;
    bool exitBlueMenu;
    bool blueSelect;
    bool yellowSelect;
    private bool inEarase;

    float blueState = 0;
    float teamSelectState = 0;

    private void Start()
    {
        controlsText.text =
            "Reset: C  | Erase: E | Switch Troop Team: L | Open Buy Menu: B | Troop Select: 1-2 | Toggle Cursor: P |Start Battle: Tab";

    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        ModeText();
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

    void ModeText()
    {
        if (inEarase != true)
        {
            if (blueSelect == true)
                modeText.text = "Mode: Blue Place";
            else if (yellowSelect == true)
                modeText.text = "Mode: Yellow Place";
            else
                modeText.text = "Mode: No mode selected";
        }
        else
        {
            modeText.text = "Mode: Erase";
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
                menuText.gameObject.SetActive(true);
                inBlueMenu = false;
            }
        }
        else if(exitBlueMenu == true)
        {
            if (blueOrderBackground.color.a == 0f)
            {
                brawlerIcon.gameObject.SetActive(false);
                knightIcon.gameObject.SetActive(false);
                menuText.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (teamSelectState == 0)
            {
                menuText.text = "Blue Order";
                blueSelect = true;
                yellowSelect = false;
                teamSelectState++;
            }
            else
            {
                menuText.text = "Yellow Order";
                blueSelect = false;
                yellowSelect = true;
                teamSelectState = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(blueSelect == true)
            {
                SetCursorEntity(blueBrawler);
            }
            else if(yellowSelect == true)
            {
                SetCursorEntity(yellowBrawler);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(blueSelect == true)
            {
                SetCursorEntity(blueKnight);
            }
            else if (yellowSelect == true)
            {
                SetCursorEntity(yellowKnight);
            }

        }
            
    }

    public void SetCursorObj(Cursor currentCursor)
    {
        cursor = currentCursor;
    }

    public void SetInErase(bool isInErase)
    {
        inEarase = isInErase;
    }
}
