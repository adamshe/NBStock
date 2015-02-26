namespace NB.StockStudio.WinControls
{
    using System;

    public class OverlayItem
    {
        private string description;
        private string name;

        public OverlayItem(string Name, string Description)
        {
            this.name = Name;
            this.description = Description;
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}

