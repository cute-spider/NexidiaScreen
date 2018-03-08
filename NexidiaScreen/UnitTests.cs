using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NexidiaScreen
{
	[TestFixture]
	class UnitTests
	{
		NexidiaScreenMath NSM = new NexidiaScreenMath();
		IList<long> primes = new List<long>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997, 1009 };
		String testCases = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\..\\TestCases\\";

		[TestCase]
		public void Factorize()
		{
			Assert.AreEqual(new List<long> { 2, 11 }, NSM.Factorize(22));
			Assert.AreEqual(new List<long> { 167 }, NSM.Factorize(167));
			Assert.AreEqual(new List<long> { 2 }, NSM.Factorize(2));
			Assert.AreEqual(new List<long> { 2, 2, 5, 5 }, NSM.Factorize(100));
			Assert.AreEqual(new List<long> { 2, 2 }, NSM.Factorize(4));
			Assert.AreEqual(new List<long> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 }, NSM.Factorize(200560490130));
		}

		[TestCase]
		public void Factorize_Random()
		{
			long i = 0;
			long j;
			Random rnd = new Random();

			while (i < 10000)
			{
				long testProduct = 1;
				List<long> targetList = new List<long>();

				for (j = 0; j < 5; j++)
				{
					long selectPrime = primes[rnd.Next(0, 15)];
					long selectFactor = rnd.Next(0, 3);
					testProduct *= (long)Math.Pow(selectPrime, selectFactor);
					for (; selectFactor > 0; selectFactor--)
					{
						targetList.Add(selectPrime);
					}
				}
				targetList.Sort();

				if (testProduct == 1) targetList = new List<long>() { 1 };

				// Toss out the overflows.
				if (testProduct > 0)
				{
					// Uncomment the following to see all those random test cases.
					TestContext.Progress.WriteLine("Testing: " + String.Join(" * ", targetList.ToArray()) + " = " + testProduct);
					Assert.AreEqual(targetList, NSM.Factorize(testProduct));
				}
				i++;
			}
		}

		[TestCase]
		public void Gcd()
		{
			Assert.AreEqual(NSM.Gcd(5, 2), 1);
			Assert.AreEqual(NSM.Gcd(4, 2), 2);
			Assert.AreEqual(NSM.Gcd(15, 3), 3);
			Assert.AreEqual(NSM.Gcd(55, 11), 11);
			Assert.AreEqual(NSM.Gcd(5, 0), 5);
			Assert.AreEqual(NSM.Gcd(0, 0), 0);
			Assert.AreEqual(NSM.Gcd(-10, -2), -2);
			Assert.AreEqual(NSM.Gcd(-10, 2), 2);
			Assert.AreEqual(NSM.Gcd(5, 1), 1);
		}

		[TestCase]
		public void TestFactors()
		{
			Assert.AreEqual(6, NSM.TestFactors(2, 8051));

			Assert.Throws<DivideByZeroException>(() => NSM.TestFactors(8, 0));
		}

		[TestCase]
		public void PollardRho()
		{
			//Pollard's Rho algorythm does not imperically detect whether a 
			//number is prime. I've added some detection to the algorithm to 
			//predict primes. 
			//Though I've truncated this list to 1000, I tested primes based on this list http://www.primos.mat.br/primeiros_10000_primos.txt
			//Note: Somewhere around 50k, prime detection becomes very expensive. Given more time, I would like to explore
			//faster prime detection. 

			foreach (long x in primes)
			{
				// TestContext.Progress.WriteLine("testing " + x);
				Assert.AreEqual(x, NSM.PollardRho(x));
			}

			Assert.AreEqual(83, NSM.PollardRho(8051));
			Assert.AreEqual(2, NSM.PollardRho(4));

			Assert.AreEqual(2, NSM.PollardRho(7474));
			Assert.AreEqual(17, NSM.PollardRho(2839));
			Assert.AreEqual(67, NSM.PollardRho(871));
			Assert.AreEqual(12, NSM.PollardRho(1848));
			Assert.AreEqual(0, NSM.PollardRho(0));

		}

		[TestCase]
		public void Integration_NoArguement()
		{
			Assert.AreEqual("NexidiaScreen.exe requires a input file. Exiting.\r\n",
				new IntegrationProcess("").result);
		}

		[TestCase]
		public void Integration_ManyArguements()
		{
			Assert.AreEqual("NexidiaScreen.exe requires a single argument. Exiting.\r\n",
				new IntegrationProcess("foo bar").result);
		}

		[TestCase]
		public void Integration_FileNotFound()
		{
			Assert.AreEqual("NexidiaScreen.exe could not find file: TestFile-1.txt. Exiting\r\n",
			new IntegrationProcess("TestFile-1.txt").result);
		}

		[TestCase]
		public void Integration_TextFile1()
		{
			Assert.AreEqual("2\r\n3\r\n5\r\n7\r\n11\r\n13\r\n17\r\n23\r\n29\r\n",
				new IntegrationProcess(@testCases + "TestFile1.txt").result);
		}

		[TestCase]
		public void Integration_TextFile2()
		{
			Assert.AreEqual("2, 5\r\n2, 2, 5, 5\r\n2, 2, 2, 5, 5, 5\r\n2, 2, 2, 2, 5, 5, 5, 5\r\n2, 2, 11\r\n0\r\n2, 2, 2, 2, 2, 2, 2, 2\r\n2, 2, 3, 3, 5, 5\r\n2, 3, 167\r\n",
				new IntegrationProcess(@testCases + "TestFile2.txt").result);
		}

		[TestCase]
		public void Integration_TextFile3()
		{
			TestContext.Progress.WriteLine(new IntegrationProcess(@testCases + "TestFile3.txt").result);

			Assert.AreEqual("2, 2, 2, 5\r\n2, 3, 5\r\n2, 2, 5\r\n2, 5\r\nUnable to convert to integer value: 5.5\r\n",
				new IntegrationProcess(@testCases + "TestFile3.txt").result);
		}

		[TestCase]
		public void Integration_TextFile4()
		{
			Assert.AreEqual("Unable to convert to integer value: a\r\n2, 2, 11\r\n0\r\n-2, 3, 11\r\n0\r\n",
				new IntegrationProcess(@testCases + "TestFile4.txt").result);
		}

		public class IntegrationProcess
		{
			private Process p;
			public String result;

			public IntegrationProcess(string commandLineInput)
			{
				//https://msdn.microsoft.com/en-us/library/system.diagnostics.process.standardoutput.aspx
				p = new Process();
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NexidiaScreen.exe";
				p.StartInfo.Arguments = commandLineInput;
				p.StartInfo.CreateNoWindow = true;
				p.Start();

				// To avoid deadlocks, always read the output stream first and then wait.
				result = p.StandardOutput.ReadToEnd();
				p.WaitForExit();
			}
		}
	}
}
