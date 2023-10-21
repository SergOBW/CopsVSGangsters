﻿using UnityEngine;

namespace EnemyCore
{
    [CreateAssetMenu()]
    public class EnemySoundSo : ScriptableObject
    {
        public AudioClip[] dieClip;
        public AudioClip[] longRangeAttack;
        public AudioClip[] shortAtackClip;
        public AudioClip[] woundringClip;
        public AudioClip[] hitClips;
        
    }
}