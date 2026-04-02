using UnityEngine;

namespace WBC.Engine
{
    [RequireComponent(typeof(Controller))]
    public abstract class Action : MonoBehaviour
    {
        public abstract bool Act();
    }
}
