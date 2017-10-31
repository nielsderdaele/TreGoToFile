﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using TreGoToFile.Extensions;

namespace TreGoToFile.Parsers
{
    internal class WebpackParser : IErrorParser
    {
        private static readonly Regex lineRegex = new Regex(@"ERROR in (?<path>(?>\w\:\\)?(?>[\w\.\-]+(?>\\|\/))*[\w\.\-]+\.\w+)(?>( \(|\s)(?<line>\d+)(?>,(?<column>\d+))?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public FilePoint GetError(TextPointer pointer)
        {
            var line = pointer.GetLine(0) + " " + pointer.GetLine(1);
            var match = lineRegex.Match(line);
            if (match.Success)
            {
                return new FilePoint
                {
                    Path = match.Groups["path"].Value,
                    Line = ToNullableInt(match.Groups["line"]),
                    Column = ToNullableInt(match.Groups["column"]),
                };
            }
            else
            {
                return null;
            }
        }

        private static int? ToNullableInt(Group group)
        {
            return group.Success ? int.Parse(group.Value) : (int?)null;
        }
    }
}
