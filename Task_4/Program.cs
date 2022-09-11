////happy implementation
////_____________________________________________________________________________
//for (int i = 1; i <= 100; i++)
//{
//    if (i % 3 == 0 && i % 5 == 0)
//    {
//        Console.WriteLine("FizzBuzz");
//    }
//    else if (i % 3 == 0)
//    {
//        Console.WriteLine("Fizz");
//    }
//    else if (i % 5 == 0)
//    {
//        Console.WriteLine("Buzz");
//    }
//    else
//    {
//        Console.WriteLine(i);

//    }

//}
////_____________________________________________________________________________

List<int> threeTimes = new();
List<int> fiveTimes = new();

int i = 3;
while (i <= 100)
{
    threeTimes.Add(i);
    i = i + 3;

}

i = 5;

while (i <= 100)
{
    fiveTimes.Add(i);
    i = i + 5;

}


for (i = 1; i <= 100; i++)
{

    var threeTimesTest = threeTimes.Where(m => m.Equals(i));
    var fivesTest = fiveTimes.Where(m => m.Equals(i));

    if (threeTimesTest.Count() > 0 && fivesTest.Count() > 0)
    {
        Console.WriteLine("FizzBuzz");
    }
    else if (threeTimesTest.Count() > 0)
    {
        Console.WriteLine("Fizz");
    }
    else if (fivesTest.Count() > 0)
    {
        Console.WriteLine("Buzz");
    }
    else
    {
        Console.WriteLine(i);
    }
}