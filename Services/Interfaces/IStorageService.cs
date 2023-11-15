namespace SakeFigureShop.Services.Interfaces
{
    public interface IStorageService
    {
        public Task<string> UploadFileToStorage(Stream stream, string fileName);
    }
}
