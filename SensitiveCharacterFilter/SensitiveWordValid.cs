using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SensitiveWordFilterLib
{
	public class SensitiveWordValid
	{
		private Dictionary<string, object> hash;
		private BitArray firstCharCheck;
		private BitArray allCharCheck;
		private int maxLength;
		private readonly string sensitivewordfilename;

		public SensitiveWordValid()
		{
			sensitivewordfilename = @"sensitiveword.txt";
		}

		public SensitiveWordValid(string sf)
		{
			sensitivewordfilename = string.IsNullOrWhiteSpace(sf) ? "sensitiveword.txt" : sf;
		}

		private IList<string> ReadSensitiveWord()
		{
			IList<string> result = null;

			if (!string.IsNullOrWhiteSpace(sensitivewordfilename))
			{
				var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", sensitivewordfilename);

				if (File.Exists(fullPath))
				{
					result = new List<string>();

					using (StreamReader sr = new StreamReader(fullPath))
					{
						var line = string.Empty;
						line = sr.ReadLine();
						while (!string.IsNullOrWhiteSpace(line))
						{
							foreach (var item in line.Split('|'))
							{
								if (!string.IsNullOrWhiteSpace(item))
								{
									result.Add(item);
								}
							}

							line = sr.ReadLine();
						}
					}
				}
			}

			return result;
		}

		public void Init()
		{
			hash = new Dictionary<string, object>();
			firstCharCheck = new BitArray(char.MaxValue);
			allCharCheck = new BitArray(char.MaxValue);
			maxLength = 0;

			try
			{
				var badwords = ReadSensitiveWord();

				foreach (string word in badwords)
				{
					if (!hash.ContainsKey(word))
					{
						hash.Add(word, null);
						maxLength = Math.Max(maxLength, word.Length);
						firstCharCheck[word[0]] = true;

						foreach (char c in word)
						{
							allCharCheck[c] = true;
						}
					}
				}
			}
			catch (ArgumentException)
			{
				throw;
			}
			catch (DirectoryNotFoundException)
			{
				throw;
			}
			catch (FileNotFoundException)
			{
				throw;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (IOException)
			{
				throw;
			}
		}

		public bool SensitiveWordExist(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}

			int index = 0;
			//int offset = 0;
			while (index < text.Length)
			{
				if (!firstCharCheck[text[index]])
				{
					while (index < text.Length - 1 && !firstCharCheck[text[++index]]) ;
				}

				for (int j = 1; j <= Math.Min(maxLength, text.Length - index); j++)
				{
					if (!allCharCheck[text[index + j - 1]])
					{
						break;
					}

					string sub = text.Substring(index, j);

					if (hash.ContainsKey(sub))
					{
						return true;
					}
				}

				index++;
			}

			return false;
		}

		public string ReplaceSensitiveWord(string text, string r)
		{
			var result = text;

			if (!string.IsNullOrWhiteSpace(text))
			{
				int index = 0;
				//int offset = 0;
				while (index < text.Length)
				{
					if (!firstCharCheck[text[index]])
					{
						while (index < text.Length - 1 && !firstCharCheck[text[++index]]) ;
					}

					for (int j = 1; j <= Math.Min(maxLength, text.Length - index); j++)
					{
						if (!allCharCheck[text[index + j - 1]])
						{
							break;
						}

						string sub = text.Substring(index, j);

						if (hash.ContainsKey(sub))
						{
							var replaceStr = string.Empty;
							for (var i = 0; i < sub.Length; i++)
							{
								replaceStr += r;
							}

							result = result.Replace(sub, replaceStr);
						}
					}

					index++;
				}
			}

			return result;
		}

	}
}
