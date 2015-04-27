using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Contracts.TimeManagement
{
    /// <summary>
    /// Class that represents a coordinate
    /// </summary>
    [DataContract]
    public class Coordinate
    {
        [DataMember]
        public decimal Lattitude;
        [DataMember]
        public decimal Longitude;
        [DataMember] 
        public DateTime Date;
    }
}
