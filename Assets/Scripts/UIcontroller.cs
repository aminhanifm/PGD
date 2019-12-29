using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIcontroller : MonoBehaviour, IPointerDownHandler
{
    KoganeUnityLib.dialogueTemplate dt;
    GameObject dialogmanagerobj;
    DialogueManager dialogmanager;

    private Transform moneyimg;

    //seatbelt
    private bool seatbeltbool;
    public Image seatbeltimg;
    private bool seatwanted = false;

    //repair
    public int currentrepair;
    public Image repairimg;

    //wantedlvl
    public Image wantedfill;
    private bool melanggar = false;
    private float curtotalwanted;
    private bool isdecreasingwantedlevel = false;

    //fuel
    public Image fuelfill;
    private bool ismoving;

    public TextMeshProUGUI moneytxt;
    private Savemanagement save;

    public Image fader;
    [HideInInspector] public bool activated;

    private isocar_controller carcontroller;
    public TextMeshProUGUI speedtxt;
    private string curspeed;
    public Transform speedmeter;
    private int currotation;

    public CanvasGroup Vol;
    public CanvasGroup UIcanvas;
    public CanvasGroup MMCanvas;
    public CanvasGroup MMContent;
    public CanvasGroup CreditCanvas;
    public CanvasGroup PausedCanvas;
    public CanvasGroup Wantedbox;
    public CanvasGroup dialogueCanvas;

    public GameObject UIobj;
    public GameObject MainMenuObj;
    public GameObject i_pausedcanvas;
    

    [HideInInspector] public bool fadingmm = false;
    private bool credit = false;
    private bool fadingui = false;
    private bool pengaturan = false;
    private bool ispaused = false;
    private bool iswantedbox = false;
    private bool islost = false;
    [HideInInspector] public bool iswanted = false;
    private bool needrepair = false;
    [HideInInspector] public bool isplayingdialogue = false;
    [HideInInspector] public bool isbackfromgame = false;

    public Transform camtransform;
    public Camera camsize;

    private Vector3 initvectorcam;
    private float initcamsize;

    private Vector3 lastvectorcam;
    private float lasttcamsize;

    private int currentmission;

    private string subwantedC;
    private string wantedC;
    private string moneyC;

    void Start()
    {
        seatbeltimg = GameObject.Find("Seatbelt Icon").GetComponent<Image>();
        repairimg = GameObject.Find("Repair Icon").GetComponent<Image>();
        fuelfill = GameObject.Find("F_Bar_Mask").GetComponent<Image>();
        wantedfill = GameObject.Find("W_Bar_Mask").GetComponent<Image>();
        save = FindObjectOfType(typeof(Savemanagement)) as Savemanagement; ;
        moneytxt = GameObject.Find("Moneytxt").GetComponent<TextMeshProUGUI>();
        speedtxt = GameObject.Find("Speedtxt").GetComponentInChildren<TextMeshProUGUI>();
        carcontroller = GameObject.FindWithTag("Player").GetComponent<isocar_controller>();
        speedmeter = GameObject.Find("Speed meter").GetComponent<Transform>();
        fader = GameObject.Find("Fader").GetComponent<Image>();
        PausedCanvas = GameObject.Find("PausedCanvas").GetComponent<CanvasGroup>();
        i_pausedcanvas = GameObject.Find("I_PausedCanvas");
        camtransform = GameObject.Find("MainCamera").GetComponent<Transform>();
        camsize = GameObject.Find("MainCamera").GetComponent<Camera>();
        Vol = GameObject.Find("VolUI").GetComponent<CanvasGroup>();
        MainMenuObj = GameObject.Find("MainMenu");
        UIobj = GameObject.Find("UI");
        Wantedbox = GameObject.Find("WantedWindow").GetComponent<CanvasGroup>();
        moneyimg = GameObject.Find("Money").GetComponent<Transform>();

        subwantedC = "substractwanted";
        wantedC = "addwanted";
        moneyC = "Addmoney";

        dt = FindObjectOfType(typeof(KoganeUnityLib.dialogueTemplate)) as KoganeUnityLib.dialogueTemplate;
        dialogmanager = FindObjectOfType(typeof(DialogueManager)) as DialogueManager;
        dialogmanagerobj = GameObject.Find("dialogueMaster");

        initvectorcam = new Vector3(camtransform.position.x, camtransform.position.y, camtransform.position.z);
        initcamsize = camsize.orthographicSize;

        MMCanvas = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
        MMContent = GameObject.Find("Main_Obj").GetComponent<CanvasGroup>();
        UIcanvas = GameObject.Find("UI").GetComponent<CanvasGroup>();
        CreditCanvas = GameObject.Find("CreditContent").GetComponent<CanvasGroup>();
        dialogueCanvas = GameObject.Find("dialogueMaster").GetComponent<CanvasGroup>();

        StartCoroutine(Fader(true));

        UIcanvas.interactable = false;
        i_pausedcanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentrepair = save.repair;
        currotation = carcontroller.curspeedint;
        curspeed = carcontroller.curspeed.ToString("0");
        speedtxt.SetText(curspeed.ToString() + "Km/h");
        speedmeter.rotation = Quaternion.Euler(0, 0, currotation * -6);
        if (fadingmm && UIcanvas.interactable)
        {
            print(needrepair);
            if (save.repair <= 30 && !needrepair)
            {
                needrepair = true;
                StartCoroutine(BlinkRepair(false));
            }

            if (!needrepair)
            {
                StopCoroutine("BlinkRepair");
                repairimg.DOColor(Color.white, 1f);
            }

        }

        if (seatbeltbool == false && seatwanted == false && fadingui == true)
        {
            seatwanted = true;
            curtotalwanted += 0.05f;
        }

        if (!isplayingdialogue)
        {
            dialogueCanvas.interactable = false;
        }
        else
        {
            dialogueCanvas.interactable = true;
        }

        if (currotation > 0 && ismoving == false)
        {
            ismoving = true;
            StartCoroutine(substractfuel());
        }

        if (MMCanvas.alpha < 1 && fadingmm == false)
        {
            MMCanvas.alpha += 0.5f * Time.deltaTime;
        }

        if (fadingmm == true && MMCanvas.alpha > 0)
        {
            MMCanvas.alpha -= 0.5f * Time.deltaTime;
            if (MMCanvas.alpha == 0)
            {
                fadingui = true;
                GameObject.Find("MainCamera").GetComponent<CameraEvent>().enabled = true;
                MainMenuObj.SetActive(false);
                money();
                seatbeltbool = false;
                StartCoroutine(subwantedC);
                StartCoroutine("BlinkSeatbelt", false);
            }
        }

        if (fadingui == true && UIcanvas.alpha < 1)
        {
            UIcanvas.alpha += 0.5f * Time.deltaTime;
        }

        //back to main menu
        if (!fadingui && UIcanvas.alpha > 0)
        {
            UIcanvas.alpha -= 0.5f * Time.deltaTime;
            fadingmm = false;
            
        }

        if (ispaused == true && PausedCanvas.alpha < 1)
        {
            PausedCanvas.alpha += 2f * Time.unscaledDeltaTime;
        }
        if (ispaused == false && PausedCanvas.alpha > 0)
        {
            PausedCanvas.alpha -= 2f * Time.unscaledDeltaTime;
            if (PausedCanvas.alpha == 0)
            {
                i_pausedcanvas.SetActive(false);
            } 
        }
        if (isbackfromgame && !ispaused)
        {
            camtransform.position = Vector3.Lerp(lastvectorcam, initvectorcam, 0.5f * Time.deltaTime);
            camsize.orthographicSize = Mathf.Lerp(lasttcamsize, initcamsize, 0.5f * Time.deltaTime);
        }

        if (wantedfill.fillAmount == 1 && !iswanted)
        {
            if (!iswantedbox && Wantedbox.alpha < 1)
            {
                Wantedbox.alpha += 1f * Time.deltaTime;
                Wantedbox.interactable = true;
                if(Wantedbox.alpha == 1f)
                {
                    Wantedbox.interactable = true;
                    iswanted = true;
                    SoundsManager.PlaySound("Wanted");
                }
                if (iswanted)
                {
                    //print("start tilang");
                    dialogmanagerobj.SetActive(true);
                    dialog(1);
                }
            }
        }


        if (iswantedbox && Wantedbox.alpha > 0)
        {
            TextMeshProUGUI wantedtxt = GameObject.Find("TextWantedBox").GetComponent<TextMeshProUGUI>();
            Wantedbox.alpha -= 1f * Time.deltaTime;
            if (Wantedbox.alpha == 0)
            {
                wantedtxt.SetText("Anda dikenakan denda sebesar rp. 100.000,- dikarenakan melanggar lalu lintas");
                Wantedbox.interactable = false;
                iswantedbox = false;
            }
        }
    }

    private void FixedUpdate()
    {
        
        moneytxt.text = "Rp. " + save.money.ToString() + ",-";
        lastvectorcam = new Vector3(camtransform.position.x, camtransform.position.y, camtransform.position.z);
        lasttcamsize = camsize.orthographicSize;

        if (fadingmm && UIcanvas.interactable)
        {

            if (melanggar == false && curtotalwanted > 0)
            {
                melanggar = true;
                StartCoroutine(wantedC);
            }

            else if (melanggar == true && curtotalwanted == 0)
            {
                melanggar = false;
                StopCoroutine(wantedC);
            }

            if (curtotalwanted > 0 && isdecreasingwantedlevel == false)
            {
                isdecreasingwantedlevel = true;
                StartCoroutine(subwantedC);
            }
            
        }
        

        PlayerPrefs.SetInt("Repair", save.repair);
        PlayerPrefs.SetInt("Curmoney", save.money);
        PlayerPrefs.SetFloat("Wantedlvl", wantedfill.fillAmount);
    }


    public void dialog(int index)
    {
        dialogmanager.initScenario(index);
        dialogmanager.continueLine();
    }

    public void carrepaired()
    {
        needrepair = false;
    }

    public void loadfuel()
    {
        fuelfill.fillAmount = save.fuel;
    }


    public void loadwanted()
    {
        wantedfill.fillAmount = save.wantedlvl;
    }

    public void loadrepair()
    {
        currentrepair = save.repair;
    }

    public void money()
    {
        StartCoroutine(moneyC);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown called.");
    }

    public void seatbeltturn()
    {
        if (seatbeltbool == false && UIcanvas.interactable)
        {
            seatbeltbool = true;
            seatwanted = false;
            StopCoroutine("BlinkSeatbelt");
            curtotalwanted -= 0.05f;
            Debug.Log(seatbeltbool);
        }
    }

    public void repaired()
    {
        if(UIcanvas.interactable)
        {
            carrepaired();
            save.repair = 100;
            StopCoroutine("BlinkRepair");
            repairimg.DOColor(Color.white, 1f);
            
        }
    }

    public void setvolume()
    {

    }

    public void removebox(BaseEventData input)
    {
        if (Wantedbox.interactable)
        {
            if (save.money < 100000)
            {
                boxremoved(false);
            }
            else if (save.money > 100000)
            {
                boxremoved(true);
            }
            
            Wantedbox.interactable = false;
        }
    }

    public void boxremoved(bool ispaid)
    {
        TextMeshProUGUI wantedtxt = GameObject.Find("TextWantedBox").GetComponent<TextMeshProUGUI>();

        if (ispaid)
        {
            save.money -= 100000;
            wantedtxt.SetText("Uang Anda telah dikurangi untuk membayar denda Rp. 100.000,-");
            wantedfill.fillAmount = 0f;
            StartCoroutine(Fadewantedbox(true));

        }
        else
        {
            wantedtxt.SetText("Anda Tidak memiliki cukup uang untuk membayar denda Rp. 100.000,- Permainan Berakhir");
            StartCoroutine(Fadewantedbox(false));
        }
    }

    public void pauseevent(BaseEventData input)
    {
        string name;


        name = input.selectedObject.name;

        if (fadingui == true && !iswanted)
        {
            if (name == "Pause Icon")
            {
                ispaused = !ispaused;

                if (ispaused)
                {
                    i_pausedcanvas.SetActive(true);
                    Time.timeScale = 0;
                    print("paused");
                }
                else
                {
                    Time.timeScale = 1;
                    print("unpaused");
                }
                SoundsManager.PlaySound("Button");
            }

            if (name == "P_MainMenu")
            {
                SoundsManager.PlaySound("Button");
                GameObject.Find("MainCamera").GetComponent<CameraEvent>().enabled = false;
                dialogmanager.showHide("dialog", false, false);
                stopcoroutine();
                melanggar = false;
                UIcanvas.interactable = false;
                MainMenuObj.SetActive(true);
                Time.timeScale = 1;
                ispaused = false;
                isbackfromgame = true;
                fadingui = false;

            }
        }
    }

    public void mainmenuevent(BaseEventData input)
    {
        string name;

        name = input.selectedObject.name;
        //print(name);
        if (fadingmm == false && credit == false)
        {
            if (name == "Mulai")
            {
                if (islost)
                {
                    islost = false;
                    needrepair = false;
                    dialogmanager.initScenario(0);
                }
                UIcanvas.interactable = true;
                fadingmm = true;
                isbackfromgame = false;
                SoundsManager.PlaySound("Button");
            }

            if (name == "Credit")
            {
                credit = true;
                StartCoroutine(creditfade(false));
                StartCoroutine(mmcontent(true));
                SoundsManager.PlaySound("Button");
            }

            if (name == "Pengaturan")
            {
                pengaturan = !pengaturan;
                if (pengaturan)
                {
                    Vol.alpha = 1;
                    bool volslider = GameObject.Find("VolSliderMM").GetComponent<Slider>().enabled = true;
                }
                else
                {
                    bool volslider = GameObject.Find("VolSliderMM").GetComponent<Slider>().enabled = false;
                    Vol.alpha = 0;
                }
                SoundsManager.PlaySound("Button");
            }

            if (name == "Keluar")
            {
                SoundsManager.PlaySound("Button");
                Application.Quit();
            }

        }
    }

    IEnumerator Fader(bool Fading)
    {

        if (Fading)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fader.color = new Color(0, 0, 0, i);
                yield return null;
            }
            fader.enabled = false;
        }
        else
        {
            fader.enabled = true;
            yield return new WaitForSeconds(2f);
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                fader.color = new Color(0, 0, 0, i);                
                yield return null;
            }
        }

    }

    IEnumerator substractfuel()
    {
        if (currotation > 0)
        {
            yield return new WaitForSeconds(1f);
            fuelfill.fillAmount -= 0.005f;
            save.fuel = fuelfill.fillAmount;
            PlayerPrefs.SetFloat("Curfuel", save.fuel);
            StartCoroutine(substractfuel());
        }
        if (currotation <= 0)
        {
            ismoving = false;
            StopCoroutine("substractfuel");
        }
    }

    IEnumerator Addmoney()
    {
        //if (wantedfill.fillAmount)
        //if (fadingmm)
        {
            yield return new WaitForSeconds(1);
            moneyimg.DOScale(1.5f, 0.5f);
            save.money += 10;
            Debug.Log("Money added");
            moneyimg.DOScale(1, 0.5f);
            StartCoroutine(moneyC);
        }
    }

    IEnumerator BlinkRepair(bool ison)
    {
        if (!ison)
        {
            repairimg.enabled = false;
            yield return new WaitForSeconds(0.5f);
            repairimg.enabled = true;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("BlinkRepair", false);
        }
        else
        {
            print("stopped");
        }
        
    }

    IEnumerator BlinkSeatbelt(bool ison)
    {
        if (ison == false)
        {
            seatbeltimg.enabled = false;
            yield return new WaitForSeconds(0.5f);
            seatbeltimg.enabled = true;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("BlinkSeatbelt", false);
        }
    }

    IEnumerator addwanted()
    {
        //if (fadingmm)
        {
            yield return new WaitForSeconds(2f);
            wantedfill.fillAmount += curtotalwanted;
            StartCoroutine(wantedC);
        }
    }

    IEnumerator substractwanted()
    {
        yield return new WaitForSeconds(3f);
        wantedfill.fillAmount -= 0.01f;
        StartCoroutine(subwantedC);
    }

    IEnumerator creditfade(bool Fading)
    {
        if (Fading)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                CreditCanvas.alpha = i;
                yield return null;
            }
            credit = false;
            CreditCanvas.alpha = 0;
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                CreditCanvas.alpha = i;
                yield return null;
            }
            CreditCanvas.alpha = 1;
            yield return new WaitForSeconds(3f);
            StartCoroutine(creditfade(true));
        }
    }

    IEnumerator mmcontent(bool Fading)
    {
        if (Fading)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                MMContent.alpha = i;
                yield return null;
            }
            MMContent.alpha = 0;
            yield return new WaitForSeconds(3f);
            StartCoroutine(mmcontent(false));
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                MMContent.alpha = i;
                yield return null;
            }
            MMContent.alpha = 1;
        }
    }

    IEnumerator Fadewantedbox(bool ispaid)
    {
        if (ispaid)
        {
            yield return new WaitForSeconds(1f);
            iswantedbox = true;
            iswanted = false;
            yield return new WaitForSeconds(1f);
            UIcanvas.interactable = true;
        }
        else
        {
            yield return new WaitForSeconds(2f);
            GameObject.Find("MainCamera").GetComponent<CameraEvent>().enabled = false;
            islost = true;
            melanggar = false;
            dialogmanager.showHide("dialog", false, false);
            iswanted = false;
            iswantedbox = true;
            //StopAllCoroutines();
            UIcanvas.interactable = false;
            MainMenuObj.SetActive(true);
            isbackfromgame = true;
            fadingui = false;
            PlayerPrefs.DeleteAll();
            save.Newgame();
            stopcoroutine();
            loadfuel();
            loadwanted();
            loadrepair();
        }
    }

    public void stopcoroutine()
    {
        StopCoroutine(wantedC);
        StopCoroutine(subwantedC);
        StopCoroutine(moneyC);
    }
}
