using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameOverCase
{
    None,
    RedWin,
    BlueWin,
}

public class GameOverPanel : Panel
{
    public static GameOverPanel Instance { get; private set; }

    private List<Image> winImgList;
    
    [SerializeField]
    private Image standImgPrefab;

    [SerializeField]
    private Transform winImgParent;

    [SerializeField]
    private Image winTxtImg;
    [SerializeField]
    private Sprite blueteamTxtSprite;
    [SerializeField]
    private Sprite redteamTxtSprite;

    [SerializeField]
    private Effect[] gameEndEffects;

    protected override void Awake()
    {
        base.Awake();

        winImgList = new List<Image>();

        if(Instance == null)
            Instance = this;
    }

    protected override void Start()
    {
        EventManager.SubGameOver(gameOverCase =>
        {
            Open(gameOverCase);
        });
        EventManager.SubGameStart(p => ClearWinImg());
    }

    public void Open(GameOverCase gameOverCase)
    {
        switch (gameOverCase)
        {
            case GameOverCase.None:
                SetWin(true, true);
                UtilClass.SetCanvasGroup(cvs, 1, true, true);
                break;
            case GameOverCase.BlueWin:
                {
                    SetWin(true);
                    UtilClass.SetCanvasGroup(cvs, 1, true, true);
                }
                break;
            case GameOverCase.RedWin:
                {
                    SetWin(false);
                    UtilClass.SetCanvasGroup(cvs, 1, true, true);
                }
                break;
            default:
                {
                    print("유효하지 않은 Case입니다");
                }
                break;
        }

        base.Open();

        for (int i = 0; i < gameEndEffects.Length; i++)
        {
            gameEndEffects[i].gameObject.SetActive(true);
            gameEndEffects[i].LoopingPlay();
        }

        NetworkManager.instance.GameEnd();
    }

    private void SetWin(bool isBlueWin,bool isNone = false)
    {
        if(isNone)
        {
            winTxtImg.sprite = null;
            return;
        }

        winTxtImg.sprite = isBlueWin ? blueteamTxtSprite : redteamTxtSprite;
    }

    public bool FindWinImg(out Image img)
    {
        img = winImgList.Find(img => !img.gameObject.activeSelf);

        return img != null;
    }

    public void MakeWinImg(Player p)
    {
        Image img = null;

        if (!FindWinImg(out img))
        {
            img = Instantiate(standImgPrefab, winImgParent);
        }
        else
        {
            img.transform.SetParent(winImgParent);
        }

        img.sprite = p.curSO.standImg;
        img.gameObject.SetActive(true);

        winImgList.Add(img);
    }

    public void ClearWinImg()
    {
        for (int i = 0; i < winImgList.Count; i++)
        {
            winImgList[i].gameObject.SetActive(false);
        }
    }

    public void CloseGameOverPanel()
    {
        NetworkManager.instance.User.canMove =true;

        for (int i = 0; i < gameEndEffects.Length; i++)
        {
            gameEndEffects[i].Disable();
        }

        SoundManager.Instance.PlayBGM(SoundManager.Instance.DaylightBGM);

        UtilClass.SetCanvasGroup(cvs);
        base.Close();
    }
}
