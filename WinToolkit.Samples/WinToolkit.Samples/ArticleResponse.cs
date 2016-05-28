using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinToolkit.Samples
{
    public class ArticlesResponse
    {
        public Article[] Content { get; set; }
    }

    public class Article
    {
        public Media Media { get; set; }
    }

    public class Media
    {
        public Image[] Images { get; set; }
    }

    public class Image
    {
        public string SmallHdUrl { get; set; }
    }

}
