// --------------------------------------------------------------------------------------------------------------------
// <copyright file="performanceBase.cs" company="imbVeles" >
//
// Copyright (C) 2017 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbACE.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Core.data.measurement
{
    using imbACE.Core.extensions.io;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.extensions.data.modify;
    using imbSCI.DataComplex.extensions.data.schema;
    using imbSCI.DataComplex.tables;
    using imbSCI.DataComplex.trends;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// performance measurement base class
    /// </summary>
    public abstract class performanceBase<T> : IPerformanceTaker where T : class, IPerformanceTake, new()
    {
        public abstract Int32 secondsBetweenTakesDefault { get; }

        public abstract void prepare();

        public abstract void measure(T t);

        protected performanceBase()
        {
            takes = new objectTable<T>(nameof(IPerformanceTake.idk), typeof(T).Name.getCleanPropertyName());
            prepare();
        }

        protected performanceBase(String __name)
        {
            name = __name;

            takes = new objectTable<T>(nameof(IPerformanceTake.idk), name.getCleanPropertyName());

            prepare();
        }

        private String _name;

        /// <summary>
        ///
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        public T GetLastTake()
        {
            return lastTake;
        }

        private Int32 _secondsBetweenTakes = -1;

        /// <summary>
        ///
        /// </summary>
        public Int32 secondsBetweenTakes
        {
            get
            {
                if (_secondsBetweenTakes == -1) _secondsBetweenTakes = secondsBetweenTakesDefault; //

                return _secondsBetweenTakes;
            }
            set { _secondsBetweenTakes = value; }
        }

        /// <summary>
        /// Makes take() if interval passed
        /// </summary>
        public void checkTake()
        {
            if (lastTake != null)
            {
                if (DateTime.Now.Subtract(lastTake.samplingTime).TotalSeconds >= secondsBetweenTakes)
                {
                    take();
                }
            }
            else
            {
                take();
            }
        }

        protected Object takeLock = new Object();

        public List<T> GetLastSamples(Int32 count)
        {
            List<T> output = takes.GetLastNEntries(count);
            return output;
        }

        /// <summary>
        /// Gets the trend.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="trendTaker">The trend taker.</param>
        /// <returns></returns>
        public measureTrend GetTrend(Func<T, double> selector, measureTrendTaker trendTaker)
        {
            var sampleset = GetLastSamples(trendTaker.MacroSampleSize);

            //sampleset.Min(x=>x.samplingTime)

            var sValues = (from num in sampleset select selector(num));

            measureTrend trend = new measureTrend(sValues, trendTaker, sampleset.GetTimeSpan());

            return trend;
        }

        /// <summary>
        /// Gets the trend.
        /// </summary>
        /// <param name="typedTaker">The typed taker.</param>
        /// <returns></returns>
        public measureTrend GetTrend(measureTrendTaker<T> typedTaker)
        {
            var sampleset = GetLastSamples(typedTaker.MacroSampleSize);

            var sValues = (from num in sampleset select typedTaker.selector(num));

            measureTrend trend = new measureTrend(sValues, typedTaker, sampleset.GetTimeSpan());

            return trend;
        }

        internal T takeCreate()
        {
            DateTime lastCall = DateTime.Now;
            Int32 __id = takes.Count();
            if (lastTake != null)
            {
                lastCall = lastTake.samplingTime;
                __id = lastTake.id + 1;
            }

            var t = takes.GetOrCreate(__id.ToString()); //new T();
            t.id = __id;

            t.secondsSinceLastTake = DateTime.Now.Subtract(lastCall).TotalSeconds;

            t.PerMinuteFactor = ((Double)60 / t.secondsSinceLastTake);
            t.samplingTime = DateTime.Now;

            return t;
        }

        internal void takeAdd(T t)
        {
            takes.AddOrUpdate(t);
        }

        internal virtual List<T> takeList
        {
            get
            {
                return takes.GetList();
            }
        }

        /// <summary>
        /// Procedure that creates new take. Thread safe
        /// </summary>
        public void take()
        {
            lock (takeLock)
            {
                var t = takeCreate();

                measure(t);

                takeAdd(t);

                lastTake = t;
            }
            // return t;
        }

        private T _lastTake = new T();

        /// <summary> </summary>
        protected T lastTake
        {
            get
            {
                return _lastTake;
            }
            set
            {
                _lastTake = value;
            }
        }

        public Double GetTimeSpanInMinutes()
        {
            Double output = 0;

            foreach (T t in takeList)
            {
                output += t.secondsSinceLastTake;
            }

            return (output / (Double)60);
        }

        /// <summary>
        /// Gets the time span covered by this performance collection
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeSpan()
        {
            DateTime first = DateTime.MinValue;
            DateTime last = DateTime.MinValue;

            foreach (T t in takeList)
            {
                if (first == DateTime.MinValue)
                {
                    first = t.samplingTime;
                }
                last = t.samplingTime;
            }

            return last.Subtract(first);
        }

        //public UInt64 GetSummaryUInt64()
        //{
        //    UInt64 output = 0;

        //    foreach (performanceTake t in takes.ToList())
        //    {
        //        output += Convert.ToUInt64(t.reading);
        //    }
        //    return output;
        //}

        //public UInt64 GetMaxUInt64()
        //{
        //    UInt64 output = UInt64.MinValue;

        //    foreach (performanceTake t in takes.ToList())
        //    {
        //        output = Math.Max(output, Convert.ToUInt64(t.reading));
        //    }
        //    return output;
        //}

        //public Double GetMax()
        //{
        //    Double output = Double.MinValue;

        //    foreach (performanceTake t in takes.ToList())
        //    {
        //        output = Math.Max(output, t.reading);
        //    }
        //    return output;
        //}

        //public Double GetSummary()
        //{
        //    Double output = 0;

        //    foreach (performanceTake t in takes.ToList())
        //    {
        //        output += t.reading;
        //    }
        //    return output;
        //}

        /// <summary>
        /// Gets the average of the main reading
        /// </summary>
        /// <returns></returns>
        public Double GetAverage()
        {
            Double output = 0;

            foreach (T t in takeList)
            {
                output += t.reading;
            }

            output = output / ((Double)takes.Count() - 1);
            return output;
        }

        /// <summary>
        /// Number of takes
        /// </summary>
        /// <returns></returns>
        public virtual Int32 CountTakes()
        {
            return takes.Count;
        }

        private objectTable<T> _takes;

        /// <summary> </summary>
        protected objectTable<T> takes
        {
            get
            {
                return _takes;
            }
            set
            {
                _takes = value;
            }
        }

        //protected objectTable<T> takesTable { get; set; }

        public void LoadDataTable(DataTable input)
        {
            takes.Load(input, null);
        }

        /// <summary>
        /// Gets the data table with <see cref="objectTable{T}"/>
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTableBase(String tname = "")
        {
            return takes.GetDataTable(null, tname);
        }

        /// <summary>
        /// Loads the data table.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="removeCurrent">if set to <c>true</c> [remove current].</param>
        public virtual void LoadDataTable(DataTable input, Boolean removeCurrent = true)
        {
            DateTime start = DateTime.Now;
            if (removeCurrent) takes.Clear();
            foreach (DataRow row in input.Rows)
            {
                if (!row[3].toStringSafe().isNullOrEmpty())
                {
                    DateTime time_min = start.AddMinutes(row[1].imbConvertValueSafeTyped<Double>());
                    Double time_bet = row[2].imbConvertValueSafeTyped<Double>();
                    Double reading = row[3].imbConvertValueSafeTyped<Double>();
                    T tk = new T();
                    tk.samplingTime = time_min;
                    tk.secondsSinceLastTake = time_bet;
                    tk.reading = reading;
                    takeAdd(tk);
                }
            }

            lastTake = takes.Last();
        }

        protected Object GetDataTableLock = new Object();

        /// <summary>
        /// Gets data table with all readings
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(String prefix = "")
        {
            DataTable output = null;

            lock (GetDataTableLock)
            {
                output = new DataTable(prefix + name + "_measure");
                output.Add("TakeN", "", "T_i", typeof(Int32));
                output.Add("Time_min", "", "Td_min", typeof(Double));
                output.Add("Between_sec", "", "Tb_sec", typeof(Double));
                output.Add("Reading", "", "R_i", typeof(Double));

                DateTime first = DateTime.MinValue;

                Int32 c = 1;
                foreach (IPerformanceTake t in takeList)
                {
                    if (first == DateTime.MinValue) first = t.samplingTime;

                    output.AddRow(c, t.samplingTime.Subtract(first).TotalMinutes, t.secondsSinceLastTake, t.reading);
                    c++;
                }
            }
            return output;
        }
    }
}