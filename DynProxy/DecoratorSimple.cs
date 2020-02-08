using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;

using DynProxy.NGPVAttributes;

namespace DynProxy
{
    public class LoggingDecorator<T> : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                #region NGPV Attributes Caching =======================================================
                if (!_attrPriorCache.ContainsKey(targetMethod.Name))    //  only needs to check one cache
                {
                    object[] attrs = _decorated.GetType()
                                     .GetMember(targetMethod.Name)[0]  //  To Do: support overload function
                                     .GetCustomAttributes(false);

                    List<IAttrPrior> attrPrior = new List<IAttrPrior>();
                    List<IAttrPost> attrPost = new List<IAttrPost>();
                    foreach (var attr in attrs)
                    {
                        switch (attr)
                        {
                            case IAttrPrior prior:
                                attrPrior.Add(prior);
                                break;

                            case IAttrPost post:
                                attrPost.Add(post);
                                break;

                            default:
                                //  method does not have attribute
                                break;
                        }   //  switch (attr)
                    }   //  foreach (attrs)

                    _attrPriorCache[targetMethod.Name] = attrPrior;
                    _attrPostCache[targetMethod.Name] = attrPost;
                }   //  if (cache == none)
                #endregion  NGPV Attribtes Caching

                LogBefore(targetMethod, args);

                var result = targetMethod.Invoke(_decorated, args);

                LogAfter(targetMethod, args, result);
                return result;
            }
            catch (Exception ex) when (ex is TargetInvocationException)
            {
                LogException(ex.InnerException ?? ex, targetMethod);
                throw ex.InnerException ?? ex;
            }
        }

        public static T Create(T decorated)
        {
            object proxy = Create<T, LoggingDecorator<T>>();
            ((LoggingDecorator<T>)proxy).SetParameters(decorated);

            return (T)proxy;
        }

        private void SetParameters(T decorated)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            _decorated = decorated;
        }

        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} threw exception:\n{exception}");
        }

        private void LogAfter(MethodInfo methodInfo, object[] args, object result)
        {
            if (_attrPostCache.ContainsKey(methodInfo.Name))
                foreach (IAttrPost attr in _attrPostCache[methodInfo.Name]) attr.InvokePost();

            Console.WriteLine($"==>\t{_decorated.GetType().FullName}.{methodInfo.Name} executed\tOutput: {result}\n");
        }

        private void LogBefore(MethodInfo methodInfo, object[] args) 
        {
            if (_attrPriorCache.ContainsKey(methodInfo.Name))
                foreach (IAttrPrior attr in _attrPriorCache[methodInfo.Name]) attr.InvokePrior();

            Console.Write($"==> \t{_decorated.GetType().FullName}.{methodInfo.Name} is executing\tArguments:");
            foreach (var o in args) Console.Write($" {o}");
            Console.WriteLine();
        }

        private T _decorated;
        private Dictionary<string, IList<IAttrPrior>> _attrPriorCache = new Dictionary<string, IList<IAttrPrior>>();
        private Dictionary<string, IList<IAttrPost>> _attrPostCache = new Dictionary<string, IList<IAttrPost>>();
    }   //  class LoggingDecorator<T>
}   //  namespace DynProxy