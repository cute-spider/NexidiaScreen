using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexidiaScreen
{
	public class NexidiaScreenMath
	{
		public List<long> Factorize(long input)
		{
			Boolean negative = false;
			if (input == 0)
			{
				return new List<long> { 0 };
			}
			if (input < 0)
			{
				input = Math.Abs(input);
				negative = true;
			}


			List<long> retList = new List<long>();
			long x = PollardRho(input);
			long y = input / x;

			if (y == 1)
			{
				retList.Add(x);
				return retList;
			}
			else
			{
				retList.AddRange(Factorize(x));
				if (y == PollardRho(y))
				{
					retList.Add(y);
				}
				else
				{
					retList.AddRange(Factorize(y));
				}

				retList.Sort();

				// Since the input was negative, go ahead and make the first prime element negative.
				if (negative)
				{
					retList[0] = -retList[0];
				}

				return retList;
			}
		}

		//And implimtation of pollard's row. Finds a single divisor of input n
		public long PollardRho(long n, long init_x = 2)
		{
			//Algorithm based on https://en.wikipedia.org/wiki/Pollard%27s_rho_algorithm#Algorithm
			//with some extra consideration for detecting whether we're being fed a prime n.

			//Note: Somewhere around 50k, prime detection becomes very expensive. Given more time, I would like to explore
			//faster prime detection. 

			long x = init_x;
			long y = 2;
			long d = 1;
			long c = 0;

			while (d == 1 && c < n)
			{
				x = TestFactors(x, n);
				y = TestFactors(TestFactors(y, n), n);
				d = Gcd(Math.Abs(x - y), n);
				c++;
			}

			if (c == n || d == n)
			{
				if ((Math.Sqrt(n) + 3) < init_x)
				{
					// At this polong, each longeger between 2 and sqrt(n) + 1 has been tested, along
					// with a fair few additional longegers. With no divisors found, this is a prime 
					// number.
					return n;
				}
				return PollardRho(n, (init_x + 1));
			}

			else
			{
				if (d == 1) return n;
				return d;
			}
		}

		public long TestFactors(long x, long n)
		{
			if (n == 0) throw new DivideByZeroException();
			return ((x * (x + 1))) % n;
		}



		//https://en.wikipedia.org/wiki/Euclidean_algorithm#Implementations
		public long Gcd(long a, long b)
		{
			long t;
			while (b != 0)
			{
				t = b;
				b = a % b;
				a = t;
			}
			return a;
		}
	}
}
