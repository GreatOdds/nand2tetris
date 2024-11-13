using System.Collections.Generic;

namespace Hack
{
    public class SymbolTable
    {
        private readonly Dictionary<string, int> table;

        public SymbolTable()
        {
            table = [];
        }

        public void AddEntry(string symbol, int address)
        {
            table.Add(symbol, address);
        }

        public bool Contains(string symbol)
        {
            return table.ContainsKey(symbol);
        }

        public int GetAddress(string symbol)
        {
            return table.GetValueOrDefault(symbol, -1);
        }
    }
}