using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 图鉴面板的单位基本信息显示
/// </summary>
public class Item_Info : MonoBehaviour {

    public Text item_Name;
    public Image blackImage;
    public Item.Camp item_Camp;
    public Item.Type item_Type;
    public void NameData(Item item)
    {
        
        if (item == null)
        {
            return;
        }
        this.item_Camp = item.Item_Camp;
        this.item_Type = item.Item_Type;
        this.item_Name.text = item.item_Name;
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
        blackImage.color = new Color(0,0,0,1);
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="waitTiem"></param>
    /// <returns></returns>
    public IEnumerator  DisPlay(float waitTiem)
    {
        yield return new WaitForSeconds(waitTiem);
        this.gameObject.SetActive(true);
        blackImage.DOColor(new Color(0,0,0,0),0.2f).SetEase(Ease.Linear);
    }
}
