using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; /* bu paket text nesnelerini kontrol� sa�lar*/


public class PuanManager : MonoBehaviour
{
    private int toplamPuan;
    private int puanArtisi; /*zorluk derecesine g�re*/


    [SerializeField]
    private Text puanText;


    void Start()
    {
        puanText.text = toplamPuan.ToString();
    }
    /*GameManager dan kontrol edilecek*/
    public void PuaniArtir(string zorlukSeviyesi)
    {
        switch (zorlukSeviyesi)
        {
            case "kolay":
                puanArtisi = 5;
                break;

            case "orta":
                puanArtisi = 10;
                break;

            case "zor":
                puanArtisi = 15;
                break;
        }

        toplamPuan += puanArtisi;

        puanText.text = toplamPuan.ToString();
    }

   
}
