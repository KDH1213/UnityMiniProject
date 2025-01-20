using Newtonsoft.Json;

// 실제 값을 직렬, 역직렬 할 것인지,
// 참조형을 생성해 줘야 하는지

//public class SaveItemData
//{
//    public int instanceId;
//    [JsonConverter(typeof(ItemDataConverter))]
//    public ItemData data;
//    public System.DateTime creationTime;

//    public SaveItemData()
//    {
//        System.Guid guid = System.Guid.NewGuid();
//        instanceId = guid.GetHashCode();
//        creationTime = System.DateTime.Now;
//    }
//}
