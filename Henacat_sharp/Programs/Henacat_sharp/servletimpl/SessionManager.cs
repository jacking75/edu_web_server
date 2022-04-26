using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using Henacat_sharp.util;

namespace Henacat_sharp.servletimpl
{
    class SessionManager
    {
        private static Timer scheduler;
        private static int CLEAN_INTERVAL = 60; // seconds
        private static int SESSION_TIMEOUT = 10; // minutes
        private SessionIdGenerator sessionIdGenerator;
        private Map<string, HttpSessionImpl> sessions = new ConcurrentHashMap<string, HttpSessionImpl>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public HttpSessionImpl getSession( string id )
        {
            HttpSessionImpl ret = sessions.get(id);
            if ( ret != null )
            {
                ret.access();
            }
            return ret;
        }

        public HttpSessionImpl createSession()
        {
            string id = this.sessionIdGenerator.generateSessionId();
            HttpSessionImpl session = new HttpSessionImpl(id);
            sessions.Add(id, session);
            return session;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void cleanSessions()
        {
            foreach ( string id in sessions.Keys )
            {
                HttpSessionImpl session = this.sessions.get(id);
                if ( session.getLastAccessedTime()
                    < DateTime.Now.Ticks
                       - (SESSION_TIMEOUT * 60 * 1000) )
                {
                    sessions.Remove(id);
                }
            }
        }

        private void run( object sender, ElapsedEventArgs e )
        {
            cleanSessions();
        }

        public SessionManager()
        {
            scheduler = new Timer(CLEAN_INTERVAL * 1000);
            scheduler.Elapsed += run;

            this.sessionIdGenerator = new SessionIdGenerator();
        }
    }
}
