using System;

namespace atm.Helpers
{
	public static class SecureUrlRewriter
	{
		public static string CreateSecureUri(Uri original)
		{

			if (original.Scheme == "https" && original.Port == 443) return original.ToString();

			return new UriBuilder(original)
			{
				Port = 443,
				Scheme = "https"
			}.ToString();
		}

	}
}