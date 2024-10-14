using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationButtons : MonoBehaviour, ISelectHandler
{
    private SetupMenuController setupMenuController;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        setupMenuController = GetComponentInParent<SetupMenuController>();

        //Debug.Log("Button Selected: " + gameObject.name);
        //GetComponent<Button>().onClick.Invoke();
    }
}
