using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Converters;
using UnityEngine;
 
public class AotTypeEnforcer : MonoBehaviour
{
    public void Awake()
    {
        AotHelper.EnsureType<StringEnumConverter>();
    }
}
