using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemParticle : MonoBehaviour
{
    public ParticleSystem particle;

    private void OnEnable()
    {
        particle.Play();
    }

    private void OnParticleCollision(GameObject other)
    {      

        if (other.CompareTag("Ground"))
        {
            Vector3 spawnPosition = other.transform.position + new Vector3(0, 1.0f, 0); // y������ 1 ���� ���� �̵�
            GameObject obj = Array.Find(SettingManager.Instance.Dropitems, item => item.GetComponent<DropItem>() != null);
            DropItem dropItem = Instantiate(obj).GetComponent<DropItem>();
            dropItem.gameObject.transform.position = spawnPosition;
            Destroy(gameObject);
        }
    }
}
