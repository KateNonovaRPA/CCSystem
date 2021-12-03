using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using FileCDN.Models.Data;
using System.Linq;
using ImageProcessor;

namespace FileCDN.Models.Service
{
    public class CdnService<T> : ICdnService<T> where T : DbContext
    {
        private readonly T db;
        private DbSet<FileCdn> FILES;
        private DbSet<FileCdnContent> CONTENT;
        private readonly IImageService imgService;

        public CdnService(T _db, IImageService _imgService)
        {
            db = _db;
            FILES = db.Set<FileCdn>();
            CONTENT = db.Set<FileCdnContent>();

            imgService = _imgService;
        }

        public bool Disable(FileSelect model)
        {
            var cdnFile = FILES.Find(model.FileId);
            if (cdnFile != null)
            {
                cdnFile.IsActive = false;
                FILES.Update(cdnFile);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public FileInfo Download(FileSelect model)
        {
            FileInfo result = new FileInfo();
            try
            {
                Func<FileCdn, bool> fileSelect = x => false;
                if (!string.IsNullOrEmpty(model.FileId))
                {
                    fileSelect = x => x.Id == model.FileId;
                }
                else
                {
                    fileSelect = x => x.SourceType == model.SourceType && x.SourceId == model.SourceID;
                }
                var cdnFile = FILES.Where(fileSelect).FirstOrDefault();
                if (cdnFile != null)
                {
                    result.Id = cdnFile.Id;
                    result.FileName = cdnFile.FileName;
                    result.FileTitle = cdnFile.FileTitle;
                    result.ContentType = cdnFile.ContentType;
                    var contents = CONTENT.Where(x => x.CdnFileId == cdnFile.Id).OrderBy(x => x.Id).ToList();

                    List<byte> content = new List<byte>();

                    foreach (var contentItem in contents)
                    {
                        content.AddRange(contentItem.Content);
                    }

                    result.FileContent = content.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.FileDescription = ex.Message;
            }

            return result;
        }


        public IEnumerable<FileInfo> Select(FileSelect model)
        {
            Func<FileCdn, bool> where = x => true;

            if (!string.IsNullOrEmpty(model.SearchText))
            {
                model.SearchText = model.SearchText.ToLower();
            }

            if (!string.IsNullOrEmpty(model.FileId))
            {
                where = x => x.Id == model.FileId;
            }
            else
            {
                where = x => x.SourceType == model.SourceType && x.SourceId == model.SourceID;
            }

            return FILES
                .Where(where)
                .Select(x => new FileInfo
                {
                    Id = x.Id,
                    FileTitle = x.FileTitle ?? "",
                    FileDescription = x.FileDescription,
                    FileName = x.FileName,
                    DateUploaded = x.DateUploaded,
                    IsActive = x.IsActive,
                    UserUploaded = x.UserUploaded,
                    SourceID = x.SourceId,
                    SourceType = x.SourceType
                })
                .Where(x => x.FileTitle.ToLower().Contains(model.SearchText ?? x.FileTitle.ToLower()));
        }

        public FileUploadResult Upload(FileRequest model)
        {
            var result = SaveFile(model);

            if (model.ThumbSourceType > 0)
            {
                try
                {
                    var tmb = new FileRequest()
                    {
                        SourceType = model.ThumbSourceType,
                        SourceID = result.FileId.ToString(),
                        FileName = model.FileName,
                        UserUploaded = model.UserUploaded,
                        ContentType = model.ContentType,                        
                        FileContent = imgService.Resize(model.FileContent, model.ThumbMaxSize, model.ContentType)
                    };


                    SaveFile(tmb);
                }
                catch (Exception ex) { }
            }

            return result;
        }

        private FileUploadResult SaveFile(FileRequest model)
        {
            FileUploadResult result = new FileUploadResult()
            {
                SavedOK = false
            };
            try
            {
                FileCdn cdnFile = new FileCdn()
                {
                    SourceType = model.SourceType,
                    SourceId = model.SourceID,
                    FileName = model.FileName,
                    FileSize = model.FileContent.Length,
                    FileTitle = model.FileTitle,
                    FileDescription = model.FileDescription,
                    DateUploaded = DateTime.Now,
                    UserUploaded = model.UserUploaded,
                    ContentType = model.ContentType,
                    DateExparing = model.DateExparing,
                    IsReportVisible = model.IsReportVisible,
                    IsActive = true
                };

                //Те това е къде 1Мб
                int maxFilePartLenght = 1048576;
                using (System.IO.Stream fileContent = new System.IO.MemoryStream(model.FileContent))
                {

                    int totalChunks = Convert.ToInt32(Math.Ceiling((decimal)cdnFile.FileSize / maxFilePartLenght));
                    for (int i = 0; i <= totalChunks - 1; i++)
                    {
                        int startIndex = (i * maxFilePartLenght);
                        int endIndex = 0;
                        if ((startIndex + maxFilePartLenght > cdnFile.FileSize))
                        {
                            endIndex = cdnFile.FileSize;
                        }
                        else
                        {
                            endIndex = startIndex + maxFilePartLenght;
                        }
                        int length = endIndex - startIndex;
                        byte[] bytes = new byte[length];
                        fileContent.Read(bytes, 0, bytes.Length);

                        cdnFile.FileContents.Add(new FileCdnContent()
                        {
                            Content = bytes
                        });
                    }

                    FILES.Add(cdnFile);
                    db.SaveChanges();
                    result.FileId = cdnFile.Id;
                    result.SavedOK = !string.IsNullOrEmpty(result.FileId);
                }

                result.SavedOK = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }


}
