using System;
using System.IO;

namespace FinalGriever.ContentTools.PrepReplays
{
    public class FileMover
    {
        protected string From { get; set; }

        protected string To { get; set; }

        protected string Prefix { get; set; }

        public FileMover(string from, string to, string prefix)
        {
            From = from;
            To = to;
            Prefix = prefix;
        }

        public void Move()
        {
            LocationExists(From);
            LocationExists(To);
            foreach (string file in Directory.EnumerateFiles(From, "*", SearchOption.AllDirectories))
            {
                Move(file, From, To);
            }
        }

        protected void Move(string file, string from, string to)
        {
            var fileName = System.IO.Path.GetFileName(file);
            System.IO.File.Move(file, $"{to}\\\\\\{Prefix}{fileName}");
        }

        protected void LocationExists(string location)
        {
            if(!System.IO.Directory.Exists(location)) {
                throw new UserException("The provided directory does not exist");
            }
        }
    }
}
