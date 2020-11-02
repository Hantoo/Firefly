using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ServerResources
{
    public static class utils
    {

		public static string ByteArrayToString(byte[] ba)
		{
			byte[] test = new byte[] {0x10, 0x10, 0x10 };
			return System.Text.Encoding.UTF8.GetString(ba);
		}

		public static byte[] StringToByteArray(string s)
		{
			return System.Text.Encoding.UTF8.GetBytes(s);
		}

		public static string ByteArrayToHexString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2} ", b);
			return hex.ToString();
		}

		public static string IntArrayToHexString(int[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (int b in ba)
				hex.AppendFormat("{0:}", b);
			return hex.ToString();
		}

		public static bool CompareByteArrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;
			return ByteArrayStartsWith(a, b);
		}

		public static bool ByteArrayStartsWith(byte[] a, byte[] b)
		{
			for (int i = 0; i < b.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			return true;
		}

		public static long GetUnixTimestamp()
		{
			System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			return (long)(System.DateTime.UtcNow - epochStart).TotalSeconds;
		}

		public static long GetUnixTimestampMillis()
		{
			System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			return (long)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		}

		public static IPAddress GetLocalIPAddress()
		{
			IPHostEntry host;
			IPAddress localIP = IPAddress.Any;
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					localIP = ip;
					break;
				}
			}
			return localIP;
		}

		public static string GetLocalBroadcastAddress()
		{
			string localIP = GetLocalIPAddress().ToString();
			localIP = localIP.Substring(0, localIP.LastIndexOf(".") + 1);
			localIP += "255";
			return localIP;
		}

		public static int ConvertLoHiBytesToInt(byte Low, byte High)
        {
			int valueInt = (High * 256) + Low;
			return valueInt;
        } 


		public static byte[] EncodeConsistentOverheadByteStuffing(byte[] data)
        {
			byte[] reparsedData = new byte[data.Length + 2];
			byte delimiter = 0x00; //The marker for an end of packet
			int positionOfLastKnownByteClash = 0;

			for (int i = 1; i < data.Length + 1; i++)
			{
				if (data[i - 1] == delimiter)
				{
					
					reparsedData[positionOfLastKnownByteClash] = (byte)(i - positionOfLastKnownByteClash);
					positionOfLastKnownByteClash = i;
				}
				else
				{
					reparsedData[i] = data[i - 1];
				}
			}
			reparsedData[positionOfLastKnownByteClash] = (byte)(reparsedData.Length - (positionOfLastKnownByteClash + 1));

			return reparsedData;

		}

		public static byte[] DecodeConsistentOverheadByteStuffing(byte[] data)
		{
			byte[] reparsedData = new byte[data.Length - 2];
			byte delimiter = 0x00; //The marker for an end of packet
			int positionOfLastKnownByteClash = data[0];

			for (int i = 1; i < data.Length - 1; i++)
			{
				if (i == positionOfLastKnownByteClash)
				{
					positionOfLastKnownByteClash = positionOfLastKnownByteClash + (int)data[i];
					reparsedData[i - 1] = delimiter;
				}
				else
				{
					reparsedData[i - 1] = data[i];
				}
			}
			return reparsedData;
		}
	}

}

