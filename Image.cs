using System;

namespace Daily
{
	public class Image
	{

		public string url{get; set;}
		public string description { get; set;}
		public string timeStamp { get; set;}
		public string copyrightUrl { get; set;}
		public string copyright { get; set;}

		public override string ToString ()
		{
			return string.Format ("[Image: url={0}, description={1}, timeStamp={2}]", url, description, timeStamp);
		}
	}
}

