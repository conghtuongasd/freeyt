using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouYou.Models
{
    public class QualityVM
    {
        public string mimeType { get; set; }
        public long contentLength { get; set; }
        public string url { get; set; }
        public string signatureCipher { get; set; }
        public string qualityLabel { get; set; }
    }
}
