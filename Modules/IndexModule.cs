using Nancy;
using MFCChatClient;
using System;
using System.Linq;
using Nancy.Json;
using System.Net.Sockets;

namespace ModelsOnline.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                var mfc = new MFCClient();
                JsonSettings.MaxJsonLength = Int32.MaxValue;

                Exception socketError = null ;
                mfc.SocketError += (object s, SocketErrorEventArgs e) =>
                {
                    socketError = e.Exception;
                };

                var processed = false;
                mfc.UsersProcessed += (s, e) =>
                {
                    processed = true;
                };

                var now = DateTime.Now;
                var timeout = now.AddSeconds(10);
                while (!processed && DateTime.Now < timeout && null == socketError) { };

                if (null != socketError)
                    return Response.AsJson(new { error = "Socket exception.", exception = socketError });
                if (!processed)
                    return Response.AsJson(new { error = "Timed out." });

                var res = mfc.Models.Select(x => new
                {
                    name = x.Name,
                    broadcasterId = x.UserId,
                    state = x.VideoState.ToString(),
                    camscore = x.ModelDetails.Camscore
                }).OrderByDescending(y => y.camscore);

                return Response.AsJson(res);
            };
        }
    }
}