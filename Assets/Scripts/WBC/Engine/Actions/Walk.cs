using UnityEngine;

namespace WBC.Engine
{
    public class Walk : Action
    {
        public override bool Act()
        {
            Debug.Log("Walk Action");
            return false;
        }
    }
}
