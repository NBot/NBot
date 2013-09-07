using System;
using System.Net;
using Microsoft.TeamFoundation.Client;

namespace NBot.Plugins.TeamFoundationServer
{
    public class ConnectByImplementingCredentialsProvider : ICredentialsProvider
    {
        private NetworkCredential credentials;

        #region ICredentialsProvider Members
        public ConnectByImplementingCredentialsProvider(string user, string domain, string password)
        {
            credentials = new NetworkCredential(user, password, domain);
        }

        public ICredentials GetCredentials(Uri uri, ICredentials failedCredentials)
        {
            return credentials;
        }

        public void NotifyCredentialsAuthenticated(Uri uri)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}