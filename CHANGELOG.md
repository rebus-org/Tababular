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

## 3.0.0
* Change style to contain more space visually
* Use FastMember for ultra-fast reflection
* Add .NET Standard 2.0 as a target

## 3.0.1
* When formatting objects, columns are now returned in the order the corresponding properties are defined

## 4.0.0
* Better exception when value extraction fails on object

## 4.1.0
* Update some packages, modernize code base

## 4.2.0
* Make newline character sequence configurable - thanks [antifree]

[antifree]: https://github.com/antifree
[gary-palmer]: https://github.com/gary-palmer
