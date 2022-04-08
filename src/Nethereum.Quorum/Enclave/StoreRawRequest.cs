﻿using System.Runtime.Serialization;

namespace Nethereum.Quorum.Enclave
{
    [DataContract]
    public class StoreRawRequest
    {
        [DataMember(Name =  "payload")]
        public string Payload { get; set; }
        [DataMember(Name =  "from")]
        public string From { get; set; }
    }
}