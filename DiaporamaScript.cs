using System;

namespace DiaporamaPlayer
{
    internal class DiaporamaScript
    {
        public int MaximumPicturesPerLayout { get; set; }
        public TimeSpan StartTemporisationDuration { get; set; }
        public TimeSpan FadeoutDuration { get; set; }
        public TimeSpan FinalFadeoutAfterLastStepDelay { get; set; }
        public TimeSpan FinalFadeoutAfterLastStepDuration { get; set; }
        public DiaporamaStep[] Steps { get; set; } = Array.Empty<DiaporamaStep>();
        public float MaximumAbsoluteEndAngleDeviation { get; set; }
        public float MaximumAbsoluteStartAngleDeviation { get; set; }
        public float BigPictureMarginRatio { get; set; }
        public float SmallPictureMarginRatio { get; set; }
        public string DataFolder { get; set; } = string.Empty;
        public string SongFilename { get; set; } = string.Empty;
    }
}
