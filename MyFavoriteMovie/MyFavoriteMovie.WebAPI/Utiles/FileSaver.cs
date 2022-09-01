namespace MyFavoriteMovie.WebAPI.Utiles
{
    public static class FileSaver
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
            

            
            ////var newFileName = Guid.NewGuid().ToString();

            //var path = environment.WebRootPath + $"/{destinationFolderName}";
            //var request = httpRequest.Form;

            ////List<string> newFileNameList = new();
            ////List<IFormFile> fileList = new();
            //////var file = request.Files[0];
            ////List<string> fileExtensionList = new();
            //////var fileExtension = Path.GetExtension(file.FileName);

            ////List<FileStream> fileStreamList = new();

            //string newFileName;
            //string fileExtension;


            //foreach (var file in request.Files)
            //{
            //    //newFileNameList.Add(Guid.NewGuid().ToString());
            //    //
            //    //fileList.Add(file);
            //    //fileExtensionList.Add(file.FileName);

            //    newFileName = Guid.NewGuid().ToString();
            //    fileExtension = Path.GetExtension(file.FileName);

            //    //fileStreamList.Add(new FileStream(Path.Combine(path, newFileName + fileExtension), FileMode.Create));

            //    await file.CopyToAsync(new FileStream(Path.Combine(path, newFileName + fileExtension), FileMode.Create));
            //}




            ////using (var fileStream = new FileStream(
            ////    Path.Combine(path, newFileName + fileExtension), FileMode.Create))
            ////{
            ////    await fileList. CopyToAsync(fileStream);
            ////}

            //return newFileName + fileExtension;
        }
    }
}
