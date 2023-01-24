using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement; /*sahneler arasý gecis icin*/
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startBtn, exitBtn;
    
    void Start()
    {
        FadeOut(); /*start ve exit buton çalýþmasý icin fadeOut() fonks. calýstýrdýk.*/
    }


    void Update()
    {
        
    }
    void FadeOut()
    {
       startBtn.GetComponent<CanvasGroup>().DOFade(1,0.7f); /*startbutonun alpha deðerini acma islemi*/
       exitBtn.GetComponent<CanvasGroup>().DOFade(1, 0.7f).SetDelay(0.5f); /*exit butonun alpha degeri acýlarak gelsin. start butondan gecikmeli acýlmasý icin SetDelay kullandým*/

    }

    /*oyundan cýkýs*/
    public void ExitGame()
    {
        Application.Quit(); /*Oyundan cýkýs ýslemý*/
    }

    /*start butonu ile sahne gecisi yapýlsýn*/
    public void StartGameLevel()
    {
        SceneManager.LoadScene("gameLevel");
    }
}
