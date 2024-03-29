﻿using System;
using NUnit.Framework;

namespace Tababular.Tests.Examples;

[TestFixture]
public class ReadmeCode
{
    [Test]
    public void Example1()
    {
        var tableFormatter = new TableFormatter();

        var objects = new[]
        {
            new {FirstColumn = "r1", SecondColumn = "hej", ThirdColumn = "hej igen"},
            new {FirstColumn = "r2", SecondColumn = "hej", ThirdColumn = "hej igen"},
        };

        var text = tableFormatter.FormatObjects(objects);

        Console.WriteLine(text);
    }

    [Test]
    public void Example2()
    {
        var objects = new[]
        {
            new {MachineName = "ctxtest01", Ip = "10.0.0.10", Ports = new[] {80, 8080, 9090}},
            new {MachineName = "ctxtest02", Ip = "10.0.0.11", Ports = new[] {80, 5432}}
        };

        var text = new TableFormatter().FormatObjects(objects);

        Console.WriteLine(text);
    }

    [Test]
    public void Example3_WithoutHint()
    {
        var objects = new[]
        {
            new {MachineName = "ctxtest01", Ip = "10.0.0.10", Ports = new[] {80, 8080, 9090}, Comments = ""},
            new {MachineName = "ctxtest02", Ip = "10.0.0.11", Ports = new[] {5432},
                Comments = @"This bad boy hosts our database and a couple of internal jobs."}
        };

        var formatter = new TableFormatter();

        var text = formatter.FormatObjects(objects);

        Console.WriteLine(text);

        /*

+----------------------------------------------------------------+-----------+-------------+-------+
| Comments                                                       | Ip        | MachineName | Ports |
+----------------------------------------------------------------+-----------+-------------+-------+
|                                                                | 10.0.0.10 | ctxtest01   | 80    |
|                                                                |           |             | 8080  |
|                                                                |           |             | 9090  |
+----------------------------------------------------------------+-----------+-------------+-------+
| This bad boy hosts our database and a couple of internal jobs. | 10.0.0.11 | ctxtest02   | 5432  |
+----------------------------------------------------------------+-----------+-------------+-------+

        */
    }

    [Test]
    public void Example3_WithHint()
    {
        var objects = new[]
        {
            new {MachineName = "ctxtest01", Ip = "10.0.0.10", Ports = new[] {80, 8080, 9090}, Comments = ""},
            new {MachineName = "ctxtest02", Ip = "10.0.0.11", Ports = new[] {5432},
                Comments = @"This bad boy hosts our database and a couple of internal jobs."}
        };

        var hints = new Hints { MaxTableWidth = 80 };
        var formatter = new TableFormatter(hints);

        var text = formatter.FormatObjects(objects);

        Console.WriteLine(text);

        /*

+------------------------------------------------+-----------+-------------+-------+
| Comments                                       | Ip        | MachineName | Ports |
+------------------------------------------------+-----------+-------------+-------+
|                                                | 10.0.0.10 | ctxtest01   | 80    |
|                                                |           |             | 8080  |
|                                                |           |             | 9090  |
+------------------------------------------------+-----------+-------------+-------+
| This bad boy hosts our database and a couple   | 10.0.0.11 | ctxtest02   | 5432  |
| of internal jobs.                              |           |             |       |
+------------------------------------------------+-----------+-------------+-------+
       
        */
    }
}