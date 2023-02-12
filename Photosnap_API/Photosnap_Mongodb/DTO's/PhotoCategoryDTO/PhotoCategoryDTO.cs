using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.PhotoCategoryDTO
{
    public class PhotoCategoryDTO
    {
        #region Field(s)

        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }

        #endregion Field(s)

        #region Constructor

        public PhotoCategoryDTO() { }

        public PhotoCategoryDTO(string cateogoryName, string categoryColor) 
        {
            this.CategoryName = cateogoryName;  
            this.CategoryColor = categoryColor; 
        }

        #endregion Constructor
    }
}
