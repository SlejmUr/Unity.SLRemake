using Mirror;
using SLRemake.Extensions;
using UnityEngine;

namespace SLRemake.Network.Behaviours
{
    public class NetworkParenter : MonoBehaviour
    {
        public GameObject MainObject;
        public GameObject Parent;

        public void Awake()
        {
            MainObject.transform.SetParent(Parent.transform, false);
        }
    }

}