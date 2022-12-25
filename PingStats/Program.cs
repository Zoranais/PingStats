// See https://aka.ms/new-console-template for more information
using OfficeOpenXml;
using PingStats;
using PingStats.Enums;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

string input = string.Empty;
string address = "www.google.com";

while (!Pinger.ValidateAddress(input))
{
	Console.WriteLine($"Enter an address to ping or press Enter to keep default value ({address})");
	input = Console.ReadLine();
	if (input == string.Empty)
		input = address;
}

address = input;

Pinger pinger = new Pinger(address);

pinger.StartTimer(1000);

Console.WriteLine("Press Enter to stop");
Console.ReadLine();

pinger.EndTimer();
var pings = pinger.GetPingList();
if (pings.Count < 1)
	return;

Console.WriteLine($"{address} - minimal {pings.Min(x=> x.Ping)} ms - awerage {pings.Sum(x => x.Ping) / pings.Count} ms - maximum {pings.Max(x => x.Ping)} ms");

Console.WriteLine("Do you want to save values?");

while (input != "y" && input != "n")
{
	Console.WriteLine("y or n");
	input = Console.ReadLine();
}
if (input == "n")
	return;

Console.WriteLine("Select an input method from the following:");
int enumLength = Enum.GetValues(typeof(OutputTypeEnum)).Length;

for (int i = 0; i < enumLength; i++)
	Console.WriteLine($"{i}: {Enum.GetNames(typeof(OutputTypeEnum))[i]}");

OutputTypeEnum inputType;
while (!Enum.TryParse(input, true, out inputType) || !((int)inputType <= enumLength) || !((int)inputType >= 0))
{
	input = Console.ReadLine();
}

var fileName = $"{address} {pings.First().Time} - {pings.Last().Time}";
switch (inputType)
{
	case OutputTypeEnum.None:
		return;
	case OutputTypeEnum.Excel:
		ExcelOutput excel = new ExcelOutput(new FileInfo($"{GetCorrectFileName(fileName, ',', '?', '*', '\\', '/')}.xlsx"));
		excel.SaveExcelFileAsync(pings);
		break;
	case OutputTypeEnum.Txt:
		TxtOutput txt = new TxtOutput(GetCorrectFileName(fileName, ',', '?', '*', '\\', '/') + ".txt");
		txt.WriteInFileAsync(pings);
		break;
	default:
		break;
}

string GetCorrectFileName(string fileName, params char[] chars)
{
	fileName = fileName.Replace(':', '-');
    foreach (var item in chars)
		fileName = fileName.Replace(item.ToString(), string.Empty);

	return fileName;
}
