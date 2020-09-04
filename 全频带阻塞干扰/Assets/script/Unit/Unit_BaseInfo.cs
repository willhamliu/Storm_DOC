using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 对指定单位共有的显示信息进行管理
/// </summary>
public abstract class Unit_BaseInfo : Unit_Base
{
    public SpriteRenderer upLayer;
    public SpriteRenderer buffer;

    protected float Hurtspeed = 5f;//缓冲速度

    /// <summary>
    /// 单位死亡
    /// </summary>
    private void Death()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    ///受到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void Be_Hit(int damage)
    {
        hp -= damage;
        upLayer.size = new Vector2((hp / max_HP) * 10, upLayer.size.y);
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        while (buffer.size.x > upLayer.size.x)
        {
            buffer.size = new Vector2(buffer.size.x - (Hurtspeed * Time.deltaTime), buffer.size.y);
            yield return null;
        }
        if (buffer.size.x == 0)
        {
            Death();
        }
        buffer.size = upLayer.size;
    }
}
