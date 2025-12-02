namespace Core.Shared.Modules;

public abstract class BaseDayModule : IDayModule
{
    private InputDataProvider _inputDataProvider;

    public BaseDayModule(ITestOutputHelper outputHelper)
    {
        _inputDataProvider = new InputDataProvider();

        // the test class gets instantiated once per test, so we can output the header here to make it display at the start of each unit test
        OutputHeader();
    }

    private void OutputHeader()
    {
        WriteHorizontalRule();
        WriteLine($"Advent of Code {Year} - Day {Day}: {Title}");
        WriteHorizontalRule();
    }

    public int Year => 2025;
    public abstract int Day { get; }
    public abstract string Title { get; }
    
    protected string GetData(InputType inputType) => _inputDataProvider.GetInputData(Year, Day, inputType);
    protected string GetData(string inputType) => _inputDataProvider.GetInputData(Year, Day, inputType);

    /// <summary>
    /// When set to true, calls to Debug() will be written to the output.
    /// </summary>
    public static bool DebugEnabled { get; set; }
    
    internal void WriteLine(string line = "") => Console.WriteLine(line);

    internal void Debug(string line = "")
    {
        if (DebugEnabled)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }
    }

    internal void WriteHorizontalRule(int length = 80) => WriteLine(new string('-', length));
}