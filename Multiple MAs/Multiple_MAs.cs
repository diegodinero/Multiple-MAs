// Copyright QUANTOWER LLC. © 2017-2023. All rights reserved.

using System.Drawing;
using TradingPlatform.BusinessLayer;
using TradingPlatform.BusinessLayer.Utils;

namespace Multiple_MAs
{
    /// <summary>
    /// Displays up to 4 fully configurable Moving Averages on the same chart panel.
    /// Each MA delegates calculation to Quantower's built-in MA indicators so the
    /// values exactly match what the platform's own SMA/EMA/SMMA/LWMA indicators produce.
    /// Each MA supports independent period, type, source price, and show/hide toggle.
    /// Line colors are configurable via the Quantower UI.
    /// </summary>
    public class Multiple_MAs : Indicator
    {
        #region MA type constants
        private const int TYPE_SMA  = 0;
        private const int TYPE_EMA  = 1;
        private const int TYPE_SMMA = 2;
        private const int TYPE_LWMA = 3;
        #endregion

        #region Input parameters – MA 1

        [InputParameter("MA 1 – Show", 0)]
        public bool Ma1Show = true;

        [InputParameter("MA 1 – Period", 1, 1, 999, 1, 0)]
        public int Ma1Period = 10;

        [InputParameter("MA 1 – Type", 2, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma1Type = TYPE_SMA;

        [InputParameter("MA 1 – Source", 3, variants: new object[]
        {
            "Close",    PriceType.Close,
            "Open",     PriceType.Open,
            "High",     PriceType.High,
            "Low",      PriceType.Low,
            "Typical",  PriceType.Typical,
            "Median",   PriceType.Median,
            "Weighted", PriceType.Weighted
        })]
        public PriceType Ma1Source = PriceType.Close;

        private LineOptions _ma1Line = new LineOptions() { Color = Color.DodgerBlue, Width = 2, LineStyle = LineStyle.Solid };
        [InputParameter("MA 1 – Line", 4)]
        public LineOptions Ma1Line
        {
            get => _ma1Line;
            set { _ma1Line = value; if (LinesSeries.Count > 0) ApplyLineOptions(LinesSeries[0], value); }
        }

        #endregion

        #region Input parameters – MA 2

        [InputParameter("MA 2 – Show", 5)]
        public bool Ma2Show = true;

        [InputParameter("MA 2 – Period", 6, 1, 999, 1, 0)]
        public int Ma2Period = 20;

        [InputParameter("MA 2 – Type", 7, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma2Type = TYPE_EMA;

        [InputParameter("MA 2 – Source", 8, variants: new object[]
        {
            "Close",    PriceType.Close,
            "Open",     PriceType.Open,
            "High",     PriceType.High,
            "Low",      PriceType.Low,
            "Typical",  PriceType.Typical,
            "Median",   PriceType.Median,
            "Weighted", PriceType.Weighted
        })]
        public PriceType Ma2Source = PriceType.Close;

        private LineOptions _ma2Line = new LineOptions() { Color = Color.Orange, Width = 2, LineStyle = LineStyle.Solid };
        [InputParameter("MA 2 – Line", 9)]
        public LineOptions Ma2Line
        {
            get => _ma2Line;
            set { _ma2Line = value; if (LinesSeries.Count > 1) ApplyLineOptions(LinesSeries[1], value); }
        }

        #endregion

        #region Input parameters – MA 3

        [InputParameter("MA 3 – Show", 10)]
        public bool Ma3Show = true;

        [InputParameter("MA 3 – Period", 11, 1, 999, 1, 0)]
        public int Ma3Period = 50;

        [InputParameter("MA 3 – Type", 12, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma3Type = TYPE_EMA;

        [InputParameter("MA 3 – Source", 13, variants: new object[]
        {
            "Close",    PriceType.Close,
            "Open",     PriceType.Open,
            "High",     PriceType.High,
            "Low",      PriceType.Low,
            "Typical",  PriceType.Typical,
            "Median",   PriceType.Median,
            "Weighted", PriceType.Weighted
        })]
        public PriceType Ma3Source = PriceType.Close;

        private LineOptions _ma3Line = new LineOptions() { Color = Color.LimeGreen, Width = 2, LineStyle = LineStyle.Solid };
        [InputParameter("MA 3 – Line", 14)]
        public LineOptions Ma3Line
        {
            get => _ma3Line;
            set { _ma3Line = value; if (LinesSeries.Count > 2) ApplyLineOptions(LinesSeries[2], value); }
        }

        #endregion

        #region Input parameters – MA 4

        [InputParameter("MA 4 – Show", 15)]
        public bool Ma4Show = true;

        [InputParameter("MA 4 – Period", 16, 1, 999, 1, 0)]
        public int Ma4Period = 200;

        [InputParameter("MA 4 – Type", 17, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma4Type = TYPE_SMA;

        [InputParameter("MA 4 – Source", 18, variants: new object[]
        {
            "Close",    PriceType.Close,
            "Open",     PriceType.Open,
            "High",     PriceType.High,
            "Low",      PriceType.Low,
            "Typical",  PriceType.Typical,
            "Median",   PriceType.Median,
            "Weighted", PriceType.Weighted
        })]
        public PriceType Ma4Source = PriceType.Close;

        private LineOptions _ma4Line = new LineOptions() { Color = Color.Red, Width = 2, LineStyle = LineStyle.Solid };
        [InputParameter("MA 4 – Line", 19)]
        public LineOptions Ma4Line
        {
            get => _ma4Line;
            set { _ma4Line = value; if (LinesSeries.Count > 3) ApplyLineOptions(LinesSeries[3], value); }
        }

        #endregion

        // Inner built-in MA indicator instances – one per line.
        private Indicator ma1, ma2, ma3, ma4;

        /// <summary>
        /// Indicator's constructor. Contains general information: name, description, LineSeries etc.
        /// </summary>
        public Multiple_MAs()
            : base()
        {
            Name        = "Multiple MAs";
            Description = "Displays up to 4 configurable Moving Averages (SMA, EMA, SMMA, LWMA).";

            // Four lines with distinct default colors; users can override colors in the Quantower UI.
            AddLineSeries("MA 1", Color.DodgerBlue, 2, LineStyle.Solid);
            AddLineSeries("MA 2", Color.Orange,     2, LineStyle.Solid);
            AddLineSeries("MA 3", Color.LimeGreen,  2, LineStyle.Solid);
            AddLineSeries("MA 4", Color.Red,        2, LineStyle.Solid);

            SeparateWindow = false;
        }

        /// <summary>
        /// Called after creating the indicator and after any input-parameter change.
        /// Recreates the four built-in MA sub-indicators with the current settings.
        /// </summary>
        protected override void OnInit()
        {
            ShortName = "Multiple MAs";

            // Dispose previous sub-indicators (handles parameter-change re-init).
            DisposeSubIndicators();

            // Create and attach a Quantower built-in MA for each line.
            ma1 = CreateBuiltInMA(Ma1Type, Ma1Period, Ma1Source);
            ma2 = CreateBuiltInMA(Ma2Type, Ma2Period, Ma2Source);
            ma3 = CreateBuiltInMA(Ma3Type, Ma3Period, Ma3Source);
            ma4 = CreateBuiltInMA(Ma4Type, Ma4Period, Ma4Source);

            AddIndicator(ma1);
            AddIndicator(ma2);
            AddIndicator(ma3);
            AddIndicator(ma4);

            // Apply line appearance from input parameters.
            ApplyLineOptions(LinesSeries[0], Ma1Line);
            ApplyLineOptions(LinesSeries[1], Ma2Line);
            ApplyLineOptions(LinesSeries[2], Ma3Line);
            ApplyLineOptions(LinesSeries[3], Ma4Line);
        }

        /// <summary>
        /// Calculation entry point – reads values from the built-in sub-indicators.
        /// </summary>
        protected override void OnUpdate(UpdateArgs args)
        {
            SetLineValue(Ma1Show, ma1, lineIndex: 0);
            SetLineValue(Ma2Show, ma2, lineIndex: 1);
            SetLineValue(Ma3Show, ma3, lineIndex: 2);
            SetLineValue(Ma4Show, ma4, lineIndex: 3);
        }

        /// <summary>
        /// Called when the indicator is removed or the chart is reset.
        /// </summary>
        protected override void OnClear()
        {
            DisposeSubIndicators();
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        /// <summary>Applies a LineOptions value to a LineSeries.</summary>
        private static void ApplyLineOptions(LineSeries line, LineOptions opts)
        {
            line.Color = opts.Color;
            line.Width = opts.Width;
            line.Style = opts.LineStyle;
        }

        /// <summary>Returns a Quantower built-in MA indicator for the requested type.</summary>
        private Indicator CreateBuiltInMA(int type, int period, PriceType source)
        {
            return type switch
            {
                TYPE_EMA  => Core.Instance.Indicators.BuiltIn.EMA(period, source),
                TYPE_SMMA => Core.Instance.Indicators.BuiltIn.SMMA(period, source),
                TYPE_LWMA => Core.Instance.Indicators.BuiltIn.LWMA(period, source),
                _         => Core.Instance.Indicators.BuiltIn.SMA(period, source),
            };
        }

        /// <summary>Reads the current value from a sub-indicator and writes it to the given line.</summary>
        private void SetLineValue(bool show, Indicator ma, int lineIndex)
        {
            if (!show || ma == null)
                return;

            double value = ma.GetValue();
            if (!double.IsNaN(value))
                SetValue(value, lineIndex);
        }

        /// <summary>Disposes all four sub-indicators and nulls the references.</summary>
        private void DisposeSubIndicators()
        {
            ma1?.Dispose();
            ma1 = null;
            ma2?.Dispose();
            ma2 = null;
            ma3?.Dispose();
            ma3 = null;
            ma4?.Dispose();
            ma4 = null;
        }
    }
}
