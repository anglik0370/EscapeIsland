using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOutline : MonoBehaviour
{
    public Color color = Color.white;

    private Image image;
    private Outline outline;

    [SerializeField]
    private ColorPicker redTeamPicker;
    [SerializeField]
    private ColorPicker blueTeamPicker;

    private float defaultTime = 30f;
    private float curTime = 0f;

    private bool isOccupy = false;

    private void Start()
    {
        EventManager.SubGameOver(goc => SetOccupy(Team.NONE));
        EventManager.SubExitRoom(() => SetOccupy(Team.NONE));
    }

    void OnEnable()
    {
        image = GetComponent<Image>();
        outline = GetComponent<Outline>();

        UpdateOutline(false);
    }

    void OnDisable()
    {
        //UpdateOutline(false);
    }

    void Update()
    {
        //UpdateOutline(true);
        if (!isOccupy) return;

        curTime -= Time.deltaTime;

        if(curTime <= 0f)
        {
            isOccupy = false;
            SetOccupy(Team.NONE);
        }
    }

    void UpdateOutline(bool outline)
    {
        this.outline.enabled = outline;
        this.outline.effectColor = color;
    }

    public void SetOccupy(Team team)
    {
        if(Team.NONE.Equals(team))
        {
            color = UtilClass.limpidityColor;
            image.color = UtilClass.opacityColor;
            UpdateOutline(false);
        }
        else
        {
            color = UtilClass.GetTeamColor(team);
            curTime = defaultTime;
            image.color = team.Equals(Team.RED) ? redTeamPicker.pickColor : blueTeamPicker.pickColor;

            UpdateOutline(true);
            isOccupy = true;
        }
    }
}
