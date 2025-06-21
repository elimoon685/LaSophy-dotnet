using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContract.HttpClient
{
    public class BookMetaData
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }

        public string PdfPath { get; set; }

        public string ImgPath { get; set; }
    }


}
