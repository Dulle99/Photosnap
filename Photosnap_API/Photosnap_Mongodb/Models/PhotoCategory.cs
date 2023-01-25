using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Models
{
    public class PhotoCategory
    {
        #region Field(s)

        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }

        public List<User> UsersInterestedOfCategory { get; set; }

        public List<MongoDBRef> Photos { get; set; }

        #endregion Field(s)

        #region Constructor

        public PhotoCategory()
        {
            UsersInterestedOfCategory= new List<User>();
            Photos = new List<MongoDBRef>();
        }  

        #endregion Constructor
    }
}
