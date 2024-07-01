using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dialogix.AppCode;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Dialogix.Hubs
{
    public interface IChatHub
    {
        //Task SendNotificationToUser(int userId, string message);
       // Task<bool> IsUserConnected(int userId);
        Task SendMessage(string recipientUserId, string message);
    }

    public class ChatHub : Hub, IChatHub
    {
        private readonly string _connectionString;
        private readonly ISessionService _sessionService;


        private static readonly ConcurrentDictionary<string, List<string>> _userConnections = new ConcurrentDictionary<string, List<string>>();

        public ChatHub(IConfiguration configuration, ISessionService sessionService)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
            _sessionService = sessionService;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            var userId = _sessionService.GetInt32("UserID");

            if (userId.HasValue)
            {
                string userID = userId.ToString();


                _userConnections.AddOrUpdate(userID, new List<string> { connectionId }, (key, existingList) =>
                {
                    existingList.Add(connectionId);
                    return existingList;
                });

                Console.WriteLine($"User {userId} connected with Connection ID: {connectionId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            var userId = _sessionService.GetInt32("UserID");

            if (userId.HasValue)
            {
                string userID = userId.ToString();

                // Remove connection ID from the list of user connections
                if (_userConnections.TryGetValue(userID, out var connections))
                {
                    connections.Remove(connectionId);

                    // Remove the user from the dictionary if no connections are left
                    if (connections.Count == 0)
                    {
                        _userConnections.TryRemove(userID, out _);
                    }
                }

                Console.WriteLine($"User {userId} disconnected with Connection ID: {connectionId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string recipientUserId, string message)
        {
            await SaveMessageToDatabase(recipientUserId, message);

            var senderUserName = _sessionService.GetString("UserName");

            // Send message to all connections of the recipient
            if (_userConnections.TryGetValue(recipientUserId, out var connectionIds))
            {
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderUserName, message);
                }
            }
        }

        private async Task SaveMessageToDatabase(string recipientId, string message)
        {
            try
            {
                var senderUserId = _sessionService.GetInt32("UserID");

                if (!senderUserId.HasValue)
                {
                    Console.WriteLine("Sender user ID is not available.");
                    return;
                }

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("usp_MessageRecord", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 1);
                        cmd.Parameters.AddWithValue("@SenderID", senderUserId.Value);
                        cmd.Parameters.AddWithValue("@ReceiverID", recipientId);
                        cmd.Parameters.AddWithValue("@Message", message);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
            }
        }

        //#region "Send Notification"
        //public async Task SendNotificationToUser(int userId, string message)
        //{
        //    string userID = userId.ToString();
        //    string senderUserName = _sessionService.GetString("UserName");

        //    // Check if user has any active connections
        //    if (_userConnections.TryGetValue(userID, out var connectionIds))
        //    {
        //        Console.WriteLine($"User {userID} has {connectionIds.Count} active connections.");

        //        if (connectionIds == null || connectionIds.Count == 0)
        //        {
        //            Console.WriteLine($"User {userID} has no active connections.");
        //            return; // Exit if there are no active connections
        //        }

        //        foreach (var connectionId in connectionIds)
        //        {
        //            if (string.IsNullOrEmpty(connectionId))
        //            {
        //                Console.WriteLine("Connection ID is null or empty.");
        //                continue;
        //            }

        //            try
        //            {
        //                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
        //                Console.WriteLine($"Sent notification to {connectionId}: {message}");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Error sending notification to connection {connectionId}: {ex}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"No connections found for user {userID}. Notification not sent.");
        //    }
        //}

        //public Task<bool> IsUserConnected(int userId)
        //{
        //    return Task.FromResult(_userConnections.ContainsKey(userId.ToString()));
        //}
        //#endregion
    }
}
