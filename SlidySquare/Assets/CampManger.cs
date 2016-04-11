using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CampManger : MonoBehaviour {

    public GameObject ContentArea;

    public GameObject derp;

	// Use this for initialization
	void Start () {
        CampaignBank.LoadFromFile();

        for(int i = 0; i < CampaignBank.boards.Count; i++)
        {
            var temp = Instantiate(derp);
            derp.GetComponent<Text>().text = i + " " + CampaignBank.boards[i].name;
            temp.transform.parent = ContentArea.transform;
            
        }

       // var rem1 = CampaignBank.boards.Find(x => x.name == "20");
        //CampaignBank.boards.Remove(rem1);
       // CampaignBank.SaveToFile();


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
