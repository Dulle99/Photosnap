using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service
{
    public class PhotosnapCollections
    {
        public static string User { get { return "user"; } }
        public static string Photo { get { return "photo"; } }
        public static string PhotoCategory { get { return "photoCategory"; } }
        public static string Following { get { return "following"; } }
        public static string Like { get { return "like"; } }
        public static string Comment { get { return "comments"; } }
    }
}
