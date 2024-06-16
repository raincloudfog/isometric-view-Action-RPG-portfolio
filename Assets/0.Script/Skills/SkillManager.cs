using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Skills;
using System;

public class SkillManager : Singleton<SkillManager>
{
    public enum SkillName
    {
        ActiveGroundHit,
        Telpo,
        Heal,
        Starfall

    }

    public Skill ActiveGroundHit;
    public Skill Telpo;
    public Skill Heal;
    public Skill Starfall;    

    public void SkillSet()
    {
        ActiveGroundHit = 
            Array.Find( SettingManager.Instance.Skills, skill => skill is ActiveGroundHit) ;
        Telpo =
            Array.Find(SettingManager.Instance.Skills, skill => skill is Teleport);
        Heal =
            Array.Find(SettingManager.Instance.Skills, skill => skill is Heal);
        Starfall =
            Array.Find(SettingManager.Instance.Skills, skill => skill is Starfall);
    }

    public Skill GetSkill(SkillName name)
    {
        Skill skill = null;
        switch (name)
        {
            case SkillName.ActiveGroundHit:
                skill = Instantiate(ActiveGroundHit);
                break;
            case SkillName.Telpo:
                skill = Instantiate(Telpo);
                break;
            case SkillName.Heal:
                skill = Instantiate(Heal);
                break;
            case SkillName.Starfall:
                skill = Instantiate(Starfall);
                break;
            default:

                break;
        }
        

        return skill;
    }

    

}
