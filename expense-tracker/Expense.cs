using System;

namespace expense_tracker;

class Expense
{
    public int id { get; set; }
    public DateOnly date { get; set; }
    public string description { get; set; } = string.Empty;
    public decimal amount { get; set; }
}