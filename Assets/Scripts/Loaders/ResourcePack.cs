using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]public class ResourcePack_Lump {
	[Header("Lump Name")]public string lumpName;
	[Header("Lump Type")]public string lumpType;


	[Header("Lump Data")]public byte[] lumpData;
	[Header("Lump Object Data")] public string[] objectData;

	/// <summary>
	/// Initializes a new instance of the <see cref="LE.LE_Resources+ResourcePack_Lump"/> class.
	/// </summary>
	/// <param name="lumpName">Lump name.</param>
	/// <param name="lumpType">Lump type.</param>
	/// <param name="lumpData">Lump data.</param>
	/// <param name="dobjectData">Dobject data.</param>
	public ResourcePack_Lump(string lumpName, string lumpType, byte[] lumpData, string dobjectData) {
		this.lumpName=lumpName;
		this.lumpData=lumpData;
		this.lumpType=lumpType;
		if (dobjectData!="") this.objectData = dobjectData.Split('|');
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="LE.LE_Resources+ResourcePack_Lump"/> class.
	/// </summary>
	/// <param name="lumpName">Lump name.</param>
	/// <param name="lumpType">Lump type.</param>
	/// <param name="lumpData">Lump data.</param>
	/// <param name="objectData">Object data.</param>
	public ResourcePack_Lump(string lumpName, string lumpType, byte[] lumpData, string[] objectData) {
		this.lumpName=lumpName;
		this.lumpData=lumpData;
		this.lumpType=lumpType;
		this.objectData = objectData;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="LE.LE_Resources+ResourcePack_Lump"/> class.
	/// </summary>
	/// <param name="lumpName">Lump name.</param>
	/// <param name="texturePath">Texture path.</param>
	/// <param name="sprite">Sprite.</param>
	public ResourcePack_Lump(string lumpName,string texturePath, Sprite sprite) {
		this.lumpName = lumpName;
		this.lumpType = "Sprite";
		string data = texturePath+"|"+sprite.rect.position.x.ToString()+"|"+sprite.rect.position.y.ToString()+"|"+sprite.rect.size.x.ToString()+"|"+sprite.rect.size.y.ToString();
		this.objectData = data.Split ('|');
	}
}
[System.Serializable]public class ResourcePack
{
	/// <summary>
	/// The lumps.  
	/// </summary>
	public List<ResourcePack_Lump> lumps;
	/// <summary>
	/// Initializes a new instance of the <see cref="LE.LE_Resources+ResourcePack"/> class.
	/// </summary>
	/// <param name="lumps">Lumps.</param>
	public ResourcePack(List<ResourcePack_Lump> lumps){this.lumps=lumps;}
	/// <summary>
	/// Initializes a new instance of the <see cref="LE.LE_Resources+ResourcePack"/> class.
	/// </summary>
	public ResourcePack() {
		this.lumps = new List<ResourcePack_Lump> ();
	}
	/// <summary>
	/// Save the specified path.
	/// </summary>
	/// <param name="path">Path.</param>
	public void Save(string path) {

		Directory.CreateDirectory(Application.persistentDataPath+"/Resources/");
		string rpath = Application.persistentDataPath+"/Resources/"+path+".wad";
		using (Stream stream = File.Open(rpath, FileMode.Create))
		{

			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			binaryFormatter.Serialize(stream, this);
		}
		Debug.Log ("<color=green>Saved WAD Succesfully to " + rpath + "</color>");
	}
	/// <summary>
	/// Load the specified path.
	/// </summary>
	/// <param name="path">Path.</param>
	public void Load(string path) {
		Directory.CreateDirectory(Application.persistentDataPath+"/Resources/");
		string rpath = Application.persistentDataPath+"/Resources/"+path+".wad";
		using (Stream stream = File.Open(rpath, FileMode.Open))
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			ResourcePack rp = (ResourcePack)binaryFormatter.Deserialize(stream);

			this.lumps = rp.lumps;
		}
		Debug.Log ("<color=green>Loaded WAD Succesfully from " + rpath + "</color>");
	}
public void Load(bool nonRelative, string path) {
	if (!nonRelative)
		return;
		Directory.CreateDirectory(Application.persistentDataPath+"/Resources/");
		string rpath = path;
		using (Stream stream = File.Open(rpath, FileMode.Open))
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			ResourcePack rp = (ResourcePack)binaryFormatter.Deserialize(stream);

			this.lumps = rp.lumps;
		}
	}
}
