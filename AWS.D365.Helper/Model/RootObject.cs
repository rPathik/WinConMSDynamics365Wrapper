using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.D365.Helper.Model
{
    public class RootObject<T>
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }
        public List<T> Value { get; set; }
    }
}
