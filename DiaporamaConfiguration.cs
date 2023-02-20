namespace DiaporamaPlayer
{
    internal class DiaporamaConfiguration
    {
        public float MaximumAbsoluteEndAngleDeviation { get; set; } = 3;
        public float MaximumAbsoluteStartAngleDeviation { get; set; } = 20;
        public float BigPictureMarginRatio { get; set; } = 0.075f;
        public float SmallPictureMarginRatio { get; set; } = 0.15f;
        public string ImageFolder { get; set; } = $"C:\\temp\\diaporama";
    }
}
