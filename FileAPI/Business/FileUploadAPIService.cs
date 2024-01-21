using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileAPI.Business
{

    public class FileUploadAPIService
    {
        readonly IWebHostEnvironment _webHostEnviroment;
        public FileUploadAPIService(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            string mainPah = Path.Combine(_webHostEnviroment.WebRootPath, pathOrContainerName);
            System.IO.File.Delete(Path.Combine(mainPah, fileName));
        }


        public List<string> GetFiles(string pathOrContainerName)
        {
            DirectoryInfo directory = new DirectoryInfo(pathOrContainerName);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string pathOrContainerName, string fileName)
           => System.IO.File.Exists(Path.Combine(pathOrContainerName, fileName));



        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnviroment.WebRootPath, pathOrContainerName);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new List<(string fileName, string path)>();

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
                await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);

                datas.Add((fileNewName, Path.Combine(pathOrContainerName, fileNewName)));

            }

            return datas;

        }

        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private int Counter { get; set; } = 1;
        protected async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        {

            string newFileName = await Task.Run<string>(async () =>
            {
                string extenitons = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);

                if (!first)
                {
                    int idx = oldName.LastIndexOf('-');
                    string before = oldName.Substring(0, idx);
                    oldName = $"{before}-{Counter}";
                }

                string newFileName = CharecterRegulatory(oldName) + extenitons;

                if (HasFile(path, newFileName))
                {
                    Counter = Counter + 1;
                    if (first)
                        return await FileRenameAsync(path, $"{Path.GetFileNameWithoutExtension(newFileName)}-{Counter}{Path.GetExtension(newFileName)}", false);
                    return await FileRenameAsync(path, $"{Path.GetFileNameWithoutExtension(newFileName)}{Path.GetExtension(newFileName)}", false);
                }
                else
                {
                    Counter = 1;
                    return newFileName;
                }
            });

            return newFileName;
        }
        public string CharecterRegulatory(string name)
        {
            return name.Replace('İ', 'I')
                        .Replace('Ü', 'U')
                        .Replace('Ş', 'S')
                        .Replace('Ç', 'C')
                        .Replace('Ö', 'O')
                        .Replace('Ğ', 'G')
                        .Replace('ü', 'u')
                        .Replace('ş', 's')
                        .Replace('ç', 'c')
                        .Replace('ı', 'i')
                        .Replace('ö', 'o')
                        .Replace('ğ', 'g');
        }
    }
}
