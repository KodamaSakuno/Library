using Sakuno.Parsers;
using Sakuno.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sakuno.UserInterface.Internal
{
    using Parser = Parser<IInput<char>, Inline>;

    static class BBCodeParsers
    {
        static Lazy<BrushConverter> r_BrushConverter = new Lazy<BrushConverter>();

        static Parser SimpleText =>
            from rText in StringParsers.AnyChar.Until(StringParsers.Char('[')).AsString()
            select new Run(rText);

        static Parser<IInput<char>, string> Parameter =>
            from __ in StringParsers.Char('=')
            from rParameter in StringParsers.AnyChar.Until(StringParsers.Char(']')).AsString()
            select rParameter;

        static Parser<IInput<char>, OpeningTagInfo> OpeningTag =>
            from _ in StringParsers.Char('[')
            from rName in StringParsers.Letter.AtLeastOnce().AsString()
            from rParameter in Parameter.Optional()
            from __ in StringParsers.Char(']')
            select new OpeningTagInfo(rName, rParameter);
        static Parser NormalTag =>
            from rOpeningTag in OpeningTag
            from rSubBBCode in BBCode
            from _ in StringParsers.Char('[')
            from __ in StringParsers.Char('/')
            from rClosingTag in StringParsers.String(rOpeningTag.Name)
            from ___ in StringParsers.Char(']')
            select GetInline(rOpeningTag, rSubBBCode);
        static Parser BindingTag =>
            from _ in StringParsers.String("[binding]")
            from rPath in StringParsers.LetterOrDigit.Or(StringParsers.Char('.')).AtLeastOnce().AsString()
            from __ in StringParsers.String("[/binding]")
            select GetBinding(rPath);

        static Parser Tag => BindingTag.Or(NormalTag);

        static Parser BBCode => Tag.LeftIfPartiallyParsed(SimpleText).Many().Select(GetSpan);

        static Inline GetInline(OpeningTagInfo rpOpeningTag, Inline rpContent)
        {
            var rName = rpOpeningTag.Name;

            if (rName.OICEquals("b"))
                return new Bold(rpContent);
            else if (rName.OICEquals("i"))
                return new Italic(rpContent);
            else if (rName.OICEquals("u"))
                return new Underline(rpContent);
            else if (rName.OICEquals("color"))
                return new Span(rpContent) { Foreground = (Brush)r_BrushConverter.Value.ConvertFromInvariantString(rpOpeningTag.Parameter) };
            else if (rName.OICEquals("size"))
                return new Span(rpContent) { FontSize = double.Parse(rpOpeningTag.Parameter) };

            Func<Inline, string, Inline> rCTWPConsturctor;
            if (BBCodeBlock.CustomTagsWithParameter.TryGetValue(rpOpeningTag.Name, out rCTWPConsturctor))
                return rCTWPConsturctor(rpContent, rpOpeningTag.Parameter) ?? rpContent;

            Func<Inline, Inline> rCTConsturctor;
            if (BBCodeBlock.CustomTags.TryGetValue(rpOpeningTag.Name, out rCTConsturctor))
                return rCTConsturctor(rpContent) ?? rpContent;

            return rpContent;
        }
        static Inline GetSpan(IEnumerable<Inline> rpInlines)
        {
            var rSpan = new Span();

            foreach (var rInline in rpInlines)
                rSpan.Inlines.Add(rInline);

            return rSpan;
        }

        static Inline GetBinding(string rpPath)
        {
            var rResult = new Run();
            rResult.SetBinding(Run.TextProperty, new Binding(rpPath) { Mode = BindingMode.OneWay });

            return rResult;
        }

        public static Inline Parse(string rpBBCode)
        {
            var rResult = BBCode(new StringInput(rpBBCode));
            if (!rResult.Success)
                return new Run(rpBBCode);

            return rResult.Value;
        }

        struct OpeningTagInfo
        {
            public string Name { get; }

            public string Parameter { get; }

            public OpeningTagInfo(string rpName, string rpParameter)
            {
                Name = rpName;

                Parameter = rpParameter;
            }
        }
    }
}
