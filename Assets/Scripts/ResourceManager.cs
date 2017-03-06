using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System.Collections.Generic;
using System.IO;
using System;


[CreateAssetMenu(fileName = "ResourceManager")]
public class ResourceManager : GameManager<ResourceManager> {
	
	public List<string> costumePath = new List<string>();
	public List<string> attachHair  = new List<string>();
	public List<string> accessories = new List<string>();
	public List<string> weapons		= new List<string>();



	
}
