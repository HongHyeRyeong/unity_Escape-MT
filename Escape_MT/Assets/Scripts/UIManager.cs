﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = (UIManager)FindObjectOfType(typeof(UIManager));
            return instance;
        }
    }

    public Text score;
    public Text playTime;
    public Image alcoholGauge;
    public Text script;
    public GameObject ending;
    public GameObject gameResult;
    public Text[] gameResultScore;

    private float curGauge;
    private float saveGauge;

    public void Init()
    {
        score.text = "Score  0";
        UpdateAlcoholGauge(0);
        ending.SetActive(false);
        gameResult.SetActive(false);

        StartCoroutine(UpdatePlayTime());
    }

    public void UpdateScore(float curScore)
    {
        score.text = "Score  " + Mathf.Round(curScore).ToString();
    }

    public IEnumerator UpdatePlayTime()
    {
        int time = 0;
        while (GameManager.Instance.isPlay)
        {
            playTime.text = "Play Time " + time.ToString() + "초";
            yield return new WaitForSeconds(1f);
            time++;
        }
    }

    public void UpdateAlcoholGauge(float gauge)
    {
        curGauge = alcoholGauge.fillAmount * 100;
        saveGauge = gauge;

        if (saveGauge < curGauge)
            SetAlcoholGauge(saveGauge);
        else
            StartCoroutine(AnimAlcoholGauge());
    }

    private IEnumerator AnimAlcoholGauge()
    {
        while (curGauge < saveGauge)
        {
            curGauge += 20 * Time.deltaTime;
            SetAlcoholGauge(curGauge);

            yield return null;
        }
    }

    private void SetAlcoholGauge(float gauge)
    {
        alcoholGauge.fillAmount = gauge / 100f;

        // 기본
        alcoholGauge.color = Color.white;
        script.transform.parent.gameObject.SetActive(false);

        if (30 <= gauge) // 1단계
        {
            alcoholGauge.color = Color.yellow;

            script.transform.parent.gameObject.SetActive(true);
            script.text = "어지럽네...";
        }

        if (60 <= gauge) // 2단계
        {
            alcoholGauge.color = new Color(1f, 0.5f, 0);

            script.transform.parent.gameObject.SetActive(true);
            script.text = "몸이 말을 안들어...";
        }

        if (100 <= gauge) // 3단계
        {
            alcoholGauge.color = Color.red;

            script.transform.parent.gameObject.SetActive(true);
            script.text = "우..우욱..!!";
        }
    }

    public void ShowEnding(List<float> scores)
    {
        ending.SetActive(true);
        StartCoroutine("ShowGameResult", scores);
    }

    private IEnumerator ShowGameResult(List<float> scores)
    {
        yield return new WaitForSeconds(3);

        gameResult.SetActive(true);

        for (int i = 0; i < 3; ++i)
        {
            gameResultScore[i].text = "0";
        }

        for (int i = 0; i < 3 && i < scores.Count; ++i)
        {
            gameResultScore[i].text = Mathf.Round(scores[i]).ToString();
        }
    }

    public void OnClickTitle()
    {
        SceneController.Instance.ToTitleScene();
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}