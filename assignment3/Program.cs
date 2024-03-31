
// TODO: declare a constant to represent the max size of the values
// and dates arrays. The arrays must be large enough to store
// values for an entire month.
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

int physicalSize = 31;
int logicalSize = 0;

// TODO: create a double array named 'values', use the max size constant you declared
// above to specify the physical size of the array.
double[] values = new double[physicalSize];

// TODO: create a string array named 'dates', use the max size constant you declared
// above to specify the physical size of the array.
string[] dates = new string[physicalSize];

bool goAgain = true;
  while (goAgain)
  {
    try
    {
      DisplayMainMenu();
      string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
      if (mainMenuChoice == "L")
        logicalSize = LoadFileValuesToMemory(dates, values);
      if (mainMenuChoice == "S")
        SaveMemoryValuesToFile(dates, values, logicalSize);
      if (mainMenuChoice == "D")
        DisplayMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "A")
        logicalSize = AddMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "E")
        EditMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "Q")
      {
        goAgain = false;
        throw new Exception("Bye, hope to see you again.");
      }
      if (mainMenuChoice == "R")
      {
        while (true)
        {
          if (logicalSize == 0)
					  throw new Exception("No entries loaded. Please load a file into memory");
          DisplayAnalysisMenu();
          string analysisMenuChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
          if (analysisMenuChoice == "A"){
            double sum = FindAverageOfValuesInMemory(values, logicalSize);
            Console.WriteLine($"Average of values = {sum}");}
          if (analysisMenuChoice == "H"){
            double max = FindHighestValueInMemory(values, logicalSize);
            Console.WriteLine($"The highest value = {max}");}
          if (analysisMenuChoice == "L"){
            double min = FindLowestValueInMemory(values, logicalSize);
            Console.WriteLine($"The lowest value = {min}");}
          if (analysisMenuChoice == "G")
            GraphValuesInMemory(dates, values, logicalSize);
          if (analysisMenuChoice == "R")
            throw new Exception("Returning to Main Menu");
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
    }
  }

void DisplayMainMenu()
{
	Console.WriteLine("\nMain Menu");
	Console.WriteLine("L) Load Values from File to Memory");
	Console.WriteLine("S) Save Values from Memory to File");
	Console.WriteLine("D) Display Values in Memory");
	Console.WriteLine("A) Add Value in Memory");
	Console.WriteLine("E) Edit Value in Memory");
	Console.WriteLine("R) Analysis Menu");
	Console.WriteLine("Q) Quit");
}

void DisplayAnalysisMenu()
{
	Console.WriteLine("\nAnalysis Menu");
	Console.WriteLine("A) Find Average of Values in Memory");
	Console.WriteLine("H) Find Highest Value in Memory");
	Console.WriteLine("L) Find Lowest Value in Memory");
	Console.WriteLine("G) Graph Values in Memory");
	Console.WriteLine("R) Return to Main Menu");
}

string Prompt(string prompt)
{
  string response = "";
  Console.Write(prompt);
  response = Console.ReadLine();
  return response;
}

string GetFileName()
{
	string fileName = "";
	do
	{
		fileName = Prompt("Enter file name including .csv or .txt: ");
	} while (string.IsNullOrWhiteSpace(fileName));
	return fileName;
}

int LoadFileValuesToMemory(string[] dates, double[] values)
{
	string fileName = GetFileName();
	int logicalSize = 0;
	string filePath = $"./data/{fileName}";
	if (!File.Exists(filePath))
		throw new Exception($"The file {fileName} does not exist.");
	string[] csvFileInput = File.ReadAllLines(filePath);
	for(int i = 0; i < csvFileInput.Length; i++)
	{
		Console.WriteLine($"lineIndex: {i}; line: {csvFileInput[i]}");
		string[] items = csvFileInput[i].Split(',');
		for(int j = 0; j < items.Length; j++)
		{
			Console.WriteLine($"itemIndex: {j}; item: {items[j]}");
		}
		if(i != 0)
		{
		dates[logicalSize] = items[0];
    values[logicalSize] = double.Parse(items[1]);
    logicalSize++;
		}
	}
  Console.WriteLine($"Load complete. {fileName} has {logicalSize} data entries");
	return logicalSize;
}

void DisplayMemoryValues(string[] dates, double[] values, int logicalSize)
{
	if(logicalSize == 0)
		throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
	Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
	Console.WriteLine($"   Date     Value");
	for (int i = 0; i < logicalSize; i++)
		Console.WriteLine($"{dates[i]}   {values[i]}");
}

