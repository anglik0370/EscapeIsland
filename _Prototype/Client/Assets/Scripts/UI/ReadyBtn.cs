using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBtn : MonoBehaviour
{
    [SerializeField]
    private Sprite readySprite;
    [SerializeField]
    private Sprite startSprite;

    private Player p;

    private Button btn;
    private Image img;
    private Text txt;
    private CanvasGroup cvs;

    private void Awake() 
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        txt = GetComponentInChildren<Text>();
        cvs = GetComponent<CanvasGroup>();
    }

    private void Start() 
    {
        EventManager.SubEnterRoom(p =>
        {   
            this.p = p;

            Init();
            UtilClass.SetCanvasGroup(cvs, 1, true, true);
        });

        EventManager.SubBackToRoom(() => 
        {
            Init();
            UtilClass.SetCanvasGroup(cvs, 1, true, true);
        });

        EventManager.SubGameStart(p => 
        {
            UtilClass.SetCanvasGroup(cvs);
        });
    }

    public void Init()
    {  
        if(p.master)
        {
            img.sprite = startSprite;
            txt.text = "게임 시작";
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(SendManager.Instance.GameStart);
        }
        else
        {
            img.sprite = readySprite;
            txt.text = "준비";
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => txt.text = !p.isReady ? "취소" : "준비");
            btn.onClick.AddListener(() => SendManager.Instance.Send("READY"));
        }
    }
}
