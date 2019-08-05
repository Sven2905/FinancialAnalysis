using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Utilities
{
    /// <remark>
    /// Die statische Klasse ConvertTo stellt Methoden zur Konvertierung von Datentypen bereit, alle diese Methoden können mit null und DBNull umgehen
    /// </remark>
    public static class ConvertTo
    {
        static public CultureInfo Culture = CultureInfo.GetCultureInfo("de-DE");

        /// <summary>
        /// MinDatValue stellt den Basiswert für ConvertTo.MinDat bereit, der Wert ist mit dem 01.01.2000 00:00:00 voreingestellt
        /// </summary>
        static public DateTime MinDatValue = new DateTime(2000, 1, 1);

        /// <summary>
        /// MaxDatValue stellt den Basiswert für ConvertTo.MinDat bereit, der Wert ist mit dem 31.12.2199 23:59:59 voreingestellt
        /// </summary>
        static public DateTime MaxDatValue = new DateTime(2199, 12, 31, 23, 59, 59);

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem ByteArray
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public byte[] ByteArray(object value)
        {
            return ConvertTo.ByteArray(value, Encoding.Unicode);
        }

        static public byte[] ByteArray(object value, Encoding encoding)
        {
            if (value is byte[])
            {
                return (byte[])value;
            }
            else
            {
                if ((value == System.DBNull.Value) || (value == null))
                {
                    return new byte[0];
                }
                else
                {
                    return encoding.GetBytes(value.ToString());
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem String
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>string</returns>
        static public string String(object value)
        {
            string result = ConvertTo.StringNull(value);
            if (result != null)
                return result;
            else
                return "";
        }

        static public string StringNull(object value)
        {
            if (value is string)
            {
                return (string)value;
            }
            else
            {
                if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                {
                    return null;
                }
                else
                {
                    return value.ToString();
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Integer (int32)
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <param name="defaultValue">Standardwert</param>
        /// <returns>int</returns>
        static public int Int(object value, int defaultValue)
        {
            int? result = ConvertTo.IntNull(value);

            if (result.HasValue)
                return result.Value;
            else
                return defaultValue;
        }

        static public int Int(object value)
        {
            return ConvertTo.Int(value, 0);
        }

        static public int? IntNull(object value)
        {
            if (value is int)
            {
                return (int)value;
            }
            else if (value is int?)
            {
                return (int?)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return null;
                    }
                    else
                    {
                        return System.Convert.ToInt32(value);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Boolean
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>bool</returns>
        static public bool Bool(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            else
            {
                if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                {
                    return false;
                }
                else
                {
                    string testval = value.ToString().ToUpper();
                    if (testval == "TRUE")
                    {
                        return true;
                    }
                    else if (testval == "FALSE")
                    {
                        return false;
                    }
                    else if (testval == "0")
                    {
                        return false;
                    }
                    else if (testval == "1" || testval == "-1")
                    {
                        return true;
                    }
                    else
                    {
                        try
                        {
                            return System.Convert.ToBoolean(value);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
        }

        static public bool? BoolNull(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            else if (value is bool?)
            {
                return (bool?)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return null;
                    }
                    else
                    {
                        return System.Convert.ToBoolean(value);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Double
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>double</returns>
        static public double Double(object value, double defaultValue)
        {
            double? result = ConvertTo.DoubleNull(value);

            if (result.HasValue)
                return result.Value;
            else
                return defaultValue;
        }

        public static double Double(object value, int roundCommercialDecimal)
        {
            double ret = ConvertTo.Double(value);
            if (roundCommercialDecimal > 0)
                ret = RoundCommercial(ret, roundCommercialDecimal);
            return ret;
        }

        public static double Double(object value)
        {
            return ConvertTo.Double(value, 0.0);
        }

        static public double? DoubleNull(object value)
        {
            if (value is double)
            {
                return (double)value;
            }
            else if (value is double?)
            {
                return (double?)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return null;
                    }
                    else
                    {
                        return System.Convert.ToDouble(value);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Decimal
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>decimal</returns>
        static public decimal Decimal(object value, decimal defaultValue)
        {
            decimal? result = ConvertTo.DecimalNull(value);

            if (result.HasValue)
                return result.Value;
            else
                return defaultValue;
        }

        public static decimal Decimal(object value)
        {
            return ConvertTo.Decimal(value, 0.0M);
        }

        static public decimal? DecimalNull(object value)
        {
            if (value is decimal)
            {
                return (decimal)value;
            }
            else if (value is decimal?)
            {
                return (decimal?)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return null;
                    }
                    else
                    {
                        return System.Convert.ToDecimal(value);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Long (int64)
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>long</returns>
        static public long Long(object value, long defaultValue)
        {
            long? result = ConvertTo.LongNull(value);
            if (result.HasValue)
                return result.Value;
            else
                return defaultValue;
        }

        static public long Long(object value)
        {
            return Long(value, 0);
        }

        static public long? LongNull(object value)
        {
            if (value is long || value is long?)
            {
                return (long?)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return (long?)null;
                    }
                    else
                    {
                        return System.Convert.ToInt64(value);
                    }
                }
                catch
                {
                    return (long?)null;
                }
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Datetime, im Falle von null oder DBNull wird ConvertTo.MinDatValue zurückgegeben
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>DateTime</returns>
        static public DateTime MaxDat(object value)
        {
            try
            {
                if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                {
                    return MaxDatValue;
                }
                else
                {
                    return System.Convert.ToDateTime(value);
                }
            }
            catch
            {
                return MaxDatValue;
            }
        }

        /// <summary>
        /// Konvertiert das angegebene Objekt zu einem Datetime, im Falle von null oder DBNull wird ConvertTo.MinDatValue zurückgegeben
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>DateTime</returns>
        static public DateTime MinDat(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return MinDatValue;
                    }
                    else
                    {
                        return System.Convert.ToDateTime(value);
                    }
                }
                catch
                {
                    return MinDatValue;
                }
            }
        }

        /// <summary>
        /// /// Konvertiert das angegebene Objekt zu einem Datetime, im Falle von null oder DBNull wird DateTime.Now zurückgegeben
        /// </summary>
        /// <param name="value">Zu Konvertierendes Objekt</param>
        /// <returns>DateTime</returns>
        static public DateTime NowDat(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return DateTime.Now;
                    }
                    else
                    {
                        return System.Convert.ToDateTime(value);
                    }
                }
                catch
                {
                    return DateTime.Now;
                }
            }
        }

        static public DateTime? NullDat(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            else if (value is DateTime?)
            {
                return (DateTime?)value;
            }
            else
            {
                try
                {
                    if ((value == System.DBNull.Value) || (value == null) || (value.ToString() == ""))
                    {
                        return null;
                    }
                    else
                    {
                        return System.Convert.ToDateTime(value);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Konvertiert einen angegebenen string in die durch den angegebenen Typparameter repräsentierten enum-Wert
        /// </summary>
        /// <typeparam name="T">enum, in den Konvertiert werden soll</typeparam>
        /// <param name="value">Zu Konvertierender Wert</param>
        /// <param name="defaultValue">Standardwert, der zurückgegeben wird, falls sich der string nicht parsen lässt</param>
        /// <returns>enum vom Typ T</returns>
        //public static T GetEnum<T>(string value, T defaultValue) where T : struct, IConvertible
        //{
        //    if (!typeof(T).IsEnum) throw new ArgumentException("Type parameter must be an enum!");
        //    if (string.IsNullOrEmpty(value)) return defaultValue;
        //    foreach (T item in Enum.GetValues(typeof(T)))
        //    {
        //        if (item.ToString().ToLower().Equals(value.Trim().ToLower())) return item;
        //    }
        //    return defaultValue;
        //}

        /// <summary>
        /// Konvertiert eine angegebene Währungsrepräsentation in einen double
        /// </summary>
        /// <param name="value">Zu konvertierender Wert</param>
        /// <returns>double</returns>
        public static double GetFromCurrency(object value)
        {
            if (value != null && value != DBNull.Value)
            {
                return ConvertTo.Double(value.ToString().Replace(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol, "").Trim());
            }
            else
            {
                return 0.0;
            }
        }

        public static object DBNullValue(object value)
        {
            return value == null ? (object)DBNull.Value : value;
        }

        public static string ByteArrayToString(byte[] array)
        {
            string result = "";

            if (array != null)
            {
                foreach (byte b in array)
                {
                    result += string.Format("{0:x2}", b);
                }
            }

            return result;
        }

        public static byte[] StringToByteArray(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                return Enumerable.Range(0, data.Length).Where(x => x % 2 == 0).Select(x => System.Convert.ToByte(data.Substring(x, 2), 16)).ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Konvertiert einen angegebenen string in die durch den angegebenen Typparameter repräsentierten enum-Wert
        /// </summary>
        /// <typeparam name="T">enum, in den Konvertiert werden soll</typeparam>
        /// <param name="value">Zu Konvertierender Wert</param>
        /// <param name="defaultValue">Standardwert, der zurückgegeben wird, falls sich der string nicht parsen lässt</param>
        /// <returns>enum vom Typ T</returns>
        public static T GetEnum<T>(string value, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type parameter must be an enum!");

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString().ToLower().Equals(value.Trim().ToLower())) return item;
            }

            return defaultValue;
        }

        public static T GetEnum<T>(int value, T defaultValue) where T : struct, IConvertible
        {
            try
            {
                object obj = Enum.ToObject(typeof(T), value);
                if (obj == null)
                {
                    return defaultValue;
                }
                else
                {
                    return (T)obj;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return defaultValue;
            }
        }

        static public DateTime MinDatJS(object value)
        {
            DateTime? result = ConvertTo.NullDatJS(value);
            if (!result.HasValue)
                return MinDatValue;
            else
                return result.Value;
        }

        static public DateTime MaxDatJS(object value)
        {
            DateTime? result = ConvertTo.NullDatJS(value);
            if (!result.HasValue)
                return MaxDatValue;
            else
                return result.Value;
        }

        static public DateTime NowDatJS(object value)
        {
            DateTime? result = ConvertTo.NullDatJS(value);
            if (!result.HasValue)
                return DateTime.Now;
            else
                return result.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public DateTime? NullDatJS(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }
            else
            {
                string stringValue = value.ToString();
                stringValue = stringValue.Substring(0, 24);

                CultureInfo provider = CultureInfo.InvariantCulture;

                var format = "ddd MMM dd yyyy HH:mm:ss";

                return DateTime.ParseExact(stringValue, format, provider);
            }
        }

        /// <summary>
        /// Liefert ein Abrechnungsdatum als Integer Wert zurück im Format YYYYMM
        /// </summary>
        /// <param name="datum">Datum zur Konvertierung</param>
        /// <returns>Integer Wert zurück im Format YYYYMM</returns>
        public static int GetAbrDatum(DateTime datum)
        {
            return datum.Year * 100 + datum.Month;
        }

        /// <summary>
        /// Erzeugt eine TZeitangabe im Format Stunden: Minuten: Sekunden
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        static public string HHMMSS(double seconds)
        {
            try
            {
                if ((seconds > 0) && (!double.IsInfinity(seconds)))
                {
                    TimeSpan xSpan = new TimeSpan(Convert.ToInt64(seconds) * 1000 * 1000 * 10);
                    return System.Math.Floor(xSpan.TotalHours).ToString("00") + ":" + xSpan.Minutes.ToString("00") + ":" + xSpan.Seconds.ToString("00");
                }
                else
                {
                    return "00:00:00";
                }
            }
            catch (Exception ex)
            {
                Debug("Exception in Globals - ConvertToHHMMSS");
                Debug(ex.Message);
                Debug(ex.StackTrace);
                return "00:00:00";
            }
        }

        /// <summary>
        /// Konvertiert ein Dateum in das Format ShortDate + ShortTime
        /// </summary>
        /// <param name="value">Datum zum formatieren</param>
        /// <returns></returns>
        static public string ShortDateTime(DateTime value)
        {
            string ret = value.ToShortDateString() + " " + value.ToShortTimeString();
            return ret;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        static public double RoundCommercial(double value, int dec)
        {
            return Math.Round(value, dec, MidpointRounding.AwayFromZero);
        }

        static public decimal RoundCommercialDecimal(decimal value, int dec)
        {
            return Math.Round(value, dec, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Liefert aus der übergebenen Zeichenfolge die enthaltenen Zahlen zurück. Der String von links nach rechts durchgeparst.
        /// Die Eingabe '1a2' liefert zum Beispiel den Wert '12' zurück.
        /// </summary>
        /// <param name="value"></param>
        /// Zeichenfolge, aus der die Zahlen extrahiert werden sollen.
        /// <returns></returns>
        static public int GetNumberStringInteger(string value)
        {
            string ret = "";
            string AvailableChars = "0123456789";
            for (int i = 0; i < value.Length; i++)
            {
                if (AvailableChars.IndexOf((value.Substring(i, 1))) > -1)
                {
                    ret = ret + value.Substring(i, 1);
                }
            }
            return Int(ret);
        }

        /// <summary>
        /// Liefert aus der übergebenen Zeichenfolge die enthaltenen Zahlen zurück. Der String von links nach rechts durchgeparst.
        /// Fließkommazahlen werden durch Komma-Trennung auomatisch korrigiert.
        /// Die Eingabe '1.a2' liefert zum Beispiel den Wert '1,2' zurück.
        /// </summary>
        /// <param name="pNumberString"></param>
        /// Zeichenfolge, aus der die Zahlen extrahiert werden sollen.
        /// <returns></returns>
        static public double GetNumberStringDouble(string value)
        {
            if (value == "")
                return 0D;

            string ret = "";
            string AvailableChars = "-,.0123456789";

            if (value.Contains("("))
                value.Replace("(", "");
            if (value.Contains(")"))
                value.Replace(")", "");

            value = value.Replace(".", ",");

            for (int i = 0; i < value.Length; i++)
            {
                if (AvailableChars.IndexOf((value.Substring(i, 1))) > -1)
                {
                    ret = ret + value.Substring(i, 1);
                }
            }
            return Double(ret);
        }

        public static ArrayList ArrayList(List<int> list)
        {
            ArrayList ret = new ArrayList(list.Count);
            try
            {
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        foreach (int item in list)
                        {
                            ret.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug(e.Message);
                ret.Clear();
            }
            return ret;
        }

        public static void Debug(string text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            sb.Append(" [");
            sb.Append(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            sb.Append("]: ");
            sb.Append(text);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }
    }
}