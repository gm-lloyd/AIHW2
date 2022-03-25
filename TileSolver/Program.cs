using System;
using System.Collections.Generic;
using System.Linq;

namespace TileSolver
{
	class Program
	{
		static void Main(string[] args)
		{

			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine("enter puzzle state: 0,1,2,3,4,5,6,7,8 =\n012\n345\n678");
				string input = Console.ReadLine();
				Astar(input);
				DFID(input);
				BFS(input);
			}
			

		}

		/// <summary>
		/// Method to handle taking input and prepping it for BFS
		/// Also Times the algo and prints the moves
		/// </summary>
		/// <param name="input"></param>
		private static void BFS(string input)
		{
			List<string> tileState = input.Split(",").ToList();
			Node first = new Node(null, tileState, "Start");
			var before = DateTime.Now;
			var solution = BreadthFirstSolve(first, tileState);
			var after = DateTime.Now;

			var timeTaken = after.Subtract(before);
			List<string> moveList = new List<string>();
			//get all moves and flip 
			Console.WriteLine("BFS:");
			PrintMoves(solution, timeTaken.ToString());
		}

		/// <summary>
		/// implementing Breadth First Search iteratively
		/// </summary>
		/// <param name="node"></param>
		/// <param name="tileState"></param>
		/// <returns></returns>
		private static Node BreadthFirstSolve(Node node, List<string> tileState)
		{
			List<Node> front = new List<Node>();
			front.Add(node);
			node = front[0];
			front.RemoveAt(0);
			while (!solved(tileState))
			{
				
				int blankIndex = tileState.FindIndex(FindSpace);
				int mIndex = -1;
				string move = "";
				// left side
				if(blankIndex == 0 || blankIndex == 3 || blankIndex == 6)
				{
					// 2,5,8 moves right
					mIndex = blankIndex + 2;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node newNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move);
					front.Add(newNode);

					// 1,4,7 moves left
					mIndex = blankIndex + 1;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node newNode2 = new Node(node, swap(leftTileState, blankIndex, mIndex), move);
					front.Add(newNode2);
				} 
				// right side
				else if (blankIndex == 2 || blankIndex == 5 || blankIndex == 8)
				{
					// 1,4,7 moves right
					mIndex = blankIndex - 1;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node newNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move);
					front.Add(newNode);

					// 0,3,6 moves left
					mIndex = blankIndex - 2;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node newNode2 = new Node(node, swap(leftTileState, blankIndex, mIndex), move);
					front.Add(newNode2);
				}
				else
				{
					// 0,3,6 moves right
					mIndex = blankIndex - 1;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node newNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move);
					front.Add(newNode);

