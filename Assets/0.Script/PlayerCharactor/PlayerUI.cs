using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IUserInterface
{

}

public class PlayerUI : MonoBehaviour , IUserInterface
{

    public Image HPBar;
    public Image ManaBar;

    public Image[] SkillsImg;

    public TMP_Text HPtext;
    public TMP_Text Manatext;

    

    public void ChangeBar(bool isHP, int Max, int Num)
    {
        if(isHP)
        {
            HPBar.fillAmount = (float)Num / Max;
            HPtext.text = Num + " / " + Max;
        }
        else
        {
            ManaBar.fillAmount = (float)Num / Max;
            Manatext.text = Num + " / " + Max;
        }

    }

    public IEnumerator SkillCool(SkillManager.SkillName skillname , int MaxCool)
    {

        float timer = 0;
        int index = -1;

        switch (skillname)
        {
            case SkillManager.SkillName.ActiveGroundHit:
                index = 0;
                break;
            case SkillManager.SkillName.Telpo:
                index = 1;
                break;
            case SkillManager.SkillName.Heal:
                index = 2;
                break;
            case SkillManager.SkillName.Starfall:
                index = 3;
                break;
            default:
                break;
        }

        if(index == -1)
            yield break;

        while(timer < MaxCool)
        {
            yield return null;

            timer += Time.deltaTime;

            SkillsImg[index].fillAmount = (float)timer/MaxCool;

        }

        SkillsImg[index].fillAmount = 1;

    }
}
