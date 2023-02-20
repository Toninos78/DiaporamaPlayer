using System;

namespace DiaporamaPlayer
{
    internal record DiaporamaStep(string Filename, TimeSpan Duration, FinalLayout FinalLayout, StepSource Source);

    internal enum FinalLayout
    {
        Left,
        Right,
        Full
    }

    internal enum StepSource
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Random
    }

    internal enum Orientation
    {
        Portrait,
        Landscape,
    }
}
