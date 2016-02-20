# Tababular

A simple .NET monospace text table formatting library.

You can use it if you are standing with a bunch of objects or dictionaries in your hand, and you
wish for them to become as nice as this:

	===============================================
	| FirstColumn  | SecondColumn  | ThirdColumn  |
	===============================================
	| r1           | hej           | hej igen     |
	===============================================
	| r2           | hej           | hej igen     |
	===============================================

This can easily be achieved by newing up the `TableFormatter` like this:

	var formatter = new TableFormatter();

and then you call an appropriate `Format***` method on it, e.g. `FormatObjects`:

	var objects = new[]
	{
		new {FirstColumn = "r1", SecondColumn = "hej", ThirdColumn = "hej igen"},
		new {FirstColumn = "r2", SecondColumn = "hej", ThirdColumn = "hej igen"},
	};

	var text = tableFormatter.FormatObjects(objects);

	Console.WriteLine(text);

which will result in the table shown above.

For now, Tababular does not support that much stuff - but one nice thing about it is that
it will properly format lines in cells, so that e.g.

	var objects = new[]
	{
		new  { MachineName = "ctxtest01", Ip = "10.0.0.10", Ports = new[] {80, 8080, 9090}},
		new  { MachineName = "ctxtest02", Ip = "10.0.0.11", Ports = new[] {80, 5432}}
	};

	var text = new TableFormatter().FormatObjects(objects);

	Console.WriteLine(text);

becomes nice like this:

	======================================
	| MachineName  | Ip         | Ports  |
	======================================
	| ctxtest01    | 10.0.0.10  | 80     |
	|              |            | 8080   |
	|              |            | 9090   |
	======================================
	| ctxtest02    | 10.0.0.11  | 80     |
	|              |            | 5432   |
	======================================

which looks pretty neat if you ask me.

# License

[The MIT License (MIT)](http://opensource.org/licenses/MIT)