namespace ContextItems.CommonGeneration.StringGenerator
{
    using System;
    using System.Collections.Generic;

    public interface IStringGenerator
    {
        void PushIndent();

        void PopIndent();

        void Braces(Action action, bool semicolon = false);

        void AppendLine(string line);

        void AppendLine();

        void AppendLinesIndented(List<string> lines);

        void AppendLines(List<string> lines);
    }
}