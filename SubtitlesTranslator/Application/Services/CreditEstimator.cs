using SharpToken;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.Services
{
    public class CreditEstimator : ICreditEstimator
    {
        private readonly string _tokensPerCredit;
        public CreditEstimator(IConfiguration config)
        {
            _tokensPerCredit = config["CreditSystem:TokensPerCredit"];
        }
        public int EstimateCredits(string prompt, string srtText)
        {
            var encoding = GptEncoding.GetEncoding("cl100k_base");
            var inputTokens = encoding.Encode(prompt + srtText).Count;
            var outputTokens = inputTokens * 1.6; // + tokens for response
            outputTokens += outputTokens * 0.1; // + our profit 10%
            int tokensPerCredit = int.Parse(_tokensPerCredit);  // convert tokens to credits
            return (int)Math.Ceiling(outputTokens / tokensPerCredit);
        }
    }
}
