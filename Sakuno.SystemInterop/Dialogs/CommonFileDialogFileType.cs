using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sakuno.SystemInterop.Dialogs
{
    public class CommonFileDialogFileType
    {
        static Regex r_ExtensionRegex = new Regex(@"(?:\*\.|\.)?(\w+|\*)");

        public string Name { get; set; }

        public IList<string> Extensions { get; }

        public CommonFileDialogFileType(string rpName, string rpExtensions)
        {
            Name = rpName;

            Extensions = r_ExtensionRegex.Matches(rpExtensions).OfType<Match>().Select(r => r.Groups[1].Value).ToList();
        }

        internal NativeStructs.COMDLG_FILTERSPEC ToFilterSpec()
        {
            var rBuilder = new StringBuilder();
            foreach (var rExtension in Extensions)
            {
                if (rBuilder.Length > 0)
                    rBuilder.Append(';');

                rBuilder.Append("*.");
                rBuilder.Append(rExtension);
            }

            return new NativeStructs.COMDLG_FILTERSPEC()
            {
                pszName = Name,
                pszSpec = rBuilder.ToString(),
            };
        }
    }
}
