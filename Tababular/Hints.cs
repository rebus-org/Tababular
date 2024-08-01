using System;

namespace Tababular;

/// <summary>
/// Represents some hints on how to format things
/// </summary>
public class Hints
{
    /// <summary>
    /// Can be used to specify the max width that the table should try to fit within
    /// </summary>
    public int? MaxTableWidth { get; set; }

    /// <summary>
    /// Can be used to specify that the table formatter can make the table vertically more compact
    /// when no cell has more than one line
    /// </summary>
    public bool CollapseVerticallyWhenSingleLine { get; set; }

    /// <summary>
    /// Can be used to specify the separator that the table formatter will use for new lines.
    /// By default has value of <see cref="Environment.NewLine"/>
    /// </summary>
    public string NewLineSeparator { get; set; } = Environment.NewLine;
}