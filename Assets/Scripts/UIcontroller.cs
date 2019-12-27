using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIcontroller : MonoBehaviour, IPointerDownHandler
{
    //seatbelt
    private bool seatbeltbool;
    public Image seatbeltimg;
    private bool seatwanted = false;

    //repair
    private int currentrepair;
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

    public GameObject UIobj;
    public GameObject MainMenuObj;
    public GameObject i_pausedcanvas;
    

    [HideInInspector] public bool fadingmm = false;
    private bool credit = false;
    private bool fadingui = false;
    private bool pengaturan = false;
    private bool ispaused = false;
    private bool iswantedbox = false;
    [HideInInspector] public bool isbackfromgame = false;

    public Transform camtransform;
    public Camera camsize;

    private Vector3 initvectorcam;
    private float initcamsize;

    private Vector3 lastvectorcam;
    private float lasttcamsize;

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

        initvectorcam = new Vector3(camtransform.position.x, camtransform.position.y, camtransform.position.z);
        initcamsize = camsize.orthographicSize;

        MMCanvas = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
        MMContent = GameObject.Find("Main_Obj").GetComponent<CanvasGroup>();
        UIcanvas = GameObject.Find("UI").GetComponent<CanvasGroup>();
        CreditCanvas = GameObject.Find("CreditContent").GetComponent<CanvasGroup>();
        

        StartCoroutine(Fader(true));
        StartCoroutine(BlinkRepair());

        UIcanvas.interactable = false;
        i_pausedcanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currotation = carcontroller.curspeedint;
        curspeed = carcontroller.curspeed.ToString("0");
        speedtxt.SetText(curspeed.ToString() + "Km/h");
        speedmeter.rotation = Quaternion.Euler(0, 0, currotation * -4);

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

        if (wantedfill.fillAmount == 1 && !iswantedbox)
        {
            Wantedbox.alpha += 1f * Time.deltaTime;
            UIcanvas.interactable = false;
            Wantedbox.interactable = true;
        }
        if (iswantedbox && Wantedbox.alpha > 0)
        {
            TextMeshProUGUI wantedtxt = GameObject.Find("TextWantedBox").GetComponent<TextMeshProUGUI>();
            Wantedbox.alpha -= 1f * Time.deltaTime;
            if (Wantedbox.alpha == 0)
            {
                wantedtxt.SetText("Anda dikenakan denda sebesar rp. 100.000,- dikarenakan melanggar lalu lintas");
                iswantedbox = false;
            }
        }
    }

    private void FixedUpdate()
    {
        moneytxt.text = "Rp. " + save.money.ToString() + ",-";
        lastvectorcam = new Vector3(camtransform.position.x, camtransform.position.y, camtransform.position.z);
        lasttcamsize = camsize.orthographicSize;

        if (melanggar == false && curtotalwanted > 0)
        {
            melanggar = true;
            StartCoroutine(addwanted());
            
        }

        else if (melanggar == true && curtotalwanted == 0)
        {
            melanggar = false;
            StopCoroutine("addwanted");
        }

        if (curtotalwanted > 0 && isdecreasingwantedlevel == false)
        {
            isdecreasingwantedlevel = true;
            StartCoroutine(substractwanted());
        }

        else if (curtotalwanted == 0 && isdecreasingwantedlevel == true)
        {
            StopCoroutine("substractwanted");
        }

        if (seatbeltbool == false && seatwanted == false && fadingui == true)
        {
            seatwanted = true;
            curtotalwanted += 0.05f;
        }


        PlayerPrefs.SetInt("Curmoney", save.money);
        PlayerPrefs.SetFloat("Wantedlvl", wantedfill.fillAmount);
    }

    public void loadfuel()
    {
        fuelfill.fillAmount = save.fuel;
    }


    public void loadwanted()
    {
        wantedfill.fillAmount = save.wantedlvl;
    }

    public void money()
    {
        StartCoroutine(Addmoney());
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

    public void setvolume()
    {

    }

    public void removebox(BaseEventData input)
    {
        if (save.money < 100000)
        {
            boxremoved(false);
        }
        else if (save.money > 100000)
        {
            boxremoved(true);
        }
    }

    public void boxremoved(bool ispaid)
    {
        TextMeshProUGUI wantedtxt = GameObject.Find("TextWantedBox").GetComponent<TextMeshProUGUI>();

        if (ispaid)
        {
            save.money -= 100000;
            wantedtxt.SetText("Uang Anda telah dikurangi untuk membayar denda Rp. 100.000,-");
            wantedfill.fillAmount = 0;
            melanggar = false;
            StartCoroutine(Fadewantedbox());

        }
        else
        {
            wantedtxt.SetText("Anda Tidak memiliki cukup uang untuk membayar denda Rp. 100.000,-");
            StartCoroutine(Fadewantedbox());
        }
    }

    public void pauseevent(BaseEventData input)
    {
        string name;


        name = input.selectedObject.name;

        if (fadingui == true)
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

            }

            if (name == "P_MainMenu")
            {
                GameObject.Find("MainCamera").GetComponent<CameraEvent>().enabled = false;
                UIcanvas.interactable = false;
                MainMenuObj.SetActive(true);
                Time.timeScale = 1;
                ispaused = false;
                isbackfromgame = true;
                fadingui = false;
                StopAllCoroutines();
            }
        }
    }

    public void mainmenuevent(BaseEventData input)
    {
        string name;

        name = input.selectedObject.name;
        print(name);
        if (fadingmm == false && credit == false)
        {
            if (name == "Mulai")
            {
                UIcanvas.interactable = true;
                fadingmm = true;
                isbackfromgame = false;
            }

            if (name == "Credit")
            {
                credit = true;
                StartCoroutine(creditfade(false));
                StartCoroutine(mmcontent(true));
            }

            if (name == "Pengaturan")
            {
                pengaturan = !pengaturan;
                if (pengaturan)
                {
                    Vol.alpha = 1;
                    bool volslider = GameObject.Find("VolSlider").GetComponent<Slider>().enabled = true;
                }
                else
                {
                    bool volslider = GameObject.Find("VolSlider").GetComponent<Slider>().enabled = false;
                    Vol.alpha = 0;
                }
            }

            if (name == "Keluar")
            {
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
        yield return new WaitForSeconds(60);
        save.money += 10;
        Debug.Log("Money added");
        StartCoroutine(Addmoney());
    }

    IEnumerator BlinkRepair()
    {
        repairimg.enabled = false;
        yield return new WaitForSeconds(0.5f);
        repairimg.enabled = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(BlinkRepair());
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
        else
        {
            Debug.Log("Stopped");
        }
    }

    IEnumerator addwanted()
    {
        yield return new WaitForSeconds(2f);
        wantedfill.fillAmount += curtotalwanted;
        StartCoroutine(addwanted());
    }

    IEnumerator substractwanted()
    {
        yield return new WaitForSeconds(3f);
        wantedfill.fillAmount -= 0.01f;
        StartCoroutine(substractwanted());
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

    IEnumerator Fadewantedbox()
    {
        yield return new WaitForSeconds(1f);
        iswantedbox = true;
        yield return new WaitForSeconds(1f);
        UIcanvas.interactable = true;
        Wantedbox.interactable = false;
    }
}
