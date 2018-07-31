using UnityEngine;
using System;
using UnityEngine.Audio;

//namespace audioConfiguration {
public class AudioManager : MonoBehaviour {


        public static AudioManager instance;

		public Sound[] sounds; 

		void Awake()
		{
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);

			foreach (Sound s in sounds) 
			{

				s.source =  gameObject.AddComponent<AudioSource> (); 
				s.source.clip = s.clip; 

				s.source.volume = s.volume; 
				s.source.pitch = s.pitch; 
			}

		}


		public void Play(string name)
		{
			Sound s =  Array.Find(sounds, Sound => Sound.name == name ); 

			s.source.Play(); 

		}


}
//}