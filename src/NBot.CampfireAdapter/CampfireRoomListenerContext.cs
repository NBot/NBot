﻿using System;
using System.Net;
using System.Text;
using NBot.Core;

namespace NBot.CampfireAdapter
{
    public class CampfireRoomListenerContext
    {
        private static readonly string[] AvailableSubdomains = { "streaming", "streaming1", "streaming2", "streaming3" };
        private static readonly Object SyncRoot = new Object();
        private static int _nextSubdomainIndex = 0;

        public CampfireRoomListenerContext(int roomId, string token, int userId, Action<Message> messageRecieved)
        {
            RoomId = roomId;
            UserId = userId;
            AuthorizationHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(token + ":X"));
            MessageRecieved = messageRecieved;
            IsListening = true;
        }

        public int RoomId { get; private set; }
        public int UserId { get; private set; }
        public bool IsListening { get; private set; }
        public string AuthorizationHeader { get; private set; }
        public Action<Message> MessageRecieved { get; private set; }

        public void StopListening()
        {
            IsListening = true;
        }

        private string GetRoomStreamingUrl()
        {
            // Pick a subdomain at random
            return string.Format("https://{0}.campfirenow.com/room/{1}/live.json", GetNextSubdomain(), RoomId);
        }

        private string GetUserAgent()
        {
            return string.Format("NBot {0}", RoomId);
        }

        public HttpWebRequest CreateRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(GetRoomStreamingUrl());
            request.Headers.Add(HttpRequestHeader.Authorization, AuthorizationHeader);
            request.UserAgent = GetUserAgent();
            return request;
        }

        private string GetNextSubdomain()
        {
            string result;
            lock (SyncRoot)
            {
                result = AvailableSubdomains[_nextSubdomainIndex];
                _nextSubdomainIndex = (_nextSubdomainIndex + 1) % AvailableSubdomains.Length;
            }
            return result;
        }
    }
}