using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RandomizeName : MonoBehaviour {


    public GameObject TextFieldObject;
    public TextAsset animals;
    public TextAsset adjectives;

    private InputField text;
    private string [] adjlist;
    private string [] animalist;

    // Use this for initialization
    void Start () {
        text = TextFieldObject.GetComponent<InputField>();
        text.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(addedChar); };

        animalist = animals.text.Split('\n');
        adjlist = adjectives.text.Split('\n');

        if (text.text == null || text.text == "")
        {
            RandomizeIt();
        }
        
    }

    private char MyValidate(char charToValidate)
    {
        //Checks if a dollar sign is entered....
        if (!char.IsLetter(charToValidate) && !char.IsWhiteSpace(charToValidate))
        {
            // ... if it is change it to an empty character.
            charToValidate = '\0';
        }
        return charToValidate;
    }

    public void RandomizeIt()
    {

        var adj = adjlist[UnityEngine.Random.Range(0, adjlist.Length)];
        var ani = animalist[UnityEngine.Random.Range(0, animalist.Length)];
        adj = char.ToUpper(adj[0]) + adj.Substring(1);
        ani = char.ToUpper(ani[0]) + ani.Substring(1);

        text.text = adj + " " + ani;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
