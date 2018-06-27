using UnityEngine;
using System;

public class ObjectProps : MonoBehaviour {
	public bool _addPlayTimeTag = true;

    public bool _interactable = false;

	[Tooltip ("The name of the object.")]
    [TextArea(1, 2)]
    public String _name;

	[Tooltip("The description of the object.")]
    [TextArea(5, 7)]
    public String _description;

    [Tooltip("The text that will be shown in the \"Press button to...\" window")]
    [TextArea(1, 2)]
    public String _InteractText;


        
    void Start () {
        if (_addPlayTimeTag)
    		this.tag = "DetailedObject";

        if (_name == "") {
            _name = gameObject.name;
        }

        if (_description == "") {
            _description = "<No information available>";
        }
	}
}
