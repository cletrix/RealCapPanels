using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixerControl : MonoBehaviour 
{

	public AudioManager audioManager;
	public Slider sMUSIC;
	public Slider sSFX;
	public Text txtMusicCount;
	public Text txtSfxCount;
	void Start () 
	{
		audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		sMUSIC.value=audioManager._volumeMusic;
		sSFX.value=audioManager._volumeSFX;
		txtMusicCount.text=(sMUSIC.value*100).ToString("000")+"%";
		txtSfxCount.text=(sSFX.value*100).ToString("000")+"%";
	}
	
	// Update is called once per frame
	public void SfxAtualizar()
	{
		audioManager._volumeSFX=sSFX.value;
		if(sSFX.value==100)
		{
			txtSfxCount.text=(sSFX.value*100).ToString("000")+"%";
		}
		else
		{
			txtSfxCount.text=(sSFX.value*100).ToString("00")+"%";
		}
	}
	public void MusicAtualizar()
	{
		audioManager._volumeMusic=sMUSIC.value;
		if(sSFX.value==100)
		{
			txtMusicCount.text=(sMUSIC.value*100).ToString("000")+"%";
		}
		else
		{
			txtMusicCount.text=(sMUSIC.value*100).ToString("00")+"%";
		}
	}
}
