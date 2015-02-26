namespace NB.StockStudio.Foundation
{
    using System;
    using System.CodeDom.Compiler;

    public class FormulaErrorException : Exception
    {
        public CompilerErrorCollection ces;

        public FormulaErrorException(CompilerErrorCollection ces)
        {
            this.ces = ces;
        }
    }
}

