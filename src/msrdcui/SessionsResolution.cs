using System;

namespace rdclauncher
{
    public sealed class SessionsResolution
    {
        public const int MinimumResolution = 200;
        public const int MaximumResolution = 8192;
        public const int UnsetResolutionValue = -1;

        public SessionsResolution()
        {
            ResolutionWidth = UnsetResolutionValue;
            ResolutionHeight = UnsetResolutionValue;
        }

        public SessionsResolution(int width, int height)
        {
            ResolutionWidth = width;
            ResolutionHeight = height;
        }

        private int _resolutionWidth;
        public int ResolutionWidth
        {
            get => _resolutionWidth;
            private set
            {
                if (value != UnsetResolutionValue && (value < MinimumResolution || value > MaximumResolution))
                {
                    throw new ArgumentOutOfRangeException("value", value, string.Format("The valid resolution value is between {0} and {1}.", MinimumResolution, MaximumResolution));
                }
                _resolutionWidth = value;
            }
        }

        private int _resolutionHeight;
        public int ResolutionHeight
        {
            get => _resolutionHeight;
            private set
            {
                if (value != UnsetResolutionValue && (value < MinimumResolution || value > MaximumResolution))
                {
                    throw new ArgumentOutOfRangeException("value", value, string.Format("The valid resolution value is between {0} and {1}.", MinimumResolution, MaximumResolution));
                }
                _resolutionHeight = value;
            }
        }
    }
}
