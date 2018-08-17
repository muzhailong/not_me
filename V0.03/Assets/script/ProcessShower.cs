using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using my;

public class ProcessShower : MonoBehaviour {

    private ProcessEntry pe;
    public Text opTimeTxt;
    public Text errTimesTxt;
    public Text successRateTxt;
    public Text allTimeTxt;
    private bool isStart = false;

	void Start ()
    {
        pe = GameObject.FindGameObjectWithTag("resource_manager").GetComponent<Adjuster>().pe;
        isStart = false;
	}
	
	void Update ()
    {
        if (isStart)
        {
            show();
        }
	}

    public void run()
    {
        isStart = true;
        pe.reset();
    }

    private void defaultState()
    {
        pe.reset();
        show();
    }

    private void show()
    {
        opTimeTxt.text = ((int)Time.time - pe.startTime).ToString();
        errTimesTxt.text = pe.errTimes.ToString();
        //successRateTxt.text = pe.getSuccessRate().ToString();
        allTimeTxt.text = pe.allTime.ToString();
    }

    public void stop()
    {
        isStart = false;
    }
}
