using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
[CreateAssetMenu(fileName = "LayerManager")]
public class LayerManager : GameManager<LayerManager> {

	public LayerMask groundAndConstruction;
	public LayerMask goundLayer;
	public LayerMask constructionLayer;
	public LayerMask unitsLayer;

	
}
