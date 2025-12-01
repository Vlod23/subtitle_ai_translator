using System.ComponentModel.DataAnnotations;

namespace SubtitlesTranslator.Domain.Entities {
    public class SubtitleLike {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int SubtitleTranslationId { get; set; }

        // Foreign keys:
        public ApplicationUser User { get; set; } // foreign key connection with User table. UserId → points to AspNetUsers.Id
        public SubtitleTranslation SubtitleTranslation { get; set; } // foreign key connection with SubtitleTranslation table. SubtitleTranslationId → points to SubtitleTranslation.Id
    }
}
