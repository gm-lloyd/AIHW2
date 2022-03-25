using System;
using System.Collections.Generic;
using System.Linq;

namespace QueenSolver
{
	class Program
	{
		//starting point for program
		static void Main(string[] args)
		{


			Console.WriteLine("N-Queens, n=");
			var input = Console.ReadLine();
			var before = DateTime.Now;
			List<int> soln = QueenAnnealing(int.Parse(input));
			var after = DateTime.Now;
			int row = 1;
			foreach(int col in soln)
			{
				Console.WriteLine("put queen at row:" + row.ToString() + " col:" + (col+1).ToString());
				row++;
			}
			Console.WriteLine("\nsolved in " + after.Subtract(before).ToString());
		}

		/// <summary>
		/// implementation of simulated annealing with n-queens problem
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		private static List<int> QueenAnnealing(int size = 8)
		{
			int[] init = new int[size];
			List<int> boardState = randomSoln(init);

			int temp = 1000000;
			//while no queens are attacking each other
			while (countIntersections(boardState) != 0)
			{
				List<int> newBoardState = randomNbr(boardState);

				if (countIntersections(newBoardState) <= countIntersections(boardState))
				{
					boardState = newBoardState;
				} 
				else if (acceptLowerScore(temp, countIntersections(newBoardState) - countIntersections(boardState)))
				{
					boardState = newBoardState;
				}
				temp--;

				//reset temp if stuck in unsovlable minimum (Not likely)
				if(temp < -10000000)
				{
					temp = 1000000;
					Console.WriteLine("resetting temp");
				}
			}
			return boardState;
		}

		private static bool acceptLowerScore(int temp, int delta)
		{
			double threshold = 0.001;

			return Math.Exp(temp / delta) > threshold;
		}

		private static int countIntersections(List<int> boardState)
		{
			int count = 0;
			
			for(int i = 0; i < boardState.Count-1; i++)
			{
				for (int j = i + 1; j < boardState.Count; j++)
				{
					if (boardState[i] == boardState[j])
						count++;
					else if (boardState[i] - (i) == boardState[j] - (j))
						count++;
					else if (boardState[i] + i == boardState[j] + j)
						count++;
				}
			}

			return count;
		}

		private static List<int> randomSoln(int[] boardState)
		{
			Random random = new Random();
			for (int i = 0; i < boardState.Length; i++)
			{
				int col = random.Next(0, 8);
				boardState[i] = col;

			}

			return boardState.ToList();

		}

		private static List<int> randomNbr(List<int> boardState)
		{
			if(boardState.Count == 0)
			{
				throw new Exception("pass non empty list");
			}
			Random random = new Random();
			int col = random.Next(0, 8);
			int row = random.Next(0, 8);
			List<int> newBoardState = new List<int>(boardState);
			newBoardState[row] = col;

			return newBoardState;

		}
	}
}
