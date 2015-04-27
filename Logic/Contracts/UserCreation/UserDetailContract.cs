using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    [DataContract]
    public class UserDetailContract
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int CompanyID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string EmployerNumber { get; set; }
        [DataMember]
        public string IdNumber { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string CellPhoneNumber { get; set; }
    }
}
