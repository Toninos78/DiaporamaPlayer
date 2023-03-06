using System;

namespace DiaporamaPlayer
{
    internal class DiaporamaScript
    {
        public int MaximumPicturesPerLayout { get; set; }
        public TimeSpan StartTemporisationDuration { get; set; }
        public TimeSpan FadeoutDuration { get; set; }
        public DiaporamaStep[] Steps { get; set; } = Array.Empty<DiaporamaStep>();
        public float MaximumAbsoluteEndAngleDeviation { get; set; }
        public float MaximumAbsoluteStartAngleDeviation { get; set; }
        public float BigPictureMarginRatio { get; set; }
        public float SmallPictureMarginRatio { get; set; }
        public string ImageFolder { get; set; } = string.Empty;
        public string SongFullpath { get; set; } = String.Empty;
    }
}
