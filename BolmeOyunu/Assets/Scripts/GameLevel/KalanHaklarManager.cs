using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalanHaklarManager : MonoBehaviour
{
    /*kalpleri kontrol edece�iz*/
    [SerializeField]
    private GameObject kalanHak1,kalanHak2,kalanHak3;

    

    public void KalanHaklariKontrolEt(int kalanHak)
    {
        /*buray� GameManager i�erisinden kontrolunu yapaca��z*/
        /*kalan hakka g�re kalplerin g�r�n�rl���*/
        switch (kalanHak)
        {
            case 3: /*3 hakk�m�z varsa 3 kalp de g�r�ns�n*/
                kalanHak1.SetActive(true);
                kalanHak2.SetActive(true);
                kalanHak3.SetActive(true);
                break;

            case 2: /*2 hakk�m�z kald�ysa ilk 2 kalp de g�r�ns�n sonuncu g�r�nmesin*/
                kalanHak1.SetActive(true);
                kalanHak2.SetActive(true);
                kalanHak3.SetActive(false);
                break;

            case 1: /*1 hakk�m�z kald�ysa 1 kalp g�r�ns�n son iki kalp g�r�nmesin*/
                kalanHak1.SetActive(true);
                kalanHak2.SetActive(false);
                kalanHak3.SetActive(false);
                break;

            case 0: /*0 hakk�m�z kald�ysa 3 kalp de g�r�nmesin*/
                kalanHak1.SetActive(false);
                kalanHak2.SetActive(false);
                kalanHak3.SetActive(false);
                break;
        }
    }
}
