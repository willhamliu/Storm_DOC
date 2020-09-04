using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/// <summary>
/// 管理所有单位，对指定单位发出命令
/// </summary>
public class Unit_Management : MonoBehaviour
{
    public static Unit_Management instance;
    public Camera_Control camera_Control;

    public GameObject endRoundPanel;//结束回合确认面板
    public GameObject revocationPanel;
    public Dictionary<Vector2Int, GameObject> enemyCoordinate = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> friendlyCoordinate = new Dictionary<Vector2Int, GameObject>();

    bool unableChoose;
    Unit_Control selectedUnit;


    void Awake()
    {
        Config_Item.Instance.Config_Item_Json();//打包时记得注释
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        UI_Management.instance.AddButtonEventTrigger<Button>("Home_Button", ReturnHomeOnClick);
        UI_Management.instance.AddButtonEventTrigger<Button>("Revocat_Button", RevocationOnClick);
        //UI_Management.instance.AddButtonEventTrigger<Button>("EndRound_Button", EndRoundOnClick);
        //UI_Management.instance.AddButtonEventTrigger<Button>("EndRoundCancel_Button", EndRoundCancelOnClick);
        //UI_Management.instance.AddButtonEventTrigger<Button>("EndRoundConfirm_Button", EndRoundConfirmOnClick);

        camera_Control.cameraDrag = () => { unableChoose = true; };
        camera_Control.cameraDragEnd = () => { unableChoose = false; };
        endRoundPanel.SetActive(false);
        revocationPanel.SetActive(false);
    }

    public void Unit_Selected(Unit_Control unit_Control)
    {
        if (unableChoose || unit_Control.transform.tag == "Enemy") return;
        selectedUnit = unit_Control;
        if (selectedUnit.isCanMove)
            selectedUnit.MovePointDisplay();

        if (selectedUnit.isCanAttack)
            selectedUnit.AttackTargetDisplay();
    }

    public void Unit_Move(Vector2Int target)
    {
        selectedUnit.Unit_Move(target);
    }

    public void RevocationPanelDisPlay()//显示撤消面板
    {
        revocationPanel.SetActive(true);
    }

    void RevocationOnClick()
    {
        selectedUnit.Revocation();
        revocationPanel.SetActive(false);
    }

    void ReturnHomeOnClick()
    {
        SceneManager.LoadScene("Home");
    }

    void EndRoundOnClick()
    {
        //int unableActPlayerCount = 0;//无法行动的玩家数量
        //foreach (var item in Player_List)
        //{
        //    if (!item.Value.isCanAttack&& !item.Value.isCanMove)
        //    {
        //        unableActPlayerCount++;
        //    }
        //}
        //if (unableActPlayerCount == Player_List.Count)
        //{
        //    foreach (var item in Player_List)
        //    {
        //        item.Value.isCanAttack = true;
        //        item.Value.isCanMove = true;
        //    }
        //}
        //else
        //{
        //    endRoundPanel.SetActive(true);
        //}
    }

    /// <summary>
    /// 继续这次回合
    /// </summary>
    void EndRoundCancelOnClick()
    {
        endRoundPanel.SetActive(false);
    }

    /// <summary>
    /// 确认结束回合
    /// </summary>
    void EndRoundConfirmOnClick()
    {
        //foreach (var item in Player_List)
        //{
        //    item.Value.isCanAttack = true;
        //    item.Value.isCanMove = true;
        //}
        //endRoundPanel.SetActive(false);
    }
}
