using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//NexidiaScreen.cs
// Takes as a command-line argument the path to a file containing one integer per line
// For each integer in the file, output to the console a comma-delimited list of the integer's prime factors
// The list of integers on each line of the output should multiply to produce the input integer

namespace NexidiaScreen
{
	public class NexidiaScreen
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				System.Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " requires a input file. Exiting.");
				throw new ArgumentException(System.AppDomain.CurrentDomain.FriendlyName + " requires a input file.");
			}
			if (args.Length > 1)
			{
				System.Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " requires a single argument. Exiting.");
				throw new ArgumentException(System.AppDomain.CurrentDomain.FriendlyName + " requires a input file.");
			}
			string inputPath = args[0];

			if (!File.Exists(inputPath))
			{
				// File not found
				System.Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " could not find file: " + inputPath + ". Exiting");
				throw new FileNotFoundException(System.AppDomain.CurrentDomain.FriendlyName + " requires a input file.");
			}

			string[] lines = System.IO.File.ReadAllLines(@inputPath);
			NexidiaScreenMath NSM = new NexidiaScreenMath();

			foreach (string line in lines)
			{
				long target;
				try
				{
					target = Convert.ToInt64(line);
					Console.WriteLine(String.Join(", ", NSM.Factorize(target).ToArray()));
				}
				catch
				{
					Console.WriteLine("Unable to convert to integer value: " + line);
				}
			}
		}
	}
}
