using System.Runtime.CompilerServices;

namespace CQRSCoreV2.Core
{
    using System;

    public static class LoggerExtensions
    {
        public static IDisposable MeasureTime(this ILogger instance, string label)
        {
            return MeasureTime(instance, label, null);
        }

        public static IDisposable MeasureTimeByCaller(this ILogger instance, string label,
                                                            int? warnWhenExceedsMiliSec = null,
                                                            [CallerMemberName] string memberName = "",
                                                            [CallerFilePath] string sourceFilePath = "",
                                                            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var callerInfo = new CallerInfo
            {
                MemberName = memberName,
                SourceFile = System.IO.Path.GetFileName(sourceFilePath),
                LineNumber = sourceLineNumber
            };

            var warnWhenExceeds = warnWhenExceedsMiliSec.HasValue ? TimeSpan.FromMilliseconds(warnWhenExceedsMiliSec.Value) : TimeSpan.Zero;

            return instance.MeasureTime(label, null, warnWhenExceeds, callerInfo);
        }

        public static IDisposable MeasureTime(this ILogger instance, string label, string id)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.MeasureTime(label, id, TimeSpan.Zero);
        }

        public static IDisposable MeasureTime(this ILogger instance, string label, object propertyValues)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.MeasureTime(label, null, TimeSpan.Zero, propertyValues);
        }

        public static IDisposable MeasureTime(this ILogger instance, string label, TimeSpan warnWhenExceeds)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.MeasureTime(label, null, warnWhenExceeds);
        }

        public static IDisposable MeasureTime(this ILogger instance, string label, double warnWhenExceeds)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.MeasureTime(label, null, TimeSpan.FromMilliseconds(warnWhenExceeds));
        }

        public class CallerInfo
        {
            public string MemberName { get; set; }
            public string SourceFile { get; set; }
            public int LineNumber { get; set; }

            public override string ToString()
            {
                return $"{MemberName} in File {SourceFile} Line {LineNumber}";
            }
        }
    }
}