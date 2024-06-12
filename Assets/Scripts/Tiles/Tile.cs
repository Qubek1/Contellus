using Entities;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        protected Player Player;
        public Material m;
        public Color PowerOnColor;
        public Color PowerOnColor2;
        public Color PowerOffColor;
        public Color ProxyHighlight;
        public Gradient DamageGradient;
        public bool PowerGenerator;
        public bool Powered = true;
        public float MaxHealth = 100;
        public float Health = 100;
        private float Regeneration = 10;
        public PowerUpManager PUM;
        public Vector2Int Position;

        private void Awake()
        {
            if (Game.Instance.CurrentPlayer == null)
            {
                Debug.Log("Player nie istnieje, jestesmy w dupie");
            }
            else
            {
                Player = Game.Instance.CurrentPlayer.GetComponent<Player>();
            }
        }

        private void Update()
        {
            Health += Regeneration * Time.deltaTime;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            if (Health < MaxHealth && Powered)
            {
                var MR = gameObject.GetComponent<MeshRenderer>();
                MR.material.SetColor("_Color", DamageGradient.Evaluate(Health / MaxHealth));
            }
        }

        public virtual void ApplyPowerUP()
        {
        }

        public virtual void PowerOn()
        {
            Color color = PowerOnColor;
            var MR = gameObject.GetComponent<MeshRenderer>();
            MR.material.SetColor("_NeonColor", PowerOnColor2);
            MR.material.SetColor("_EmissionColor", Color.white);
            var MR2 = gameObject.GetComponentsInChildren<MeshRenderer>();
            if (MR2.Length > 1)
            {
                MR2[1].material.color = color * 2;
            }

            Powered = true;
        }

        public virtual void PowerOff()
        {
            Color color = PowerOffColor;
            var MR = gameObject.GetComponent<MeshRenderer>();
            MR.material.SetColor("_NeonColor", PowerOffColor);
            var MR2 = gameObject.GetComponentsInChildren<MeshRenderer>();
            if (MR2.Length > 1)
            {
                MR2[1].material.color = color;
            }

            Powered = false;
        }

        public void Highlight(bool t)
        {
            if (Health == MaxHealth)
            {
                if (t == true)
                {
                    var MR = gameObject.GetComponent<MeshRenderer>();
                    MR.material.SetColor("_Color", ProxyHighlight);
                }
                else
                {
                    var MR = gameObject.GetComponent<MeshRenderer>();
                    MR.material.SetColor("_Color", Color.white);
                }
            }
        }

        public void TakeDamage(float Amount)
        {
            if (Powered)
            {
                Health -= Amount * Time.deltaTime;
                if (Health <= 0f)
                {
                    Health = MaxHealth;
                    PUM.DestroyTile(Position);
                    var mr = gameObject.GetComponent<MeshRenderer>();
                    mr.material.SetColor("_Color", Color.white);
                }
            }
        }
    }
}
