using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Data.Entities
{
    public class StudentCourse
    {
        public string StudentId { get; set; }

        public User Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public Grade? Grade { get; set; }

        [MaxLength(DataConstants.MaxFileLength)]
        public byte[] ExamSubmission { get; set; }
    }
}