namespace Tommava.Services
{
    public interface ICommon
    {
        Task<string> UploadIconAsync(IFormFile file);
        Task<string> UploadVideoAsync(IFormFile file);
        Task<string> UploadChunkVideoAsync(IFormFile file);
        Task<string> UploadImgVideoAsync(IFormFile file);
    }
}
