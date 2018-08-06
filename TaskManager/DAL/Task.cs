namespace TaskManager.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TaskName { get; set; }

        public int? TaskTypeId { get; set; }

        public string Description { get; set; }

        public int? TaskPriorityId { get; set; }

        public int? UserId { get; set; }

        public int? TaskStatusId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateClose { get; set; }

        public virtual Project Project { get; set; }

        public virtual TaskPriority TaskPriority { get; set; }

        public virtual TaskStatus TaskStatus { get; set; }

        public virtual TaskType TaskType { get; set; }

        public virtual User User { get; set; }
    }
}
