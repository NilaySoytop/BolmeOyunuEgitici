using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    /*coðaltacaðýmýz nesne icin: */
    [SerializeField]
    private GameObject karePrefab;

    /*prefablarý yukleyeceðimiz nesne gerekli*/
    [SerializeField]
    private Transform karelerPaneli;

    private GameObject[] karelerDizisi = new GameObject[25]; /*kareleri olusturacaðýz*/

    [SerializeField]
    private Transform soruPaneli;


    /*soruText in içerisine sorumuzu yazdýrma iþlemi*/
    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Sprite[] kareSprites;

    /*bölüm deðerlerini listede tutalým*/
    List<int> bolumDegerleriListesi = new List<int>();

    /*Bölen ve bölünen sayý olmak üzere 2 adet int degisken belirlendi*/
    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int dogruSonuc;
    /*butona basýlýnca yazýlan deðeri int aktarma*/
    int butonDegeri;
    bool butonaBasilsinmi;

    /*Kalanhaklar kontrolu burada yaptýrýyorum*/
    int kalanHak;

    string sorununZorlukDerecesi;

    /*Bir script içerisinden baþka bir scripte ulaþma*/
    KalanHaklarManager kalanHaklarManager;

    PuanManager puanManager;

    GameObject gecerliKare; /*hangi butona basýldýðýný akýlda tutan nesne tanýmladýk */

    [SerializeField]
    private GameObject sonucPaneli; /*sonucPaneli nesnesi tanýmladýk*/

    [SerializeField]
    AudioSource audioSource;

    public AudioClip butonSesi; /* oynatýlacak ses*/

    private void Awake()
    {
        kalanHak = 3;

        audioSource = GetComponent<AudioSource>(); /*GameManager in baðlý old. nesne içerisindeki audio source ye ulastýk*/


        /*oyun baslar baslamaz sonuc paneli local scale deðeri sýfýr yapýldý. sonuc panelinin baslangýçta görünmemesini saðlar*/
        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;


        kalanHaklarManager = Object.FindObjectOfType<KalanHaklarManager>(); /*NESNE ULASILDI*/
        puanManager = Object.FindObjectOfType<PuanManager>(); /*puanManager ismine sahip script dosyasýný bul*/
        kalanHaklarManager.KalanHaklariKontrolEt(kalanHak); /*NESNE ÝÇERÝSÝNDEKÝ FONKS KONTROL EDÝYORUZ. Baþta 3 kalp de açýk gözükmesi saðlandý*/
        
    }

    void Start()
    {
        butonaBasilsinmi = false;

        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;/*soru panelinin scale deðerinin ebatlarýný sýfýra cektik (soru paneli gözukmez)*/


        kareleriOlustur();
    }

    public void kareleriOlustur()
    {
        for(int i = 0; i < 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);
            /*Butona basýldýðýnda bir olay ekleme*/
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)]; /*karesprites dizisinden rastgele eleman secip resim atayacaðýz*/
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            /*kareleri kareler dizisine atalým*/
            karelerDizisi[i] = kare;
        }

        BolumDegerleriniTexteYazdir(); /*rastgele kare deðerleri olusacak*/
        StartCoroutine(DoFadeRoutine()); /*DoFadeRoutine calýstýrýldý (kareler için)*/
        Invoke("SoruPaneliniAc", 2f); /*sroupaneli 2 saniye gecikmeli acýlacak*/
    }

    void ButonaBasildi()
    {
        if (butonaBasilsinmi)
        {
            audioSource.PlayOneShot(butonSesi); /*butona basýlýnca buton sesi çalýþsýn*/

            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;/*butona basýlýnca resim gelme iþlemi için */

            SonucuKontrolEt();
        }

    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            /*butona týklanýnca doðruysa butona basýldýðýnda ýmage kýsmý aktýf edilsin*/
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";/*kutunun texti kaybolsun*/
            gecerliKare.transform.GetComponent<Button>().interactable = false;/*butona resim acýldýktan sonra tekrar basýlamasýn*/


            puanManager.PuaniArtir(sorununZorlukDerecesi);/*sorunun zorluk derecesine göre puan artacak*/
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
            kalanHak--; /*yanlýþ yaptýðýmýzda kalan haklar eksilmeli*/
            kalanHaklarManager.KalanHaklariKontrolEt(kalanHak); /*kalan haklarý kontrol ediyor*/
        }

        if (kalanHak <= 0)
        {
            OyunBitti();
        }
    }


    void OyunBitti()
    {
        butonaBasilsinmi = false; /*oyun bitince rakam butonlarýna basýlmasýn*/
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);/*sonuc paneli acýlýsý*/

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

            kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString(); /*Kare içerisindeki textin 1. child kýsmýna ulasýyoruz*/
        }
        
    }

    /*StartCoroutine acýldýktan sonra soru paneli kýsmý gelmesi için:*/
    void SoruPaneliniAc()
    {
        SoruyuSor();
        butonaBasilsinmi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack); /*sorupaneli acýlmasý için tekrar rectransform deðerini deðiþtireceðiz*/
    }

    /*Soru paneli için soru hazýrlama */
    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);

        kacinciSoru = Random.Range(0, bolumDegerleriListesi.Count);/*cevap farklý kutucuklarda olsun*/
        dogruSonuc = bolumDegerleriListesi[kacinciSoru];
        bolunenSayi = bolenSayi * dogruSonuc;/* bölüm degerleri listesinden sayý alacak ve bolen sayý ile çarparak bölünen sayý bulunacak. Bu iþlem sonucun varlýðýný garantilemek amacý ile yapýldý*/
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
