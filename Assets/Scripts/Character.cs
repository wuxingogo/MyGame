using UnityEngine;
using System.Collections;

public enum Gender {
	Male = 1,
	Famale = 2
}
public class Character : MonoBehaviour {
	


	public CharacterController controller = null;

	public Gender gender = Gender.Male;
	public GameObject costumeBody = null;
	public GameObject hairBody = null;
	public GameObject[] accessories = null;



}
