using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDragable : MonoBehaviour
{
    GameObject canvas;
    [SerializeField] Image imagePrefab;

    Image image;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("GameCanvas");
        image = Instantiate(imagePrefab, transform.position , transform.rotation );
        image.GetComponent<UIDrag>().canvas = canvas.GetComponent<Canvas>();
        image.transform.SetParent(canvas.transform);
        image.GetComponent<UIDrag>().SetLinkedObject(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = image.transform.position;
    }
}
