using System;
using Foundation;
using System.Net;
using System.IO;
using System.Json;
using UIKit;

namespace Daily
{
	public delegate void FinishedRequest(string response);
	public delegate void WeatherReady(Weather w);
	public delegate void QuoteReady(Quote q);
	public delegate  void ImageReady(Image i);
	public delegate void WordReady(Word w);


	public static class DailyAPIFetcher
	{
		private static string quoteUrl = "http://api.theysaidso.com/qod.json";
		private static string imageUrl = "http://image-a-day.herokuapp.com/image";
		private static string wordUrl = "https://www.kimonolabs.com/api/44goxs92?apikey=9956a8bbe54954cff332c53e96c634ec";
		private static string weatherUrl = "http://api.openweathermap.org/data/2.5/weather?units=imperial&";
		private static WebRequest req;
		private static FinishedRequest finished;
		private static QuoteReady GetQuote;
		private static ImageReady GetImage;

		public static QuoteReady FinishedQuote{get { return GetQuote;} set{ GetQuote = value;}}
		public static ImageReady FinishedImage{get { return GetImage;} set{ GetImage = value;}}
		public static WeatherReady wr { get; set;}
		public static WordReady FinishedWord{ get; set;}

		public static void FetchQuote(){
			

			BeginRequest (quoteUrl, "application/json", "GET");

			finished = delegate(string content) {
				Console.WriteLine (content + "quote content");
				if (content != null) {
					var obj = JsonObject.Parse (content);
					var quote = obj ["contents"] ["quotes"] [0] ["quote"];
					var author = obj ["contents"] ["quotes"] [0] ["author"];
					Console.WriteLine ("Quote: " + quote + " author: " + author);
					Quote q = new Quote { DailyQuote = quote, Author = author, TimeStamp = DateTime.Now.ToString ("yyMMdd") };
					GetQuote (q);
				}
			}; 



			
		}

		public static void FetchImage(){
			
			BeginRequest(imageUrl, "application/json", "GET");
			finished = delegate(string content) {
				Console.WriteLine("success");
			 
				if (content != null) {
					var obj = JsonObject.Parse (content);
					var imageURL = obj ["imageUrl"];
					var copyrightURL = obj ["copyrightLink"];
					var description = obj ["description"];
					var copyright = obj ["copyright"];
					Console.WriteLine(content + "success");
					Image i = new Image {
						url = imageURL,
						copyright = copyright,
						description = description,
						copyrightUrl = copyrightURL,
						timeStamp = DateTime.Now.ToString ("yyMMdd")
					};
					GetImage(i);

				}
			};
		
		}

		public static void FetchWord(){
			BeginRequest (wordUrl, "application/json", "GET");
			finished = delegate (string content) {
	
				if (content != null) {
					var obj = JsonObject.Parse (content);
					var today = obj ["results"] ["collection1"] [0];
					var word = today ["word"];
					var definition = today ["definition"];
					var pronunciation = today ["pronunciation"];
					Word w = new Word {
						Definition = definition,
						TheWord = word,
						Pronunciation = pronunciation ,
						TimeStamp = DateTime.Now.ToString ("ddMMyy")
					};
					FinishedWord(w);

				}

			};

		}

		public static void FetchWeather(string lat, string lon){
			 BeginRequest (weatherUrl+"lat="+lat+"&lon="+lon, "application/json", "GET");
			finished = delegate(string content) {

				if (content != null) {
					var obj = JsonObject.Parse (content);
					var description = obj ["weather"] [0] ["description"];
					var temp = Math.Round ((double)obj ["main"] ["temp"]);
					var location = obj ["name"];
					var conditionCode = obj ["weather"] [0] ["id"];
					var icon = obj ["weather"] [0] ["icon"];
					Weather todaysWeather = new Weather {
						Temp = temp.ToString () + "°",
						Condition = description,
						Location = location,
						TimeStamp = DateTime.Now.ToString ("yyMMdd"),
						condition = conditionCode,
						Iconcode = icon
					};
					wr (todaysWeather);
				}
		
			};

		}

		public static void FetchAsync(){
			BeginRequest (wordUrl, "application/json", "GET");
			finished = delegate(string response) {
				//Console.WriteLine(response);
			};
		}

		private static string MakeRequest(string url, string type, string method)
		{
			Console.WriteLine ("url " + url);
			var req = WebRequest.Create (url); 
			req.ContentType = type;
			req.Method = method;

			using (HttpWebResponse response = req.GetResponse () as HttpWebResponse) {
				
				if (response.StatusCode != HttpStatusCode.OK)
				 	Console.Out.WriteLine ("Error fetching data. Server returned status code " + response.StatusCode);
				    
				using (StreamReader reader = new StreamReader (response.GetResponseStream ())) {
					var content = reader.ReadToEnd ();
					response.Close ();
					if (string.IsNullOrWhiteSpace (content)) {
						Console.Out.WriteLine ("Response contained empty body");
					} else {
						return content;
					}
				}

			}

			return null;
		}

		private static void BeginRequest(string url, string type, string method){
			req = WebRequest.Create (url); 

			req.ContentType = type;
			req.Method = method;
			req.BeginGetResponse (new AsyncCallback (FinishRequest), req);
		}

		private static void FinishRequest(IAsyncResult results){
			Console.WriteLine ("finished");
			try {
				HttpWebResponse response = (results.AsyncState as HttpWebRequest).EndGetResponse(results) as HttpWebResponse;
			Console.WriteLine ("finished1");
			if (response.StatusCode != HttpStatusCode.OK)
				Console.Out.WriteLine ("Error fetching data. Server returned status code " + response.StatusCode);
			using (StreamReader reader = new StreamReader (response.GetResponseStream ())) {
				var content = reader.ReadToEnd ();
				response.Close ();
				if (string.IsNullOrWhiteSpace (content + " : fetched content")) {
					Console.Out.WriteLine ("Response contained empty body");
				} else {
						Console.WriteLine(content + "response content");
					finished (content);
				}
			}
			} catch(Exception ex) {
				Console.WriteLine ("Exception " + ex);
			
			}
		}  









	
	}
}

