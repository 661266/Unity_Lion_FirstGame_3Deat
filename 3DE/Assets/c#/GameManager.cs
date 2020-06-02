using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI
using UnityEngine.SceneManagement; //場景

public class GameManager : MonoBehaviour
{
    #region 屬性與欄位
    [Header("道具")]
    public GameObject[] porps;
    [Header("文字介面:道具總數")]
    public Text textCount;
    [Header("時間")]
    public Text textTime;
    [Header("文字介面:標題")]
    public Text textTitle;
    [Header("結束畫面")]
    public CanvasGroup final;


    /// <summary>
    /// 道具數量
    /// </summary>
    private int countTotal;

    /// <summary>
    /// 取得道具數量
    /// </summary>
    private int countProp;
    /// <summary>
    /// 遊戲時間
    /// </summary>

    private float gameTime = 30; 
    #endregion

    #region 方法
    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="prop">想要生成的道具數量</param>
    /// <param name="count">想要生成的道具數量+隨機值 + - 5 </param>
    /// <returns>傳回生成幾顆</returns>
    private int CreateProp(GameObject prop,int count)
    {
        //取得隨機道具數量-指定的數量 + - 5
        int total = count + Random.Range(-5, 5);

        //for迴圈
        for (int i = 0; i < total; i++)
        {
            //座標 = (隨機,1.5,隨機)
            Vector3 pos = new Vector3(Random.Range(-9, 9), 1.5f, Random.Range(-9, 9));
            //生成(物件,座標,角度)
            Instantiate(prop, pos, Quaternion.identity);
        }
        //回傳道具數量
        return total;
    }
    
    /// <summary>
    /// 時間倒數
    /// </summary>
    private void CountTime()
    {
        if (countProp == countTotal) return;
        //遊戲時間 遞減 一貞的時間
        gameTime -= Time.deltaTime;

        //遊戲時間 = 數學.夾住(遊戲時間,最小值,最大值)
        gameTime = Mathf.Clamp(gameTime, 0, 100);

        //更新倒數時間介面ToString("f小數點位數")
        textTime.text = "倒數時間:" + gameTime.ToString("f2");
        Lose();
    }

    #endregion

    public void Getprop(string prop)
    {
        if (prop =="雞腿")
        {
            countProp++;
            textCount.text = "道具數量" + countProp + "/" + countTotal;
            Win(); //呼叫勝利方法
        }
        else if (prop == "紅酒")
        {
            gameTime -= 2;
            textTime.text = "時間倒數" + gameTime.ToString("f2");
           Lose();
        }
    }
    #region 勝利吃完雞腿
    private void Win()
    {
        if (countProp == countTotal)        //如果雞腿數量 = 雞腿總數
        {
            final.alpha = 1;                //顯示結束畫面-啟動互動-啟動遮罩
            final.interactable = true;
            final.blocksRaycasts = true;    
            textTitle.text = "甲霸金困";     //更新結束畫面標題
        }
    }
    #endregion

    #region 失敗未吃完

    private void Lose()
    {
        if (gameTime == 0)
        {
            final.alpha = 1;                
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "回去再吃";
            FindObjectOfType<Player>().enabled = false; //取得玩家.啟動 = false
        }
    }

    #endregion
    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }
    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit(); //應用程式.離開
    }




    #region 事件
    private void Start()
    {
        countTotal = CreateProp(porps[0], 5); //道具總數 - 生成道具(道具一號,指定數量)

        textCount.text = "道具數量: 0 / " + countTotal;

        CreateProp(porps[1], 10); //生成(道具二號)
    }

    private void Update()
    {
        CountTime();
    }
    #endregion
}
