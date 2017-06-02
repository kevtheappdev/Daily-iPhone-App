using System;
using CoreGraphics;
using UIKit;
using System.Net;
using System.IO;
using Foundation;

namespace Daily
{
	public class DImageView : DailyView
	{
		private Image i;
		private UIImageView iv;
		private UILabel l;
		private UILabel cl;

		public DImageView (CGRect frame) : base(frame)
		{

			BackgroundColor = UIColor.Black;
			iv = new UIImageView (new CGRect (0, 0, frame.Size.Width, frame.Size.Height));
			iv.ContentMode = UIViewContentMode.ScaleAspectFit;
			AddSubview (iv);
			l = new UILabel ();
			cl = new UILabel ();
			l.Lines = 0;

			cl.Lines = 0;
			l.Text = "My Text";
			cl.Text = "Copyright";
			cl.TextColor = UIColor.Gray;
			l.TextColor = UIColor.White;
			AddSubview (cl);
			AddSubview (l);
			l.Font = UIFont.SystemFontOfSize (25);
			l.TranslatesAutoresizingMaskIntoConstraints = false;
			cl.Font = UIFont.SystemFontOfSize (11);
			cl.TranslatesAutoresizingMaskIntoConstraints = false;
			var views = NSDictionary.FromObjectsAndKeys (new NSObject[]{l, cl}, new NSObject[]{new NSString("label"), new NSString("copylabel")});

			var c = NSLayoutConstraint.FromVisualFormat ("|-[label]-|", 0, null, views);
			AddConstraints (c);
			c = NSLayoutConstraint.FromVisualFormat ("V:[label]-[copylabel]", 0, null, views);
			AddConstraints (c); 

			c = NSLayoutConstraint.FromVisualFormat ("|-[copylabel]-|", 0 , null, views); 
			AddConstraints (c);

			c = NSLayoutConstraint.FromVisualFormat ("V:[copylabel]-|", 0, null, views);
			AddConstraints (c);

			this.cover = new DailyCoverView (new CGRect(0,0, frame.Size.Width, frame.Size.Height), DailyType.DailyImage);
			AddSubview (this.cover);
			Console.WriteLine ("my frame: " + frame + " cover frame: " + this.cover.Frame);
		}

		public override void FetchData(){
			
			Console.Write ("worked");
			i = DailyLocalFetcher.TodaysImage ();
			System.Console.WriteLine (i);
			if (i != null) {
				if (i.timeStamp.Equals (DateTime.Now.ToString ("yyMMdd"))) {
					i = DailyLocalFetcher.TodaysImage ();
					string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					string localPath = Path.Combine (documentsPath, "downloaded.png");
					iv.Image = UIImage.FromFile (localPath);

					SetupUI (i);

					Console.WriteLine ("Image is good");
				} else {
					FetchAPI ();
				}
			} else {
				FetchAPI ();
			}
		}

		private void FetchAPI(){
			DailyAPIFetcher.FetchImage ();
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localPath = Path.Combine (documentsPath, "downloaded.png");
			DailyAPIFetcher.FinishedImage += delegate(Image i) {
				DailyLocalFetcher.ImageUpdate (i);
				FetchImage (i.url);
				Console.WriteLine (i + " finished");
				InvokeOnMainThread (() => {

					SetupUI(i);


				});
			};

		}

		public override void SetupUI(object o){
			Image i = (Image)o;
			l.Text = i.description;
			cl.Text = i.copyright;
			done ();
		}

		private void FetchImage(string imageUrl){
			Console.WriteLine ("Fetching image");
			var webClient = new WebClient ();
			webClient.DownloadDataCompleted += (s, e) => {
				var bytes = e.Result; // get the downloaded data
				string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string localFilename = "downloaded.png";
				string localPath = Path.Combine (documentsPath, localFilename);
				File.WriteAllBytes (localPath, bytes); // writes to local storage   
				InvokeOnMainThread (() => {
					iv.Image = UIImage.FromFile (localPath);

					 
					done ();
					//self.view.backgroundColor = [UIColor colorWithPatternImage:[UIImage imageNamed:@"Default"]];
				});
			};
			var url = new Uri(imageUrl);
			webClient.DownloadDataAsync(url);
		}


	}
}

