using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Interface.AttachmentService
{
    public interface IAttachmentService
    {
        public string? Upload(string FolderName, IFormFile file);

        public bool Delete(string FolderName, string fileName);
    }
}
