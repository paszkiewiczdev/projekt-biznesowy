using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessViews;
using MVVMFirma.ViewModels.Abstract;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRaportTopTowaryViewModel : WszystkieViewModel<RaportTopTowaryDto>
    {
        private const double PieRadius = 100;
        private static readonly Point PieCenter = new Point(PieRadius, PieRadius);
        private ObservableCollection<RaportTopTowaryPieSlice> _PieSlices;

        public WszystkieRaportTopTowaryViewModel()
            : base()
        {
            DisplayName = "Raport: Top towary";
            SetSortOptions(new[] { "Id", "Towar", "Ilość" });
            RefreshCommand = new BaseCommand(load);
            ApplyFilterCommand = new BaseCommand(ApplyFilterAndSortWithChartRefresh);
            ClearFilterCommand = new BaseCommand(ClearFilterWithChartRefresh);
            PieSlices = new ObservableCollection<RaportTopTowaryPieSlice>();
        }

        public ICommand RefreshCommand { get; }
        public new ICommand ApplyFilterCommand { get; }
        public new ICommand ClearFilterCommand { get; }

        public ObservableCollection<RaportTopTowaryPieSlice> PieSlices
        {
            get => _PieSlices;
            private set
            {
                _PieSlices = value;
                OnPropertyChanged(() => PieSlices);
            }
        }

        public override void load()
        {
            base.load();
            UpdatePieSlices();
        }

        protected override IEnumerable<RaportTopTowaryDto> LoadData()
        {
            return fakturyEntities.Database.SqlQuery<RaportTopTowaryDto>(
                "SELECT IdTowaru, TowarNazwa, IloscRazem, WartoscNetto, WartoscVat, WartoscBrutto FROM dbo.v_RaportTopTowary"
            ).ToList();
        }

        protected override bool MatchesFilter(RaportTopTowaryDto item, string filterText)
        {
            var text = filterText.ToLowerInvariant();

            return item.IdTowaru.ToString().Contains(text)
                   || (item.TowarNazwa != null && item.TowarNazwa.ToLowerInvariant().Contains(text));
        }

        protected override IOrderedEnumerable<RaportTopTowaryDto> ApplySort(
            IEnumerable<RaportTopTowaryDto> query,
            string sortField,
            bool descending)
        {
            return sortField switch
            {
                "Towar" => descending
                    ? query.OrderByDescending(r => r.TowarNazwa)
                    : query.OrderBy(r => r.TowarNazwa),

                "Ilość" => descending
                    ? query.OrderByDescending(r => r.IloscRazem)
                    : query.OrderBy(r => r.IloscRazem),

                _ => descending
                    ? query.OrderByDescending(r => r.IdTowaru)
                    : query.OrderBy(r => r.IdTowaru)
            };
        }

        private void ApplyFilterAndSortWithChartRefresh()
        {
            ApplyFilterAndSort();
            UpdatePieSlices();
        }

        private void ClearFilterWithChartRefresh()
        {
            ClearFilter();
            UpdatePieSlices();
        }

        private void UpdatePieSlices()
        {
            if (List == null || List.Count == 0)
            {
                PieSlices = new ObservableCollection<RaportTopTowaryPieSlice>();
                return;
            }

            var orderedItems = List
                .OrderByDescending(item => item.IloscRazem)
                .ToList();

            var total = orderedItems.Sum(item => item.IloscRazem);
            if (total <= 0)
            {
                PieSlices = new ObservableCollection<RaportTopTowaryPieSlice>();
                return;
            }

            var totalDouble = (double)total;
            var palette = new[]
            {
                Color.FromRgb(245, 124, 0),
                Color.FromRgb(30, 136, 229),
                Color.FromRgb(67, 160, 71),
                Color.FromRgb(142, 36, 170),
                Color.FromRgb(255, 112, 67),
                Color.FromRgb(0, 137, 123),
                Color.FromRgb(94, 53, 177),
                Color.FromRgb(251, 192, 45)
            };

            var slices = new List<RaportTopTowaryPieSlice>();
            double currentAngle = -90;
            var colorIndex = 0;

            foreach (var item in orderedItems)
            {
                var value = (double)item.IloscRazem;
                if (value <= 0)
                    continue;

                var angle = value / totalDouble * 360.0;
                var brush = new SolidColorBrush(palette[colorIndex % palette.Length]);
                brush.Freeze();

                var geometry = CreatePieSliceGeometry(currentAngle, angle);

                slices.Add(new RaportTopTowaryPieSlice
                {
                    Name = string.IsNullOrWhiteSpace(item.TowarNazwa) ? "Towar" : item.TowarNazwa,
                    Value = item.IloscRazem,
                    Percentage = value / totalDouble,
                    Geometry = geometry,
                    Fill = brush
                });

                currentAngle += angle;
                colorIndex++;
            }

            PieSlices = new ObservableCollection<RaportTopTowaryPieSlice>(slices);
        }

        private static Geometry CreatePieSliceGeometry(double startAngle, double sweepAngle)
        {
            if (sweepAngle >= 360)
                return new EllipseGeometry(PieCenter, PieRadius, PieRadius);

            var startRadians = DegreesToRadians(startAngle);
            var endRadians = DegreesToRadians(startAngle + sweepAngle);

            var startPoint = new Point(
                PieCenter.X + PieRadius * Math.Cos(startRadians),
                PieCenter.Y + PieRadius * Math.Sin(startRadians));

            var endPoint = new Point(
                PieCenter.X + PieRadius * Math.Cos(endRadians),
                PieCenter.Y + PieRadius * Math.Sin(endRadians));

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(PieCenter, true, true);
                context.LineTo(startPoint, true, true);
                context.ArcTo(
                    endPoint,
                    new Size(PieRadius, PieRadius),
                    0,
                    sweepAngle > 180,
                    SweepDirection.Clockwise,
                    true,
                    true);
            }

            geometry.Freeze();
            return geometry;
        }

        private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}
