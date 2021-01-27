using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutubeGetLink.Models
{
   public class Params
   {
      public string q { get; set; }
      public string fl { get; set; }
      public string start { get; set; }
      public string fq { get; set; }
      public string rows { get; set; }
      public string wt { get; set; }
   }

   public class ResponseHeader
   {
      public int status { get; set; }
      public int QTime { get; set; }
      public Params Params { get; set; }
   }

   public class Doc
   {
      public int listen_no { get; set; }
      public string full_singer { get; set; }
      public string url { get; set; }
      public string image { get; set; }
      public string full_name { get; set; }
   }

   public class Response
   {
      public int numFound { get; set; }
      public int start { get; set; }
      public IList<Doc> docs { get; set; }
   }

   public class Example
   {
      public ResponseHeader responseHeader { get; set; }
      public Response response { get; set; }
   }

}
