using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameOverCase
{
    KillAllCitizen,
    FindAllKidnapper,
    CollectAllItem,
}

public class GameOverPanel : Panel
{
    public static GameOverPanel Instance { get; private set; }

    public CanvasGroup citizenCg;
    public CanvasGroup kidnapperCg;

    private List<Image> winImgList;
    
    [SerializeField]
    private Image standImgPrefab;

    //0 - kidnapper, 1 - citizen
    [SerializeField]
    private Transform[] kidnapperImgParent;
    [SerializeField]
    private Transform[] citizenImgParent;

    private CanvasGroup curCg = null;

    private WaitForSeconds closeSec;

    protected override void Awake()
    {
        base.Awake();

        closeSec = new WaitForSeconds(1.5f);
        winImgList = new List<Image>();

        if(Instance == null)
            Instance = this;
    }

    private void Start()
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
            case GameOverCase.KillAllCitizen:
                {
                    CanvasGroupOpenAndClose(kidnapperCg, true);
                    print("��� �ù� ���");
                }
                break;
            case GameOverCase.FindAllKidnapper:
                {
                    CanvasGroupOpenAndClose(citizenCg, true);
                    print("��� ��ġ�� �˰�");
                }
                break;
            case GameOverCase.CollectAllItem:
                {
                    print("��� ��� ����");
                    CanvasGroupOpenAndClose(citizenCg, true);
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
        //StartCoroutine(ClosePanel());
    }

    public bool FindWinImg(out Image img)
    {
        img = winImgList.Find(img => !img.gameObject.activeSelf);

        return img != null;
    }

    public void MakeWinImg(Player p, bool isKidnapperWin)
    {
        Image img = null;
        if(!FindWinImg(out img))
        {
            int idx = isKidnapperWin ? 0 : 1;
            img = Instantiate(standImgPrefab, (p.isKidnapper) ? kidnapperImgParent[idx] : citizenImgParent[idx]);
        }

        img.sprite = p.curSO.standImg;
        if (p.isDie)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
        }
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
    public void CanvasGroupOpenAndClose(CanvasGroup cg,bool isOpen)
    {
        if (cg == null) return;
        curCg = cg;

        curCg.alpha = isOpen ? 1f : 0f;
        curCg.interactable = isOpen;
        curCg.blocksRaycasts = isOpen;
    }

    public void CloseGameOverPanel()
    {
        CanvasGroupOpenAndClose(curCg, false);
        base.Close();
    }

    IEnumerator ClosePanel()
    {
        //yield return new WaitUntil(() => cvs.interactable);

        yield return closeSec;

        CanvasGroupOpenAndClose(curCg, false);
        base.Close();
        NetworkManager.instance.GameEnd();
    }
}
