using System;
using System.Diagnostics;
using System.Reflection;

namespace DynProxy.NGPVAttributes
{

    #region Base Interface ========================================================================
    interface IAttrPrior
    {
        public void InvokePrior();
    }   //  IAttrPrior

    interface IAttrPost
    {
        public void InvokePost();
    }   //  IAttrPost
    #endregion  Base Interface

    #region NGPV Log Attribute ====================================================================
    public enum NGPV_LogCategory
    {
        LogHost = 0,
        LogHistogram = 1,
        LogProfile = 2,
        LogDoctorPreference = 3,
    }   //  NGPV_LogCategory

    [AttributeUsage(AttributeTargets.Method)]
    public class NGPVLogAttribute : Attribute, IAttrPrior
    {
        public NGPVLogAttribute(NGPV_LogCategory type)
        {
            _logType = type;
            CallStack = false;
        }

        public NGPV_LogCategory LogType => _logType;
        public bool CallStack;

        void IAttrPrior.InvokePrior()
        {
            Console.WriteLine($"Log Type: {_logType}\tCall Stack: {CallStack}");
            Trace.WriteLine($"Log Type: {_logType}\tCall Stack: {CallStack}");
        }

        private NGPV_LogCategory _logType;
    }   //  class NGPVLogAttribute
    #endregion  NGPV Log Attribute

    #region NGPV Profile Attribute ================================================================
    public enum NGPV_ProfileCategory
    {
        PerformanceProfileing = 0,
        Throughput = 1,
        CpuUsage = 2,
        DiskIO = 3,
    }   //  NGPV_ProfileCategory

    public enum NGPV_ProfileMethodology
    {
        BradSmith = 0,
        KenXu = 1,
    }   //  NGPV_ProfileMethodology

    [AttributeUsage(AttributeTargets.All)]
    public class NGPVProfileAttribute : Attribute, IAttrPost
    {
        public NGPVProfileAttribute(NGPV_ProfileCategory type)
        {
            _profileCategory = type;
            ProfileMethdology = NGPV_ProfileMethodology.BradSmith;
        }

        public NGPV_ProfileCategory ProfileCategory => _profileCategory;
        public NGPV_ProfileMethodology ProfileMethdology;

        void IAttrPost.InvokePost()
        {
            Console.WriteLine($"Profile Category: {ProfileCategory}\tProfile Methodology: {ProfileMethdology}");
            Trace.WriteLine($"Profile Category: {ProfileCategory}\tProfile Methodology: {ProfileMethdology}");
        }

        private NGPV_ProfileCategory _profileCategory;
    }   //  class NGPVProfileAttribute
    #endregion  NGPV Profile Attribute
}   //  namespace DynProxy.NGPVAttributes