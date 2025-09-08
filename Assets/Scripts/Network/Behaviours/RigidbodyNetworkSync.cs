using Mirror;
using UnityEngine;

namespace SLRemake.Network.Behaviours
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyNetworkSync : NetworkBehaviour
    {
        public Rigidbody Rb;

        [SyncVar(hook = nameof(OnMassChanged))]
        public float Mass;

        [SyncVar(hook = nameof(OnUseGravityChanged))]
        public bool UseGravity;

        [SyncVar(hook = nameof(OnIsKinematicChanged))]
        public bool IsKinematic;

        public bool IsFrozen
        {
            get
            {
                return Rb.isKinematic && Rb.IsSleeping();
            }
            set
            {
                Rb.isKinematic = value;
                if (!value)
                    Rb.Sleep();
                else
                    Rb.WakeUp();
            }
        }
        protected void Awake()
        {
            if (Rb == null)
                Rb = GetComponent<Rigidbody>();
            IsKinematic = Rb.isKinematic;
            Mass = Rb.mass;
            UseGravity = Rb.useGravity;
        }

        private void OnMassChanged(float _, float newValue)
        {
            Rb.mass = Mathf.Max(0.001f, newValue);
        }

        private void OnUseGravityChanged(bool _, bool newValue)
        {
            Rb.useGravity = newValue;
        }

        private void OnIsKinematicChanged(bool _, bool newValue)
        {
            Rb.isKinematic = newValue;
        }
    }
}