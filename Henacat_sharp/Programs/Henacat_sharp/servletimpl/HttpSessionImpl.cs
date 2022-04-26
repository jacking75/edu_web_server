using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Henacat_sharp.servlet.http;
using Henacat_sharp.util;
using System.Runtime.CompilerServices;

namespace Henacat_sharp.servletimpl
{
    class HttpSessionImpl : HttpSession
    {
        private string id;
        private Map<string, object> attributes
            = new ConcurrentHashMap<string, object>();
        private long lastAccessedTime;
        private object lockobj = new object();


        public object getAttribute( string name )
        {
            return this.attributes.get(name);
        }

        public IEnumerable<string> getAttributeNames()
        {
            List<string> names = new List<string>();
            names.AddRange(attributes.Keys.ToList());

            return names;
        }

        public string getId()
        {
            return this.id;
        }

        public void removeAttribute( string name )
        {
            this.attributes.Remove(name);
        }

        public void setAttribute( string name, object value )
        {
            if ( value == null )
            {
                removeAttribute(name);
                return;
            }
            if ( this.attributes.ContainsKey(name) )
            {
                this.attributes[name] = value;
            }
            else
            {
                this.attributes.Add(name, value);
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void access()
        {
            lock ( lockobj )
            {
                this.lastAccessedTime = DateTime.Now.Ticks;
            }
        }

        public long getLastAccessedTime()
        {
            lock ( lockobj )
            {
                return this.lastAccessedTime;
            }
        }

        public HttpSessionImpl( string id )
        {
            this.id = id;
            this.access();
        }
    }
}
