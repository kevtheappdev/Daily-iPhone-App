using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace Daily
{  
	public enum DailyType {
	    DailyQuote,
		DailyImage,
		DailyWord,
		DailyLaugh,
		DailyWeather
	}
	
	public class DailyCoverView : UIView
	{
		private DailyType t;

		public UIActivityIndicatorView LoadingView{ get; set;}

		public DailyCoverView (CGRect frame, DailyType type) : base(frame)
		{
			this.type = type;
	         
			switch (type) {
			case DailyType.DailyQuote:
				MakeCover ("quotes.png", "Quote of the Day");
				this.BackgroundColor = UIColor.Red;
				break;
			case DailyType.DailyImage:
				this.BackgroundColor = UIColor.Blue;
				MakeCover ("image.png", "Image of the day");
				break;
			case DailyType.DailyWord:
				this.BackgroundColor = UIColor.Orange;
				MakeCover ("book.png", "Word of the Day");
				break;
			case DailyType.DailyWeather:
				this.BackgroundColor = UIColor.Green;
				MakeCover ("sun.png", "Weather for Today");
				break;
			}

		

		}

		public void MakeVisible(){
			Alpha = 1;
		}

		public void disapear(){
			
				UIView.Animate (.5, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
					Alpha = 0;
				}, 
					() => {
						this.RemoveFromSuperview();
					});
			
		}

		public void StartLoadingAnimation(){ 
			this.LoadingView.StartAnimating ();
		}

		public void StopLoadingAnimation(){
			Console.WriteLine ("stop animating " + LoadingView);
			this.LoadingView.StopAnimating ();
			Console.WriteLine ("animating: " + LoadingView.IsAnimating);
		}

		private void MakeCover(string imageName, string type){
			
		
			UILabel Label = new UILabel ();
			Label.Text = type;
			Label.TextColor = UIColor.White;
			Label.AdjustsFontSizeToFitWidth = true;
			Label.TextAlignment = UITextAlignment.Center;
			Label.Font = UIFont.FromName ("AppleSDGothicNeo-Bold", 100);

			AddSubview (Label);

			UIImageView Image = new UIImageView (new UIImage (imageName));
			AddSubview (Image);

			UIActivityIndicatorView Loading = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);

			AddSubview (Loading);

			UIView spacer1 = new UIView ();
			AddSubview (spacer1);
			UIView spacer2 = new UIView ();
			AddSubview (spacer2);


			Loading.TranslatesAutoresizingMaskIntoConstraints = false;
			spacer1.TranslatesAutoresizingMaskIntoConstraints = false;
			spacer2.TranslatesAutoresizingMaskIntoConstraints = false;
			Image.TranslatesAutoresizingMaskIntoConstraints = false;
			Label.TranslatesAutoresizingMaskIntoConstraints = false;


			NSLayoutConstraint cn = NSLayoutConstraint.Create (Loading, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0);
			AddConstraint (cn);

			cn = NSLayoutConstraint.Create (Label, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0);
			AddConstraint (cn);


			cn = NSLayoutConstraint.Create (Image, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0);
			AddConstraint (cn);

			cn = NSLayoutConstraint.Create (Label, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, Frame.Size.Width-50);
			AddConstraint (cn);

			NSDictionary views = NSDictionary.FromObjectsAndKeys (new object[]{spacer1, Label, Image, Loading, spacer2}, new object[] {"spacer1", "Label", "Image", "Loading", "spacer2"});
			NSLayoutConstraint[] constraints = NSLayoutConstraint.FromVisualFormat ("V:|-[spacer1]-[Label]-[Image]-30-[Loading]-[spacer2(==spacer1)]-|", 0, null, views);
			AddConstraints (constraints);

			this.LoadingView = Loading;
		}

		public DailyType type {
			get { return t;}
			set { t = value;}
		}
	}
}

