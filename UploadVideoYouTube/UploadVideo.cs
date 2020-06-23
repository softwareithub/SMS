using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace UploadVideoYouTube
{
    public class UploadVideo
    {

        public async Task UploadVideoToYouTube()
        {
            string credentialPath = @"E:\GITRepository\SMS\UploadVideoYouTube\client_secrets.json";
            UserCredential credential;
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });
            string filePath = @"E:\GITRepository\SMS\UploadVideoYouTube\Videos\VID_20190904_003935.mp4";

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = "LMS Video Data";
            video.Snippet.Description = "LMS SERP Learning Data";
            video.Snippet.Tags = new string[] { "tag1", "tag2" };
            video.Snippet.CategoryId = "22";
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "private";

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videoInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videoInsertRequest.ProgressChanged += VideoInsertRequest_ProgressChanged;
                videoInsertRequest.ResponseReceived += VideoInsertRequest_ResponseReceived;
                await videoInsertRequest.UploadAsync();
            }

        }

        private void VideoInsertRequest_ResponseReceived(Video obj)
        {
            
        }

        private void VideoInsertRequest_ProgressChanged(IUploadProgress obj)
        {
            switch (obj.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} byte sent", obj.BytesSent);
                    break;
                case UploadStatus.Failed:
                    Console.WriteLine(obj.Exception);
                    break;
            }
        }
    }
}



