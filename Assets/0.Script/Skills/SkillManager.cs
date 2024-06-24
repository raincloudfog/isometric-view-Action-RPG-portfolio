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

    public ObjectPool _telpoeffect;

    public void SkillSet()
    {

        int groundindex = SettingManager.Instance.ASkills.FindIndex(skill => skill.GetComponent<ActiveGroundHit>() != null);
        int telportindex = SettingManager.Instance.ASkills.FindIndex(skill => skill.GetComponent<Teleport>() != null);
        int Healindex = SettingManager.Instance.ASkills.FindIndex(skill => skill.GetComponent<Heal>() != null);
        int StarFallindex = SettingManager.Instance.ASkills.FindIndex(skill => skill.GetComponent<Starfall>() != null);

        if (groundindex != -1 && telportindex != -1 && Healindex != -1 && StarFallindex != -1)
        {
            Debug.Log(groundindex + " / " + telportindex + " / " + Healindex + " / " + StarFallindex);
            ActiveGroundHit = SettingManager.Instance.ASkills[groundindex].GetComponent< ActiveGroundHit>();
            Telpo = SettingManager.Instance.ASkills[telportindex].GetComponent<Teleport>();
            Heal = SettingManager.Instance.ASkills[Healindex].GetComponent<Heal>();
            Starfall = SettingManager.Instance.ASkills[StarFallindex].GetComponent<Starfall>();
        }

        _telpoeffect = new ObjectPool(Telpo.gameObject, 10);


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

        skill.gameObject.SetActive(true);
         
        return skill;
    }

    

}
