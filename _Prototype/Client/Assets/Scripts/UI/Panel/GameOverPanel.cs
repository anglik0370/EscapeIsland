using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameOverCase
{
    BlueWin,
    RedWin,
}

public class GameOverPanel : Panel
{
    public static GameOverPanel Instance { get; private set; }

    [SerializeField]
    private CanvasGroup curCg;

    private List<Image> winImgList;
    
    [SerializeField]
    private Image standImgPrefab;
    [SerializeField]
    private Image topBar;

    [SerializeField]
    private Text winText;

    [SerializeField]
    private Transform winImgParent;

    private readonly string BLUE_WIN = "����� �¸�";
    private readonly string RED_WIN = "������ �¸�";

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
            case GameOverCase.BlueWin:
                {
                    SetWin(true);
                    CanvasGroupOpenAndClose(true);
                }
                break;
            case GameOverCase.RedWin:
                {
                    SetWin(false);
                    CanvasGroupOpenAndClose(true);
                }
                break;
            default:
                {
                    print("��ȿ���� ���� Case�Դϴ�");
                }
                break;
        }

        base.Open();

        NetworkManager.instance.GameEnd();
    }

    private void SetWin(bool isBlueWin)
    {
        topBar.color = isBlueWin ? Color.blue : Color.red;
        winText.text = isBlueWin ? BLUE_WIN : RED_WIN;
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

    //�ϴ� �̷��� ���� ���߿� Ʈ������ ������ �ϸ� �� ��
    public void CanvasGroupOpenAndClose(bool isOpen)
    {
        curCg.alpha = isOpen ? 1f : 0f;
        curCg.interactable = isOpen;
        curCg.blocksRaycasts = isOpen;
    }

    public void CloseGameOverPanel()
    {
        NetworkManager.instance.User.canMove =true;

        CanvasGroupOpenAndClose(false);
        base.Close();
    }
}
