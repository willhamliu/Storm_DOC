using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Game_State : MonoBehaviour
{
    public Button Combat;
    public Button Demo;
    public Button Setting;
    public Button List;
    public Button Quit;



    public Button combat_Close;
    public Button list_Close;
    public Button setting_Close;

    public Button quit_Confrim;
    public Button quit_Cancel;

    List<Button> buttons = new List<Button>();

    public Action<PointerEventData, GameObject> onClickDown { get; set; }

    public enum Gamestate
    {
        UnKown = -1,
        Combatopen,
        Demo,
        Settingopen,
        Listopen,
        Quit,
        Combatcloce,
        Listclose,
        Settingclose,
        Quitconfrim,
        Quitcancel
    }

    private Gamestate game_state = Gamestate.UnKown;

    void Awake()
    {
        Gather_button();
    }
    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var button_name = buttons[buttons.IndexOf(buttons[i])].name;

            buttons[i].onClick.AddListener(delegate() { State_Change(button_name);});
        }
    }

    public void Gather_button()
    {
        buttons.Add(Combat);
        buttons.Add(Demo);
        buttons.Add(Setting);
        buttons.Add(List);
        buttons.Add(Quit);

        buttons.Add(combat_Close);
        buttons.Add(list_Close);
        buttons.Add(setting_Close);
        buttons.Add(quit_Confrim);
        buttons.Add(quit_Cancel);
    }

    public void State_Change(string button_name)//状态转换
    {
        switch (button_name)
        {
            case "Combat":
                game_state = Gamestate.Combatopen;
                break;
            case "Demo":
                game_state = Gamestate.Demo;
                break;
            case "Setting":
                game_state = Gamestate.Settingopen;
                break;
            case "List":
                game_state = Gamestate.Listopen;
                break;
            case "Quit":
                game_state = Gamestate.Quit;
                break;
            case "Combat_Close_Button":
                game_state = Gamestate.Combatcloce;
                break;
            case "List_Close_Button":
                game_state = Gamestate.Listclose;
                break;
            case "Setting_Close_Button":
                game_state = Gamestate.Settingclose;
                break;
            case "Quit_confrim":
                game_state = Gamestate.Quitconfrim;
                break;
            default:
                game_state = Gamestate.Quitcancel;
                break;
        }
        switch (game_state)//状态行为
        {
            case Gamestate.UnKown:
                break;
            case Gamestate.Combatopen:
                UI_Management.UI_management.Combat_Open();
                break;
            case Gamestate.Demo:
                UI_Management.UI_management.Demo();
                break;
            case Gamestate.Settingopen:
                UI_Management.UI_management.Setting_Open();
                break;
            case Gamestate.Listopen:
                UI_Management.UI_management.List_Open();
                break;
            case Gamestate.Quit:
                UI_Management.UI_management.Quit();
                break;
            case Gamestate.Combatcloce:
                UI_Management.UI_management.Combat_Close();
                break;
            case Gamestate.Listclose:
                UI_Management.UI_management.List_Close();
                break;
            case Gamestate.Settingclose:
                UI_Management.UI_management.Setting_Close();
                break;
            case Gamestate.Quitconfrim:
                UI_Management.UI_management.Quit_Confrim();
                break;
            default:
                UI_Management.UI_management.Quit_Cancel();
                break;
        }
    }
}
