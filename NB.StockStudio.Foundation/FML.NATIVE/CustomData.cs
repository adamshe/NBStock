namespace FML.NATIVE
{
    using System;

    public class CustomData
    {
        public double[] Data;
        public double[] Date;
        public string Name;

        public CustomData(string Name, double[] Date, double[] Data)
        {
            this.Name = Name;
            this.Date = Date;
            this.Data = Data;
        }
    }
}

