using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using atm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.tests.unit.SecureUrlRewriterTests
{
	[TestClass]
	public class CreateSecureUriTests
	{
		[TestMethod]
		public void HttpShouldBeHttps()
		{
			var uri = new UriBuilder(@"http://sygmanetwork.com");
			const string expected = @"https://sygmanetwork.com:443/";
			var result = SecureUrlRewriter.CreateSecureUri(uri.Uri);

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ItAlreadyHttpsShouldNotChange()
		{
			var uri = new UriBuilder(@"https://sygmanetwork.com");
			const string expected = @"https://sygmanetwork.com/";
			var result = SecureUrlRewriter.CreateSecureUri(uri.Uri);

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ItPreservesPaths()
		{
			var uri = new UriBuilder(@"http://sygmanetwork.com/subdir/page.html");
			const string expected = @"https://sygmanetwork.com:443/subdir/page.html";
			var result = SecureUrlRewriter.CreateSecureUri(uri.Uri);

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ItPreservesQueryStrings()
		{
			var uri = new UriBuilder(@"http://sygmanetwork.com/subdir/route?name=test");
			const string expected = @"https://sygmanetwork.com:443/subdir/route?name=test";
			var result = SecureUrlRewriter.CreateSecureUri(uri.Uri);

			Assert.AreEqual(expected, result);
		}
	}
}