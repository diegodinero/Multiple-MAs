// Copyright QUANTOWER LLC. © 2017-2023. All rights reserved.

using System;
using System.Drawing;
using TradingPlatform.BusinessLayer;

namespace Multiple_MAs
{
    /// <summary>
    /// Displays up to 4 fully configurable Moving Averages on the same chart panel.
    /// Each MA supports independent period, type (SMA/EMA/SMMA/LWMA), source price,
    /// and show/hide toggle. Line colors are configurable via the Quantower UI.
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

        #endregion

        #region Input parameters – MA 2

        [InputParameter("MA 2 – Show", 4)]
        public bool Ma2Show = true;

        [InputParameter("MA 2 – Period", 5, 1, 999, 1, 0)]
        public int Ma2Period = 20;

        [InputParameter("MA 2 – Type", 6, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma2Type = TYPE_EMA;

        [InputParameter("MA 2 – Source", 7, variants: new object[]
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

        #endregion

        #region Input parameters – MA 3

        [InputParameter("MA 3 – Show", 8)]
        public bool Ma3Show = true;

        [InputParameter("MA 3 – Period", 9, 1, 999, 1, 0)]
        public int Ma3Period = 50;

        [InputParameter("MA 3 – Type", 10, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma3Type = TYPE_EMA;

        [InputParameter("MA 3 – Source", 11, variants: new object[]
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

        #endregion

        #region Input parameters – MA 4

        [InputParameter("MA 4 – Show", 12)]
        public bool Ma4Show = true;

        [InputParameter("MA 4 – Period", 13, 1, 999, 1, 0)]
        public int Ma4Period = 200;

        [InputParameter("MA 4 – Type", 14, variants: new object[]
        {
            "SMA",  TYPE_SMA,
            "EMA",  TYPE_EMA,
            "SMMA", TYPE_SMMA,
            "LWMA", TYPE_LWMA
        })]
        public int Ma4Type = TYPE_SMA;

        [InputParameter("MA 4 – Source", 15, variants: new object[]
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

        #endregion

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
        /// </summary>
        protected override void OnInit()
        {
            ShortName = "Multiple MAs";
        }

        /// <summary>
        /// Calculation entry point – called on every price update.
        /// </summary>
        protected override void OnUpdate(UpdateArgs args)
        {
            CalculateAndSet(Ma1Show, Ma1Type, Ma1Period, Ma1Source, lineIndex: 0);
            CalculateAndSet(Ma2Show, Ma2Type, Ma2Period, Ma2Source, lineIndex: 1);
            CalculateAndSet(Ma3Show, Ma3Type, Ma3Period, Ma3Source, lineIndex: 2);
            CalculateAndSet(Ma4Show, Ma4Type, Ma4Period, Ma4Source, lineIndex: 3);
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private void CalculateAndSet(bool show, int type, int period, PriceType source, int lineIndex)
        {
            if (!show || Count < period)
                return;

            double value = type switch
            {
                TYPE_EMA  => CalculateEMA(lineIndex, period, source),
                TYPE_SMMA => CalculateSMMA(lineIndex, period, source),
                TYPE_LWMA => CalculateLWMA(period, source),
                _         => CalculateSMA(period, source),   // TYPE_SMA (default)
            };

            if (!double.IsNaN(value))
                SetValue(value, lineIndex);
        }

        /// <summary>Simple Moving Average – arithmetic mean of the last <paramref name="period"/> bars.</summary>
        private double CalculateSMA(int period, PriceType source)
        {
            double sum = 0.0;
            for (int i = 0; i < period; i++)
                sum += GetPrice(source, i);
            return sum / period;
        }

        /// <summary>Exponential Moving Average – uses smoothing factor k = 2/(period+1).</summary>
        private double CalculateEMA(int lineIndex, int period, PriceType source)
        {
            double k       = 2.0 / (period + 1);
            double prevEMA = double.IsNaN(GetValue(lineIndex, 1))
                ? GetPrice(source)
                : GetValue(lineIndex, 1);
            return prevEMA + k * (GetPrice(source) - prevEMA);
        }

        /// <summary>Smoothed Moving Average – each bar is given equal weighting over time.</summary>
        private double CalculateSMMA(int lineIndex, int period, PriceType source)
        {
            double prevSMMA = GetValue(lineIndex, 1);
            if (double.IsNaN(prevSMMA))
            {
                // Seed with SMA on the first valid bar.
                double sum = 0.0;
                for (int i = 0; i < period; i++)
                    sum += GetPrice(source, i);
                return sum / period;
            }
            return (prevSMMA * (period - 1) + GetPrice(source)) / period;
        }

        /// <summary>Linear Weighted Moving Average – most recent bar receives the highest weight.</summary>
        private double CalculateLWMA(int period, PriceType source)
        {
            double weightedSum = 0.0;
            double weightSum   = 0.0;
            for (int i = 0; i < period; i++)
            {
                double weight = period - i;   // weight: period, period-1, …, 1
                weightedSum += GetPrice(source, i) * weight;
                weightSum   += weight;
            }
            return weightedSum / weightSum;
        }
    }
}
