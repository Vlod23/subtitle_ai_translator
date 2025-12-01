namespace SubtitlesTranslator.Application.Interfaces
{
    public interface ICreditEstimator
    {
        public int EstimateCredits(string targetLanguage, string srtText);
    }
}
