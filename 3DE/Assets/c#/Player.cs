using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位與屬性
    [Header("移動速度"), Range(1, 1000)]
    public float speed = 10;
    [Header("跳躍高度"), Range(1, 5000)]
    public float height;

    /// <summary>
    /// 是否在地板上
    /// </summary>
    /// 

    private bool isGround 
    {
        get
        {       
                if (transform.position.y < 0.09f) return true;
                else return false;
        }
    }

    /// <summary>
    /// 選轉角度
    /// </summary>
    private Vector3 angle;

    private Animator ani;    //動畫
    private Rigidbody rig;   //剛體
    private AudioSource aud; //喇叭
    private GameManager gm;  //遊戲管理器
    /// <summary>
    /// 跳躍力道:從0開始
    /// </summary>
    private float  jump;
    [Header("雞腿音效")]
    public AudioClip sonudChicken;
    [Header("紅酒音效")]
    public AudioClip sonudBeer;

    #endregion


    #region 方法
    /// <summary>
    /// 移動:透過鍵盤
    /// </summary>
    private void Move()
    {
        #region 移動
        //浮點數 前後值 = 輸入類別.取得前後值("垂直") - 垂直 WS 上下
        float v = Input.GetAxisRaw("Vertical");
        //浮點數 AD 左右
        float h = Input.GetAxisRaw("Horizontal");

        //剛體.添加推力 -世界座標
        // rig.AddForce(0, 0, speed * v);
        //剛體.添加推力(三圍向量) -朝著角色方向做運動
        //前方transform.forward -Z
        //右方transform.right   -X  
        //上方transform.up      -Z
        rig.AddForce(transform.forward * speed * Mathf.Abs(v));
        rig.AddForce(transform.forward * speed * Mathf.Abs(h));
        //動畫.設定布林值("跑步開關,布林值") -當 前後取絕對值 大於0時勾選
        ani.SetBool("跑步開關", Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0);
        //ani.SetBool("跑步開關", v == 1 || v==-1 ); //使用邏輯運算子
        #endregion

        #region 轉向
        Vector3 angle = Vector3.zero; //區域變動 - 只能在此結構() 內互動
             if (v == 1) angle = new Vector3(0, 0, 0);           //前 Y 0
        else if (v == -1) angle = new Vector3(0, 180, 0);   //後 Y 180
        else if (h == 1) angle = new Vector3(0, 90, 0);     //左 Y 90
        else if (h == -1) angle = new Vector3(0, 270, 0);   //右 Y 270
        //只要類別後面有:MonoBehaviour
        //就可以直接使用關鍵字 Transform 取得此物件的 Transform元件
        //eulerAngles 歐拉角度 0 - 360
        transform.eulerAngles = angle; 
        print("角度:" + angle);
        #endregion
    }


    /// <summary>
    /// 跳躍:判斷在地板上並按下空白建
    /// </summary>
    private void Jump()
    {
        //如果 在地板上 有 勾選 定且按下空白建
        if (isGround && Input.GetButtonDown("Jump"))
        {
            //每次跳躍 值都從0開始
            jump = 0;
            //剛體.推力(0,跳躍高度,0)
            rig.AddForce(0, height, 0);
        }
        //如果不再地板上(空中)
        if (!isGround)
        {
            //跳躍 遞增 時間.一貞時間
            jump += Time.deltaTime;
        }
        //動畫.設定浮點數("跳躍力道")跳躍時間;
        ani.SetFloat("跳躍力道", jump);

    }

    /// <summary>
    /// 碰到道具:碰到帶有標籤帶有[雞腿]的物件
    /// </summary>
    private void HitProp(GameObject prop)
    {
        if (prop.tag == "雞腿")
        {
            aud.PlayOneShot(sonudChicken, 2); //喇叭.播放一次音效(音效片段.音量)
            Destroy(prop);                    //刪除.(物件)
        }
        else if (prop.tag == "紅酒")
        {
            aud.PlayOneShot(sonudBeer, 2);
            Destroy(prop);
        }
        gm.Getprop(prop.tag);
    }

   
    #endregion

    #region 事件
    private void Start()
    {
        //GetComponent<泛型>() 泛型方法 - 泛型 所有類型 Rigidbody,Transform,Collider...
        //剛體 = 取得元件 <剛體>();
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        //FOOT 僅限於場景上只有一個類別存在時使用
        //例如:場景上只有一個GameManager類別時可以使用他來取得
        gm = FindObjectOfType<GameManager>();
    }

    //固定更新頻率: 1秒 50貞,使用物理必須在此事件內
    private void FixedUpdate()
    {
        Move();
    }
    //更新事件: 一秒約60貞
    private void Update()
    {
        Jump();
    }


    private void OnTriggerEnter(Collider other)
    {
        //碰到道具(碰撞資訊.標籤)
        HitProp(other.gameObject);
    }
    #region 碰撞事件&觸發事件:介紹
    /*
    //碰撞事件:當物件碰撞開始時實執行一次,(沒有勾選 Is Trigger)
    private void OnCollisionEnter(Collision other)
    {
       
    }
    

    //碰撞事件:當物件碰撞開始時實執行一次,(沒有勾選 Is Trigger)
    private void OnCollisionEnter(Collision collision)
    {

    }
    //碰撞事件:當物件碰撞離開後實執行一次,(沒有勾選 Is Trigger)
    private void OnCollisionExit(Collision collision)
    {

    }
    //碰撞事件:當物件碰撞開始時持續實執行(沒有勾選 Is Trigger)
    private void OnCollisionStay(Collision collision)
    {

    }

    /*___________*/
    /*//觸發事件:當物件碰撞開始時實執行一次,(有勾選 Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
   
    }
    /*
    //觸發事件:當物件碰撞離開後實執行一次,(有勾選 Is Trigger)
    private void OnTriggerExit(Collider other)
    {

    }
    //觸發事件:當物件碰撞開始時持續實執行(有勾選 Is Trigger)
    private void OnTriggerStay(Collider other)
    {

    }
    */
    #endregion



    #endregion



}
