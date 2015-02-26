// Decompiled with JetBrains decompiler
// Type: FML.MFI
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class MFI : FormulaBase
  {
    public MFI()
    {
      base.\u002Ector();
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(this.get_HIGH(), this.get_LOW()), this.get_CLOSE()), FormulaData.op_Implicit(3.0));
      formulaData1.Name = (__Null) "TP ";
      FormulaData formulaData2 = FormulaData.op_Multiply(formulaData1, this.get_V());
      formulaData2.Name = (__Null) "MF ";
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData2
      }, "");
    }
  }
}
