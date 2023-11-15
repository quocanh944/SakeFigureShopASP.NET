using Firebase.Auth;
using Firebase.Storage;
using SakeFigureShop.Services.Interfaces;
using System.Drawing;

namespace SakeFigureShop.Services
{
    public class FirebaseStorageService : IStorageService
    {
        private static string ApiKey = "AIzaSyClaGfQMhhCZD4kDNFoKi-7kTGz3OfjBpg";
        private static string Bucket = "sake-figure-shop-2.appspot.com";
        private static string AuthEmail = "quocanh944@gmail.com";
        private static string AuthPassword = "Asp.NETMVC";
        private readonly ILogger<FirebaseStorageService> _logger;

        public FirebaseStorageService(ILogger<FirebaseStorageService> logger)
        {
            _logger = logger;
        }

        public async Task<string> UploadFileToStorage(Stream stream ,string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions() {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            }).Child(DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + "-" + filename).PutAsync(stream, cancellation.Token);

            return await task;
        }

    }
}
