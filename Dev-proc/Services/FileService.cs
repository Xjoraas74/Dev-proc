using Dev_proc.Constants;
using Dev_proc.Data;
using Dev_proc.Models.Data;
using Dev_proc.Models.Enums;
using Dev_proc.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Services
{
    public interface IFileService
    {
        public Task UploadResume(ResumeFileViewModel resume);
        public Task UploadPracticeDiary(ResumeFileViewModel practiceDiary);
        public Task DeleteResume(UploadedFile? resume);
        public Task DeletePracticeDiary(PracticeDiary? practiceDiary);
    }
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;
        public FileService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task UploadResume(ResumeFileViewModel resume)
        {
            if (resume?.Resume == null)
            {
                throw new ArgumentNullException(nameof(resume));
            }

            var student = await _context.Users
                .Include(s=>s.Resume)
                .Where(x => x.Id == resume.StudentId).FirstOrDefaultAsync();

            if (student == null)
            {
                throw new ArgumentNullException(nameof(resume.StudentId));
            }
            if (student.Resume?.Name != null || student.Resume?.Path != null)
            {
                throw new Exception("Resume already exists");
            }

            string path = FolderPaths.ResumeFolder + resume.StudentId.ToString();
            CreateDirectoryIfDoesntExists(path);
            path += "/" + resume.Resume.FileName;
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await resume.Resume.CopyToAsync(fileStream);
            }
            UploadedFile file = new UploadedFile { Name = resume.Resume.FileName, Path = path, User = student };
            await _context.Files.AddAsync(file);
            student.Resume = file;
            _context.Users.Update(student);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteResume(UploadedFile? resume)
        {
            if (resume == null)
            {
                throw new ArgumentNullException(nameof(resume));
            }
            if (resume.Path == null)
            {
                throw new ArgumentNullException(nameof(resume.Path));
            }
            var user = await _context.Users.Where(u => u.Id == resume.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            DeleteFile(resume.Path);
            user.Resume = null;
            user.ResumeId = null;
            _context.Files.Remove(resume);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

        }
        public async Task DeletePracticeDiary(PracticeDiary practiceDiary)
        {
            if (practiceDiary == null)
            {
                throw new ArgumentNullException(nameof(practiceDiary));
            }
            if (practiceDiary.Path == null)
            {
                throw new ArgumentNullException(nameof(practiceDiary.Path));
            }
            var user = await _context.Users.Where(u => u.Id == practiceDiary.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            DeleteFile(practiceDiary.Path);
            user.PracticeDiary.Path = null;
            user.PracticeDiary.Name = null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

        }
        public async Task UploadPracticeDiary(ResumeFileViewModel practiceDiary)
        {
            if (practiceDiary?.Resume == null)
            {
                throw new ArgumentNullException(nameof(practiceDiary));
            }

            var student = await _context.Users
                .Include(s => s.PracticeDiary)
                .Where(x => x.Id == practiceDiary.StudentId).FirstOrDefaultAsync();

            if (student == null)
            {
                throw new ArgumentNullException(nameof(practiceDiary.StudentId));
            }
            if (student.PracticeDiary?.Name != null || student.PracticeDiary?.Path != null)
            {
                throw new Exception("Practice diary already exists");
            }

            string path = FolderPaths.PracticeDiaryFolder + practiceDiary.StudentId.ToString();
            CreateDirectoryIfDoesntExists(path);
            path += "/" + practiceDiary.Resume.FileName;
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await practiceDiary.Resume.CopyToAsync(fileStream);
            }
            if (student.PracticeDiary == null) 
            { 
                PracticeDiary file = new PracticeDiary { Name = practiceDiary.Resume.FileName, Path = path, PracticeDiaryStatus = PracticeDiaryStatus.New, User = student };
                await _context.PracticeDiaries.AddAsync(file);
                student.PracticeDiary = file;
            }
            else
            {
                student.PracticeDiary.Path = path;
                student.PracticeDiary.Name = practiceDiary.Resume.FileName;
            }
            
            _context.Users.Update(student);
            await _context.SaveChangesAsync();
        }
        private void CreateDirectoryIfDoesntExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                //TODO: MB some logic here
            }
            else
            {
                File.Delete(filePath);
            }
        }
    }
}
