using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using NExpect;
using NExpect.Interfaces;
using NExpect.MatcherLogic;
using PeanutButter.Utils;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace CorsairBatteryTrayIcon.Tests;

[TestFixture]
public class TestAppIcons
{
    [Test]
    public void ShouldLoadTheDefaultIcon()
    {
        // Arrange
        var expected = LoadIcon("default");
        // Act
        var sut = Create();
        // Assert
        Expect(sut.Default)
            .Not.To.Be.Null();
        Expect(sut.Default)
            .To.Match(expected);
    }

    [Test]
    public void ShouldLoadTheDefaultDefaultIcon()
    {
        // Arrange
        using var tmpFolder = new AutoTempFolder();
        var expected = LoadIcon("default");
        // Act
        var sut = Create(tmpFolder.Path);
        // Assert
        Expect(sut.Default)
            .Not.To.Be.Null();
        Expect(sut.Default)
            .To.Match(expected);
    }

    [Test]
    public void ShouldLoadTheChargingIcon()
    {
        // Arrange
        var expected = LoadIcon("charging");
        // Act
        var sut = Create();
        // Assert
        Expect(sut.Charging)
            .Not.To.Be.Null();
        Expect(sut.Charging)
            .To.Match(expected);
    }

    [Test]
    public void ShouldLoadTheDefaultChargingIcon()
    {
        // Arrange
        using var tmpFolder = new AutoTempFolder();
        var expected = LoadIcon("charging");
        // Act
        var sut = Create(tmpFolder.Path);
        // Assert
        Expect(sut.Charging)
            .Not.To.Be.Null();
        Expect(sut.Charging)
            .To.Match(expected);
    }

    [Test]
    public void ShouldLoadTheUnknownIcon()
    {
        // Arrange
        var expected = LoadIcon("unknown");
        // Act
        var sut = Create();
        // Assert
        Expect(sut.Unknown)
            .Not.To.Be.Null();
        Expect(sut.Unknown)
            .To.Match(expected);
    }

    [Test]
    public void ShouldLoadTheDefaultUnknownIcon()
    {
        // Arrange
        using var tmpFolder = new AutoTempFolder();
        var expected = LoadIcon("Unknown");
        // Act
        var sut = Create(tmpFolder.Path);
        // Assert
        Expect(sut.Unknown)
            .Not.To.Be.Null();
        Expect(sut.Unknown)
            .To.Match(expected);
    }

    [TestCase(
        0,
        "0"
    )]
    [TestCase(
        4,
        "0"
    )]
    [TestCase(
        10,
        "10"
    )]
    [TestCase(
        12,
        "10"
    )]
    [TestCase(
        25,
        "25"
    )]
    [TestCase(
        45,
        "50"
    )]
    [TestCase(
        50,
        "50"
    )]
    [TestCase(
        60,
        "50"
    )]
    [TestCase(
        75,
        "75"
    )]
    [TestCase(
        78,
        "75"
    )]
    [TestCase(
        90,
        "90"
    )]
    [TestCase(
        94,
        "90"
    )]
    [TestCase(
        95,
        "100"
    )]
    [TestCase(
        96,
        "100"
    )]
    [TestCase(
        100,
        "100"
    )]
    public void ShouldProvideClosest(
        int batteryPercent,
        string icon
    )
    {
        // Arrange
        var expected = LoadIcon(icon);
        var sut = Create();
        // Act
        var result = sut.BatteryIconForPercent(batteryPercent);
        // Assert
        Expect(result)
            .Not.To.Be.Null();
        Expect(result)
            .To.Match(expected);
    }

    [TestCase(
        0,
        "0"
    )]
    [TestCase(
        4,
        "0"
    )]
    [TestCase(
        10,
        "10"
    )]
    [TestCase(
        12,
        "10"
    )]
    [TestCase(
        25,
        "25"
    )]
    [TestCase(
        45,
        "50"
    )]
    [TestCase(
        50,
        "50"
    )]
    [TestCase(
        60,
        "50"
    )]
    [TestCase(
        75,
        "75"
    )]
    [TestCase(
        78,
        "75"
    )]
    [TestCase(
        90,
        "90"
    )]
    [TestCase(
        94,
        "90"
    )]
    [TestCase(
        95,
        "100"
    )]
    [TestCase(
        96,
        "100"
    )]
    [TestCase(
        100,
        "100"
    )]
    public void ShouldProvideClosestDefaultBatteryIcon(
        int batteryPercent,
        string icon
    )
    {
        // Arrange
        using var tmpFolder = new AutoTempFolder();
        var expected = LoadIcon(icon);
        var sut = Create(tmpFolder.Path);
        // Act
        var result = sut.BatteryIconForPercent(batteryPercent);
        // Assert
        Expect(result)
            .Not.To.Be.Null();
        Expect(result)
            .To.Match(expected);
    }

    [TestCase(
        -1,
        "0"
    )]
    [TestCase(
        101,
        "100"
    )]
    public void ShouldProvideEdgeCaseIcons(
        int batteryPercent,
        string icon
    )
    {
        // Arrange
        var expected = LoadIcon(icon);
        var sut = Create();
        // Act
        var result = sut.BatteryIconForPercent(batteryPercent);
        // Assert
        Expect(result)
            .Not.To.Be.Null();
        Expect(result)
            .To.Match(expected);
    }

    [Test]
    public void ShouldProvideDefaultIconWhenNoBatteryIcons()
    {
        // Arrange
        using var tmpDir = new AutoTempFolder();
        var sut = Create(tmpDir.Path);
        // Act
        var result = sut.BatteryIconForPercent(
            GetRandomInt(
                0,
                100
            )
        );
        // Assert
    }

    private static Icon LoadIcon(
        string name
    )
    {
        var iconPath = Path.Combine(
            IconDir,
            $"{name}.ico"
        );
        using var fs = File.Open(
            iconPath,
            FileMode.Open
        );
        return new Icon(fs);
    }

    private static AppIcons Create(
        string iconFolder = null
    )
    {
        return new(iconFolder ?? IconDir);
    }

    private static string IconDir => Path.Combine(
        AppDir,
        "icons"
    );

    private static string AppDir => Path.GetDirectoryName(
        new Uri(
            typeof(AppIcons).Assembly.Location
        ).LocalPath
    );
}

public static class IconMatchers
{
    public static IMore<Icon> Match(
        this ITo<Icon> to,
        Icon expected
    )
    {
        return to.AddMatcher(
            actual =>
            {
                var dimensionsMatch =
                    actual.Height == expected.Height &&
                    actual.Width == expected.Width;
                var pixelsMatch = true;
                if (dimensionsMatch)
                {
                    using var actualBmp = actual.ToBitmap();
                    using var expectedBmp = expected.ToBitmap();
                    for (var x = 0; x < actualBmp.Width; x++)
                    {
                        for (var y = 0; y < actualBmp.Height; y++)
                        {
                            if (actualBmp.GetPixel(
                                    x,
                                    y
                                ) != expectedBmp.GetPixel(
                                    x,
                                    y
                                ))
                            {
                                pixelsMatch = false;
                                break;
                            }
                        }

                        if (!pixelsMatch)
                        {
                            break;
                        }
                    }
                }

                var passed = dimensionsMatch && pixelsMatch;
                return new MatcherResult(
                    passed,
                    () => dimensionsMatch
                        ? "images are same size but don't match"
                        : "images aren't even the same size"
                );
            }
        );
    }
}