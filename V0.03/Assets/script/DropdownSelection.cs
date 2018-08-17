using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using my;

public class DropdownSelection : MonoBehaviour {
    private Dropdown dds;
    private Dictionary<string, VideoItem> hashMap;
    private GameObject globalObj;
    
	void Start ()
    {
        dds = GetComponent<Dropdown>();
        globalObj = GameObject.FindGameObjectWithTag("resource_manager");
        hashMap= globalObj.GetComponent<ResourceManager>().vm.getMp();

        dds.options.Clear();
        Dropdown.OptionData ddo;
        foreach(var item in hashMap)
        {
            ddo = new Dropdown.OptionData();
            ddo.text = item.Key;
            dds.options.Add(ddo);
        }
        dds.captionText.text = dds.options[0].text;
	}
	
	void Update ()
    {
		
	}
}
