using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIManager : MonoBehaviour
{
    public static BuffUIManager Instance { get; private set; }

    [SerializeField] private GameObject buffIconPrefab; // Prefab UI ikon buff
    [SerializeField] private Transform buffContainer;   // Transform Horizontal Layout Group

    // Menyimpan data ikon buff yang sedang aktif berdasarkan ID unik
    private Dictionary<string, BuffIconInfo> activeBuffs = new Dictionary<string, BuffIconInfo>();

    private class BuffIconInfo
    {
        public GameObject iconInstance;
        public Image fillImage;
        public Coroutine timerCoroutine;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerBuff(string buffId, Sprite buffSprite, float duration)
    {
        if (buffIconPrefab == null || buffContainer == null)
        {
            Debug.LogWarning("BuffUIManager: Prefab atau Container belum dipasang di Inspector!");
            return;
        }

        // Jika buff sudah aktif, reset timer-nya
        if (activeBuffs.TryGetValue(buffId, out BuffIconInfo info))
        {
            if (info.timerCoroutine != null)
            {
                StopCoroutine(info.timerCoroutine);
            }
            info.timerCoroutine = StartCoroutine(UpdateBuffTimer(buffId, info, duration));
        }
        else
        {
            // Buat instansiasi ikon buff baru
            GameObject newIcon = Instantiate(buffIconPrefab, buffContainer);
            
            // Set gambar ikon utama
            Image mainImage = newIcon.GetComponent<Image>();
            if (mainImage != null)
            {
                mainImage.sprite = buffSprite;
            }

            // Cari komponen Image cooldown/fill di children
            Image fillImg = null;
            Image[] images = newIcon.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                if (img != mainImage)
                {
                    fillImg = img;
                    break;
                }
            }

            BuffIconInfo newInfo = new BuffIconInfo
            {
                iconInstance = newIcon,
                fillImage = fillImg
            };

            newInfo.timerCoroutine = StartCoroutine(UpdateBuffTimer(buffId, newInfo, duration));
            activeBuffs.Add(buffId, newInfo);
        }
    }

    private IEnumerator UpdateBuffTimer(string buffId, BuffIconInfo info, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (info.fillImage != null)
            {
                info.fillImage.fillAmount = 1f - (elapsed / duration);
            }
            yield return null;
        }

        // Hancurkan ikon setelah durasi berakhir dan hapus dari tracking
        if (info.iconInstance != null)
        {
            Destroy(info.iconInstance);
        }
        activeBuffs.Remove(buffId);
    }
}
