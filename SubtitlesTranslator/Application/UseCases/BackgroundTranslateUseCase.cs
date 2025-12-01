using System.Diagnostics;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases
{
    public class BackgroundTranslateUseCase
    {
        private readonly IChatGptService _chatGpt;
        private readonly ISubtitleRepository _subtitleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly string? _apiModel;
        private readonly UpdateUserCreditsUseCase _updateUserCredits;

        public BackgroundTranslateUseCase(
            IConfiguration config, 
            IChatGptService chatGpt, 
            ISubtitleRepository subtitleRepository, 
            IUserRepository userRepository, 
            IEmailService emailService, 
            IEmailTemplateService emailTemplateService,
            UpdateUserCreditsUseCase updateUserCredits)
        {
            _chatGpt = chatGpt;
            _subtitleRepository = subtitleRepository;
            _userRepository = userRepository;
            _apiModel = config["OpenAI:ApiModel"];
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _updateUserCredits = updateUserCredits;
        }

        public async Task ExecuteAsync(int subtitleId)
        {
            var subtitle = await _subtitleRepository.GetUserSubtitleAsync(subtitleId);
            var user = await _userRepository.GetUserByIdAsync(subtitle.UserId);

            // Translate chunks of 30 timestamps
            var chunks = ChunkByBlocks(subtitle.OriginalText, blocksPerChunk: 30);
            var translatedChunks = new List<string>();
            var fullTranslation = "";
            var prompt = _chatGpt.SetPrompt(subtitle.TargetLanguage);
                                                                                                var stopwatch = Stopwatch.StartNew();
            try {
                foreach (var chunk in chunks) {
                    var translated = await _chatGpt.TranslateAsync(chunk, prompt, subtitle.TargetLanguage);
                    translatedChunks.Add(translated);
                }
                fullTranslation = string.Join("\n\n", translatedChunks);
                subtitle.SetTranslationSuccess(fullTranslation);

                await _subtitleRepository.UpdateAsync(subtitle);
                                                                                                stopwatch.Stop();
                                                                                                var result = $"Model: {_apiModel}; Execution Time: {stopwatch.ElapsedMilliseconds} ms ({stopwatch.ElapsedMilliseconds / 60000} minutes). Name: {subtitle.Name}";
                                                                                                File.AppendAllText("log.txt", result);
                                                                                                File.AppendAllText("log.txt", Environment.NewLine);
                // Deduct credits
                await _updateUserCredits.ExecuteAsync(subtitle.UserId, subtitle.CreditsUsed, add: false);

                var mailSubject = _emailTemplateService.GetMailSubjectTranslated();
                var mailHtmlBody = _emailTemplateService.GetMailBodyTranslated(subtitleId, subtitle.Name);

                // Send translation success email
                await _emailService.SendEmailAsync(user.Email, user.UserName, mailSubject, "developer testing", mailHtmlBody);
            } 
            catch (Exception ex)
            {
                // Return credits
                await _updateUserCredits.ExecuteAsync(subtitle.UserId, subtitle.CreditsUsed, add: true);

                // Set failure
                subtitle.SetTranslationFailure();
                await _subtitleRepository.UpdateAsync(subtitle);
                                                                                                File.AppendAllText("translation_errors.txt", $"[{DateTime.Now}] SubtitleId {subtitle.Id} failed: {ex.Message}\n");
                                                                                                File.AppendAllText("translation_errors.txt", Environment.NewLine);
                // Send translation failure email
                var mailSubject = _emailTemplateService.GetMailSubjectTranslationFailed();
                var mailHtmlBody = _emailTemplateService.GetMailBodyTranslationFailed(subtitleId, subtitle.Name);

                await _emailService.SendEmailAsync(user.Email, user.UserName, mailSubject, "developer testing", mailHtmlBody);
            }
        }

        public List<string> ChunkByBlocks(string srtContent, int blocksPerChunk) 
        {
            var chunks = new List<string>();
            var currentChunk = new List<string>();
            int blockCount = 0;

            // Split to blocks by double newline
            var blocks = srtContent.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var block in blocks) {
                currentChunk.Add(block.Trim());
                blockCount++;

                if (blockCount >= blocksPerChunk) {
                    chunks.Add(string.Join("\n\n", currentChunk));
                    currentChunk.Clear();
                    blockCount = 0;
                }
            }

            // Add remaining chunk if any
            if (currentChunk.Any())
                chunks.Add(string.Join("\n\n", currentChunk));

            return chunks;
        }
    }
}
