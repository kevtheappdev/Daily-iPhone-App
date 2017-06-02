using System;
using Foundation;
using CoreLocation;
using UIKit;

namespace Daily
{


	public class LocationManager
	{
		private CLLocationManager lm;
		public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;

		public LocationManager ()
		{
			this.lm = new CLLocationManager ();
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				
				lm.RequestWhenInUseAuthorization (); // only in foreground
			}
		}

		public void StartLocationUpdates(){
			Console.WriteLine ("location update started");
			if (CLLocationManager.LocationServicesEnabled) {
				//set the desired accuracy, in meters
				Console.WriteLine("location services enabled");
				lm.DesiredAccuracy = 1;
				lm.StartUpdatingLocation();
				lm.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{ 
					Console.WriteLine("Hello");
					// fire our custom Location Updated event
					LocationUpdated (this, new LocationUpdatedEventArgs (e.Locations [e.Locations.Length - 1]));
				};

			}
		}

		public void EndLocationUpdates(){
			lm.StopUpdatingLocation ();

		}
	}
}

