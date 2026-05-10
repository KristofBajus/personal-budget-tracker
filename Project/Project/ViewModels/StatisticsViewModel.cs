using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Project.Models.Services;
using Project.Models.Services.Interfaces;
using SkiaSharp;

namespace Project.ViewModels;

public partial class StatisticsViewModel : BaseViewModel
{
    private readonly IStatisticsService _statisticsService;
    private readonly SessionService _session;

    public StatisticsViewModel(IStatisticsService statisticsService, SessionService session)
    {
        _statisticsService = statisticsService;
        _session = session;
    }

    public List<string> TimeRangeOptions { get; } =
    [
        "Last 30 days",
        "Last 3 months",
        "Last 6 months",
        "This year",
        "All time"
    ];

    [ObservableProperty]
    public partial string SelectedTimeRange { get; set; } = "Last 3 months";

    partial void OnSelectedTimeRangeChanged(string value) => _ = LoadAsync();

    // Summary card
    [ObservableProperty]
    public partial decimal TotalIncome { get; set; }

    [ObservableProperty]
    public partial decimal TotalExpenses { get; set; }

    [ObservableProperty]
    public partial decimal NetAmount { get; set; }

    [ObservableProperty]
    public partial bool HasNoData { get; set; }

    // Bar chart
    [ObservableProperty]
    public partial ISeries[] BarSeries { get; set; } = [];

    [ObservableProperty]
    public partial Axis[] BarXAxes { get; set; } = [new Axis()];

    // Pie chart
    [ObservableProperty]
    public partial ISeries[] PieSeries { get; set; } = [];

    [ObservableProperty]
    public partial bool HasPieData { get; set; }

    // Line chart
    [ObservableProperty]
    public partial ISeries[] LineSeries { get; set; } = [];

    [ObservableProperty]
    public partial Axis[] LineXAxes { get; set; } = [new DateTimeAxis(TimeSpan.FromDays(1), d => d.ToString("MMM dd"))];

    [ObservableProperty]
    public partial bool HasLineData { get; set; }

    private DateTime GetFromDate() => SelectedTimeRange switch
    {
        "Last 30 days"  => DateTime.Today.AddDays(-30),
        "Last 3 months" => DateTime.Today.AddMonths(-3),
        "Last 6 months" => DateTime.Today.AddMonths(-6),
        "This year"     => new DateTime(DateTime.Today.Year, 1, 1),
        _               => DateTime.MinValue
    };

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        var userId = _session.CurrentUser!.Id;
        var from   = GetFromDate();

        var monthlyTask  = _statisticsService.GetMonthlyTotalsAsync(userId, from);
        var categoryTask = _statisticsService.GetCategoryBreakdownAsync(userId, from);
        var balanceTask  = _statisticsService.GetBalanceHistoryAsync(userId, from);

        await Task.WhenAll(monthlyTask, categoryTask, balanceTask);

        var monthly = monthlyTask.Result;
        var cats    = categoryTask.Result;
        var balance = balanceTask.Result;

        // Summary
        TotalIncome   = monthly.Sum(m => m.Income);
        TotalExpenses = monthly.Sum(m => m.Expense);
        NetAmount     = TotalIncome - TotalExpenses;
        HasNoData     = monthly.Count == 0 && cats.Count == 0 && balance.Count == 0;

        // Bar chart
        var monthLabels = monthly
            .Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m.Month))
            .ToArray();

        BarSeries =
        [
            new ColumnSeries<double>
            {
                Name   = "Income",
                Values = monthly.Select(m => (double)m.Income).ToArray(),
                Fill   = new SolidColorPaint(SKColor.Parse("#22C55E"))
            },
            new ColumnSeries<double>
            {
                Name   = "Expense",
                Values = monthly.Select(m => (double)m.Expense).ToArray(),
                Fill   = new SolidColorPaint(SKColor.Parse("#EF4444"))
            }
        ];
        BarXAxes = [new Axis { Labels = monthLabels }];

        // Pie chart
        HasPieData = cats.Count > 0;
        PieSeries = cats
            .Select(c => (ISeries)new PieSeries<double>
            {
                Name   = c.Name,
                Values = [(double)c.Total],
                Fill   = new SolidColorPaint(SKColor.Parse(c.Color))
            })
            .ToArray();

        // Line chart
        HasLineData = balance.Count > 0;
        LineSeries =
        [
            new LineSeries<DateTimePoint>
            {
                Name         = "Balance",
                Values       = balance.Select(b => new DateTimePoint(b.Date, (double)b.Balance)).ToArray(),
                Fill         = null,
                GeometrySize = 4,
                Stroke       = new SolidColorPaint(SKColor.Parse("#3B82F6")) { StrokeThickness = 2 }
            }
        ];
        LineXAxes = [new DateTimeAxis(TimeSpan.FromDays(1), d => d.ToString("MMM dd"))];

        IsBusy = false;
    }
}
