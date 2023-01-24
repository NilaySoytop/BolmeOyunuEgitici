using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalanHaklarManager : MonoBehaviour
{
    /*kalpleri kontrol edeceðiz*/
    [SerializeField]
    private GameObject kalanHak1,kalanHak2,kalanHak3;

    

    public void KalanHaklariKontrolEt(int kalanHak)
    {
        /*burayý GameManager içerisinden kontrolunu yapacaðýz*/
        /*kalan hakka göre kalplerin görünürlüðü*/
        switch (kalanHak)
        {
            case 3: /*3 hakkýmýz varsa 3 kalp de görünsün*/
                kalanHak1.SetActive(true);
                kalanHak2.SetActive(true);
                kalanHak3.SetActive(true);
                break;

            case 2: /*2 hakkýmýz kaldýysa ilk 2 kalp de görünsün sonuncu görünmesin*/
                kalanHak1.SetActive(true);
                kalanHak2.SetActive(true);
                kalanHak3.SetActive(false);
                break;

            case 1: /*1 hakkýmýz kaldýysa 1 kalp görünsün son iki kalp görünmesin*/
                kalanHak1.SetActive(true);
                kalanHak2.SetActive(false);
                kalanHak3.SetActive(false);
                break;

            case 0: /*0 hakkýmýz kaldýysa 3 kalp de görünmesin*/
                kalanHak1.SetActive(false);
                kalanHak2.SetActive(false);
                kalanHak3.SetActive(false);
                break;
        }
    }
}
