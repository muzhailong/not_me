using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using my;

public class PointerContoller : MonoBehaviour {

    public Color correntColor;
    public Color errorColor;
    public Color defaultColor;

    public Sprite correntSprite;
    public Sprite errorSprite;
    public Sprite defaultSprite;

    private GameObject pointerTxtObj;
    private GameObject pointerImgObj;
    private Text showTxt;

    private CEQueue cep;

    void Start ()
    {
        pointerTxtObj = GameObject.Find("pointer_txt");
        showTxt = pointerTxtObj.GetComponentInChildren<Text>();
        pointerImgObj = GameObject.Find("pointer_img");
        hidden();
      


        cep = new CEQueue(Config.CORRECT_ERROR_QUEUE_NUMBER);
    }

    private void show()
    {
        pointerImgObj.SetActive(true);
        pointerTxtObj.SetActive(true);
    }

    public void defaultShow()
    {
        pointerImgObj.GetComponent<Image>().sprite = correntSprite;
        pointerTxtObj.GetComponent<Image>().color = correntColor;
        showTxt.text = Config.POINTER_DEFAULT_TXT;
        show();
    }

    public void correntShow()
    {
        cep.correct();
        shower(cep.pointer());
    }

    public void errorShow()
    {
        cep.error();
        shower(cep.pointer());
    }

    private void shower(bool b)
    {
        if (b)
        {
            pointerImgObj.GetComponent<Image>().sprite = correntSprite;
            pointerTxtObj.GetComponent<Image>().color = correntColor;
            showTxt.text = Config.POINTER_CORRECT_TXT;
            show();
        }
        else
        {
            pointerImgObj.GetComponent<Image>().sprite = errorSprite;
            pointerTxtObj.GetComponent<Image>().color = errorColor;
            showTxt.text = Config.POINTER_ERROR_TXT;
            show();
        }
    }

    private void hidden()
    {
        pointerImgObj.SetActive(false);
        pointerTxtObj.SetActive(false);
    }
}

class CEQueue
{
    private int capacity;
    private int errorNum;

    public CEQueue(int capacity)
    {
        this.capacity = capacity;
        errorNum = 0;
    }

    private void push(bool b)
    {
        if (b)
        {
            errorNum = 0;
        }
        else
        {
            ++errorNum;
        }
    }

    public void correct()
    {
        push(true);
    }

    public void error()
    {
        push(false);
    }

    public bool pointer()
    {
        return errorNum < capacity;
    }

}
