using System.Collections;
using UnityEngine;

public static class NameGenerator {
	
	private static string[] names = new string[] {"Tiririca", "Bambam", "Batman", "Exódia", "Elvis", "Michael Jackson", "Monstro", "Fora Temer", "Inocente", "Cazalbé", "Leroy", "Gandalf", "20matar70correr", "Boludo", "Troll"};

	public static string getName () {
		return names [Random.Range (0, names.Length)];
	}
}
