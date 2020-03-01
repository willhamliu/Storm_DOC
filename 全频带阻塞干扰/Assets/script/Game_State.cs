using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Game_State : MonoBehaviour
{
    public Button combat;
    public Button demo;
    public Button setting;
    public Button list;
    public Button quit;



    public Button combat_Close;
    public Button list_Close;
    public Button setting_Close;

    public Button quit_Confrim;
    public Button quit_Cancel;

    List<Button> buttons = new List<Button>();

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
        buttons.Add(combat);
        buttons.Add(demo);
        buttons.Add(setting);
        buttons.Add(list);
        buttons.Add(quit);

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
                UI_Management.ui_Management.Combat_OpenOnClick();
                break;
            case Gamestate.Demo:
                UI_Management.ui_Management.DemoOnClick();
                break;
            case Gamestate.Settingopen:
                UI_Management.ui_Management.Setting_OpenOnClick();
                break;
            case Gamestate.Listopen:
                UI_Management.ui_Management.List_OpenOnClick();
                break;
            case Gamestate.Quit:
                UI_Management.ui_Management.QuitOnClick();
                break;
            case Gamestate.Combatcloce:
                UI_Management.ui_Management.Combat_CloseOnClick();
                break;
            case Gamestate.Listclose:
                UI_Management.ui_Management.List_CloseOnClick();
                break;
            case Gamestate.Settingclose:
                UI_Management.ui_Management.Setting_CloseOnClick();
                break;
            case Gamestate.Quitconfrim:
                UI_Management.ui_Management.Quit_ConfrimOnClick();
                break;
            default:
                UI_Management.ui_Management.Quit_CancelOnClick();
                break;
        }
    }
}
