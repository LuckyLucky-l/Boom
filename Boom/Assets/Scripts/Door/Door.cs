using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    void Awake()
    {
        anim=GetComponent<Animator>();
        coll=GetComponent<Collider2D>();
    }
    void Start()
    {
        coll.enabled=false;
        GameManager.instance.IsExitDoor(this);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogError(other.name);
        Debug.Log("触发");
        if (other.CompareTag("Player"))
        {
            //进到下一层
            if (SceneManager.GetActiveScene().buildIndex==3)
            {
                UIManger.Instance.WinPanelUI();
            }else{
                Debug.LogError("没有下一层");
                GameManager.instance.NextLevel();
            }
        }
    }
   public void OpenDoor(){
        coll.enabled=true;
        anim.Play("Opening");
    }
}
