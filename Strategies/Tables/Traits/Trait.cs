﻿using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

public class Trait : IRow
{
    public int Priority { get; set; }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}
