using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines a locktime 
    /// </summary>
    public class LockTime
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="type">A LockTimeType indicating the type of the locktime.</param>
        /// <param name="timestamp">An uint indicating the timestamp of the locktime.</param>
        public LockTime(LockTimeType type, uint timestamp)
        {
            Type = type;

            if (type == LockTimeType.BlockHeight)
                TargetBlock = timestamp;
            else if (type == LockTimeType.Timestamp)
            {
                TargetTimestamp = timestamp;
                TargetTimestampDateTime = HelperFunctions.ConvertTimestamp(timestamp);
            }
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return Type switch
            {
                LockTimeType.BlockHeight => $"Block Height: {TargetBlock}",
                LockTimeType.NoLockTime => "No locktime",
                LockTimeType.Timestamp => $"Timestamp: {TargetTimestampDateTime}",
                _ => base.ToString(),
            };
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the target block if specified in the locktime.
        /// </summary>
        public uint TargetBlock { get; }

        /// <summary>
        /// Gets the target timestamp if specified in the locktime.
        /// </summary>
        public uint TargetTimestamp { get; }

        /// <summary>
        /// Gets the target DateTime if specified in the locktime.
        /// </summary>
        public DateTime TargetTimestampDateTime { get; }

        /// <summary>
        /// Gets the type of the locktime.
        /// </summary>
        public LockTimeType Type { get; }

        #endregion
    }
}