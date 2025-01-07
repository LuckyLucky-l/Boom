using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player State")]
    public playerControl player;
    public static GameManager instance;
    public bool gameOver;//代表游戏结束
    public List<Enemy> enemies=new List<Enemy>();
    public Door doorExit;
    void Awake()
    {
            instance=this;
            Time.timeScale=1;
        // player=FindObjectOfType<playerControl>();
        // doorExit=FindObjectOfType<Door>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name!="Mian")
        {
            gameOver=player.isDead;
            UIManger.Instance.GameOverUI(player.isDead);//玩家和UI面板的桥梁
        }
    }
    public void IsEnemy(Enemy enemy){//添加怪物到列表
        enemies.Add(enemy);
    }
    public void EnemyDead(Enemy enemy){
        enemies.Remove(enemy);
        if (enemies.Count==0)
        {
            doorExit.OpenDoor();
            SaveData();
        }
    }
    public void NextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void SaveData(){
        PlayerPrefs.SetFloat("PlayerHealth",player.Healthy);
        PlayerPrefs.SetInt("ScenseIndex",SceneManager.GetActiveScene().buildIndex+1);
        PlayerPrefs.Save();        
    }
    public float LoadHealth(){
        if (!PlayerPrefs.HasKey("PlayerHealth"))
        {
            PlayerPrefs.SetFloat("PlayerHealth",3);
        }
       return PlayerPrefs.GetFloat("PlayerHealth");
    }
    public void IsPlayer(playerControl playerControl){
        player=playerControl;
    }
    public void IsExitDoor(Door door){
        doorExit=door;
    }
    public void NewGame(){
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }
    public void Continue(){
        if (PlayerPrefs.HasKey("ScenseIndex"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("ScenseIndex"));
            PlayerPrefs.GetFloat("PlayerHealth");
            if (LoadHealth()==0)
            {
                PlayerPrefs.SetFloat("PlayerHealth",1);
            }
        }else
        {
                NewGame();
        }            
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
