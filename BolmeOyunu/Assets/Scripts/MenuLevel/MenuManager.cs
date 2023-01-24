using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement; /*sahneler aras� gecis icin*/
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startBtn, exitBtn;
    
    void Start()
    {
        FadeOut(); /*start ve exit buton �al��mas� icin fadeOut() fonks. cal�st�rd�k.*/
    }


    void Update()
    {
        
    }
    void FadeOut()
    {
       startBtn.GetComponent<CanvasGroup>().DOFade(1,0.7f); /*startbutonun alpha de�erini acma islemi*/
       exitBtn.GetComponent<CanvasGroup>().DOFade(1, 0.7f).SetDelay(0.5f); /*exit butonun alpha degeri ac�larak gelsin. start butondan gecikmeli ac�lmas� icin SetDelay kulland�m*/

    }

    /*oyundan c�k�s*/
    public void ExitGame()
    {
        Application.Quit(); /*Oyundan c�k�s �slem�*/
    }

    /*start butonu ile sahne gecisi yap�ls�n*/
    public void StartGameLevel()
    {
        SceneManager.LoadScene("gameLevel");
    }
}
