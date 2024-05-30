using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float dissolveDuration = 1.5f; 

    private float dissolveStrength = 0f;

    private int currentLife = 3;

    void Start()
    {
        dissolveMaterial.SetFloat("_dissolveStrength", dissolveStrength);
        UpdateLife();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spikes")
        {
            currentLife--;
            UpdateLife();
        }

        if (other.gameObject.tag == "Lava")
        {
            int layerIndex = gameObject.layer;
            string layerName = LayerMask.LayerToName(layerIndex);
            if (layerName!="Outline Objects")
            {
                currentLife = 0;
                UpdateLife();
            }
        }

        if (other.gameObject.tag == "Heart")
        {
            Destroy(other.gameObject);
            if (currentLife < 3)
            {
                currentLife++;
            }
            UpdateLife();
        }

        if (other.gameObject.tag == "LavaItem")
        {
            Destroy(other.gameObject);
            ChangePlayerLayer("Outline Objects");
        }
    }

    void UpdateLife()
    {
        heart1.SetActive(currentLife >= 1);
        heart2.SetActive(currentLife >= 2);
        heart3.SetActive(currentLife >= 3);

        if (currentLife == 0)
        {
            StartCoroutine(DissolveEffect());
            // Trigger Death Canvas
        }
    }

    void ChangePlayerLayer(string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);
        if (layerIndex == -1)
        {
            Debug.LogError($"Layer '{layerName}' does not exist.");
            return;
        }
        gameObject.layer = layerIndex;

        if (transform.childCount > 0)
        {
            Transform firstChild = transform.GetChild(0);
            firstChild.gameObject.layer = layerIndex;
        }
    }

    IEnumerator DissolveEffect()
    {
        float elapsedTime = 0f;
        while (elapsedTime < dissolveDuration)
        {
            dissolveStrength = Mathf.Lerp(0f, 1f, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_dissolveStrength", dissolveStrength);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        dissolveMaterial.SetFloat("_dissolveStrength", 1f); 
    }
}