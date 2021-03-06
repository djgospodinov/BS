﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    [DataContract]
    public class VariableModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
    }

    [DataContract]
    public class LicenseVariableModel
    {
        [DataMember]
        public int Id{ get; set; }
        [DataMember]
        public Guid LicenseId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public object Value { get; set; }
    }
}
