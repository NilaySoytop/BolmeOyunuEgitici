using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    /*co�altaca��m�z nesne icin: */
    [SerializeField]
    private GameObject karePrefab;

    /*prefablar� yukleyece�imiz nesne gerekli*/
    [SerializeField]
    private Transform karelerPaneli;

    private GameObject[] karelerDizisi = new GameObject[25]; /*kareleri olusturaca��z*/

    [SerializeField]
    private Transform soruPaneli;


    /*soruText in i�erisine sorumuzu yazd�rma i�lemi*/
    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Sprite[] kareSprites;

    /*b�l�m de�erlerini listede tutal�m*/
    List<int> bolumDegerleriListesi = new List<int>();

    /*B�len ve b�l�nen say� olmak �zere 2 adet int degisken belirlendi*/
    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int dogruSonuc;
    /*butona bas�l�nca yaz�lan de�eri int aktarma*/
    int butonDegeri;
    bool butonaBasilsinmi;

    /*Kalanhaklar kontrolu burada yapt�r�yorum*/
    int kalanHak;

    string sorununZorlukDerecesi;

    /*Bir script i�erisinden ba�ka bir scripte ula�ma*/
    KalanHaklarManager kalanHaklarManager;

    PuanManager puanManager;

    GameObject gecerliKare; /*hangi butona bas�ld���n� ak�lda tutan nesne tan�mlad�k */

    [SerializeField]
    private GameObject sonucPaneli; /*sonucPaneli nesnesi tan�mlad�k*/

    [SerializeField]
    AudioSource audioSource;

    public AudioClip butonSesi; /* oynat�lacak ses*/

    private void Awake()
    {
        kalanHak = 3;

        audioSource = GetComponent<AudioSource>(); /*GameManager in ba�l� old. nesne i�erisindeki audio source ye ulast�k*/


        /*oyun baslar baslamaz sonuc paneli local scale de�eri s�f�r yap�ld�. sonuc panelinin baslang��ta g�r�nmemesini sa�lar*/
        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;


        kalanHaklarManager = Object.FindObjectOfType<KalanHaklarManager>(); /*NESNE ULASILDI*/
        puanManager = Object.FindObjectOfType<PuanManager>(); /*puanManager ismine sahip script dosyas�n� bul*/
        kalanHaklarManager.KalanHaklariKontrolEt(kalanHak); /*NESNE ��ER�S�NDEK� FONKS KONTROL ED�YORUZ. Ba�ta 3 kalp de a��k g�z�kmesi sa�land�*/
        
    }

    void Start()
    {
        butonaBasilsinmi = false;

        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;/*soru panelinin scale de�erinin ebatlar�n� s�f�ra cektik (soru paneli g�zukmez)*/


        kareleriOlustur();
    }

    public void kareleriOlustur()
    {
        for(int i = 0; i < 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);
            /*Butona bas�ld���nda bir olay ekleme*/
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)]; /*karesprites dizisinden rastgele eleman secip resim atayaca��z*/
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            /*kareleri kareler dizisine atal�m*/
            karelerDizisi[i] = kare;
        }

        BolumDegerleriniTexteYazdir(); /*rastgele kare de�erleri olusacak*/
        StartCoroutine(DoFadeRoutine()); /*DoFadeRoutine cal�st�r�ld� (kareler i�in)*/
        Invoke("SoruPaneliniAc", 2f); /*sroupaneli 2 saniye gecikmeli ac�lacak*/
    }

    void ButonaBasildi()
    {
        if (butonaBasilsinmi)
        {
            audioSource.PlayOneShot(butonSesi); /*butona bas�l�nca buton sesi �al��s�n*/

            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;/*butona bas�l�nca resim gelme i�lemi i�in */

            SonucuKontrolEt();
        }

    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            /*butona t�klan�nca do�ruysa butona bas�ld���nda �mage k�sm� akt�f edilsin*/
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";/*kutunun texti kaybolsun*/
            gecerliKare.transform.GetComponent<Button>().interactable = false;/*butona resim ac�ld�ktan sonra tekrar bas�lamas�n*/


            puanManager.PuaniArtir(sorununZorlukDerecesi);/*sorunun zorluk derecesine g�re puan artacak*/
            bolumDegerleriListesi.RemoveAt(kacinciSoru);

            if (bolumDegerleriListesi.Count > 0)
            {
                SoruPaneliniAc();
            }
            else
            {
                OyunBitti();
            }
        }


        else
        {
            kalanHak--; /*yanl�� yapt���m�zda kalan haklar eksilmeli*/
            kalanHaklarManager.KalanHaklariKontrolEt(kalanHak); /*kalan haklar� kontrol ediyor*/
        }

        if (kalanHak <= 0)
        {
            OyunBitti();
        }
    }


    void OyunBitti()
    {
        butonaBasilsinmi = false; /*oyun bitince rakam butonlar�na bas�lmas�n*/
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);/*sonuc paneli ac�l�s�*/

    }



    /*Kareler yavasca gelsin*/
    IEnumerator DoFadeRoutine()
    {
        foreach(var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

            yield return new WaitForSeconds(0.07f); /*her 0.2 saniyede kodu cevir*/
        }
    }


    void BolumDegerleriniTexteYazdir()
    {
        foreach(var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);

            /*rastgele belirlenen 25 degeri listeye atama*/
            bolumDegerleriListesi.Add(rastgeleDeger);

            kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString(); /*Kare i�erisindeki textin 1. child k�sm�na ulas�yoruz*/
        }
        
    }

    /*StartCoroutine ac�ld�ktan sonra soru paneli k�sm� gelmesi i�in:*/
    void SoruPaneliniAc()
    {
        SoruyuSor();
        butonaBasilsinmi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack); /*sorupaneli ac�lmas� i�in tekrar rectransform de�erini de�i�tirece�iz*/
    }

    /*Soru paneli i�in soru haz�rlama */
    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);

        kacinciSoru = Random.Range(0, bolumDegerleriListesi.Count);/*cevap farkl� kutucuklarda olsun*/
        dogruSonuc = bolumDegerleriListesi[kacinciSoru];
        bolunenSayi = bolenSayi * dogruSonuc;/* b�l�m degerleri listesinden say� alacak ve bolen say� ile �arparak b�l�nen say� bulunacak. Bu i�lem sonucun varl���n� garantilemek amac� ile yap�ld�*/
        if (bolunenSayi <= 40)
        {
            sorununZorlukDerecesi = "kolay";
        }
        else if (bolunenSayi > 40 && bolunenSayi <= 80)
        {
            sorununZorlukDerecesi = "orta";
        }
        else
        {
            sorununZorlukDerecesi = "zor";
        }

        soruText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString();
    }
}
