namespace DiceRoller;

using System.Diagnostics;
using System.Threading.Tasks;

internal class Program
{
	private static void Main()
	{
		Console.WriteLine("Enter dice to roll in the format XdY, where X is the number of dice and Y is the number of sides as integers. Ctrl+C to exit.");
		while (true)
		{
			var input = new List<int>();
			while (input.Count == 0)
			{
				input = Validate(Console.ReadLine());
			}

			Console.WriteLine($"Rolled {Roll(input).Result}");
		}

		// ReSharper disable once FunctionNeverReturns
	}

	private static async Task<string> Roll(IReadOnlyList<int> dice)
	{
		var rand = new Random();
		long result = 0;
		var diceCount = dice[0];
		var timer = new Stopwatch();

		// Give the user feedback if they enter a ridiculously large number, it'll probably take > 1000ms
		if (Debugger.IsAttached || diceCount > 100000000)
		{
			Console.WriteLine("Rolling");
			timer.Start();
		}
		
		// This is, counter-intuitively, about 20% faster than just running the loop on its own
		await Task.Run(() =>
		{
			for (var i = 0; i < diceCount; i++)
			{
				result += rand.Next(1, dice[1]);
			}
		});

		if (Debugger.IsAttached)
		{
			Console.WriteLine($"Took {timer.ElapsedMilliseconds}ms");
		}

		return result.ToString();
	}

	private static List<int> Validate(string? entry)
	{
		var splitEntry = entry?.Split('d');
		if (splitEntry?.Length == 2 && int.TryParse(splitEntry[0], out var diceCount) && int.TryParse(splitEntry[1], out var diceFaces))
		{
			return
			[
				diceCount,
				diceFaces,
			];
		}

		Console.WriteLine("Enter a value in the format XdY, where X is the number of dice and Y is the number of sides as integers.");
		return [];
	}
}
