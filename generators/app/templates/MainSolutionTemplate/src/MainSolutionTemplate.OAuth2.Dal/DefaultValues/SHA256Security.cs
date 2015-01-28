using System;
using System.Security.Cryptography;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.OAuth2.Dal.DefaultValues
{
	public class SHA256Security : IOAuthSecurity
	{
		#region Implementation of IOAuthSecurity

		public string GetHash(string input)
		{
			HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
			byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
			byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
			return Convert.ToBase64String(byteHash);
		}

		#endregion
	}
}