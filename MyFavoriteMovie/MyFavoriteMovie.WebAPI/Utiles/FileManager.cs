namespace MyFavoriteMovie.WebAPI.Utiles
{
    public static class FileManager
    {
        public async static Task<string> SaveAsync(IFormFile file, string path)
        {
            string? result;

            try
            {
                if(!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var newFileName = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(file.FileName);

                using (var fileStream = new FileStream(
                    Path.Combine(path, newFileName + fileExtension), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                result = newFileName + fileExtension;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static void Delete(string fileName, string path)
        {
            try
            {
                if(!Directory.Exists(path)) throw new Exception("Directory from path not found.");
                if (!File.Exists(path + fileName)) throw new Exception("File from path not found.");

                File.Delete(path + fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
