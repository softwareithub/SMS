using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.Model
{
    public class PlayListModel
    {

        public PlaylistItemListResponse GetPlayList()
        {
            var PlaylistItemListResponseModels = new PlaylistItemListResponse();
            var yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyCfrngStOr1JQgmn0yYo_dIzkzcv3b1Hr0" });
            var channelsListRequest = yt.Channels.List("contentDetails");
            channelsListRequest.Id = "UCJjkriRG3yrZLNyLBAf4Igw";
            //channelsListRequest.ForUsername = "cbseclassvideos";
            var channelsListResponse = channelsListRequest.Execute();
            int VideoCount = 1;
            foreach (var channel in channelsListResponse.Items)
            {
                var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                var nextPageToken = "";
                var playlistItemsListRequest = yt.PlaylistItems.List("snippet");
                playlistItemsListRequest.PlaylistId = uploadsListId;
                playlistItemsListRequest.MaxResults = 5000;
                playlistItemsListRequest.PageToken = nextPageToken;
                // Retrieve the list of videos uploaded to the authenticated user's channel.  
                PlaylistItemListResponseModels = playlistItemsListRequest.Execute();

            }

            return PlaylistItemListResponseModels;
        }
    }
}
