using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]public class BGData : ScriptableObject {
	[Header("BG DATA")]public List<ParallaxGroup> targets;
	[Header("BG COLOR")]public Color color;
}