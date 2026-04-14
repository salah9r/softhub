using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace SoftHub.Models
{
    public class Software
    {
        public int Id { get; set; }

        public int DownloadCount { get; set; } = 0;

        public string? Name { get; set; }

     
        public string Description { get; set; } = "";
        // 🔥 مسارات الملفات
        public string? ImagePath { get; set; }

        public string? FilePath { get; set; }

        // 🔥 رفع الملفات (غير مخزنة في DB)
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public IFormFile? UploadFile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}