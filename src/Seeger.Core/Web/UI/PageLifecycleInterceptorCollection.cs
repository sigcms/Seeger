using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class PageLifecycleInterceptorCollection : IEnumerable<IPageLifecycleInterceptor>
    {
        private List<IPageLifecycleInterceptor> _interceptors = new List<IPageLifecycleInterceptor>();

        public int Count
        {
            get
            {
                return _interceptors.Count;
            }
        }

        public PageLifecycleInterceptorCollection(){}

        public void Add(IPageLifecycleInterceptor interceptor)
        {
            lock (_interceptors)
            {
                _interceptors.Add(interceptor);
            }
        }

        public void Remove(IPageLifecycleInterceptor interceptor)
        {
            lock (_interceptors)
            {
                _interceptors.Remove(interceptor);
            }
        }

        public void Remove(Type interceptorType)
        {
            lock (_interceptors)
            {
                foreach (var interceptor in _interceptors.ToList())
                {
                    if (interceptor.GetType() == interceptorType)
                    {
                        _interceptors.Remove(interceptor);
                    }
                }
            }
        }

        public IEnumerator<IPageLifecycleInterceptor> GetEnumerator()
        {
            return _interceptors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
