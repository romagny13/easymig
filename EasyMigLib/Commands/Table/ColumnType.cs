﻿namespace EasyMigLib.Commands
{

    // String types

    public class CharColumnType : ColumnType
    {
        public int Length { get; }

        public CharColumnType(int length = 255)
        {
            this.Length = length;
        }
    }

    public class VarCharColumnType : ColumnType
    {
        public int Length { get; }

        public VarCharColumnType(int length = 255)
        {
            this.Length = length;
        }
    }

    public class TextColumnType : ColumnType
    { }

    public class LongTextColumnType : ColumnType
    { }

    // Numbers

    public class UnsignableColumnType : ColumnType
    {
        public bool Unsigned { get; }

        public UnsignableColumnType(bool unsigned = false)
        {
            this.Unsigned = unsigned;
        }
    }

    public class TinyIntColumnType : UnsignableColumnType
    {
        public TinyIntColumnType(bool unsigned = false)
            :base(unsigned)
        { }
    }

    public class SmallIntColumnType : UnsignableColumnType
    {
        public SmallIntColumnType(bool unsigned = false)
            : base(unsigned)
        { }
    }

    public class IntColumnType : UnsignableColumnType
    {

        public IntColumnType(bool unsigned = false)
           : base(unsigned)
        { }
    }

    public class BigIntColumnType : UnsignableColumnType
    {

        public BigIntColumnType(bool unsigned = false)
           : base(unsigned)
        { }
    }

    public class BitColumnType : ColumnType
    { }

    public class FloatColumnType : ColumnType
    {
        public int? Digits { get; }

        public FloatColumnType(int? digits = null)
        {
            this.Digits = digits;
        }
    }

    // DateTime

    public class DateTimeColumnType : ColumnType
    { }

    public class DateColumnType : ColumnType
    { }

    public class TimeColumnType : ColumnType
    { }

    public class TimestampColumnType : ColumnType
    { }

    // blob

    public class BlobColumnType : ColumnType
    { }

    public class ColumnType
    {

        public bool CheckDefaultValue(object value)
        {
            if (value != null)
            {
                var type = this.GetType();
                if (type.IsSubclassOf(typeof(UnsignableColumnType)))
                {
                    return int.TryParse(value.ToString(), out int result);
                }
                else if (type == typeof(FloatColumnType))
                {
                    return double.TryParse(value.ToString(), out double result);
                }
                else if (type == typeof(BitColumnType))
                {
                    if(int.TryParse(value.ToString(),out int result))
                    {
                        return result == 0 || result == 1;
                    }
                    return false;
                }
                else
                {
                    return value.GetType() == typeof(string);
                }
            }
            return true;
        }

        // String types

        public static CharColumnType Char(int length = 10)
        {
            // CHAR(10)
            return new CharColumnType(length);
        }

        public static VarCharColumnType VarChar(int length = 255)
        {
            // VARCHAR(255)
            return new VarCharColumnType(length);
        }

        public static TextColumnType Text()
        {
            // NVARCHAR(max) or TEXT
            return new TextColumnType();
        }

        public static LongTextColumnType LongText()
        {
            // NTEXT (Sql Server) or LONGTEXT (MySQL)
            return new LongTextColumnType();
        }

        // Numbers

        public static TinyIntColumnType TinyInt(bool unsigned = false)
        {
            // TINYINT
            return new TinyIntColumnType(unsigned);
        }

        public static SmallIntColumnType SmallInt(bool unsigned = false)
        {
            // SMALLINT
            return new SmallIntColumnType(unsigned);
        }

        public static IntColumnType Int(bool unsigned = false)
        {
            // INT
            return new IntColumnType(unsigned);
        }

        public static BigIntColumnType BigInt(bool unsigned = false)
        {
            // BIGINT
            return new BigIntColumnType(unsigned);
        }

        public static BitColumnType Bit()
        {
            // BIT
            return new BitColumnType();
        }

        public static FloatColumnType Float(int? digits = null)
        {
            // FLOAT
            return new FloatColumnType(digits);
        }

        public static DateTimeColumnType DateTime()
        {
            // DATETIME
            return new DateTimeColumnType();
        }

        public static DateColumnType Date()
        {
            // DATE
            return new DateColumnType();
        }

        public static TimeColumnType Time()
        {
            // TIME(7) (Sql Server) or TIME (MySQL)
            return new TimeColumnType();
        }

        public static TimestampColumnType Timestamp()
        {
            // TIMESTAMP
            return new TimestampColumnType();
        }

        public static BlobColumnType Blob()
        {
            // TIMESTAMP
            return new BlobColumnType();
        }
    }
}
