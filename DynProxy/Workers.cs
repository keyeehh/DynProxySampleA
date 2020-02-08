using System;
using System.Collections.Generic;
using System.Text;

using DynProxy.NGPVAttributes;

namespace DynProxy
{

    #region ICalculator ===========================================================================
    interface ICalculator
    {
        public int Add(int a, int b);
        public int AddSpecial(int a, int b, int c);
    }   //  interface ICalculator

    public class Calculator : ICalculator
    {
        [NGPVLogAttribute(NGPV_LogCategory.LogDoctorPreference, CallStack = true)]
        [NGPVProfile(NGPV_ProfileCategory.PerformanceProfileing, ProfileMethdology = NGPV_ProfileMethodology.KenXu)]
        public int Add(int a, int b)
        {
            return a + b;
        }   //  Add()

        [NGPVLogAttribute(NGPV_LogCategory.LogHost)]
        [NGPVProfile(NGPV_ProfileCategory.Throughput)]
        public int AddSpecial(int a, int b, int c)
        {
            return a * 2 + b * 2 + c;
        }
    }   //  class Calculator
    #endregion  ICalculator

    #region IMyClass ==============================================================================
    public interface IMyClass
    {
        int MyMethod(string param);
    }   //  interface IMyClass

    public class MyClass : IMyClass
    {
        public int MyMethod(string param)
        {
            return param.Length;
        }
    }   //  class MyClass
    #endregion  IMyClass
}   //  namespace DynProxy
