using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundSound;
        audioSource.loop = false;  // Loop�� ���ΰ� ����� ���� ������ �ٽ� �����ϵ��� ����
        audioSource.Play();
        StartCoroutine(CheckAndIncreaseSpeed());
    }

    IEnumerator CheckAndIncreaseSpeed()
    {
        while (true)
        {
            // ���� ��� ���� Ŭ���� ���� ������ ���
            yield return new WaitForSeconds(audioSource.clip.length / audioSource.pitch);

            // ��� ����
            audioSource.pitch *= 2.5f;

            // ���� �ٽ� ���
            audioSource.Play();
        }
    }
}
