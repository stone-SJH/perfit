using System;
using System.Linq;
using System.Text;
using Client.FileOp;
using System.Collections.Generic;

namespace Client.Statistic
{

    class HandDataFileExtraction
    {
        private static string _path;
        private static FileOperation _fileOperation;
        public HandDataFileExtraction(string path)
        {
            _path = path;
            _fileOperation = new FileOperation(_path);
        }

        public Dictionary<string, double> GetStatistic()
        {
            Dictionary<string, double> ret = new Dictionary<string, double>();
            List <string> content = new List<string>();
            _fileOperation.ReadContent(out content);
            if (content.Count != 12)
            {
                return ret;
            }
            ret.Add("palm", double.Parse(content[0]));
            ret.Add("thumb", double.Parse(content[1]));
            ret.Add("forefinger", double.Parse(content[2]));
            ret.Add("middle finger", double.Parse(content[3]));
            ret.Add("ring finger", double.Parse(content[4]));
            ret.Add("little finger", double.Parse(content[5]));
            ret.Add("great", double.Parse(content[6]));
            ret.Add("good", double.Parse(content[7]));
            ret.Add("bad", double.Parse(content[8]));
            ret.Add("average time", double.Parse(content[9]));
            ret.Add("maxmum time", double.Parse(content[10]));
            ret.Add("minimum time", double.Parse(content[11]));

            foreach (KeyValuePair<string, double> k in ret)
            {
                Console.Write(k.Key + ": ");
                Console.WriteLine(k.Value);
            }
            return ret;
        }
    }
}
