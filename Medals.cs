static partial class Program
{
    static Medal[] Medals = new Medal[]{new Medal()};
    struct Medal
    {
        public Medal(string name, int minValue, int counterNumber)
        {
            Name = name;
            MinValue = minValue;
            CounterNumber = counterNumber;
        }

        public string Name { get; }
        public int MinValue { get; }
        public int CounterNumber { get; }
    }
}