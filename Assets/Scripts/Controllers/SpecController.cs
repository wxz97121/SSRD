using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecController : MonoBehaviour
{
    public Dictionary<string, SimpleSpectrum> _specDictionary; 
    public Dictionary<string, OutputVolume> _volDictionary;
    public List<SimpleSpectrum> speclist;
    public  Color MaxColor1;
    public Color MinColor1;
    public Color MaxColor2;
    public Color MinColor2;
    public Color MinColor3;
    public Color MaxColor3;

    // Start is called before the first frame update
    void Start()
    {
        _specDictionary = new Dictionary<string, SimpleSpectrum>();

        speclist = new List<SimpleSpectrum>();
        speclist.AddRange(gameObject.GetComponentsInChildren<SimpleSpectrum>());


        foreach (SimpleSpectrum item in speclist)
        {
            _specDictionary.Add(item.name, item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
