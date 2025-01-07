using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManger : MonoBehaviour
{
    private static UIManger instance;
    public static UIManger Instance=>instance;

    public GameObject hp;
    [Header("UI Elements")]
    public GameObject PauseMenu;
    public GameObject BoosHpActive;
    public Slider BoosHealthBar;
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public void Awake()

    {
        if (instance==null)
        {
            instance=this;
        }else
        {
            Destroy(gameObject);
        }
    }
    public void changeHp(float num){
      switch (num)
      {
        case 0:
                 hp.transform.GetChild(0).gameObject.SetActive(false);
                 hp.transform.GetChild(1).gameObject.SetActive(false);
                 hp.transform.GetChild(2).gameObject.SetActive(false);
        break;
        case 1:
                 hp.transform.GetChild(0).gameObject.SetActive(true);
                 hp.transform.GetChild(1).gameObject.SetActive(false);
                 hp.transform.GetChild(2).gameObject.SetActive(false);
        break;
        case 2:
                 hp.transform.GetChild(0).gameObject.SetActive(true);
                 hp.transform.GetChild(1).gameObject.SetActive(true);
                 hp.transform.GetChild(2).gameObject.SetActive(false);
        break;
        case 3:
                 hp.transform.GetChild(0).gameObject.SetActive(true);
                 hp.transform.GetChild(1).gameObject.SetActive(true);
                 hp.transform.GetChild(2).gameObject.SetActive(true);
        break;
      }
    }
    public void PuseGame(){
      PauseMenu.SetActive(true);
      Time.timeScale=0;
    }
    public void ResuGame(){
      PauseMenu.SetActive(false);
      Time.timeScale=1;
    }
    public void SetBoosHealth(float Health){
      BoosHealthBar.maxValue=Health;
    }
    public void UpdateBoosHealth(float Health){
      BoosHealthBar.value=Health;
    }
    public void RestartScence(){
      PlayerPrefs.DeleteKey("PlayerHealth");
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      Time.timeScale=1;
    }
    public void GameOverUI(bool PlayerDead){
      GameOverPanel.SetActive(PlayerDead);
    }
    public void BoosHealhActive(bool isBoos){
        BoosHpActive.SetActive(isBoos);
    }
    public void QuitGame(){
      Application.Quit();
    }
    public void WinPanelUI(){
      WinPanel.SetActive(true);
      MusicManager.instance.Win();
      StartCoroutine(WinPanelOff());
    }
    IEnumerator WinPanelOff(){
      yield return new WaitForSeconds(1);
      Time.timeScale=0;
    }
}
