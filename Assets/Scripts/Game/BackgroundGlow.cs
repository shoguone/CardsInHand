using System;
using UnityEngine;

namespace CardsInHand.Scripts.Game
{
    public class BackgroundGlow : MonoBehaviour
    {
        [Header("Set gameobject manually")]
        [SerializeField]
        private GameObject _bgrGlowing;

        [Header("OR choose prefab and parent for creating automatically")]
        [SerializeField]
        private GameObject _glowPrefab;

        [SerializeField]
        private Transform _glowParentTrf;
        
        [SerializeField]
        private string _glowParentNameToLookFor = "GlowParent";
        
        public void Glow(bool on = true)
        {
            if (_bgrGlowing != null)
            {
                _bgrGlowing.SetActive(on);
            }
        }

        private void Awake()
        {
            if (_bgrGlowing == null)
            {
                if (_glowPrefab == null)
                {
                    return;
                }
                
                if (_glowParentTrf == null)
                {
                    _glowParentTrf = transform.Find(_glowParentNameToLookFor);
                }

                _bgrGlowing = Instantiate(_glowPrefab, _glowParentTrf);
                Glow(false);
            }
        }
    }
}