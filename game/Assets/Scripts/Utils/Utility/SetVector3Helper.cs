    using UnityEngine;
    using UnityEngine.Events;

    public class SetVector3Helper : AutoMonoBehaviour
    {
        public Vector3 toSet;
        public UnityEvent<Vector3> setFunctions;

        public void DoSetVector3() {
            setFunctions.Invoke(toSet);
        }
    }