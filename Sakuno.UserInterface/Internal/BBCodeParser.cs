using Sakuno.Parsers;
using Sakuno.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Sakuno.UserInterface.Internal
{
    using Parser = Parser<IInput<char>, Inline>;

    static class BBCodeParsers
    {
        static Parser SimpleText =>
            from rText in StringParsers.AnyChar.Until(StringParsers.Char('[')).AsString()
            select new Run(rText);

        static Parser<IInput<char>, string> OpeningTag =>
            from _ in StringParsers.Char('[')
            from rName in StringParsers.Letter.AtLeastOnce().AsString()
            from __ in StringParsers.Char(']')
            select rName;
        static Parser Tag =>
            from rOpeningTag in OpeningTag
            from rSubBBCode in BBCode
            from _ in StringParsers.Char('[')
            from __ in StringParsers.Char('/')
            from rClosingTag in StringParsers.String(rOpeningTag)
            from ___ in StringParsers.Char(']')
            select GetInline(rOpeningTag, rSubBBCode);

        static Parser BBCode => Tag.LeftIfPartiallyParsed(SimpleText).Many().Select(GetSpan);

        static Inline GetInline(string rpOpeningTag, Inline rpContent)
        {
            if (rpOpeningTag.OICEquals("b"))
                return new Bold(rpContent);
            else if (rpOpeningTag.OICEquals("i"))
                return new Italic(rpContent);
            else if (rpOpeningTag.OICEquals("u"))
                return new Underline(rpContent);

            Func<Inline, Inline> rConsturctor;
            if (BBCodeBlock.CustomTags.TryGetValue(rpOpeningTag, out rConsturctor))
                return rConsturctor(rpContent) ?? rpContent;

            return rpContent;
        }
        static Inline GetSpan(IEnumerable<Inline> rpInlines)
        {
            var rSpan = new Span();

            foreach (var rInline in rpInlines)
                rSpan.Inlines.Add(rInline);

            return rSpan;
        }

        public static Inline Parse(string rpBBCode)
        {
            var rResult = BBCode(new StringInput(rpBBCode));
            if (!rResult.Success)
                return new Run(rpBBCode);

            return rResult.Value;
        }
    }
}
