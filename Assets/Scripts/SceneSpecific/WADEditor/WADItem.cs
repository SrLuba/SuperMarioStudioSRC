using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WADItem : MonoBehaviour {
	public int id;
	public Text AssetTypeIF, AssetNameIF;


	public void UpdateIFs() {
		ResourcePack_Lump lump = WADManager.wad.lumps [id];

		AssetTypeIF.text = lump.lumpType;
		AssetNameIF.text = lump.lumpName;
	}

	public void Show() {
		WADManager.instance.Edit (this.id);	
		WADManager.instance.idSelect = this.id;
	}

	public void Remove(){
		WADManager.instance.Remove (this.id);
	}
	public void Replace() {
		WADManager.instance.Replace (this.id);
	}
	public void Switch(int side) {
		WADManager.instance.Switch (side, this.id);
	}
}
