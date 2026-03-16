using UnityEngine;

namespace WBC.Engine
{
    [RequireComponent(typeof(Controller))]
    [RequireComponent(typeof(Activity))]
    public abstract class Action : MonoBehaviour
    {
        public abstract bool Act();
    }
}
