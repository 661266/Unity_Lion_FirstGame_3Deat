using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    #region 欄位與屬性
    /// <summary>
    /// 玩家變形元件
    /// </summary>
    private Transform Player;

    [Header("追蹤速度"), Range(0.1f, 50.5f)]
    public float speed = 1.5f;
    #endregion

    #region 方法
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        //攝影機與小明的距離 Y軸   5   - 3 =  2
        //攝影機與小明的距離 Z軸  -2.1 - 0 = -2.1
        Vector3 posTrack = Player.position;
        posTrack.y += 3f;
        posTrack.z += -3f;

        //攝影機座標 = 變形.座標
        Vector3 posCam = transform.position;
        //攝影機座標 = 三維向量.插值(A點,B點,百分比)
        posCam = Vector3.Lerp(posCam, posTrack, 0.5f * Time.deltaTime * speed);
        //變形.座標=攝影機座標
        transform.position = posCam;
    }
    #endregion

    #region 事件

    public float A = 0;
    public float B = 1000;

    public Vector2 V2A = Vector2.zero;
    public Vector2 V2B = Vector2.one * 1000;

    private void Update()
    {
        A = Mathf.Lerp(A, B, 0.5f);

        V2A = Vector2.Lerp(V2A, V2B, 0.5f);

    }
    #endregion

    private void Start()
    {
        //小明物件 = 遊戲物件.尋找("物件名稱").變形
        Player = GameObject.Find("小明").transform;
    }

    //延遲更新:會在Update執行後執行
    //建議:需要追蹤的座標要寫在此事件內
    private void LateUpdate()
    {
        Track();
    }

}
