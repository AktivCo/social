using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace AdUsers
{
    [Serializable]
    [XmlRoot(ElementName = "Structure", Namespace = "")]
    [DataContract]
    public class AdUser
    {
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Office { get; set; }
        [DataMember]
        public int? OfficePhone { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public int sAMAccountType { get; set; }
        /// <summary>
        /// чисто ради css
        /// </summary>
        [DataMember]
        public string Dep { get { return Department.Sum(s => s).ToString(); } set { } }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public bool HasPicture { get; set; }

        public DateTime CreatedWhen { get; set; }
        [DataMember]
        public bool Disabled { get; set; }
        [XmlIgnore]
        public byte[] Userpic { get; set; }
        [DataMember]
        public DateTime? LastLogon { get; set; }
        [DataMember]
        public string CN { get; set; }
        [DataMember]
        public string Manager { get; set; }
        [DataMember]
        public DateTime WhenCreated { get; set; }

        [DataMember]
        public string Sid { get; set; }
        [DataMember]
        public List<AdUser> Users { get; set; }
    }
}