					// 2,5,8 moves left
					mIndex = blankIndex + 1;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node newNode2 = new Node(node, swap(leftTileState, blankIndex, mIndex), move);
					front.Add(newNode2);

				}

				mIndex = (blankIndex + 3) % 9;
				move = "move " + tileState[mIndex].ToString() + " up, ";
				List<string> upTileState = new List<string>(tileState);
				Node upNode = new Node(node, swap(upTileState, blankIndex, mIndex), move);
				front.Add(upNode);

				mIndex = blankIndex - 3 < 0 ? blankIndex + 6 : blankIndex - 3;
				move = "move " + tileState[mIndex].ToString() + " down, ";
				List<string> downTileState = new List<string>(tileState);
				Node downNode = new Node(node, swap(downTileState, blankIndex, mIndex), move);
				front.Add(downNode);

				node = front[0];
				front.RemoveAt(0);
				tileState = node.TileState;

			}

			return node;
		}

		/// <summary>
		/// Handles prepping input for DFID, also timing and printing
		/// </summary>
		/// <param name="input"></param>
		private static void DFID(string input)
		{
			List<string> tileState = input.Split(",").ToList();

			Node first = new Node(null, tileState, "Start"), soln = new Node();
			var before = DateTime.Now;
			DateTime after = new DateTime();
			int depth = 1;
			bool finished = false;
			while(!finished)
			{
				soln = DepthFirstSolve(tileState, first, depth);
				if(soln == null)
				{
					depth++;
				} else
				{
					after = DateTime.Now;
					finished = true;
				}
			}
			var timeTaken = after.Subtract(before);

			//get all moves and flip 
			Console.WriteLine("DFID:");
			PrintMoves(soln, timeTaken.ToString());
		}

		/// <summary>
		/// implementing depth first iterative deepening recursively
		/// </summary>
		/// <param name="tileState"></param>
		/// <param name="node"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		private static Node DepthFirstSolve(List<string> tileState, Node node, int depth)
		{
			if(solved(tileState))
			{
				return node;
			}
			else
			{
				int blankIndex = tileState.FindIndex(FindSpace);
				int mIndex = -1;
				string move = "";
				Node soln = new Node();
				// left side
				if (blankIndex == 0 || blankIndex == 3 || blankIndex == 6)
				{
					// 2,5,8 moves right
					mIndex = blankIndex + 2;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node rightNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move);
					if (depth > 0)
					{
						soln = DepthFirstSolve(rightTileState, rightNode, depth - 1);
						if (soln != null) { return soln; }
					}

					// 1,4,7 moves left
					mIndex = blankIndex + 1;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node leftNode = new Node(node, swap(leftTileState, blankIndex, mIndex), move);
					if (depth > 0)
					{
						soln = DepthFirstSolve(leftTileState, leftNode, depth - 1);
						if (soln != null) { return soln; }
					}
				}
				// right side
				else if (blankIndex == 2 || blankIndex == 5 || blankIndex == 8)
				{
					// 1,4,7 moves right
					mIndex = blankIndex - 1;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node rightNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move);
					if (depth > 0)
					{
						soln = DepthFirstSolve(rightTileState, rightNode, depth - 1);
						if (soln != null) { return soln; }
					}

					// 0,3,6 moves left
					mIndex = blankIndex - 2;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node leftNode = new Node(node, swap(leftTileState, blankIndex, mIndex), move);
					if (depth > 0)
					{
						soln = DepthFirstSolve(leftTileState, leftNode, depth - 1);
						if (soln != null) { return soln; }
					}
				}
				else
				{
					// 0,3,6 moves right
					mIndex = blankIndex - 1;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node rightNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move);
					if (depth > 0)
					{
						soln = DepthFirstSolve(rightTileState, rightNode, depth - 1);
						if (soln != null) { return soln; }
					}

					// 2,5,8 moves left
					mIndex = blankIndex + 1;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node leftNode = new Node(node, swap(leftTileState, blankIndex, mIndex), move);
					if (depth > 0)
					{
						soln = DepthFirstSolve(leftTileState, leftNode, depth - 1);
						if (soln != null) { return soln; }
					}

				}

				mIndex = (blankIndex + 3) % 9;
				move = "move " + tileState[mIndex].ToString() + " up, ";
				List<string> upTileState = new List<string>(tileState);
				Node upNode = new Node(node, swap(upTileState, blankIndex, mIndex), move);
				if (depth > 0)
				{
					soln = DepthFirstSolve(upTileState, upNode, depth - 1);
					if (soln != null) { return soln; }
				}

				mIndex = blankIndex - 3 < 0 ? blankIndex + 6 : blankIndex - 3;
				move = "move " + tileState[mIndex].ToString() + " down, ";
				List<string> downTileState = new List<string>(tileState);
				Node downNode = new Node(node, swap(downTileState, blankIndex, mIndex), move);
				if (depth > 0)
				{
					soln = DepthFirstSolve(downTileState, downNode, depth - 1);
					if (soln != null) { return soln; }
				}

				return null;

			}
		}

		/// <summary>
		/// handles prepping input for A star search
		/// also timing and printing the moves
		/// </summary>
		/// <param name="input"></param>
		private static void Astar(string input)
		{
			List<string> tileState = input.Split(",").ToList();
			Node first = new Node(null, tileState, "Start");
			var before = DateTime.Now;
			var solution = AStarSolve(first, tileState);
			var after = DateTime.Now;

			var timeTaken = after.Subtract(before);
			//get all moves and flip 
			Console.WriteLine("A*:");
			PrintMoves(solution, timeTaken.ToString());
		}

		/// <summary>
		/// implementing A* search iteratively
		/// </summary>
		/// <param name="node"></param>
		/// <param name="tileState"></param>
		/// <returns></returns>
		private static Node AStarSolve(Node node, List<string> tileState)
		{
			List<Node> front = new List<Node>();

			front.Add(node);
			node = front[0];
			front.RemoveAt(0);
			while (!solved(tileState))
			{

				int blankIndex = tileState.FindIndex(FindSpace);
				int mIndex = -1;
				string move = "";
				// left side
				if (blankIndex == 0 || blankIndex == 3 || blankIndex == 6)
				{
					// 2,5,8 moves right
					mIndex = blankIndex + 2;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node newNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move, 
						node.ADepth + 1 + manhattanScore(rightTileState), node.ADepth + 1);
					front.Add(newNode);

					// 1,4,7 moves left
					mIndex = blankIndex + 1;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node newNode2 = new Node(node, swap(leftTileState, blankIndex, mIndex), move, 
						node.ADepth + 1 + manhattanScore(leftTileState), node.ADepth + 1);
					front.Add(newNode2);
				}
				// right side
				else if (blankIndex == 2 || blankIndex == 5 || blankIndex == 8)
				{
					// 1,4,7 moves right
					mIndex = blankIndex - 1;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node newNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move, 
						node.ADepth + 1 + manhattanScore(rightTileState), node.ADepth + 1);
					front.Add(newNode);

					// 0,3,6 moves left
					mIndex = blankIndex - 2;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node newNode2 = new Node(node, swap(leftTileState, blankIndex, mIndex), move,
						node.ADepth + 1 + manhattanScore(leftTileState), node.ADepth + 1);
					front.Add(newNode2);
				}
				else
				{
					// 0,3,6 moves right
					mIndex = blankIndex - 1;
					move = "move " + tileState[mIndex].ToString() + " right, ";
					List<string> rightTileState = new List<string>(tileState);
					Node newNode = new Node(node, swap(rightTileState, blankIndex, mIndex), move,
						node.ADepth + 1 + manhattanScore(rightTileState), node.ADepth + 1);
					front.Add(newNode);

					// 2,5,8 moves left
					mIndex = blankIndex + 1;
					move = "move " + tileState[mIndex].ToString() + " left, ";
					List<string> leftTileState = new List<string>(tileState);
					Node newNode2 = new Node(node, swap(leftTileState, blankIndex, mIndex), move,
						node.ADepth + 1 + manhattanScore(leftTileState), node.ADepth + 1);
					front.Add(newNode2);

				}

				mIndex = (blankIndex + 3) % 9;
				move = "move " + tileState[mIndex].ToString() + " up, ";
				List<string> upTileState = new List<string>(tileState);
				Node upNode = new Node(node, swap(upTileState, blankIndex, mIndex), move,
					node.ADepth + 1 + manhattanScore(upTileState), node.ADepth + 1);
				front.Add(upNode);

				mIndex = blankIndex - 3 < 0 ? blankIndex + 6 : blankIndex - 3;
				move = "move " + tileState[mIndex].ToString() + " down, ";
				List<string> downTileState = new List<string>(tileState);
				Node downNode = new Node(node, swap(downTileState, blankIndex, mIndex), move,
					node.ADepth + 1 + manhattanScore(downTileState), node.ADepth + 1);
				front.Add(downNode);



				front = front.OrderBy(o => o.score).ToList();

				node = front[0];
				front.RemoveAt(0);
				tileState = node.TileState;

			}

			return node;
		}

		/// <summary>
		/// Method for printing the moves given a list of nodes visited
		/// </summary>
		/// <param name="soln"></param>
		/// <param name="timeTaken"></param>
		private static void PrintMoves(Node soln, string timeTaken)
		{
			List<string> moveList = new List<string>();
			//get all moves and flip 
			while (soln.oldNode != null)
			{
				moveList.Add(soln.MoveInfo);
				soln = soln.oldNode;
			}
			string solnString = "";
			moveList.Reverse();
			foreach (string move in moveList)
			{
				solnString += move + " ";
			}
			solnString += "\nsolved in " + timeTaken + "\n";
			Console.WriteLine(solnString);
		}

		/// <summary>
		/// calculates the manhattan distance for each tile and assigns the tilestate a total score 
		/// </summary>
		/// <param name="tileState"></param>
		/// <returns></returns>
		private static int manhattanScore(List<string> tileState)
		{
			int totalScore = 0;
			//1 should be in index 0
			int index = tileState.FindIndex(Find1);
			if (index == 1 || index == 2 || index == 3 || index == 6)
				totalScore += 1;
			else if (index == 4 || index == 5 || index == 7 || index == 8)
				totalScore += 2;

			//2 should be in index 1
			index = tileState.FindIndex(Find2);
			if (index == 0 || index == 2 || index == 4 || index == 7)
				totalScore += 1;
			else if (index == 5 || index == 3 || index == 6 || index == 8)
				totalScore += 2;

			//3 should be in index 2
			index = tileState.FindIndex(Find3);
			if (index == 0 || index == 1 || index == 5 || index == 8)
				totalScore += 1;
			else if (index == 4 || index == 3 || index == 7 || index == 6)
				totalScore += 2;

			//4 should be in index 3
			index = tileState.FindIndex(Find4);
			if (index == 0 || index == 5 || index == 4 || index == 6)
				totalScore += 1;
			else if (index == 1 || index == 2 || index == 7 || index == 8)
				totalScore += 2;

			//5 should be in index 5
			index = tileState.FindIndex(Find5);
			if (index == 2 || index == 3 || index == 4 || index == 8)
				totalScore += 1;
			else if (index == 1 || index == 0 || index == 7 || index == 6)
				totalScore += 2;

			//6 should be in index 6
			index = tileState.FindIndex(Find6);
			if (index == 0 || index == 3 || index == 7 || index == 8)
				totalScore += 1;
			else if (index == 4 || index == 1 || index == 5 || index == 2)
				totalScore += 2;

			//7 should be in index 7
			index = tileState.FindIndex(Find7);
			if (index == 1 || index == 6 || index == 4 || index == 8)
				totalScore += 1;
			else if (index == 3 || index == 0 || index == 2 || index == 5)
				totalScore += 2;

			//8 should be in index 8
			index = tileState.FindIndex(Find8);
			if (index == 2 || index == 5 || index == 7 || index == 6)
				totalScore += 1;
			else if (index == 4 || index == 1 || index == 3 || index == 0)
				totalScore += 2;

			return totalScore;
		}

		/// <summary>
		/// takes a tile state and swaps the two tiles at the given indices
		/// </summary>
		/// <param name="newTileState"></param>
		/// <param name="blankIndex"></param>
		/// <param name="mIndex"></param>
		/// <returns></returns>
		private static List<string> swap(List<string> newTileState, int blankIndex, int mIndex)
		{
			newTileState[blankIndex] = newTileState[mIndex];
			newTileState[mIndex] = " ";
			return newTileState;
		}

		//helper methods to find values in the tile state
		#region search methods
		private static bool FindSpace(string s)
		{
			return s == " ";
		}
		private static bool Find1(string s)
		{
			return s == "1";
		}
		private static bool Find2(string s)
		{
			return s == "2";
		}
		private static bool Find3(string s)
		{
			return s == "3";
		}
		private static bool Find4(string s)
		{
			return s == "4";
		}
		private static bool Find5(string s)
		{
			return s == "5";
		}
		private static bool Find6(string s)
		{
			return s == "6";
		}
		private static bool Find7(string s)
		{
			return s == "7";
		}
		private static bool Find8(string s)
		{
			return s == "8";
		}
		#endregion

		/// <summary>
		/// helper method to check if a tile state is solved or at the goal state
		/// </summary>
		/// <param name="tileState"></param>
		/// <returns></returns>
		private static bool solved(List<String> tileState)
		{
			var isSolved = false;
			List<string> correct = new List<string>{ "1", "2", "3", "4", " ", "5", "6", "7", "8"};
			for (int i = 0; i < correct.Count; i++)
			{

				isSolved = tileState[i] == correct[i].ToString();
				if (!isSolved) { return isSolved; }
			}
			return isSolved;
		}



	}

	/// <summary>
	/// Node Class
	/// used to track moves and hold the state of the puzzle through the searches
	/// </summary>
	class Node
	{
		public string MoveInfo;
		public List<string> TileState;
		public Node oldNode;
		//manhattan score heuristic
		public int score;
		//depth of node to be used for admissible function
		public int ADepth = 0;

		//default constructor
		public Node()
		{
			MoveInfo = "";
			TileState = new List<string>();
			oldNode = null;
			score = int.MaxValue;
		}

		// creates a normal node with given params
		public Node(Node node, List<string> list, string move)
		{
			oldNode = node;
			TileState = list;
			MoveInfo = move;
		}

		// creates scored node with given params
		public Node(Node node, List<string> list, string move, int score, int dep)
		{
			oldNode = node;
			TileState = list;
			MoveInfo = move;
			this.score = score;
			ADepth = dep;
		}

	}
}
