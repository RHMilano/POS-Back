using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class InfoService
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "nameService")]
        public string NameService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "urlService")]
        public string UrlService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
