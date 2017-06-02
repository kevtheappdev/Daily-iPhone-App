using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace Daily
{
	public delegate void DoneLoading();

	public abstract class DailyView : UIView
	{
		private DailyCoverView c;
		public DoneLoading done{ get; set;}
		public UIView NavPallete { get; set;}


		public DailyView (CGRect frame) : base(frame)
		{
			NavPallete = new UIView ();
			NavPallete.TranslatesAutoresizingMaskIntoConstraints = false;
			//NavPallete.BackgroundColor = UIColor.Red;
			AddSubview (NavPallete);

			NSDictionary views = NSDictionary.FromObjectsAndKeys (new object[]{NavPallete}, new object[]{ "NavPallete"});
			NSLayoutConstraint [] cns = NSLayoutConstraint.FromVisualFormat ("|-[NavPallete]-|", 0, null, views);
			AddConstraints (cns);

			cns = NSLayoutConstraint.FromVisualFormat ("V:|-15-[NavPallete(50)]", 0, null, views);
			AddConstraints (cns);


			UIButton SettingsButton = UIButton.FromType (UIButtonType.System);
			SettingsButton.SetImage (UIImage.FromFile("cog.png"), UIControlState.Normal);
			SettingsButton.TranslatesAutoresizingMaskIntoConstraints = false;
			SettingsButton.TintColor = UIColor.White;

			UIButton ShareButton = UIButton.FromType (UIButtonType.Custom);
			ShareButton.SetImage (UIImage.FromFile("share.png"), UIControlState.Normal);
			ShareButton.TintColor = UIColor.White;
			ShareButton.TranslatesAutoresizingMaskIntoConstraints = false;


			NavPallete.AddSubview (SettingsButton);
			NavPallete.AddSubview (ShareButton);

			NSLayoutConstraint cn = NSLayoutConstraint.Create (SettingsButton, NSLayoutAttribute.LeadingMargin, NSLayoutRelation.Equal, NavPallete, NSLayoutAttribute.LeadingMargin, 1, 10);
			NavPallete.AddConstraint (cn);

			cn = NSLayoutConstraint.Create (SettingsButton, NSLayoutAttribute.TopMargin, NSLayoutRelation.Equal, NavPallete, NSLayoutAttribute.TopMargin, 1, 10);
			NavPallete.AddConstraint (cn);

			cn = NSLayoutConstraint.Create (ShareButton, NSLayoutAttribute.TrailingMargin, NSLayoutRelation.Equal, NavPallete, NSLayoutAttribute.TrailingMargin, 1, -10);
			NavPallete.AddConstraint (cn);

			cn = NSLayoutConstraint.Create (ShareButton, NSLayoutAttribute.TopMargin, NSLayoutRelation.Equal, NavPallete, NSLayoutAttribute.TopMargin, 1, 10);
			NavPallete.AddConstraint (cn);
		}

		public void DisplayCover(){
			Console.WriteLine ("display");
			AddSubview (c);
			c.MakeVisible ();
		}

		public void FadeCover(){
			c.disapear ();
		}

		public void ToggleCover(){
		   
		}

		public abstract void FetchData ();

		public abstract void SetupUI (object o);

		public DailyCoverView cover{
			get{ return c; }
			set{ c = value; }
		}

		public Boolean ContentLoaded { get; set;}
	
	}
}

