using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text resultText;

    public void ValidateInput(){
        string input = inputField.text;

        if(input.Length < 3)
        {
            resultText.text = "Write down a name that's 3 characters or longer.";
            resultText.color = Color.red;
        }
        else{
            resultText.text = "";
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
