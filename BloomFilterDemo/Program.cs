using System.Collections;
using Microsoft.VisualBasic;
using Helper;

Console.WriteLine("Hello, World!");

IMurmurHash hash = new MurmurHash<string>(10000);
var bloom = new BloomFilter(10000, hash);

int testCount = 1000;
var added = new List<string>();
for (int i = 0; i < testCount; i++)
{
    var s = $"test_{i}";
    bloom.Add(s);
    added.Add(s);
}

int missing = 0;
foreach (var s in added)
{
    if (!bloom.MightContain(s))
        missing++;
}
Console.WriteLine($"Not found among added: {missing} / {testCount}");

int falsePositive = 0;
for (int i = 0; i < testCount; i++)
{
    var s = $"never_{i}";
    if (bloom.MightContain(s))
        falsePositive++;
}
Console.WriteLine($"False positive rate: {falsePositive} / {testCount} ({(falsePositive * 100.0 / testCount):F2}%)");
