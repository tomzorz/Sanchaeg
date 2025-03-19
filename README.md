# Sanchaeg

A modern .NET implementation of high-accuracy geography functions using [Vincenty's formulae](https://en.wikipedia.org/wiki/Vincenty%27s_formulae), including

- calculating distance between two points,
- calculating intersection between coordinate - bearing pairs,
- calculating resulting coordinate for an origin, bearing and distance,
- conversion between lat/lon and x/y coordinates using various Datums.

The also library includes various useful epsilon values for geography related calculations, and built-in values for `WGS84`, `EPSG3857`, `GRS80`, `Airy1830`, `AiryModified`, `Intl1924` and `Bessel1841` [datums](https://en.wikipedia.org/wiki/Geodetic_datum).

![](https://img.shields.io/badge/platform-any-green.svg?longCache=true&style=flat-square) ![](https://img.shields.io/badge/nuget-yes-green.svg?longCache=true&style=flat-square) ![](https://img.shields.io/badge/license-MIT-blue.svg?longCache=true&style=flat-square)

## Usage

All the complexity is hidden away behind convenient extension methods:

```csharp
using Sanchaeg;

// ...

var point1 = new SanchaegGeoCoordinate
{
    Latitude = 47.438437
    Longitude = 19.252274
};

var point2 = new SanchaegGeoCoordinate
{
    Latitude = 47.6445436,
    Longitude = -122.1370065
};

var distance = point1.CalculateDistanceBetween(point2);
Console.WriteLine($"Distance: {distance} meters");

```

The "usual" `*Coordinate` types are specifically prefixed `Sanchaeg` in this library to make differentiation easy between whatever else `*Coordinate` types you might have on your end.

## Thanks

This library contains many of its calculations transcoded from Chris Veness's [excellent post](https://www.movable-type.co.uk/scripts/latlong.html) & [library](https://github.com/chrisveness/geodesy), Chris Veness © 2002-$currentYear published under the [MIT License](https://opensource.org/licenses/MIT).