double FindHighestValueInMemory(double[] values, int logicalSize)
{
	double max = 0;
      for (int i = 0; i < logicalSize; i++)
      {
        if (values[i] > max)
          max = values[i];
      }
      return max;
}

double FindLowestValueInMemory(double[] values, int logicalSize)
{
    double min = FindHighestValueInMemory(values, logicalSize);
      for (int i = 0; i < logicalSize; i++)
      {
        if (values[i] < min)
          min = values[i];
      }
      return min;
}

double FindAverageOfValuesInMemory(double[] values, int logicalSize)
{
	double sum = 0;
    for (int i = 0; i < logicalSize; i++)
      sum += values[i];
    return sum / logicalSize;
}

void SaveMemoryValuesToFile(string[] dates, double[] values, int logicalSize)
{
  string fileName = GetFileName();
  string filePath = $"./data/{fileName}";
  if(logicalSize == 0)
    throw new Exception($"No entries to save. Load file or add values.");
  Array.Sort(dates, values, 0, logicalSize);
  string[] fileLines = new string[logicalSize + 1];
  fileLines[0] = "dates,values";
  for(int i = 1; i <= logicalSize; i++)
  {
    fileLines[i] = $"{dates[i - 1]},{values[i - 1].ToString()}";
  }
  File.WriteAllLines(filePath, fileLines);
  Console.WriteLine($"Saved. {fileName} now has {logicalSize} values.");  
}

int AddMemoryValues(string[] dates, double[] values, int logicalSize)
{
  double value = 0.0;
  string date;

  date = GetDate("Enter date in format mm-dd-yyyy: ");
  for (int i = 0; i < logicalSize; i++)
    if (dates[i].Equals(date))
      throw new Exception($"{date} is already in memory. Edit entry instead.");
  value = GetValue($"Enter a double ", 0, 100);
  dates[logicalSize] =  date;
  values[logicalSize] = value;
  logicalSize++;
	return logicalSize;
}

string GetDate(string prompt)
{
  bool InvalidInput;
  DateTime date = DateTime.Today;
  do
  {
    try
    {
      Console.Write(prompt);
      date = DateTime.Parse(Console.ReadLine());
      InvalidInput = false;
    }
    catch(Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
      InvalidInput = true;
    }
  } while (InvalidInput);
  return date.ToString("MM-dd-yyyy");
}

double GetValue(string prompt, double min, double max)
{
  double value = 0.0;
  bool invalid;

  do
  {
    try
    {
      Console.Write($"{prompt}between {min} and {max}: ");
      value = Double.Parse(Console.ReadLine());
      if((value > 100) || (value < 0))
        throw new Exception($"Value is out of range. Try again.");
      else invalid = false;
    }
    catch(Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
      invalid = true;
    }
  } while (invalid);
  return value;
}

void EditMemoryValues(string[] dates, double[] values, int logicalSize)
{
    string date = GetDate($"Enter date in format MM-dd-yy: ");
    int index = 0;
    for(int i = 0; i < logicalSize; i++)
    if(dates[i].Equals(date))
        index = i;
    if(index == 0)
        throw new Exception($"Date not found. Add or load entry.");
    if(index != 0)
    {
        values[index] = GetValue($"Enter new value ", 0, 99);
    }
}

void GraphValuesInMemory(string[] dates, double[] values, int logicalSize)
{
  int num = 100;
  int count = 1;
  string[] saleday = new string[logicalSize];
  for(int index = 0; index < logicalSize; index++)
  {
    saleday[index] = dates[index].Substring(3,2);
  }
  int[] dayint = new int[logicalSize];
  for(int index = 0; index < logicalSize; index++)
  {
    dayint[index] = int.Parse(saleday[index]);
  }
	Console.Write($"Dollars");
  do
  {
      num -= 10;
      count = 1;
      Console.Write($"\n    ${num} |");
      for(int i = 0; i < logicalSize; i++)
      {
        if(values[i] < num + 10 && values[i] >= num)
        {
          while(count < dayint[i])
          {
            Console.Write("   ");
            count += 1;
          }
          Console.Write($" {values[i]}");
          count +=1;
        }
      }
  }while(num > 10);
  Console.Write("\n        -");
  while(count < 117)
  {
    Console.Write($"-");
    count += 1;
  }
  Console.Write($"\n   Days |");
  count = 1;
  while(count < 10)
  {
    Console.Write($" 0{count}");
    count += 1;
  }
    while(count < 32)
  {
    Console.Write($" {count}");
    count += 1;
  }
}