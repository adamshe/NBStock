namespace NB.StockStudio.Foundation
{
    using System;

    public class ViewChangedArgs
    {
        public int EndBar;
        public int FirstBar;
        public int LastBar;
        public int StartBar;

        public ViewChangedArgs(int StartBar, int EndBar, int FirstBar, int LastBar)
        {
            this.StartBar = StartBar;
            this.EndBar = EndBar;
            this.FirstBar = FirstBar;
            this.LastBar = LastBar;
        }
    }
}

