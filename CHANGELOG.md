# Changelog

## 1.0.0

* Made Tababular

## 1.0.1

* More robustrustness

## 1.0.2

* Added ability to supply hints - at the moment this is the ability to specify a max width for the table

## 1.0.3

* Changed total table width constraint algorithm to cut off one fourth of the widest column's width on each iteration

## 1.0.4

* Added ability to parse [JSONL](http://jsonlines.org/) too

## 1.0.5

* Added ability (via a hint) to specify that the formatter can collapse the table vertically if it has no cell with multiple lines

## 1.0.6

* Updated JSON.NET (which gets merged) to 8.0.3

## 2.0.0

* Add .NET Core support
* No longer merge JSON.NET
* Skip property enumeration for primitive-like types - thanks [gary-palmer]

[gary-palmer]: https://github.com/gary-palmer